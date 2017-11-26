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
    public class IbfDiagnosMfTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfDiagnosMf _GetNode()
        {
            return new IbfDiagnosMf();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfDiagnosMf_Construction_1()
        {
            IbfDiagnosMf node = new IbfDiagnosMf();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfDiagnosMf_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfDiagnosMf node = new IbfDiagnosMf();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfDiagnosMf first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfDiagnosMf second = bytes.RestoreObjectFromMemory<IbfDiagnosMf>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfDiagnosMf_Serialization_1()
        {
            IbfDiagnosMf node = new IbfDiagnosMf();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfDiagnosMf_Verify_1()
        {
            IbfDiagnosMf node = new IbfDiagnosMf();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfDiagnosMf_ToXml_1()
        {
            IbfDiagnosMf node = new IbfDiagnosMf();
            Assert.AreEqual("<IbfDiagnosMf />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfDiagnosMf_ToJson_1()
        {
            IbfDiagnosMf node = new IbfDiagnosMf();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfDiagnosMf_ToString_1()
        {
            IbfDiagnosMf node = new IbfDiagnosMf();
            Assert.AreEqual("DiagnosMf", node.ToString());
        }
    }
}
