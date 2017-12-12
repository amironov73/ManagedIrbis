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
    public class PftEmptyTest
    {
        private void _Execute
            (
                [NotNull] PftEmpty node,
                bool expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            bool actual = node.Value;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftEmpty_Construction_1()
        {
            PftEmpty node = new PftEmpty();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftEmpty_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Empty, 1, 1, "empty");
            PftEmpty node = new PftEmpty(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftEmpty_Compile_1()
        {
            PftEmpty node = new PftEmpty();
            node.Children.Add(new PftUnconditionalLiteral("123.45"));
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftEmpty_Compile_2()
        {
            PftEmpty node = new PftEmpty();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftEmpty_Execute_1()
        {
            PftEmpty node = new PftEmpty();
            _Execute(node, true);

            PftUnconditionalLiteral literal = new PftUnconditionalLiteral();
            node.Children.Add(literal);
            _Execute(node, true);

            literal.Text = string.Empty;
            _Execute(node, true);

            literal.Text = "   ";
            _Execute(node, false);

            literal.Text = "Hello!";
            _Execute(node, false);

            literal.Text = " Hello! ";
            _Execute(node, false);
        }

        [TestMethod]
        public void PftEmpty_PrettyPrint_1()
        {
            PftEmpty node = new PftEmpty();
            node.Children.Add(new PftUnconditionalLiteral("Hello"));
            node.Children.Add(new PftUnconditionalLiteral("world"));
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("empty('Hello''world')", printer.ToString());
        }

        [TestMethod]
        public void PftEmpty_ToString_1()
        {
            PftEmpty node = new PftEmpty();
            Assert.AreEqual("empty()", node.ToString());
        }

        [TestMethod]
        public void PftEmpty_ToString_2()
        {
            PftEmpty node = new PftEmpty();
            node.Children.Add(new PftUnconditionalLiteral("Hello"));
            Assert.AreEqual("empty('Hello')", node.ToString());
        }

        [TestMethod]
        public void PftEmpty_ToString_3()
        {
            PftEmpty node = new PftEmpty();
            node.Children.Add(new PftUnconditionalLiteral("Hello"));
            node.Children.Add(new PftUnconditionalLiteral("world"));
            Assert.AreEqual("empty('Hello' 'world')", node.ToString());
        }
    }
}
