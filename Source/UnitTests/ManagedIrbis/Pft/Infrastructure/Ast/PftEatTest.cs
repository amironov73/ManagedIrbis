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
    public class PftEatTest
    {
        private PftProgram _GetProgram()
        {
            PftProgram result = new PftProgram();
            result.Children.Add(new PftUnconditionalLiteral("Hello, "));
            PftEat eat = new PftEat();
            result.Children.Add(eat);
            eat.Children.Add(new PftUnconditionalLiteral("new "));
            result.Children.Add(new PftUnconditionalLiteral("world!"));

            return result;
        }

        private void _Execute
            (
                [NotNull] PftProgram program,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            program.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftEat_Construction_1()
        {
            PftEat node = new PftEat();
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftEat_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.EatOpen, 1, 1, ",");
            PftEat node = new PftEat(token);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftEat_Compile_1()
        {
            PftCompiler compiler = new PftCompiler();
            PftProgram program = _GetProgram();
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftEat_Execute_1()
        {
            PftProgram program = _GetProgram();
            _Execute(program, "Hello, world!");
        }

        [TestMethod]
        public void PftEat_PrettyPrint_1()
        {
            PftPrettyPrinter printer = new PftPrettyPrinter();
            PftProgram program = _GetProgram();
            program.PrettyPrint(printer);
            Assert.AreEqual("'Hello, '[[['new ']]] 'world!'", printer.ToString().DosToUnix());
        }
    }
}
