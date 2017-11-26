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
    public class IbfCopyDbTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfCopyDb _GetNode()
        {
            return new IbfCopyDb();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfCopyDb_Construction_1()
        {
            IbfCopyDb node = new IbfCopyDb();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfCopyDb_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfCopyDb node = new IbfCopyDb();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfCopyDb first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfCopyDb second = bytes.RestoreObjectFromMemory<IbfCopyDb>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfCopyDb_Serialization_1()
        {
            IbfCopyDb node = new IbfCopyDb();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfCopyDb_Verify_1()
        {
            IbfCopyDb node = new IbfCopyDb();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfCopyDb_ToXml_1()
        {
            IbfCopyDb node = new IbfCopyDb();
            Assert.AreEqual("<IbfCopyDb />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfCopyDb_ToJson_1()
        {
            IbfCopyDb node = new IbfCopyDb();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfCopyDb_ToString_1()
        {
            IbfCopyDb node = new IbfCopyDb();
            Assert.AreEqual("CopyDb", node.ToString());
        }
    }
}
