using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftBangTest
    {
        private void _Execute
            (
                [NotNull] PftBang node
            )
        {
            bool flag = false;

            PftContext context = new PftContext(null);
            Mock<PftDebugger> mock = new Mock<PftDebugger>(context);
            PftDebugger debugger = mock.Object;

            mock.Setup(d => d.Activate(It.IsAny<PftDebugEventArgs>()))
                .Callback(() => { flag = true; });

            context.Debugger = debugger;
            node.Execute(context);
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void PftBang_Construction_1()
        {
            PftBang node = new PftBang();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftBang_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Bang, 1, 1, "!");
            PftBang node = new PftBang(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftBang_Compile_1()
        {
            PftBang node = new PftBang();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftBang_Execute_1()
        {
            PftBang node = new PftBang();
            _Execute(node);
        }

        [TestMethod]
        public void PftBang_PrettyPrint_1()
        {
            PftBang node = new PftBang();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("! ", printer.ToString());
        }

        [TestMethod]
        public void PftBang_ToString_1()
        {
            PftBang node = new PftBang();
            Assert.AreEqual("!", node.ToString());
        }
    }
}
