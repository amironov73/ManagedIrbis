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
    public class IbfPrintTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfPrint _GetNode()
        {
            return new IbfPrint();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfPrint_Construction_1()
        {
            IbfPrint node = new IbfPrint();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfPrint_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfPrint node = new IbfPrint();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfPrint first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfPrint second = bytes.RestoreObjectFromMemory<IbfPrint>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfPrint_Serialization_1()
        {
            IbfPrint node = new IbfPrint();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfPrint_Verify_1()
        {
            IbfPrint node = new IbfPrint();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfPrint_ToXml_1()
        {
            IbfPrint node = new IbfPrint();
            Assert.AreEqual("<IbfPrint />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfPrint_ToJson_1()
        {
            IbfPrint node = new IbfPrint();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfPrint_ToString_1()
        {
            IbfPrint node = new IbfPrint();
            Assert.AreEqual("Print", node.ToString());
        }
    }
}
