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
    public class IbfUnlockRecordAllTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfUnlockRecordAll _GetNode()
        {
            return new IbfUnlockRecordAll();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfUnlockRecordAll_Construction_1()
        {
            IbfUnlockRecordAll node = new IbfUnlockRecordAll();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfUnlockRecordAll_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfUnlockRecordAll node = new IbfUnlockRecordAll();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfUnlockRecordAll first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfUnlockRecordAll second = bytes.RestoreObjectFromMemory<IbfUnlockRecordAll>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfUnlockRecordAll_Serialization_1()
        {
            IbfUnlockRecordAll node = new IbfUnlockRecordAll();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfUnlockRecordAll_Verify_1()
        {
            IbfUnlockRecordAll node = new IbfUnlockRecordAll();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfUnlockRecordAll_ToXml_1()
        {
            IbfUnlockRecordAll node = new IbfUnlockRecordAll();
            Assert.AreEqual("<IbfUnlockRecordAll />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfUnlockRecordAll_ToJson_1()
        {
            IbfUnlockRecordAll node = new IbfUnlockRecordAll();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfUnlockRecordAll_ToString_1()
        {
            IbfUnlockRecordAll node = new IbfUnlockRecordAll();
            Assert.AreEqual("UnlockRecordAll", node.ToString());
        }
    }
}
