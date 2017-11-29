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
    public class PftFracTest
    {
        private void _Execute
        (
            [NotNull] PftFrac node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftFrac_Construction_1()
        {
            PftFrac node = new PftFrac();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftFrac_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Frac, 1, 1, "frac");
            PftFrac node = new PftFrac(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftFrac_Execute_1()
        {
            PftFrac node = new PftFrac();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftFrac_ToString_1()
        {
            PftFrac node = new PftFrac();
            Assert.AreEqual("", node.ToString());
        }
    }
}
