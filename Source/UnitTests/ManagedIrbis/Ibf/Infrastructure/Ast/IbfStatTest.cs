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
    public class IbfStatTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfStat _GetNode()
        {
            return new IbfStat();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfStat_Construction_1()
        {
            IbfStat node = new IbfStat();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfStat_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfStat node = new IbfStat();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfStat first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfStat second = bytes.RestoreObjectFromMemory<IbfStat>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfStat_Serialization_1()
        {
            IbfStat node = new IbfStat();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfStat_Verify_1()
        {
            IbfStat node = new IbfStat();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfStat_ToXml_1()
        {
            IbfStat node = new IbfStat();
            Assert.AreEqual("<IbfStat />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfStat_ToJson_1()
        {
            IbfStat node = new IbfStat();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfStat_ToString_1()
        {
            IbfStat node = new IbfStat();
            Assert.AreEqual("Stat", node.ToString());
        }
    }
}
