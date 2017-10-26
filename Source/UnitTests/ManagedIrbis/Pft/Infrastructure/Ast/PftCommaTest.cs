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
    public class PftCommaTest
    {
        private void _Execute
            (
                [NotNull] PftComma node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftComma_Construction_1()
        {
            PftComma node = new PftComma();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
        }

        [TestMethod]
        public void PftComma_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Comma, 1, 1, ",");
            PftComma node = new PftComma(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftComma_Compile_1()
        {
            PftCompiler compiler = new PftCompiler();
            PftProgram program = new PftProgram();
            PftComma node = new PftComma();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftComma_Execute_1()
        {
            PftComma node = new PftComma();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftComma_Optimize_1()
        {
            PftComma node = new PftComma();
            Assert.IsNull(node.Optimize());
        }

        [TestMethod]
        public void PftComma_PrettyPrint_1()
        {
            PftPrettyPrinter printer = new PftPrettyPrinter();
            PftComma node = new PftComma();
            node.PrettyPrint(printer);
            Assert.AreEqual(",\n", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftComma_PrettyPrint_2()
        {
            PftPrettyPrinter printer = new PftPrettyPrinter();
            printer.Write("'Hello',");
            PftComma node = new PftComma();
            node.PrettyPrint(printer);
            Assert.AreEqual("'Hello',", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftComma_PrettyPrint_3()
        {
            PftPrettyPrinter printer = new PftPrettyPrinter();
            printer.Write("'Hello',");
            printer.WriteIndent();
            printer.WriteLine();
            PftComma node = new PftComma();
            node.PrettyPrint(printer);
            Assert.AreEqual("\'Hello\',\n", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftComma_ToString_1()
        {
            PftComma node = new PftComma();
            Assert.AreEqual(",", node.ToString());
        }
    }
}
