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
    public class PftAnyTest
    {
        private void _Execute
            (
                [NotNull] MarcRecord record,
                [NotNull] PftAny node,
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
        private PftAny _GetNode()
        {
            return new PftAny
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
        public void PftAny_Construction_1()
        {
            PftAny node = new PftAny();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
        }

        [TestMethod]
        public void PftAny_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Any, 1, 1, "any");
            PftAny node = new PftAny(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftAny_Clone_1()
        {
            PftAny first = new PftAny();
            PftAny second = (PftAny) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftAny_Clone_2()
        {
            PftAny first = _GetNode();
            PftAny second = (PftAny)first.Clone();
            Assert.IsNotNull(second);

            // TODO FIX THIS!
            //PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftAny_Execute_1()
        {
            MarcRecord record = _GetRecord();
            PftAny node = _GetNode();
            _Execute(record, node, true);
        }

        [TestMethod]
        public void PftAny_Execute_2()
        {
            MarcRecord record = _GetRecord();
            PftAny node = _GetNode();
            PftComparison comparison = (PftComparison) node.InnerCondition;
            Assert.IsNotNull(comparison);
            comparison.RightOperand = new PftUnconditionalLiteral("noSuchWord");
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftAny_Execute_2a()
        {
            MarcRecord record = _GetRecord();
            PftAny node = _GetNode();
            PftComparison comparison = (PftComparison) node.InnerCondition;
            Assert.IsNotNull(comparison);
            comparison.LeftOperand = new PftV("v444");
            _Execute(record, node, false);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void PftAny_Execute_3()
        {
            MarcRecord record = _GetRecord();
            PftAny node = _GetNode();
            PftContext context = new PftContext(null)
            {
                CurrentGroup = new PftGroup()
            };
            node.Execute(context);
        }

        [TestMethod]
        public void PftAny_Execute_4()
        {
            MarcRecord record = _GetRecord();
            PftAny node = new PftAny
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
        public void PftAny_GetNodeInfo_1()
        {
            PftAny node = _GetNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("Any", info.Name);
        }

        [TestMethod]
        public void PftAny_PrettyPrint_1()
        {
            PftAny node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("any(v300:'примечание')", printer.ToString());
        }

        private void _TestSerialization
            (
                [NotNull] PftAny first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftAny second = (PftAny) PftSerializer.Deserialize(reader);
            Assert.IsNotNull(second);

            // TODO FIX THIS!
            //PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftAny_Serialization_1()
        {
            PftAny node = new PftAny();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftAny_ToString_1()
        {
            PftAny node = new PftAny();
            Assert.AreEqual("any()", node.ToString());
        }

        [TestMethod]
        public void PftAny_ToString_2()
        {
            PftAny node = _GetNode();
            Assert.AreEqual("any(v300:'примечание')", node.ToString());
        }
    }
}
