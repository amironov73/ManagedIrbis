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
    public class IbfActualIfTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfActualIf _GetNode()
        {
            return new IbfActualIf();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfActualIf_Construction_1()
        {
            IbfActualIf node = new IbfActualIf();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfActualIf_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfActualIf node = new IbfActualIf();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfActualIf first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfActualIf second = bytes.RestoreObjectFromMemory<IbfActualIf>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfActualIf_Serialization_1()
        {
            IbfActualIf node = new IbfActualIf();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfActualIf_Verify_1()
        {
            IbfActualIf node = new IbfActualIf();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfActualIf_ToXml_1()
        {
            IbfActualIf node = new IbfActualIf();
            Assert.AreEqual("<IbfActualIf />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfActualIf_ToJson_1()
        {
            IbfActualIf node = new IbfActualIf();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfActualIf_ToString_1()
        {
            IbfActualIf node = new IbfActualIf();
            Assert.AreEqual("ActualIf", node.ToString());
        }
    }
}
