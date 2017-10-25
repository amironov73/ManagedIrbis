using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftSemicolonTest
    {
        private void _Execute
            (
                [NotNull] PftSemicolon node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftSemicolon_Construction_1()
        {
            PftSemicolon node = new PftSemicolon();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
        }

        [TestMethod]
        public void PftSemicolon_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Semicolon, 1, 1, ";");
            PftSemicolon node = new PftSemicolon(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftSemicolon_Compile_1()
        {
            PftCompiler compiler = new PftCompiler();
            PftProgram program = new PftProgram();
            PftSemicolon node = new PftSemicolon();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftSemicolon_Execute_1()
        {
            PftSemicolon node = new PftSemicolon();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftSemicolon_Optimize_1()
        {
            PftSemicolon node = new PftSemicolon();
            Assert.IsNull(node.Optimize());
        }

        [TestMethod]
        public void PftSemicolon_PrettyPrint_1()
        {
            PftPrettyPrinter printer = new PftPrettyPrinter();
            PftSemicolon node = new PftSemicolon();
            node.PrettyPrint(printer);
            Assert.AreEqual("; ", printer.ToString());
        }

        [TestMethod]
        public void PftSemicolon_ToString_1()
        {
            PftSemicolon node = new PftSemicolon();
            Assert.AreEqual(";", node.ToString());
        }
    }
}
