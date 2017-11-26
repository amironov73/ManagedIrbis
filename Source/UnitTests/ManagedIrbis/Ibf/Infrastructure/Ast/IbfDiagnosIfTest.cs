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
    public class IbfDiagnosIfTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfDiagnosIf _GetNode()
        {
            return new IbfDiagnosIf();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfDiagnosIf_Construction_1()
        {
            IbfDiagnosIf node = new IbfDiagnosIf();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfDiagnosIf_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfDiagnosIf node = new IbfDiagnosIf();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfDiagnosIf first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfDiagnosIf second = bytes.RestoreObjectFromMemory<IbfDiagnosIf>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfDiagnosIf_Serialization_1()
        {
            IbfDiagnosIf node = new IbfDiagnosIf();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfDiagnosIf_Verify_1()
        {
            IbfDiagnosIf node = new IbfDiagnosIf();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfDiagnosIf_ToXml_1()
        {
            IbfDiagnosIf node = new IbfDiagnosIf();
            Assert.AreEqual("<IbfDiagnosIf />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfDiagnosIf_ToJson_1()
        {
            IbfDiagnosIf node = new IbfDiagnosIf();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfDiagnosIf_ToString_1()
        {
            IbfDiagnosIf node = new IbfDiagnosIf();
            Assert.AreEqual("DiagnosIf", node.ToString());
        }
    }
}
