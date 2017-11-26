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
    public class IbfDeleteDbTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfDeleteDb _GetNode()
        {
            return new IbfDeleteDb();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfDeleteDb_Construction_1()
        {
            IbfDeleteDb node = new IbfDeleteDb();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfDeleteDb_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfDeleteDb node = new IbfDeleteDb();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfDeleteDb first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfDeleteDb second = bytes.RestoreObjectFromMemory<IbfDeleteDb>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfDeleteDb_Serialization_1()
        {
            IbfDeleteDb node = new IbfDeleteDb();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfDeleteDb_Verify_1()
        {
            IbfDeleteDb node = new IbfDeleteDb();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfDeleteDb_ToXml_1()
        {
            IbfDeleteDb node = new IbfDeleteDb();
            Assert.AreEqual("<IbfDeleteDb />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfDeleteDb_ToJson_1()
        {
            IbfDeleteDb node = new IbfDeleteDb();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfDeleteDb_ToString_1()
        {
            IbfDeleteDb node = new IbfDeleteDb();
            Assert.AreEqual("DeleteDb", node.ToString());
        }
    }
}
