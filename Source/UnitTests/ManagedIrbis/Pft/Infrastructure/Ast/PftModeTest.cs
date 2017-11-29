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
    public class PftModeTest
    {
        private void _Execute
        (
            [NotNull] PftMode node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftMode_Construction_1()
        {
            PftMode node = new PftMode();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftMode_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Mpl, 1, 1, "mpl");
            PftMode node = new PftMode(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftMode_Execute_1()
        {
            PftMode node = new PftMode();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftMode_ToString_1()
        {
            PftMode node = new PftMode();
            Assert.AreEqual("mpl", node.ToString());
        }
    }
}
