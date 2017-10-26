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
    public class PftTrueTest
    {
        [TestMethod]
        public void PftTrue_Construction_1()
        {
            PftTrue node = new PftTrue();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
        }

        [TestMethod]
        public void PftTrue_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.True, 1, 1, "True");
            PftTrue node = new PftTrue(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftTrue_Compile_1()
        {
            PftCompiler compiler = new PftCompiler();
            PftProgram program = new PftProgram();
            program.Children.Add(new PftTrue());
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftTrue_Execute_1()
        {
            PftContext context = new PftContext(null);
            PftTrue node = new PftTrue();
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.IsTrue(node.Value);
            Assert.AreEqual("", actual);
        }

        [TestMethod]
        public void PftTrue_PrettyPrint_1()
        {
            PftPrettyPrinter printer = new PftPrettyPrinter();
            PftProgram program = new PftProgram();
            program.Children.Add(new PftTrue());
            program.PrettyPrint(printer);
            Assert.AreEqual("true ", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftTrue_ToString_1()
        {
            PftTrue node = new PftTrue();
            Assert.AreEqual("true", node.ToString());
        }

        [TestMethod]
        public void PftTrue_Value_1()
        {
            PftTrue node = new PftTrue();
            Assert.IsTrue(node.Value);
        }

        [TestMethod]
        public void PftTrue_Value_2()
        {
            PftTrue node = new PftTrue();
            node.Value = false;
            Assert.IsTrue(node.Value);
        }
    }
}
