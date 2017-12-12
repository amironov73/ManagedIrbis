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
    public class PftTruncTest
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
        public void PftTrunc_Construction_1()
        {
            PftTrunc node = new PftTrunc();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftTrunc_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Trunc, 1, 1, "trunc");
            PftTrunc node = new PftTrunc(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftTrunc_Compile_1()
        {
            PftTrunc node = new PftTrunc();
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
        public void PftTrunc_Compile_2()
        {
            PftTrunc node = new PftTrunc();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftTrunc_Execute_1()
        {
            PftProgram program = new PftProgram();
            PftTrunc node = new PftTrunc();
            PftNumeric number = new PftNumericLiteral(123.45);
            node.Children.Add(number);
            PftF format = new PftF
            {
                Argument1 = node,
                Argument2 = new PftNumericLiteral(9),
                Argument3 = new PftNumericLiteral(5)
            };
            program.Children.Add(format);
            _Execute(program, "123.00000");

            number.Value = 123.54;
            _Execute(program, "123.00000");
        }

        [TestMethod]
        public void PftTrunc_PrettyPrint_1()
        {
            PftTrunc node = new PftTrunc();
            node.Children.Add(new PftNumericLiteral(123.45));
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("trunc(123.45)", printer.ToString());
        }

        [TestMethod]
        public void PftTrunc_ToString_1()
        {
            PftTrunc node = new PftTrunc();
            Assert.AreEqual("trunc()", node.ToString());
        }

        [TestMethod]
        public void PftTrunc_ToString_2()
        {
            PftTrunc node = new PftTrunc();
            node.Children.Add(new PftNumericLiteral(123.45));
            Assert.AreEqual("trunc(123.45)", node.ToString());
        }
    }
}
