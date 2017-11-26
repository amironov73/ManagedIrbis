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
    public class IbfUnlockDbTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfUnlockDb _GetNode()
        {
            return new IbfUnlockDb();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfUnlockDb_Construction_1()
        {
            IbfUnlockDb node = new IbfUnlockDb();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfUnlockDb_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfUnlockDb node = new IbfUnlockDb();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfUnlockDb first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfUnlockDb second = bytes.RestoreObjectFromMemory<IbfUnlockDb>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfUnlockDb_Serialization_1()
        {
            IbfUnlockDb node = new IbfUnlockDb();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfUnlockDb_Verify_1()
        {
            IbfUnlockDb node = new IbfUnlockDb();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfUnlockDb_ToXml_1()
        {
            IbfUnlockDb node = new IbfUnlockDb();
            Assert.AreEqual("<IbfUnlockDb />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfUnlockDb_ToJson_1()
        {
            IbfUnlockDb node = new IbfUnlockDb();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfUnlockDb_ToString_1()
        {
            IbfUnlockDb node = new IbfUnlockDb();
            Assert.AreEqual("UnlockDb", node.ToString());
        }
    }
}
