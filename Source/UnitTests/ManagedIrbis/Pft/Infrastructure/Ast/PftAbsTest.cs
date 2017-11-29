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
    public class PftAbsTest
        : Common.CommonUnitTest
    {
        private void _Execute
            (
                [NotNull] PftAbs node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftAbs_Construction_1()
        {
            PftAbs node = new PftAbs();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftAbs_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Abs, 1, 1, "abs");
            PftAbs node = new PftAbs(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftAbs_Execute_1()
        {
            PftAbs node = new PftAbs();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftAbs_ToString_1()
        {
            PftAbs node = new PftAbs();
            Assert.AreEqual("abs()", node.ToString());
        }
    }
}
