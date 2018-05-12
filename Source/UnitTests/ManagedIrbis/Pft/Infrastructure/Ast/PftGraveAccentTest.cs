using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ExpressionIsAlwaysNull

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftGraveAccentTest
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
        public void PftGraveAccent_Construction_1()
        {
            PftGraveAccent node = new PftGraveAccent();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsNull(node.Text);
        }

        [TestMethod]
        public void PftGraveAccent_Construction_2()
        {
            string text = "text";
            PftGraveAccent node = new PftGraveAccent(text);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreSame(text, node.Text);
        }

        [TestMethod]
        public void PftGraveAccent_Construction_3()
        {
            string text = "text";
            PftToken token = new PftToken(PftTokenKind.GraveAccent, 1, 1, text);
            PftGraveAccent node = new PftGraveAccent(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreSame(text, node.Text);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftGraveAccent_Construction_4()
        {
            string text = null;
            PftToken token = new PftToken(PftTokenKind.GraveAccent, 1, 1, text);
            new PftGraveAccent(token);
        }

        [TestMethod]
        public void PftGraveAccent_Compile_1()
        {
            PftGraveAccent node = new PftGraveAccent("text");
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftGraveAccent_Execute_1()
        {
            string text = "text";
            PftGraveAccent node = new PftGraveAccent(text);
            _Execute(node, text);
        }

        [TestMethod]
        public void PftGraveAccent_Execute_2()
        {
            PftProgram program = new PftProgram();
            program.Children.Add(new PftMode("mpu"));
            string text = "text";
            PftGraveAccent node = new PftGraveAccent(text);
            program.Children.Add(node);
            _Execute(program, "TEXT");
        }

        [TestMethod]
        public void PftGraveAccent_Optimize_1()
        {
            PftGraveAccent expected = new PftGraveAccent("text");
            PftNode actual = expected.Optimize();
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void PftGraveAccent_PrettyPrint_1()
        {
            PftGraveAccent node = new PftGraveAccent("text");
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("`text`", printer.ToString());
        }

        [TestMethod]
        public void PftGraveAccent_ToString_1()
        {
            PftGraveAccent node = new PftGraveAccent("text");
            Assert.AreEqual("`text`", node.ToString());
        }
    }
}
