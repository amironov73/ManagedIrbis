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
            [NotNull] PftComment node,
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
        }

        [TestMethod]
        public void PftComment_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Comment, 1, 1, "/*");
            PftComment node = new PftComment(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftComment_Execute_1()
        {
            PftComment node = new PftComment();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftComment_ToString_1()
        {
            PftComment node = new PftComment();
            Assert.AreEqual("/* \n", node.ToString().DosToUnix());
        }
    }
}
