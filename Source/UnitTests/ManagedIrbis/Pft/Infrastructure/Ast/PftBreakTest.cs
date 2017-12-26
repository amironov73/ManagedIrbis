using System;

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
    public class PftBreakTest
    {
        private void _Execute
            (
                [NotNull] MarcRecord record,
                [NotNull] PftNode node,
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

        [TestMethod]
        public void PftBreak_Construction_1()
        {
            PftBreak node = new PftBreak();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftBreak_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Break, 1, 1, "break");
            PftBreak node = new PftBreak(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftVariableReference_Compile_1()
        {
            PftNode node = new PftBreak();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftBreak_Execute_1()
        {
            try
            {
                MarcRecord record = _GetRecord();
                PftBreak node = new PftBreak();
                _Execute(record, node, "");
            }
            catch (Exception exception)
            {
                Assert.AreEqual("PftBreakException", exception.GetType().Name);
            }
        }

        [TestMethod]
        public void PftBreak_Execute_2()
        {
            MarcRecord record = _GetRecord();
            PftGroup node = new PftGroup
            {
                Children =
                {
                    new PftV(300),
                    new PftBreak(),
                    new PftUnconditionalLiteral(" == ")
                }
            };
            _Execute(record, node, "Первое примечание == ");
        }

        [TestMethod]
        public void PftBreak_Execute_3()
        {
            MarcRecord record = _GetRecord();
            PftGroup node = new PftGroup
            {
                Children =
                {
                    new PftV(300),
                    new PftBreak(),
                    new PftUnconditionalLiteral(" == ")
                }
            };
            bool saveBreak = PftConfig.BreakImmediate;
            try
            {
                PftConfig.BreakImmediate = true;
                _Execute(record, node, "Первое примечание");
            }
            finally
            {
                PftConfig.BreakImmediate = saveBreak;
            }
        }

        [TestMethod]
        public void PftBreak_PrettyPrint_1()
        {
            PftBreak node = new PftBreak();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("break ", printer.ToString());
        }

        [TestMethod]
        public void PftBreak_ToString_1()
        {
            PftBreak node = new PftBreak();
            Assert.AreEqual("break", node.ToString());
        }
    }
}
