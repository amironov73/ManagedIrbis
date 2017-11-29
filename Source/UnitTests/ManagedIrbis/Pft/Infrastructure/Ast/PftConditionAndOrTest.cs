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
    public class PftConditionAndOrTest
    {
        private void _Execute
        (
            [NotNull] PftConditionAndOr node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftConditionAndOr_Construction_1()
        {
            PftConditionAndOr node = new PftConditionAndOr();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftConditionAndOr_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.And, 1, 1, "and");
            PftConditionAndOr node = new PftConditionAndOr(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftConditionAndOr_Execute_1()
        {
            PftConditionAndOr node = new PftConditionAndOr();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftConditionAndOr_ToString_1()
        {
            PftConditionAndOr node = new PftConditionAndOr();
            Assert.AreEqual("  ", node.ToString());
        }
    }
}
