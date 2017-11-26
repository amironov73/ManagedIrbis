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
    public class IbfSearchTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfSearch _GetNode()
        {
            return new IbfSearch();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfSearch_Construction_1()
        {
            IbfSearch node = new IbfSearch();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfSearch_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfSearch node = new IbfSearch();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfSearch first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfSearch second = bytes.RestoreObjectFromMemory<IbfSearch>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfSearch_Serialization_1()
        {
            IbfSearch node = new IbfSearch();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfSearch_Verify_1()
        {
            IbfSearch node = new IbfSearch();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfSearch_ToXml_1()
        {
            IbfSearch node = new IbfSearch();
            Assert.AreEqual("<IbfSearch />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfSearch_ToJson_1()
        {
            IbfSearch node = new IbfSearch();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfSearch_ToString_1()
        {
            IbfSearch node = new IbfSearch();
            Assert.AreEqual("Search", node.ToString());
        }
    }
}
