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
    public class PftConditionNotTest
    {
        private void _Execute
        (
            [NotNull] PftConditionNot node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftConditionNot_Construction_1()
        {
            PftConditionNot node = new PftConditionNot();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftConditionNot_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Not, 1, 1, "not");
            PftConditionNot node = new PftConditionNot(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftConditionNot_Execute_1()
        {
            PftConditionNot node = new PftConditionNot();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftConditionNot_ToString_1()
        {
            PftConditionNot node = new PftConditionNot();
            Assert.AreEqual(" not ", node.ToString());
        }
    }
}
