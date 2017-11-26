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
    public class IbfOpenDbTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfOpenDb _GetNode()
        {
            return new IbfOpenDb();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfOpenDb_Construction_1()
        {
            IbfOpenDb node = new IbfOpenDb();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfOpenDb_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfOpenDb node = new IbfOpenDb();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfOpenDb first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfOpenDb second = bytes.RestoreObjectFromMemory<IbfOpenDb>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfOpenDb_Serialization_1()
        {
            IbfOpenDb node = new IbfOpenDb();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfOpenDb_Verify_1()
        {
            IbfOpenDb node = new IbfOpenDb();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfOpenDb_ToXml_1()
        {
            IbfOpenDb node = new IbfOpenDb();
            Assert.AreEqual("<IbfOpenDb />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfOpenDb_ToJson_1()
        {
            IbfOpenDb node = new IbfOpenDb();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfOpenDb_ToString_1()
        {
            IbfOpenDb node = new IbfOpenDb();
            Assert.AreEqual("OpenDb", node.ToString());
        }
    }
}
