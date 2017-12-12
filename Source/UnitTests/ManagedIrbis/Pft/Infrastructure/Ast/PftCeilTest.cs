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
    public class PftCeilTest
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
        public void PftCeil_Construction_1()
        {
            PftCeil node = new PftCeil();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftCeil_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Ceil, 1, 1, "ceil");
            PftCeil node = new PftCeil(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftCeil_Compile_1()
        {
            PftCeil node = new PftCeil();
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
        public void PftCeil_Compile_2()
        {
            PftCeil node = new PftCeil();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftCeil_Execute_1()
        {
            PftProgram program = new PftProgram();
            PftCeil node = new PftCeil();
            PftNumeric number = new PftNumericLiteral(123.45);
            node.Children.Add(number);
            PftF format = new PftF
            {
                Argument1 = node,
                Argument2 = new PftNumericLiteral(10),
                Argument3 = new PftNumericLiteral(5)
            };
            program.Children.Add(format);
            _Execute(program, " 124.00000");

            number.Value = 123.00;
            _Execute(program, " 123.00000");

            number.Value = -123.45;
            _Execute(program, "-123.00000");

            number.Value = 0.0;
            _Execute(program, "   0.00000");
        }

        [TestMethod]
        public void PftCeil_PrettyPrint_1()
        {
            PftCeil node = new PftCeil();
            node.Children.Add(new PftNumericLiteral(123.45));
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("ceil(123.45)", printer.ToString());
        }

        [TestMethod]
        public void PftCeil_ToString_1()
        {
            PftCeil node = new PftCeil();
            Assert.AreEqual("ceil()", node.ToString());
        }

        [TestMethod]
        public void PftCeil_ToString_2()
        {
            PftCeil node = new PftCeil();
            node.Children.Add(new PftNumericLiteral(123.45));
            Assert.AreEqual("ceil(123.45)", node.ToString());
        }
    }
}
