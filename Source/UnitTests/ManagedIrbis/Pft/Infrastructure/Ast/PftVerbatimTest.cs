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
    public class PftVerbatimTest
    {
        private void _Execute
            (
                [NotNull] PftVerbatim node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftVerbatim_Construction_1()
        {
            PftVerbatim node = new PftVerbatim();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
        }

        [TestMethod]
        public void PftVerbatim_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.TripleLess, 1, 1, "test");
            PftVerbatim node = new PftVerbatim(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftVerbatim_Compile_1()
        {
            PftCompiler compiler = new PftCompiler();
            PftProgram program = new PftProgram();
            PftVerbatim node = new PftVerbatim()
            {
                Text = "Hello"
            };
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftVerbatim_Execute_1()
        {
            PftVerbatim node = new PftVerbatim()
            {
                Text = "Hello"
            };
            _Execute(node, "Hello");
        }

        [TestMethod]
        public void PftVerbatim_PrettyPrint_1()
        {
            PftPrettyPrinter printer = new PftPrettyPrinter();
            PftVerbatim node = new PftVerbatim()
            {
                Text = "Hello"
            };
            node.PrettyPrint(printer);
            Assert.AreEqual("<<<Hello>>>", printer.ToString());
        }

        [TestMethod]
        public void PftVerbatim_ToString_1()
        {
            PftVerbatim node = new PftVerbatim()
            {
                Text = "Hello"
            };
            Assert.AreEqual("<<<Hello>>>", node.ToString());
        }
    }
}
