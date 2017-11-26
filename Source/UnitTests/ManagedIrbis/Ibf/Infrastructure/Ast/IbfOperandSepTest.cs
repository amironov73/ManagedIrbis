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
    public class IbfOperandSepTest
        : CommonIbfAstTest
    {
        [NotNull]
        private IbfOperandSep _GetNode()
        {
            return new IbfOperandSep();
        }

        [TestMethod]
        [Description("Создание объекта")]
        public void IbfOperandSep_Construction_1()
        {
            IbfOperandSep node = new IbfOperandSep();
            Assert.IsNotNull(node);
        }

        [TestMethod]
        [Description("Исполнение")]
        public void IbfOperandSep_Execute_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IbfContext context = new IbfContext(provider);
                IbfOperandSep node = new IbfOperandSep();
                node.Execute(context);
            }
        }

        private void _TestSerialization
            (
                [NotNull] IbfOperandSep first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IbfOperandSep second = bytes.RestoreObjectFromMemory<IbfOperandSep>();
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void IbfOperandSep_Serialization_1()
        {
            IbfOperandSep node = new IbfOperandSep();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        [Description("Верификация")]
        public void IbfOperandSep_Verify_1()
        {
            IbfOperandSep node = new IbfOperandSep();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void IbfOperandSep_ToXml_1()
        {
            IbfOperandSep node = new IbfOperandSep();
            Assert.AreEqual("<IbfOperandSep />", XmlUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfOperandSep_ToJson_1()
        {
            IbfOperandSep node = new IbfOperandSep();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(node));
        }

        [TestMethod]
        public void IbfOperandSep_ToString_1()
        {
            IbfOperandSep node = new IbfOperandSep();
            Assert.AreEqual("OperandSep", node.ToString());
        }
    }
}
