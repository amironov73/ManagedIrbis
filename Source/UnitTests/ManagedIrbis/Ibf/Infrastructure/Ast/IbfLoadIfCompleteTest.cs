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
    public class IbfLoadIfCompleteTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfLoadIfComplete _GetNode()
        {
            return new IbfLoadIfComplete();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfLoadIfComplete_Construction_1()
        {
            IbfLoadIfComplete node = new IbfLoadIfComplete();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfLoadIfComplete_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfLoadIfComplete node = new IbfLoadIfComplete();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfLoadIfComplete first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfLoadIfComplete second = bytes.RestoreObjectFromMemory<IbfLoadIfComplete>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfLoadIfComplete_Serialization_1()
        {
            IbfLoadIfComplete node = new IbfLoadIfComplete();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfLoadIfComplete_Verify_1()
        {
            IbfLoadIfComplete node = new IbfLoadIfComplete();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfLoadIfComplete_ToXml_1()
        {
            IbfLoadIfComplete node = new IbfLoadIfComplete();
            Assert.AreEqual("<IbfLoadIfComplete />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfLoadIfComplete_ToJson_1()
        {
            IbfLoadIfComplete node = new IbfLoadIfComplete();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfLoadIfComplete_ToString_1()
        {
            IbfLoadIfComplete node = new IbfLoadIfComplete();
            Assert.AreEqual("LoadIfComplete", node.ToString());
        }
    }
}
