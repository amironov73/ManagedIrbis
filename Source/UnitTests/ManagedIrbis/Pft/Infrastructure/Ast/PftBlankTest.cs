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
    public class PftBlankTest
    {
        private void _Execute
            (
                [NotNull] PftBlank node,
                bool expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            bool actual = node.Value;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftBlank_Construction_1()
        {
            PftBlank node = new PftBlank();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftBlank_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Blank, 1, 1, "blank");
            PftBlank node = new PftBlank(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftBlank_Compile_1()
        {
            PftBlank node = new PftBlank();
            node.Children.Add(new PftUnconditionalLiteral("123.45"));
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftBlank_Compile_2()
        {
            PftBlank node = new PftBlank();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftBlank_Execute_1()
        {
            PftBlank node = new PftBlank();
            _Execute(node, true);

            PftUnconditionalLiteral literal = new PftUnconditionalLiteral();
            node.Children.Add(literal);
            _Execute(node, true);

            literal.Text = string.Empty;
            _Execute(node, true);

            literal.Text = "   ";
            _Execute(node, true);

            literal.Text = "Hello!";
            _Execute(node, false);

            literal.Text = " Hello! ";
            _Execute(node, false);
        }

        [TestMethod]
        public void PftBlank_PrettyPrint_1()
        {
            PftBlank node = new PftBlank();
            node.Children.Add(new PftUnconditionalLiteral("Hello"));
            node.Children.Add(new PftUnconditionalLiteral("world"));
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("blank('Hello''world')", printer.ToString());
        }

        [TestMethod]
        public void PftBlank_ToString_1()
        {
            PftBlank node = new PftBlank();
            Assert.AreEqual("blank()", node.ToString());
        }

        [TestMethod]
        public void PftBlank_ToString_2()
        {
            PftBlank node = new PftBlank();
            node.Children.Add(new PftUnconditionalLiteral("Hello"));
            Assert.AreEqual("blank('Hello')", node.ToString());
        }

        [TestMethod]
        public void PftBlank_ToString_3()
        {
            PftBlank node = new PftBlank();
            node.Children.Add(new PftUnconditionalLiteral("Hello"));
            node.Children.Add(new PftUnconditionalLiteral("world"));
            Assert.AreEqual("blank('Hello' 'world')", node.ToString());
        }
    }
}
