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
    public class PftGroupTest
    {
        private void _Execute
        (
            [NotNull] PftGroup node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftGroup_Construction_1()
        {
            PftGroup node = new PftGroup();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftGroup_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.LeftParenthesis, 1, 1, "(");
            PftGroup node = new PftGroup(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftGroup_Execute_1()
        {
            PftGroup node = new PftGroup();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftGroup_ToString_1()
        {
            PftGroup node = new PftGroup();
            Assert.AreEqual("()", node.ToString());
        }
    }
}
