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
    public class PftConditionParenthesisTest
    {
        private void _Execute
        (
            [NotNull] PftConditionParenthesis node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftConditionParenthesis_Construction_1()
        {
            PftConditionParenthesis node = new PftConditionParenthesis();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftConditionParenthesis_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.LeftParenthesis, 1, 1, "(");
            PftConditionParenthesis node = new PftConditionParenthesis(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftConditionParenthesis_Execute_1()
        {
            PftConditionParenthesis node = new PftConditionParenthesis();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftConditionParenthesis_ToString_1()
        {
            PftConditionParenthesis node = new PftConditionParenthesis();
            Assert.AreEqual("()", node.ToString());
        }
    }
}
