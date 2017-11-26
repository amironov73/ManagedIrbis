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
    public class IbfUnlockRecordTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfUnlockRecord _GetNode()
        {
            return new IbfUnlockRecord();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfUnlockRecord_Construction_1()
        {
            IbfUnlockRecord node = new IbfUnlockRecord();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfUnlockRecord_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfUnlockRecord node = new IbfUnlockRecord();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfUnlockRecord first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfUnlockRecord second = bytes.RestoreObjectFromMemory<IbfUnlockRecord>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfUnlockRecord_Serialization_1()
        {
            IbfUnlockRecord node = new IbfUnlockRecord();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfUnlockRecord_Verify_1()
        {
            IbfUnlockRecord node = new IbfUnlockRecord();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfUnlockRecord_ToXml_1()
        {
            IbfUnlockRecord node = new IbfUnlockRecord();
            Assert.AreEqual("<IbfUnlockRecord />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfUnlockRecord_ToJson_1()
        {
            IbfUnlockRecord node = new IbfUnlockRecord();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfUnlockRecord_ToString_1()
        {
            IbfUnlockRecord node = new IbfUnlockRecord();
            Assert.AreEqual("UnlockRecord", node.ToString());
        }
    }
}
