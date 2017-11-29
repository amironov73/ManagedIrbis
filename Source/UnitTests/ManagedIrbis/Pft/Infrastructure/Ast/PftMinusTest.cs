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
    public class PftMinusTest
    {
        private void _Execute
        (
            [NotNull] PftMinus node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftMinus_Construction_1()
        {
            PftMinus node = new PftMinus();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftMinus_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Minus, 1, 1, "-");
            PftMinus node = new PftMinus(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftMinus_Execute_1()
        {
            PftMinus node = new PftMinus();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftMinus_ToString_1()
        {
            PftMinus node = new PftMinus();
            Assert.AreEqual("-()", node.ToString());
        }
    }
}
