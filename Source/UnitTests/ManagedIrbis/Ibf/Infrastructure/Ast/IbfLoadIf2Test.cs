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
    public class IbfLoadIf2Test
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfLoadIf2 _GetNode()
        {
            return new IbfLoadIf2();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfLoadIf2_Construction_1()
        {
            IbfLoadIf2 node = new IbfLoadIf2();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfLoadIf2_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfLoadIf2 node = new IbfLoadIf2();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfLoadIf2 first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfLoadIf2 second = bytes.RestoreObjectFromMemory<IbfLoadIf2>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfLoadIf2_Serialization_1()
        {
            IbfLoadIf2 node = new IbfLoadIf2();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfLoadIf2_Verify_1()
        {
            IbfLoadIf2 node = new IbfLoadIf2();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfLoadIf2_ToXml_1()
        {
            IbfLoadIf2 node = new IbfLoadIf2();
            Assert.AreEqual("<IbfLoadIf2 />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfLoadIf2_ToJson_1()
        {
            IbfLoadIf2 node = new IbfLoadIf2();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfLoadIf2_ToString_1()
        {
            IbfLoadIf2 node = new IbfLoadIf2();
            Assert.AreEqual("LoadIf2", node.ToString());
        }
    }
}
