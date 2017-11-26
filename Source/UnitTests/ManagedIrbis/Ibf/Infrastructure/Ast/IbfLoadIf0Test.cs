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
    public class IbfLoadIf0Test
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfLoadIf0 _GetNode()
        {
            return new IbfLoadIf0();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfLoadIf0_Construction_1()
        {
            IbfLoadIf0 node = new IbfLoadIf0();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfLoadIf0_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfLoadIf0 node = new IbfLoadIf0();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfLoadIf0 first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfLoadIf0 second = bytes.RestoreObjectFromMemory<IbfLoadIf0>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfLoadIf0_Serialization_1()
        {
            IbfLoadIf0 node = new IbfLoadIf0();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfLoadIf0_Verify_1()
        {
            IbfLoadIf0 node = new IbfLoadIf0();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfLoadIf0_ToXml_1()
        {
            IbfLoadIf0 node = new IbfLoadIf0();
            Assert.AreEqual("<IbfLoadIf0 />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfLoadIf0_ToJson_1()
        {
            IbfLoadIf0 node = new IbfLoadIf0();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfLoadIf0_ToString_1()
        {
            IbfLoadIf0 node = new IbfLoadIf0();
            Assert.AreEqual("LoadIf0", node.ToString());
        }
    }
}
