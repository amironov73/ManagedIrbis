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
    public class PftNlTest
    {
        private void _Execute
            (
                [NotNull] PftNl node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftNl_Construction_1()
        {
            PftNl node = new PftNl();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
        }

        [TestMethod]
        public void PftNl_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Nl, 1, 1, "nl");
            PftNl node = new PftNl(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftNl_Compile_1()
        {
            PftCompiler compiler = new PftCompiler();
            PftProgram program = new PftProgram();
            PftNl node = new PftNl();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftNl_Execute_1()
        {
            PftNl node = new PftNl();
            _Execute(node, "\n");
        }

        [TestMethod]
        public void PftNl_PrettyPrint_1()
        {
            PftPrettyPrinter printer = new PftPrettyPrinter();
            PftNl node = new PftNl();
            node.PrettyPrint(printer);
            Assert.AreEqual(" nl ", printer.ToString());
        }

        [TestMethod]
        public void PftNl_ToString_1()
        {
            PftNl node = new PftNl();
            Assert.AreEqual("nl", node.ToString());
        }
    }
}
