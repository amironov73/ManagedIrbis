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
    public class IbfLoadIf1Test
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfLoadIf1 _GetNode()
        {
            return new IbfLoadIf1();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfLoadIf1_Construction_1()
        {
            IbfLoadIf1 node = new IbfLoadIf1();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfLoadIf1_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfLoadIf1 node = new IbfLoadIf1();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfLoadIf1 first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfLoadIf1 second = bytes.RestoreObjectFromMemory<IbfLoadIf1>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfLoadIf1_Serialization_1()
        {
            IbfLoadIf1 node = new IbfLoadIf1();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfLoadIf1_Verify_1()
        {
            IbfLoadIf1 node = new IbfLoadIf1();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfLoadIf1_ToXml_1()
        {
            IbfLoadIf1 node = new IbfLoadIf1();
            Assert.AreEqual("<IbfLoadIf1 />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfLoadIf1_ToJson_1()
        {
            IbfLoadIf1 node = new IbfLoadIf1();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfLoadIf1_ToString_1()
        {
            IbfLoadIf1 node = new IbfLoadIf1();
            Assert.AreEqual("LoadIf1", node.ToString());
        }
    }
}
