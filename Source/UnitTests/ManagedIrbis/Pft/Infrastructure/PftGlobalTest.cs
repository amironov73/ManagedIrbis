using System.IO;
using AM.Runtime;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftGlobalTest
    {
        [TestMethod]
        public void PftGlobal_Construction_1()
        {
            PftGlobal node = new PftGlobal();
            Assert.AreEqual(0, node.Number);
            Assert.IsNotNull(node.Fields);
            Assert.AreEqual(0, node.Fields.Count);
        }

        [TestMethod]
        public void PftGlobal_Construction_2()
        {
            int number = 123;
            PftGlobal node = new PftGlobal(number);
            Assert.AreEqual(number, node.Number);
            Assert.IsNotNull(node.Fields);
            Assert.AreEqual(0, node.Fields.Count);
        }

        [TestMethod]
        public void PftGlobal_Construction_3()
        {
            int number = 123;
            string text = "123\n234";
            PftGlobal node = new PftGlobal(number, text);
            Assert.AreEqual(number, node.Number);
            Assert.IsNotNull(node.Fields);
            Assert.AreEqual(2, node.Fields.Count);
        }

        [TestMethod]
        public void PftGlobal_Parse_1()
        {
            PftGlobal node = new PftGlobal();
            Assert.AreSame(node, node.Parse(string.Empty));
            Assert.AreEqual(0, node.Fields.Count);
        }

        [TestMethod]
        public void PftGlobal_Parse_2()
        {
            PftGlobal node = new PftGlobal();
            Assert.AreSame(node, node.Parse("\n"));
            Assert.AreEqual(2, node.Fields.Count);
            Assert.AreEqual(string.Empty, node.Fields[0].Value);
            Assert.AreEqual(string.Empty, node.Fields[1].Value);
        }

        [TestMethod]
        public void PftGlobal_Parse_3()
        {
            string text = "text";
            PftGlobal node = new PftGlobal();
            Assert.AreSame(node, node.Parse(text));
            Assert.AreEqual(1, node.Fields.Count);
            Assert.AreEqual(text, node.Fields[0].Value);
        }

        [TestMethod]
        public void PftGlobal_Parse_4()
        {
            string text1 = "text1", text2 = "text2";
            PftGlobal node = new PftGlobal();
            Assert.AreSame(node, node.Parse(text1 + '\n' + text2));
            Assert.AreEqual(2, node.Fields.Count);
            Assert.AreEqual(text1, node.Fields[0].Value);
            Assert.AreEqual(text2, node.Fields[1].Value);
        }

        [TestMethod]
        public void PftGlobal_Parse_5()
        {
            PftGlobal node = new PftGlobal();
            Assert.AreSame(node, node.Parse("^atext"));
            Assert.AreEqual(1, node.Fields.Count);
            Assert.AreEqual("text", node.Fields[0].GetFirstSubFieldValue('a'));
        }

        [TestMethod]
        public void PftGlobal_Parse_6()
        {
            PftGlobal node = new PftGlobal();
            Assert.AreSame(node, node.Parse("^atext1^btext2"));
            Assert.AreEqual(1, node.Fields.Count);
            Assert.AreEqual("text1", node.Fields[0].GetFirstSubFieldValue('a'));
            Assert.AreEqual("text2", node.Fields[0].GetFirstSubFieldValue('b'));
        }

        [TestMethod]
        public void PftGlobal_Parse_7()
        {
            PftGlobal node = new PftGlobal();
            Assert.AreSame(node, node.Parse("^atext1^btext2\n^ctext3^dtext4"));
            Assert.AreEqual(2, node.Fields.Count);
            Assert.AreEqual("text1", node.Fields[0].GetFirstSubFieldValue('a'));
            Assert.AreEqual("text2", node.Fields[0].GetFirstSubFieldValue('b'));
            Assert.AreEqual("text3", node.Fields[1].GetFirstSubFieldValue('c'));
            Assert.AreEqual("text4", node.Fields[1].GetFirstSubFieldValue('d'));
        }

        [TestMethod]
        public void PftGlobal_Parse_8()
        {
            PftGlobal node = new PftGlobal();
            Assert.AreSame(node, node.Parse("^atext1^btext2"));
            Assert.AreEqual("text1", node.Fields[0].GetFirstSubFieldValue('a'));
            Assert.AreEqual("text2", node.Fields[0].GetFirstSubFieldValue('b'));
            Assert.AreEqual(1, node.Fields.Count);
            Assert.AreSame(node, node.Parse("^ctext3^dtext4"));
            Assert.AreEqual("text3", node.Fields[1].GetFirstSubFieldValue('c'));
            Assert.AreEqual("text4", node.Fields[1].GetFirstSubFieldValue('d'));
        }

        private void _TestSerialization
            (
                [NotNull] PftGlobal first
            )
        {
            byte[] bytes = first.SaveToMemory();
            PftGlobal second = bytes.RestoreObjectFromMemory<PftGlobal>();
            Assert.AreEqual(first.Number, second.Number);
            Assert.AreEqual(first.Fields.Count, second.Fields.Count);
            for (int i = 0; i < first.Fields.Count; i++)
            {
                Assert.AreEqual(first.Fields[i].ToText(), second.Fields[i].ToText());
            }
        }

        [TestMethod]
        public void PftGlobal_Serialization_1()
        {
            PftGlobal node = new PftGlobal();
            _TestSerialization(node);

            node.Number = 123;
            node.Parse("111\n^atext1^btext2");
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftGlobal_ToString_1()
        {
            PftGlobal node = new PftGlobal();
            Assert.AreEqual("", node.ToString());
        }

        [TestMethod]
        public void PftGlobal_ToString_2()
        {
            PftGlobal node = new PftGlobal(123);
            node.Parse("^aline1\n^bline2");
            Assert.AreEqual("123#^aline1\n123#^bline2", node.ToString().DosToUnix());
        }
    }
}
