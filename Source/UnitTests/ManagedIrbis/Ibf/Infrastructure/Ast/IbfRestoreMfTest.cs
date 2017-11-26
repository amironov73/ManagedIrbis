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
    public class IbfRestoreMfTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfRestoreMf _GetNode()
        {
            return new IbfRestoreMf();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfRestoreMf_Construction_1()
        {
            IbfRestoreMf node = new IbfRestoreMf();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfRestoreMf_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfRestoreMf node = new IbfRestoreMf();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfRestoreMf first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfRestoreMf second = bytes.RestoreObjectFromMemory<IbfRestoreMf>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfRestoreMf_Serialization_1()
        {
            IbfRestoreMf node = new IbfRestoreMf();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfRestoreMf_Verify_1()
        {
            IbfRestoreMf node = new IbfRestoreMf();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfRestoreMf_ToXml_1()
        {
            IbfRestoreMf node = new IbfRestoreMf();
            Assert.AreEqual("<IbfRestoreMf />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfRestoreMf_ToJson_1()
        {
            IbfRestoreMf node = new IbfRestoreMf();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfRestoreMf_ToString_1()
        {
            IbfRestoreMf node = new IbfRestoreMf();
            Assert.AreEqual("RestoreMf", node.ToString());
        }
    }
}
