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
    public class IbfReorgMfTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfReorgMf _GetNode()
        {
            return new IbfReorgMf();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfReorgMf_Construction_1()
        {
            IbfReorgMf node = new IbfReorgMf();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfReorgMf_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfReorgMf node = new IbfReorgMf();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfReorgMf first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfReorgMf second = bytes.RestoreObjectFromMemory<IbfReorgMf>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfReorgMf_Serialization_1()
        {
            IbfReorgMf node = new IbfReorgMf();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfReorgMf_Verify_1()
        {
            IbfReorgMf node = new IbfReorgMf();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfReorgMf_ToXml_1()
        {
            IbfReorgMf node = new IbfReorgMf();
            Assert.AreEqual("<IbfReorgMf />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfReorgMf_ToJson_1()
        {
            IbfReorgMf node = new IbfReorgMf();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfReorgMf_ToString_1()
        {
            IbfReorgMf node = new IbfReorgMf();
            Assert.AreEqual("ReorgMf", node.ToString());
        }
    }
}
