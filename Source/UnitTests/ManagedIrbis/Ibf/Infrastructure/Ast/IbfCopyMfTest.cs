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
    public class IbfCopyMfTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfCopyMf _GetNode()
        {
            return new IbfCopyMf();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfCopyMf_Construction_1()
        {
            IbfCopyMf node = new IbfCopyMf();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfCopyMf_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfCopyMf node = new IbfCopyMf();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfCopyMf first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfCopyMf second = bytes.RestoreObjectFromMemory<IbfCopyMf>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfCopyMf_Serialization_1()
        {
            IbfCopyMf node = new IbfCopyMf();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfCopyMf_Verify_1()
        {
            IbfCopyMf node = new IbfCopyMf();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfCopyMf_ToXml_1()
        {
            IbfCopyMf node = new IbfCopyMf();
            Assert.AreEqual("<IbfCopyMf />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfCopyMf_ToJson_1()
        {
            IbfCopyMf node = new IbfCopyMf();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfCopyMf_ToString_1()
        {
            IbfCopyMf node = new IbfCopyMf();
            Assert.AreEqual("CopyMf", node.ToString());
        }
    }
}
