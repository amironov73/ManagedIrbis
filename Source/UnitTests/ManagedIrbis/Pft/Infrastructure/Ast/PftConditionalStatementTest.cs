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
    public class PftConditionalStatementTest
    {
        private void _Execute
        (
            [NotNull] PftConditionalStatement node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftConditionalStatement_Construction_1()
        {
            PftConditionalStatement node = new PftConditionalStatement();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftConditionalStatement_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.If, 1, 1, "if");
            PftConditionalStatement node = new PftConditionalStatement(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftConditionalStatement_Execute_1()
        {
            PftConditionalStatement node = new PftConditionalStatement();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftConditionalStatement_ToString_1()
        {
            PftConditionalStatement node = new PftConditionalStatement();
            Assert.AreEqual("if  then fi", node.ToString());
        }
    }
}
