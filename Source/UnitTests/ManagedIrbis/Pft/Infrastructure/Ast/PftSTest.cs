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
    public class PftSTest
    {
        private void _Execute
            (
                [NotNull] PftS node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftS_Construction_1()
        {
            PftS node = new PftS();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftS_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.S, 1, 1, "s");
            PftS node = new PftS(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftBlank_Compile_1()
        {
            PftS node = new PftS();
            node.Children.Add(new PftUnconditionalLiteral("123.45"));
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftS_Execute_1()
        {
            PftS node = new PftS();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftS_Execute_2()
        {
            PftS node = new PftS();
            node.Children.Add(new PftUnconditionalLiteral("Hello"));
            _Execute(node, "Hello");
        }

        [TestMethod]
        public void PftS_Execute_3()
        {
            PftS node = new PftS();
            node.Children.Add(new PftUnconditionalLiteral("Hello, "));
            node.Children.Add(new PftUnconditionalLiteral("world!"));
            _Execute(node, "Hello, world!");
        }

        [TestMethod]
        public void PftS_Optimize_1()
        {
            PftS node = new PftS();
            Assert.IsNull(node.Optimize());
        }

        [TestMethod]
        public void PftS_Optimize_2()
        {
            PftS node = new PftS();
            PftUnconditionalLiteral literal = new PftUnconditionalLiteral("text");
            node.Children.Add(literal);
            Assert.AreEqual(literal, node.Optimize());
        }

        [TestMethod]
        public void PftS_Optimize_3()
        {
            PftS node = new PftS();
            node.Children.Add(new PftUnconditionalLiteral("Hello"));
            node.Children.Add(new PftUnconditionalLiteral("world"));
            Assert.AreSame(node, node.Optimize());
        }

        [TestMethod]
        public void PftS_PrettyPrint_1()
        {
            PftS node = new PftS();
            node.Children.Add(new PftUnconditionalLiteral("Hello"));
            node.Children.Add(new PftComma());
            node.Children.Add(new PftUnconditionalLiteral("world"));
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("s('Hello', 'world')", printer.ToString());
        }

        [TestMethod]
        public void PftS_ToString_1()
        {
            PftS node = new PftS();
            Assert.AreEqual("s()", node.ToString());
        }

        [TestMethod]
        public void PftS_ToString_2()
        {
            PftS node = new PftS();
            node.Children.Add(new PftUnconditionalLiteral("Hello"));
            Assert.AreEqual("s('Hello')", node.ToString());
        }

        [TestMethod]
        public void PftS_ToString_3()
        {
            PftS node = new PftS();
            node.Children.Add(new PftUnconditionalLiteral("Hello, "));
            node.Children.Add(new PftComma());
            node.Children.Add(new PftUnconditionalLiteral("world!"));
            Assert.AreEqual("s('Hello, ' , 'world!')", node.ToString());
        }
    }
}
