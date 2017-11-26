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
    public class IbfReorgIfTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfReorgIf _GetNode()
        {
            return new IbfReorgIf();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfReorgIf_Construction_1()
        {
            IbfReorgIf node = new IbfReorgIf();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfReorgIf_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfReorgIf node = new IbfReorgIf();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfReorgIf first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfReorgIf second = bytes.RestoreObjectFromMemory<IbfReorgIf>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfReorgIf_Serialization_1()
        {
            IbfReorgIf node = new IbfReorgIf();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfReorgIf_Verify_1()
        {
            IbfReorgIf node = new IbfReorgIf();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfReorgIf_ToXml_1()
        {
            IbfReorgIf node = new IbfReorgIf();
            Assert.AreEqual("<IbfReorgIf />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfReorgIf_ToJson_1()
        {
            IbfReorgIf node = new IbfReorgIf();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfReorgIf_ToString_1()
        {
            IbfReorgIf node = new IbfReorgIf();
            Assert.AreEqual("ReorgIf", node.ToString());
        }
    }
}
