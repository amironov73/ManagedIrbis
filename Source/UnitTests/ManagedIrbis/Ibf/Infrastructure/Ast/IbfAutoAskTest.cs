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
    public class IbfAutoAckTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfAutoAsk _GetNode()
        {
            return new IbfAutoAsk();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfAutoAck_Construction_1()
        {
            IbfAutoAsk node = new IbfAutoAsk();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfAutoAck_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfAutoAsk node = new IbfAutoAsk();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfAutoAsk first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfAutoAsk second = bytes.RestoreObjectFromMemory<IbfAutoAsk>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfAutoAck_Serialization_1()
        {
            IbfAutoAsk node = new IbfAutoAsk();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfAutoAck_Verify_1()
        {
            IbfAutoAsk node = new IbfAutoAsk();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfAutoAsk_ToXml_1()
        {
            IbfAutoAsk node = new IbfAutoAsk();
            Assert.AreEqual("<IbfAutoAsk />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfAutoAsk_ToJson_1()
        {
            IbfAutoAsk node = new IbfAutoAsk();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfAutoAsk_ToString_1()
        {
            IbfAutoAsk node = new IbfAutoAsk();
            Assert.AreEqual("AutoAsk", node.ToString());
        }
    }
}
