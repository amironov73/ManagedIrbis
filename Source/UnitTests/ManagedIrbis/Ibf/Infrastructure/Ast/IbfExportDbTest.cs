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
    public class IbfExportDbTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfExportDb _GetNode()
        {
            return new IbfExportDb();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfExportDb_Construction_1()
        {
            IbfExportDb node = new IbfExportDb();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfExportDb_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfExportDb node = new IbfExportDb();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfExportDb first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfExportDb second = bytes.RestoreObjectFromMemory<IbfExportDb>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfExportDb_Serialization_1()
        {
            IbfExportDb node = new IbfExportDb();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfExportDb_Verify_1()
        {
            IbfExportDb node = new IbfExportDb();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfExportDb_ToXml_1()
        {
            IbfExportDb node = new IbfExportDb();
            Assert.AreEqual("<IbfExportDb />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfExportDb_ToJson_1()
        {
            IbfExportDb node = new IbfExportDb();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfExportDb_ToString_1()
        {
            IbfExportDb node = new IbfExportDb();
            Assert.AreEqual("ExportDb", node.ToString());
        }
    }
}
