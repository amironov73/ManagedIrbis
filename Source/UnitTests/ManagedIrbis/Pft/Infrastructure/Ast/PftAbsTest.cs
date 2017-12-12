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
    public class PftAbsTest
        : Common.CommonUnitTest
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
        public void PftAbs_Construction_1()
        {
            PftAbs node = new PftAbs();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftAbs_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Abs, 1, 1, "abs");
            PftAbs node = new PftAbs(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftAbs_Compile_1()
        {
            PftAbs node = new PftAbs();
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
        public void PftAbs_Compile_2()
        {
            PftAbs node = new PftAbs();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftAbs_Execute_1()
        {
            PftProgram program = new PftProgram();
            PftAbs node = new PftAbs();
            PftNumeric number = new PftNumericLiteral(123.45);
            node.Children.Add(number);
            PftF format = new PftF
            {
                Argument1 = node,
                Argument2 = new PftNumericLiteral(9),
                Argument3 = new PftNumericLiteral(5)
            };
            program.Children.Add(format);
            _Execute(program, "123.45000");

            number.Value = -123.45;
            _Execute(program, "123.45000");

            number.Value = 0.0;
            _Execute(program, "  0.00000");
        }

        [TestMethod]
        public void PftAbs_PrettyPrint_1()
        {
            PftAbs node = new PftAbs();
            node.Children.Add(new PftNumericLiteral(123.45));
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("abs(123.45)", printer.ToString());
        }

        [TestMethod]
        public void PftAbs_ToString_1()
        {
            PftAbs node = new PftAbs();
            Assert.AreEqual("abs()", node.ToString());
        }

        [TestMethod]
        public void PftAbs_ToString_2()
        {
            PftAbs node = new PftAbs();
            node.Children.Add(new PftNumericLiteral(123.45));
            Assert.AreEqual("abs(123.45)", node.ToString());
        }
    }
}
