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
    public class PftWhileTest
    {
        private void _Execute
            (
                [NotNull] MarcRecord record,
                [NotNull] PftProgram node,
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
        private PftWhile _GetNode()
        {
            PftProgram program = _GetProgram();
            return (PftWhile) program.Children[1];
        }

        [NotNull]
        private PftProgram _GetProgram()
        {
            string name = "x";
            PftProgram result = new PftProgram();
            result.Children.Add(new PftAssignment
            {
                IsNumeric = true,
                Name = name,
                Children =
                {
                    new PftNumericLiteral(0)
                }
            });
            PftWhile node = new PftWhile
            {
                Condition = new PftComparison
                {
                    LeftOperand = new PftVariableReference(name),
                    Operation = "<",
                    RightOperand = new PftNumericLiteral(10)
                },
                Body =
                {
                    new PftVariableReference(name),
                    new PftUnconditionalLiteral(" => Прикольно!"),
                    new PftSlash(),
                    new PftAssignment
                    {
                        IsNumeric = true,
                        Name = name,
                        Children =
                        {
                            new PftNumericExpression
                            {
                                LeftOperand = new PftVariableReference(name),
                                Operation = "+",
                                RightOperand = new PftNumericLiteral(1)
                            }
                        }
                    }
                }
            };
            result.Children.Add(node);

            return result;
        }

        [TestMethod]
        public void PftWhile_Construction_1()
        {
            PftWhile node = new PftWhile();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNull(node.Condition);
            Assert.IsNotNull(node.Body);
            Assert.AreEqual(0, node.Body.Count);
        }

        [TestMethod]
        public void PftWhile_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.While, 1, 1, "while");
            PftWhile node = new PftWhile(token);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNull(node.Condition);
            Assert.IsNotNull(node.Body);
            Assert.AreEqual(0, node.Body.Count);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftWhile_Clone_1()
        {
            PftWhile first = new PftWhile();
            PftWhile second = (PftWhile) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftWhile_Clone_2()
        {
            PftWhile first = _GetNode();
            PftWhile second = (PftWhile)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftWhile_Compile_1()
        {
            PftProgram program = _GetProgram();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftWhile_Execute_1()
        {
            MarcRecord record = _GetRecord();
            PftProgram program = _GetProgram();
            _Execute
                (
                    record,
                    program,
                    "0 => Прикольно!\n1 => Прикольно!\n2 => Прикольно!\n3 => Прикольно!\n4 => Прикольно!\n5 => Прикольно!\n6 => Прикольно!\n7 => Прикольно!\n8 => Прикольно!\n9 => Прикольно!\n"
                );
        }

        [TestMethod]
        public void PftWhile_Execute_2()
        {
            MarcRecord record = _GetRecord();
            PftProgram program = _GetProgram();
            PftWhile node = (PftWhile) program.Children[1];
            node.Body.Add(new PftBreak());
            _Execute
                (
                    record,
                    program,
                    "0 => Прикольно!\n"
                );
        }

        [TestMethod]
        public void PftWhile_GetNodeInfo_1()
        {
            PftWhile node = _GetNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("While", info.Name);
        }

        [TestMethod]
        public void PftWhile_PrettyPrint_1()
        {
            PftWhile node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\nwhile $x<10\ndo\n  $x\' => Прикольно!\' / $x=$x + 1;\nend /* while $x<10\n", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                [NotNull] PftWhile first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftWhile second = (PftWhile) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftWhile_Serialization_1()
        {
            PftWhile node = new PftWhile();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftWhile_ToString_1()
        {
            PftWhile node = new PftWhile();
            Assert.AreEqual("while  do end", node.ToString());
        }

        [TestMethod]
        public void PftWhile_ToString_2()
        {
            PftWhile node = _GetNode();
            Assert.AreEqual("while $x<10 do $x ' => Прикольно!' / $x=$x+1; end", node.ToString());
        }
    }
}
