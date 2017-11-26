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
    public class IbfGlobalTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfGlobal _GetNode()
        {
            return new IbfGlobal();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfGlobal_Construction_1()
        {
            IbfGlobal node = new IbfGlobal();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfGlobal_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfGlobal node = new IbfGlobal();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfGlobal first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfGlobal second = bytes.RestoreObjectFromMemory<IbfGlobal>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfGlobal_Serialization_1()
        {
            IbfGlobal node = new IbfGlobal();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfGlobal_Verify_1()
        {
            IbfGlobal node = new IbfGlobal();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfGlobal_ToXml_1()
        {
            IbfGlobal node = new IbfGlobal();
            Assert.AreEqual("<IbfGlobal />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfGlobal_ToJson_1()
        {
            IbfGlobal node = new IbfGlobal();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfGlobal_ToString_1()
        {
            IbfGlobal node = new IbfGlobal();
            Assert.AreEqual("Global", node.ToString());
        }
    }
}
