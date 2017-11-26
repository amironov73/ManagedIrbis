using System;
using System.IO;

using AM.IO;
using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Ibf;
using ManagedIrbis.Ibf.Infrastructure;
using ManagedIrbis.Ibf.Infrastructure.Ast;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Ibf.Infrastructure.Ast
{
    [TestClass]
    public class IbfNewDbTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfNewDb _GetNode()
        {
            return new IbfNewDb();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfNewDb_Construction_1()
        {
            IbfNewDb node = new IbfNewDb();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfNewDb_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfNewDb node = new IbfNewDb();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfNewDb first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfNewDb second = bytes.RestoreObjectFromMemory<IbfNewDb>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfNewDb_Serialization_1()
        {
            IbfNewDb node = new IbfNewDb();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfNewDb_Verify_1()
        {
            IbfNewDb node = new IbfNewDb();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfNewDb_ToXml_1()
        {
            IbfNewDb node = new IbfNewDb();
            Assert.AreEqual("<IbfNewDb />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfNewDb_ToJson_1()
        {
            IbfNewDb node = new IbfNewDb();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfNewDb_ToString_1()
        {
            IbfNewDb node = new IbfNewDb();
            Assert.AreEqual("NewDb", node.ToString());
        }
    }
}
