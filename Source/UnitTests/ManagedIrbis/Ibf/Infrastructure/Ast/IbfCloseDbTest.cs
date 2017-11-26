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
    public class IbfCloseDbTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfCloseDb _GetNode()
        {
            return new IbfCloseDb();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfCloseDb_Construction_1()
        {
            IbfCloseDb node = new IbfCloseDb();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfCloseDb_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfCloseDb node = new IbfCloseDb();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfCloseDb first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfCloseDb second = bytes.RestoreObjectFromMemory<IbfCloseDb>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfCloseDb_Serialization_1()
        {
            IbfCloseDb node = new IbfCloseDb();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfCloseDb_Verify_1()
        {
            IbfCloseDb node = new IbfCloseDb();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfCloseDb_ToXml_1()
        {
            IbfCloseDb node = new IbfCloseDb();
            Assert.AreEqual("<IbfCloseDb />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfCloseDb_ToJson_1()
        {
            IbfCloseDb node = new IbfCloseDb();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfCloseDb_ToString_1()
        {
            IbfCloseDb node = new IbfCloseDb();
            Assert.AreEqual("CloseDb", node.ToString());
        }
    }
}
