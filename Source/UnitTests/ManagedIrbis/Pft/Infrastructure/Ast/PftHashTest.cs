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
    public class PftHashTest
    {
        private void _Execute
        (
            [NotNull] PftHash node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftHash_Construction_1()
        {
            PftHash node = new PftHash();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
        }

        [TestMethod]
        public void PftHash_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Hash, 1, 1, "#");
            PftHash node = new PftHash(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftHash_Execute_1()
        {
            PftHash node = new PftHash();
            _Execute(node, "\n");
        }

        [TestMethod]
        public void PftHash_ToString_1()
        {
            PftHash node = new PftHash();
            Assert.AreEqual("#", node.ToString());
        }
    }
}
