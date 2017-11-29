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
    public class PftEmptyTest
    {
        private void _Execute
        (
            [NotNull] PftEmpty node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftEmpty_Construction_1()
        {
            PftEmpty node = new PftEmpty();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftEmpty_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Empty, 1, 1, "empty");
            PftEmpty node = new PftEmpty(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftEmpty_Execute_1()
        {
            PftEmpty node = new PftEmpty();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftEmpty_ToString_1()
        {
            PftEmpty node = new PftEmpty();
            Assert.AreEqual("empty()", node.ToString());
        }
    }
}
