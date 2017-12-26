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
    public class PftSlashTest
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
        public void PftSlash_Construction_1()
        {
            PftSlash node = new PftSlash();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ComplexExpression);
            Assert.IsFalse(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftSlash_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Slash, 1, 1, "/");
            PftSlash node = new PftSlash(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ComplexExpression);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftSlash_Compile_1()
        {
            PftSlash node = new PftSlash();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftSlash_Execute_1()
        {
            PftSlash node = new PftSlash();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftSlash_Execute_2()
        {
            PftProgram node = new PftProgram
            {
                Children =
                {
                    new PftSlash(),
                    new PftSlash()
                }
            };
            _Execute(node, "");
        }

        [TestMethod]
        public void PftSlash_Execute_3()
        {
            PftProgram node = new PftProgram
            {
                Children =
                {
                    new PftSlash(),
                    new PftSlash(),
                    new PftSlash()
                }
            };
            _Execute(node, "");
        }

        [TestMethod]
        public void PftSlash_PrettyPrint_1()
        {
            PftSlash node = new PftSlash();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("/ ", printer.ToString());
        }

        [TestMethod]
        public void PftSlash_ToString_1()
        {
            PftSlash node = new PftSlash();
            Assert.AreEqual("/", node.ToString());
        }
    }
}
