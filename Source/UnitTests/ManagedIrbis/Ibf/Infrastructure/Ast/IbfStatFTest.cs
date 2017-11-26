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
    public class IbfStatFTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfStatF _GetNode()
        {
            return new IbfStatF();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfStatF_Construction_1()
        {
            IbfStatF node = new IbfStatF();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfStatF_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfStatF node = new IbfStatF();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfStatF first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfStatF second = bytes.RestoreObjectFromMemory<IbfStatF>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfStatF_Serialization_1()
        {
            IbfStatF node = new IbfStatF();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfStatF_Verify_1()
        {
            IbfStatF node = new IbfStatF();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfStatF_ToXml_1()
        {
            IbfStatF node = new IbfStatF();
            Assert.AreEqual("<IbfStatF />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfStatF_ToJson_1()
        {
            IbfStatF node = new IbfStatF();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfStatF_ToString_1()
        {
            IbfStatF node = new IbfStatF();
            Assert.AreEqual("StatF", node.ToString());
        }
    }
}
