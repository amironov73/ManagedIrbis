using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftNestedTest
    {
        private void _Execute
            (
                [NotNull] MarcRecord mainRecord,
                [NotNull] MarcRecord alternativeRecord,
                [NotNull] PftNode node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null)
            {
                Record = mainRecord,
                AlternativeRecord = alternativeRecord
            };
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private MarcRecord _GetMainRecord()
        {
            MarcRecord result = new MarcRecord();

            RecordField field = new RecordField("700");
            field.AddSubField('a', "Иванов");
            field.AddSubField('b', "И. И.");
            result.Fields.Add(field);

            field = new RecordField("701");
            field.AddSubField('a', "Петров");
            field.AddSubField('b', "П. П.");
            result.Fields.Add(field);

            field = new RecordField("200");
            field.AddSubField('a', "Заглавие");
            field.AddSubField('e', "подзаголовочное");
            field.AddSubField('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new RecordField("300", "Первое примечание");
            result.Fields.Add(field);
            field = new RecordField("300", "Второе примечание");
            result.Fields.Add(field);
            field = new RecordField("300", "Третье примечание");
            result.Fields.Add(field);

            return result;
        }

        [NotNull]
        private MarcRecord _GetAlternativeRecord()
        {
            MarcRecord record = new MarcRecord();
            record.Fields.Add(RecordField.Parse(900, "^A0^B32"));

            return record;
        }

        [NotNull]
        public PftNested _GetNode()
        {
            return new PftNested
            {
                Children =
                {
                    new PftUnconditionalLiteral(" Alternative: "),
                    new PftV("v900^b")
                }
            };
        }

        [TestMethod]
        public void PftNested_Construction_1()
        {
            PftNested node = new PftNested();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftNested_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.LeftCurly, 1, 1, "{");
            PftNested node = new PftNested(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftNested_Execute_1()
        {
            MarcRecord mainRecord = _GetMainRecord();
            MarcRecord alternativeRecord = _GetAlternativeRecord();
            PftProgram program = new PftProgram
            {
                Children =
                {
                    new PftUnconditionalLiteral("Main: "),
                    new PftV("v200^a"),
                    _GetNode(),
                    new PftUnconditionalLiteral(" Main again: "),
                    new PftV("v200^e")
                }
            };
            string expected = "Main: Заглавие Alternative: 32 Main again: подзаголовочное";
            _Execute(mainRecord, alternativeRecord, program, expected);
        }

        [TestMethod]
        public void PftNested_PrettyPrint_1()
        {
            PftPrettyPrinter printer = new PftPrettyPrinter();
            PftNested node = _GetNode();
            node.PrettyPrint(printer);
            Assert.AreEqual("{' Alternative: ' v900^b} ", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftNested_ToString_1()
        {
            PftNested node = new PftNested();
            Assert.AreEqual("{}", node.ToString());
        }

        [TestMethod]
        public void PftNested_ToString_2()
        {
            PftNested node = _GetNode();
            Assert.AreEqual("{' Alternative: ' v900^b}", node.ToString());
        }
    }
}
