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
    public class PftNestedTest
    {
        private void _Execute
        (
            [NotNull] PftNested node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftNested_Construction_1()
        {
            PftNested node = new PftNested();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftNested_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.LeftCurly, 1, 1, "{");
            PftNested node = new PftNested(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftNested_Execute_1()
        {
            PftNested node = new PftNested();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftNested_ToString_1()
        {
            PftNested node = new PftNested();
            Assert.AreEqual("{}", node.ToString());
        }
    }
}
