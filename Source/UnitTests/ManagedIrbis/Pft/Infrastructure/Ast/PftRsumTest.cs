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
    public class PftRsumTest
    {
        private void _Execute
        (
            [NotNull] PftRsum node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftRsum_Construction_1()
        {
            PftRsum node = new PftRsum();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftRsum_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Rsum, 1, 1, "rsum");
            PftRsum node = new PftRsum(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftRsum_Execute_1()
        {
            PftRsum node = new PftRsum();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftRsum_ToString_1()
        {
            PftRsum node = new PftRsum();
            Assert.AreEqual("()", node.ToString());
        }
    }
}
