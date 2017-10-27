using AM.Text;

using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftFalseTest
    {
        [TestMethod]
        public void PftFalse_Construction_1()
        {
            PftFalse node = new PftFalse();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
        }

        [TestMethod]
        public void PftFalse_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.False, 1, 1, "False");
            PftFalse node = new PftFalse(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftFalse_Compile_1()
        {
            PftCompiler compiler = new PftCompiler();
            PftProgram program = new PftProgram();
            program.Children.Add(new PftFalse());
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftFalse_Execute_1()
        {
            PftContext context = new PftContext(null);
            PftFalse node = new PftFalse();
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.IsFalse(node.Value);
            Assert.AreEqual("", actual);
        }

        [TestMethod]
        public void PftFalse_PrettyPrint_1()
        {
            PftPrettyPrinter printer = new PftPrettyPrinter();
            PftProgram program = new PftProgram();
            program.Children.Add(new PftFalse());
            program.PrettyPrint(printer);
            Assert.AreEqual("false ", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftFalse_ToString_1()
        {
            PftFalse node = new PftFalse();
            Assert.AreEqual("false", node.ToString());
        }

        [TestMethod]
        public void PftFalse_Value_1()
        {
            PftFalse node = new PftFalse();
            Assert.IsFalse(node.Value);
        }

        [TestMethod]
        public void PftFalse_Value_2()
        {
            PftFalse node = new PftFalse();
            node.Value = true;
            Assert.IsFalse(node.Value);
        }
    }
}
