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
    public class PftPercentTest
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
        public void PftPercent_Construction_1()
        {
            PftPercent node = new PftPercent();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
        }

        [TestMethod]
        public void PftPercent_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Percent, 1, 1, "%");
            PftPercent node = new PftPercent(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftPercent_Compile_1()
        {
            PftPercent node = new PftPercent();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftPercent_Execute_1()
        {
            PftPercent node = new PftPercent();
            _Execute(node, "\n");
        }

        [TestMethod]
        public void PftPercent_Execute_2()
        {
            PftProgram node = new PftProgram
            {
                Children =
                {
                    new PftPercent(),
                    new PftPercent()
                }
            };
            _Execute(node, "\n");
        }

        [TestMethod]
        public void PftPercent_Execute_3()
        {
            PftProgram node = new PftProgram
            {
                Children =
                {
                    new PftPercent(),
                    new PftPercent(),
                    new PftPercent()
                }
            };
            _Execute(node, "\n");
        }

        [TestMethod]
        public void PftPercent_PrettyPrint_1()
        {
            PftPercent node = new PftPercent();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("% ", printer.ToString());
        }

        [TestMethod]
        public void PftPercent_ToString_1()
        {
            PftPercent node = new PftPercent();
            Assert.AreEqual("%", node.ToString());
        }
    }
}
