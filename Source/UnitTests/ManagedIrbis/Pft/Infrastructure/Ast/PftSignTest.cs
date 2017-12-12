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
    public class PftSignTest
    {
        private void _Execute
            (
                [NotNull] PftNode node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftSign_Construction_1()
        {
            PftSign node = new PftSign();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftSign_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Sign, 1, 1, "sign");
            PftSign node = new PftSign(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftSign_Compile_1()
        {
            PftSign node = new PftSign();
            node.Children.Add(new PftNumericLiteral(123.45));
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftSign_Compile_2()
        {
            PftSign node = new PftSign();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftSign_Execute_1()
        {
            PftProgram program = new PftProgram();
            PftSign node = new PftSign();
            PftNumeric number = new PftNumericLiteral(123.45);
            node.Children.Add(number);
            PftF format = new PftF
            {
                Argument1 = node,
                Argument2 = new PftNumericLiteral(8),
                Argument3 = new PftNumericLiteral(5)
            };
            program.Children.Add(format);
            _Execute(program, " 1.00000");

            number.Value = -123.45;
            _Execute(program, "-1.00000");

            number.Value = 0.0;
            _Execute(program, " 0.00000");
        }

        [TestMethod]
        public void PftSign_PrettyPrint_1()
        {
            PftSign node = new PftSign();
            node.Children.Add(new PftNumericLiteral(123.45));
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("sign(123.45)", printer.ToString());
        }

        [TestMethod]
        public void PftSign_ToString_1()
        {
            PftSign node = new PftSign();
            Assert.AreEqual("sign()", node.ToString());
        }

        [TestMethod]
        public void PftSign_ToString_2()
        {
            PftSign node = new PftSign();
            node.Children.Add(new PftNumericLiteral(123.45));
            Assert.AreEqual("sign(123.45)", node.ToString());
        }
    }
}
