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
    public class IbfExitTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfExit _GetNode()
        {
            return new IbfExit();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfExit_Construction_1()
        {
            IbfExit node = new IbfExit();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfExit_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfExit node = new IbfExit();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfExit first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfExit second = bytes.RestoreObjectFromMemory<IbfExit>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfExit_Serialization_1()
        {
            IbfExit node = new IbfExit();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfExit_Verify_1()
        {
            IbfExit node = new IbfExit();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfExit_ToXml_1()
        {
            IbfExit node = new IbfExit();
            Assert.AreEqual("<IbfExit />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfExit_ToJson_1()
        {
            IbfExit node = new IbfExit();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfExit_ToString_1()
        {
            IbfExit node = new IbfExit();
            Assert.AreEqual("Exit", node.ToString());
        }
    }
}
