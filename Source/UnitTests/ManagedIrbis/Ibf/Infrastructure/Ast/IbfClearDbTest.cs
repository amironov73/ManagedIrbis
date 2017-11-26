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
    public class IbfClearDbTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfClearDb _GetNode()
        {
            return new IbfClearDb();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfClearDb_Construction_1()
        {
            IbfClearDb node = new IbfClearDb();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfClearDb_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfClearDb node = new IbfClearDb();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfClearDb first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfClearDb second = bytes.RestoreObjectFromMemory<IbfClearDb>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfClearDb_Serialization_1()
        {
            IbfClearDb node = new IbfClearDb();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfClearDb_Verify_1()
        {
            IbfClearDb node = new IbfClearDb();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfClearDb_ToXml_1()
        {
            IbfClearDb node = new IbfClearDb();
            Assert.AreEqual("<IbfClearDb />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfClearDb_ToJson_1()
        {
            IbfClearDb node = new IbfClearDb();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfClearDb_ToString_1()
        {
            IbfClearDb node = new IbfClearDb();
            Assert.AreEqual("ClearDb", node.ToString());
        }
    }
}
