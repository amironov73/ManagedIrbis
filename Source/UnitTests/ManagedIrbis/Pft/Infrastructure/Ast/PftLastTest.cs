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
    public class PftLastTest
    {
        private void _Execute
            (
                [NotNull] MarcRecord record,
                [NotNull] PftLast node,
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
        private PftLast _GetNode()
        {
            return new PftLast
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
        public void PftLast_Construction_1()
        {
            PftLast node = new PftLast();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
        }

        [TestMethod]
        public void PftLast_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Last, 1, 1, "last");
            PftLast node = new PftLast(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftLast_Clone_1()
        {
            PftLast first = new PftLast();
            PftLast second = (PftLast) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftLast_Clone_2()
        {
            PftLast first = _GetNode();
            PftLast second = (PftLast)first.Clone();
            Assert.IsNotNull(second);

            // TODO FIX THIS!
            //PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftLast_Execute_1()
        {
            MarcRecord record = _GetRecord();
            PftLast node = _GetNode();
            _Execute(record, node, 3);
        }

        [TestMethod]
        public void PftLast_Execute_2()
        {
            MarcRecord record = _GetRecord();
            PftLast node = _GetNode();
            PftComparison comparison = (PftComparison) node.InnerCondition;
            Assert.IsNotNull(comparison);
            comparison.RightOperand = new PftUnconditionalLiteral("noSuchWord");
            _Execute(record, node, 0);
        }

        [TestMethod]
        public void PftLast_Execute_2a()
        {
            MarcRecord record = _GetRecord();
            PftLast node = _GetNode();
            PftComparison comparison = (PftComparison) node.InnerCondition;
            Assert.IsNotNull(comparison);
            comparison.RightOperand = new PftV("v444");
            _Execute(record, node, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void PftLast_Execute_3()
        {
            MarcRecord record = _GetRecord();
            PftLast node = _GetNode();
            PftContext context = new PftContext(null)
            {
                CurrentGroup = new PftGroup()
            };
            node.Execute(context);
        }

        [TestMethod]
        public void PftLast_Execute_4()
        {
            MarcRecord record = _GetRecord();
            PftLast node = new PftLast
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
        public void PftLast_GetNodeInfo_1()
        {
            PftLast node = _GetNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("Last", info.Name);
        }

        [TestMethod]
        public void PftLast_PrettyPrint_1()
        {
            PftLast node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("last(v300:'примечание')", printer.ToString());
        }

        private void _TestSerialization
            (
                [NotNull] PftLast first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftLast second = (PftLast) PftSerializer.Deserialize(reader);
            Assert.IsNotNull(second);

            // TODO FIX THIS!
            //PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftLast_Serialization_1()
        {
            PftLast node = new PftLast();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftLast_ToString_1()
        {
            PftLast node = new PftLast();
            Assert.AreEqual("last()", node.ToString());
        }

        [TestMethod]
        public void PftLast_ToString_2()
        {
            PftLast node = _GetNode();
            Assert.AreEqual("last(v300:'примечание')", node.ToString());
        }
    }
}

