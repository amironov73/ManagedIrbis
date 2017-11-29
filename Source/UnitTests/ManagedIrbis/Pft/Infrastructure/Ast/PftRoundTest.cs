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
    public class PftRoundTest
    {
        private void _Execute
        (
            [NotNull] PftRound node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftRound_Construction_1()
        {
            PftRound node = new PftRound();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftRound_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Round, 1, 1, "round");
            PftRound node = new PftRound(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftRound_Execute_1()
        {
            PftRound node = new PftRound();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftRound_ToString_1()
        {
            PftRound node = new PftRound();
            Assert.AreEqual("", node.ToString());
        }
    }
}
