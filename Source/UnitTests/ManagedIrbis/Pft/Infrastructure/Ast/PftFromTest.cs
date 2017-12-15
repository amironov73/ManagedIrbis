using System.IO;

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
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
    public class PftFromTest
    {
        private void _Execute
            (
                [NotNull] MarcRecord record,
                [NotNull] PftFrom node,
                [NotNull] string name,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null)
            {
                Record = record
            };
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private MarcRecord _GetRecord()
        {
            MarcRecord record = new MarcRecord();
            record.Fields.Add(RecordField.Parse(910, "^A0^B32^C20070104^DБИНТ^E7.50^H107206G^=2^U2004/7^S20070104^!ХР"));
            record.Fields.Add(RecordField.Parse(910, "^A0^B33^C20070104^DБИНТ^E60.00^H107216G^U2004/7^S20070104^!ХР"));
            record.Fields.Add(RecordField.Parse(910, "^A0^B557^C19990924^DЧЗ^H107236G^=2^U2004/7"));
            record.Fields.Add(RecordField.Parse(910, "^A0^B558^C19990924^DЧЗ^H107246G^=2^U2004/7"));
            record.Fields.Add(RecordField.Parse(910, "^A0^B559^C19990924^H107256G^=2^U2004/7"));
            record.Fields.Add(RecordField.Parse(910, "^AU^B556^C19990924^DХР^E2400^H107226G^112^U1996/28^Y60"));
            record.Fields.Add(RecordField.Parse(910, "^AU^BЗИ-1^C20071226^DЖГ^S20140604^125^!КДИ^01^TЗИ"));

            return record;
        }

        [NotNull]
        private PftFrom _GetNode()
        {
            string name = "x";
            return new PftFrom
            {
                Variable = new PftVariableReference(name),
                Source =
                {
                    new PftGroup
                        {
                            Children =
                            { 
                                new PftV("v910^b"),
                                new PftSlash()
                            }
                        }
                },
                Where = new PftComparison
                {
                    LeftOperand = new PftVariableReference(name),
                    Operation = ":",
                    RightOperand = new PftUnconditionalLiteral("55")
                },
                Select =
                {
                    new PftUnconditionalLiteral("=> "),
                    new PftVariableReference(name)
                },
                Order =
                {
                    new PftVariableReference(name)
                }
            };
        }

        [TestMethod]
        public void PftFrom_Construction_1()
        {
            PftFrom node = new PftFrom();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNull(node.Variable);
            Assert.IsNotNull(node.Source);
            Assert.AreEqual(0, node.Source.Count);
            Assert.IsNull(node.Where);
            Assert.IsNotNull(node.Select);
            Assert.AreEqual(0, node.Select.Count);
            Assert.IsNotNull(node.Order);
            Assert.AreEqual(0, node.Order.Count);
        }

        [TestMethod]
        public void PftFrom_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.From, 1, 1, "from");
            PftFrom node = new PftFrom(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNull(node.Variable);
            Assert.IsNotNull(node.Source);
            Assert.AreEqual(0, node.Source.Count);
            Assert.IsNull(node.Where);
            Assert.IsNotNull(node.Select);
            Assert.AreEqual(0, node.Select.Count);
            Assert.IsNotNull(node.Order);
            Assert.AreEqual(0, node.Order.Count);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftFrom_Clone_1()
        {
            PftFrom first = new PftFrom();
            PftFrom second = (PftFrom) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFrom_Clone_2()
        {
            PftFrom first = _GetNode();
            PftFrom second = (PftFrom)first.Clone();
            Assert.IsNotNull(second);

            // TODO FIX THIS!
            //PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFrom_Execute_1()
        {
            MarcRecord record = _GetRecord();
            PftFrom node = _GetNode();
            string name = "x";
            _Execute(record, node, name, "=> 556\n=> 557\n=> 558\n=> 559");
        }

        [TestMethod]
        public void PftFrom_GetNodeInfo_1()
        {
            PftFrom node = _GetNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("From", info.Name);
        }

        [TestMethod]
        public void PftFrom_PrettyPrint_1()
        {
            PftFrom node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\nfrom $x in (v910^b / )\nwhere $x:\'55\'\nselect \'=> \', $x\norder $x\n\nend\n", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftFrom_PrettyPrint_2()
        {
            PftFrom node = _GetNode();
            node.Source.Add(new PftUnconditionalLiteral("Hello"));
            node.Order.Add(new PftUnconditionalLiteral("Garbage"));
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\nfrom $x in (v910^b / ), 'Hello'\nwhere $x:\'55\'\nselect \'=> \', $x\norder $x, 'Garbage'\n\nend\n", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                [NotNull] PftFrom first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftFrom second = (PftFrom) PftSerializer.Deserialize(reader);
            Assert.IsNotNull(second);

            // TODO FIX THIS!
            //PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFrom_Serialization_1()
        {
            PftFrom node = new PftFrom();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftFrom_ToString_1()
        {
            PftFrom node = new PftFrom();
            Assert.AreEqual("from  in  select  end", node.ToString());
        }

        [TestMethod]
        public void PftFrom_ToString_2()
        {
            PftFrom node = _GetNode();
            Assert.AreEqual("from $x in (v910^b /) where $x:'55' select '=> ' $x order $x end", node.ToString());
        }
    }
}
