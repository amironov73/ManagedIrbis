using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftNumericExpressionTest
    {
        private void _Execute
        (
            [NotNull] PftNumericExpression node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftNumericExpression_Construction_1()
        {
            PftNumericExpression node = new PftNumericExpression();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftNumericExpression_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Number, 1, 1, "1");
            PftNumericExpression node = new PftNumericExpression(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftNumericExpression_Execute_1()
        {
            PftNumericExpression node = new PftNumericExpression();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftNumericExpression_ToString_1()
        {
            PftNumericExpression node = new PftNumericExpression();
            Assert.AreEqual("", node.ToString());
        }
    }
}
