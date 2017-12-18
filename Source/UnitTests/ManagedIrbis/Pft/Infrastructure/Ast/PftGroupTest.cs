using System.IO;

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

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftGroupTest
    {
        private void _Execute
            (
                [NotNull] MarcRecord record,
                [NotNull] PftGroup node,
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
        private PftGroup _GetNode()
        {
            return new PftGroup
            {
                Children =
                {
                    new PftUnconditionalLiteral("=> "),
                    new PftV("v910^b"),
                    new PftComma(),
                    new PftSlash()
                }
            };
        }

        [TestMethod]
        public void PftGroup_Construction_1()
        {
            PftGroup node = new PftGroup();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(0, node.Children.Count);
        }

        [TestMethod]
        public void PftGroup_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.LeftParenthesis, 1, 1, "(");
            PftGroup node = new PftGroup(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftGroup_Clone_1()
        {
            PftGroup first = new PftGroup();
            PftGroup second = (PftGroup) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftGroup_Clone_2()
        {
            PftGroup first = _GetNode();
            PftGroup second = (PftGroup)first.Clone();
            Assert.IsNotNull(second);

            // TODO FIX THIS!
            //PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftGroup_Compile_1()
        {
            PftGroup node = _GetNode();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftGroup_Execute_1()
        {
            MarcRecord record = _GetRecord();
            PftGroup node = _GetNode();
            _Execute(record, node, "=> 32\n=> 33\n=> 557\n=> 558\n=> 559\n=> 556\n=> ЗИ-1\n=> \n");
        }

        [TestMethod]
        public void PftGroup_Execute_2()
        {
            MarcRecord record = _GetRecord();
            PftGroup node = _GetNode();
            node.Children.Add(new PftBreak());
            _Execute(record, node, "=> 32\n");
        }

        [TestMethod]
        public void PftGroup_Execute_3()
        {
            MarcRecord record = _GetRecord();
            PftGroup node = new PftGroup();
            node.Children.Add(new PftUnconditionalLiteral("=>"));
            node.Children.Add(new PftSlash());
            _Execute(record, node, "=>\n");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void PftGroup_Execute_4()
        {
            MarcRecord record = _GetRecord();
            PftGroup node = new PftGroup();
            PftContext context = new PftContext(null)
            {
                CurrentGroup = new PftGroup(),
                Record = record
            };
            node.Execute(context);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void PftGroup_Execute_5()
        {
            bool save = PftGroup.ThrowOnEmpty;
            try
            {
                PftGroup.ThrowOnEmpty = true;
                MarcRecord record = _GetRecord();
                PftGroup node = new PftGroup();
                PftContext context = new PftContext(null)
                {
                    Record = record
                };
                node.Execute(context);
            }
            finally
            {
                PftGroup.ThrowOnEmpty = save;
            }
        }

        [TestMethod]
        public void PftGroup_Execute_6()
        {
            PftContext context = new PftContext(null);
            PftGroup node = new PftGroup();
            node.Children.Add(new PftUnifor
            {
                Name = "uf",
                Children = { new PftUnconditionalLiteral("+90")}
            });
            node.Children.Add(new PftSlash());
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.StartsWith("1\n"));
            Assert.IsTrue(actual.EndsWith("500\n"));
        }


        [TestMethod]
        public void PftGroup_Optimize_1()
        {
            PftGroup node = _GetNode();
            Assert.AreEqual(node, node.Optimize());
        }

        [TestMethod]
        public void PftGroup_Optimize_2()
        {
            PftGroup node = new PftGroup();
            Assert.IsNull(node.Optimize());
        }

        [TestMethod]
        public void PftGroup_PrettyPrint_1()
        {
            PftGroup node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("( '=> ' v910^b, / )", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftGroup_PrettyPrint_2()
        {
            PftGroup node = new PftGroup
            {
                Children =
                {
                    new PftConditionalStatement
                    {
                        Condition = new PftP("v300"),
                        ThenBranch =
                        {
                            new PftUnconditionalLiteral("=>"),
                            new PftV("v300"),
                            new PftSlash()
                        }
                    }
                }
            };
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\n(\n  if p(v300)\n  then \'=>\' v300 /\n  fi,\n)\n", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                [NotNull] PftGroup first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftGroup second = (PftGroup) PftSerializer.Deserialize(reader);
            Assert.IsNotNull(second);

            // TODO FIX THIS!
            //PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftGroup_Serialization_1()
        {
            PftGroup node = new PftGroup();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftGroup_ToString_1()
        {
            PftGroup node = new PftGroup();
            Assert.AreEqual("()", node.ToString());
        }

        [TestMethod]
        public void PftGroup_ToString_2()
        {
            PftGroup node = _GetNode();
            Assert.AreEqual("('=> ' v910^b , /)", node.ToString());
        }
    }
}
