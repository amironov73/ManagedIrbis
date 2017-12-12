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
    public class PftCommentTest
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
        public void PftComment_Construction_1()
        {
            PftComment node = new PftComment();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftComment_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Comment, 1, 1, "/*");
            PftComment node = new PftComment(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftComment_Construction_3()
        {
            string text = "text";
            PftComment node = new PftComment(text);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreSame(text, node.Text);
        }

        [TestMethod]
        public void PftComment_Compile_1()
        {
            PftComment node = new PftComment("text");
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftComment_Execute_1()
        {
            PftProgram program = new PftProgram();
            program.Children.Add(new PftUnconditionalLiteral("Hello, "));
            PftComment node = new PftComment("text");
            program.Children.Add(new PftUnconditionalLiteral("world!"));
            program.Children.Add(node);
            _Execute(program, "Hello, world!");
        }

        [TestMethod]
        public void PftComment_Optimize_1()
        {
            PftComment comment = new PftComment("text");
            Assert.IsNull(comment.Optimize());
        }

        [TestMethod]
        public void PftComment_PrettyPrint_1()
        {
            PftNode program = new PftNode();
            program.Children.Add(new PftUnconditionalLiteral("Hello,"));
            program.Children.Add(new PftComment("comment"));
            program.Children.Add(new PftUnconditionalLiteral("world"));
            PftPrettyPrinter printer = new PftPrettyPrinter();
            program.PrettyPrint(printer);
            Assert.AreEqual("'Hello,'/* comment\n'world'", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftComment_ToString_1()
        {
            PftComment node = new PftComment("Hello");
            Assert.AreEqual("/* Hello\n", node.ToString().DosToUnix());
        }
    }
}

