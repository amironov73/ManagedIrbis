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
    public class PftConditionalLiteralTest
    {
        private void _Execute
        (
            [NotNull] PftConditionalLiteral node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftConditionalLiteral_Construction_1()
        {
            PftConditionalLiteral node = new PftConditionalLiteral();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftConditionalLiteral_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.ConditionalLiteral, 1, 1, "\"\"");
            PftConditionalLiteral node = new PftConditionalLiteral(token, false);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_1()
        {
            PftConditionalLiteral node = new PftConditionalLiteral();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftConditionalLiteral_ToString_1()
        {
            PftConditionalLiteral node = new PftConditionalLiteral();
            Assert.AreEqual("\"\"", node.ToString());
        }
    }
}
