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
    public class IbfSilenceTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfSilence _GetNode()
        {
            return new IbfSilence();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfSilence_Construction_1()
        {
            IbfSilence node = new IbfSilence();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfSilence_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfSilence node = new IbfSilence();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfSilence first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfSilence second = bytes.RestoreObjectFromMemory<IbfSilence>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfSilence_Serialization_1()
        {
            IbfSilence node = new IbfSilence();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfSilence_Verify_1()
        {
            IbfSilence node = new IbfSilence();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfSilence_ToXml_1()
        {
            IbfSilence node = new IbfSilence();
            Assert.AreEqual("<IbfSilence />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfSilence_ToJson_1()
        {
            IbfSilence node = new IbfSilence();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfSilence_ToString_1()
        {
            IbfSilence node = new IbfSilence();
            Assert.AreEqual("Silence", node.ToString());
        }
    }
}
