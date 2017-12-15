using System.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftAllTest
    {
        private void _Execute
            (
                [NotNull] MarcRecord record,
                [NotNull] PftAll node,
                bool expected
            )
        {
            PftContext context = new PftContext(null)
            {
                Record = record
            };
            node.Execute(context);
            bool actual = node.Value;
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();

            RecordField field = new RecordField(700);
            field.AddSubField('a', "Иванов");
            field.AddSubField('b', "И. И.");
            result.Fields.Add(field);

            field = new RecordField(701);
            field.AddSubField('a', "Петров");
            field.AddSubField('b', "П. П.");
            result.Fields.Add(field);

            field = new RecordField(200);
            field.AddSubField('a', "Заглавие");
            field.AddSubField('e', "подзаголовочное");
            field.AddSubField('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new RecordField(300, "Первое примечание");
            result.Fields.Add(field);
            field = new RecordField(300, "Второе примечание");
            result.Fields.Add(field);
            field = new RecordField(300, "Третье примечание");
            result.Fields.Add(field);

            return result;
        }

        [NotNull]
        private PftAll _GetNode()
        {
            return new PftAll
            {
                InnerCondition = new PftComparison
                {
                    LeftOperand = new PftV("v300"),
                    Operation = ":",
                    RightOperand = new PftUnconditionalLiteral("примечание")
                }
            };
        }

        [TestMethod]
        public void PftAll_Construction_1()
        {
            PftAll node = new PftAll();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
        }

        [TestMethod]
        public void PftAll_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.All, 1, 1, "all");
            PftAll node = new PftAll(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftAll_Clone_1()
        {
            PftAll first = new PftAll();
            PftAll second = (PftAll) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftAll_Clone_2()
        {
            PftAll first = _GetNode();
            PftAll second = (PftAll)first.Clone();
            Assert.IsNotNull(second);

            // TODO FIX THIS!
            //PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftAll_Execute_1()
        {
            MarcRecord record = _GetRecord();
            PftAll node = _GetNode();
            _Execute(record, node, true);
        }

        [TestMethod]
        public void PftAll_Execute_2()
        {
            MarcRecord record = _GetRecord();
            PftAll node = _GetNode();
            PftComparison comparison = (PftComparison) node.InnerCondition;
            Assert.IsNotNull(comparison);
            comparison.RightOperand = new PftUnconditionalLiteral("noSuchWord");
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftAll_Execute_2a()
        {
            MarcRecord record = _GetRecord();
            PftAll node = _GetNode();
            PftComparison comparison = (PftComparison) node.InnerCondition;
            Assert.IsNotNull(comparison);
            comparison.LeftOperand = new PftV("v444");
            _Execute(record, node, false);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void PftAll_Execute_3()
        {
            MarcRecord record = _GetRecord();
            PftAll node = _GetNode();
            PftContext context = new PftContext(null)
            {
                CurrentGroup = new PftGroup()
            };
            node.Execute(context);
        }

        [TestMethod]
        public void PftAll_Execute_4()
        {
            MarcRecord record = _GetRecord();
            PftAll node = new PftAll
            {
                InnerCondition = new PftComparison
                {
                    LeftOperand = new PftUnconditionalLiteral("1"),
                    Operation = "=",
                    RightOperand = new PftUnconditionalLiteral("1")
                }
            };
            _Execute(record, node, false);
        }
        [TestMethod]
        public void PftAll_GetNodeInfo_1()
        {
            PftAll node = _GetNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("All", info.Name);
        }

        [TestMethod]
        public void PftAll_PrettyPrint_1()
        {
            PftAll node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("all(v300:'примечание')", printer.ToString());
        }

        private void _TestSerialization
            (
                [NotNull] PftAll first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftAll second = (PftAll) PftSerializer.Deserialize(reader);
            Assert.IsNotNull(second);

            // TODO FIX THIS!
            //PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftAll_Serialization_1()
        {
            PftAll node = new PftAll();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftAll_ToString_1()
        {
            PftAll node = new PftAll();
            Assert.AreEqual("all()", node.ToString());
        }

        [TestMethod]
        public void PftAll_ToString_2()
        {
            PftAll node = _GetNode();
            Assert.AreEqual("all(v300:'примечание')", node.ToString());
        }
    }
}
