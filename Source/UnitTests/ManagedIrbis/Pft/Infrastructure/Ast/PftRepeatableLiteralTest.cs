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
    public class PftRepeatableLiteralTest
    {
        private void _Execute
        (
            [NotNull] PftRepeatableLiteral node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftRepeatableLiteral_Construction_1()
        {
            PftRepeatableLiteral node = new PftRepeatableLiteral();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftRepeatableLiteral_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.RepeatableLiteral, 1, 1, "|Hello|");
            PftRepeatableLiteral node = new PftRepeatableLiteral(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftRepeatableLiteral_Execute_1()
        {
            PftRepeatableLiteral node = new PftRepeatableLiteral();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftRepeatableLiteral_ToString_1()
        {
            PftRepeatableLiteral node = new PftRepeatableLiteral();
            Assert.AreEqual("||", node.ToString());
        }
    }
}
