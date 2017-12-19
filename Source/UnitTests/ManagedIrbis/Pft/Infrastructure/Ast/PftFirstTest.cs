using System;
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
    public class PftFirstTest
    {
        private void _Execute
            (
                [NotNull] MarcRecord record,
                [NotNull] PftFirst node,
                double expected
            )
        {
            PftContext context = new PftContext(null)
            {
                Record = record
            };
            node.Execute(context);
            double actual = node.Value;
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
        private PftFirst _GetNode()
        {
            return new PftFirst
            {
                InnerCondition = new PftComparison
                {
                    LeftOperand = new PftV("v300"),
                    Operation = ":",
                    RightOperand = new PftUnconditionalLiteral("Третье")
                }
            };
        }

        [TestMethod]
        public void PftFirst_Construction_1()
        {
            PftFirst node = new PftFirst();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
        }

        [TestMethod]
        public void PftFirst_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.First, 1, 1, "first");
            PftFirst node = new PftFirst(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftFirst_Clone_1()
        {
            PftFirst first = new PftFirst();
            PftFirst second = (PftFirst) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFirst_Clone_2()
        {
            PftFirst first = _GetNode();
            PftFirst second = (PftFirst)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFirst_Execute_1()
        {
            MarcRecord record = _GetRecord();
            PftFirst node = _GetNode();
            _Execute(record, node, 3);
        }

        [TestMethod]
        public void PftFirst_Execute_2()
        {
            MarcRecord record = _GetRecord();
            PftFirst node = _GetNode();
            PftComparison comparison = (PftComparison) node.InnerCondition;
            Assert.IsNotNull(comparison);
            comparison.RightOperand = new PftUnconditionalLiteral("noSuchWord");
            _Execute(record, node, 0);
        }

        [TestMethod]
        public void PftFirst_Execute_2a()
        {
            MarcRecord record = _GetRecord();
            PftFirst node = _GetNode();
            PftComparison comparison = (PftComparison) node.InnerCondition;
            Assert.IsNotNull(comparison);
            comparison.LeftOperand = new PftV("v444");
            _Execute(record, node, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void PftFirst_Execute_3()
        {
            MarcRecord record = _GetRecord();
            PftFirst node = _GetNode();
            PftContext context = new PftContext(null)
            {
                CurrentGroup = new PftGroup()
            };
            node.Execute(context);
        }

        [TestMethod]
        public void PftFirst_Execute_4()
        {
            MarcRecord record = _GetRecord();
            PftFirst node = new PftFirst
            {
                InnerCondition = new PftComparison
                {
                    LeftOperand = new PftUnconditionalLiteral("1"),
                    Operation = "=",
                    RightOperand = new PftUnconditionalLiteral("1")
                }
            };
            _Execute(record, node, 0);
        }

        [TestMethod]
        public void PftFirst_GetNodeInfo_1()
        {
            PftFirst node = _GetNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("First", info.Name);
        }

        [TestMethod]
        public void PftFirst_PrettyPrint_1()
        {
            PftFirst node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("first(v300:'Третье')", printer.ToString());
        }

        private void _TestSerialization
            (
                [NotNull] PftFirst first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftFirst second = (PftFirst) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFirst_Serialization_1()
        {
            PftFirst node = new PftFirst();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftFirst_ToString_1()
        {
            PftFirst node = new PftFirst();
            Assert.AreEqual("first()", node.ToString());
        }

        [TestMethod]
        public void PftFirst_ToString_2()
        {
            PftFirst node = _GetNode();
            Assert.AreEqual("first(v300:'Третье')", node.ToString());
        }
    }
}

