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
    public class IbfLockDbTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfLockDb _GetNode()
        {
            return new IbfLockDb();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfLockDb_Construction_1()
        {
            IbfLockDb node = new IbfLockDb();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfLockDb_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfLockDb node = new IbfLockDb();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfLockDb first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfLockDb second = bytes.RestoreObjectFromMemory<IbfLockDb>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfLockDb_Serialization_1()
        {
            IbfLockDb node = new IbfLockDb();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfLockDb_Verify_1()
        {
            IbfLockDb node = new IbfLockDb();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfLockDb_ToXml_1()
        {
            IbfLockDb node = new IbfLockDb();
            Assert.AreEqual("<IbfLockDb />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfLockDb_ToJson_1()
        {
            IbfLockDb node = new IbfLockDb();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfLockDb_ToString_1()
        {
            IbfLockDb node = new IbfLockDb();
            Assert.AreEqual("LockDb", node.ToString());
        }
    }
}
