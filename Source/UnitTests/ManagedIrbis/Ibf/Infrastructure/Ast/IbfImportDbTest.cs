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
    public class IbfImportDbTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfImportDb _GetNode()
        {
            return new IbfImportDb();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfImportDb_Construction_1()
        {
            IbfImportDb node = new IbfImportDb();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfImportDb_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfImportDb node = new IbfImportDb();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfImportDb first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfImportDb second = bytes.RestoreObjectFromMemory<IbfImportDb>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfImportDb_Serialization_1()
        {
            IbfImportDb node = new IbfImportDb();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfImportDb_Verify_1()
        {
            IbfImportDb node = new IbfImportDb();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfImportDb_ToXml_1()
        {
            IbfImportDb node = new IbfImportDb();
            Assert.AreEqual("<IbfImportDb />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfImportDb_ToJson_1()
        {
            IbfImportDb node = new IbfImportDb();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfImportDb_ToString_1()
        {
            IbfImportDb node = new IbfImportDb();
            Assert.AreEqual("ImportDb", node.ToString());
        }
    }
}
