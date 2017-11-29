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
    public class PftParallelWithTest
    {
        private void _Execute
        (
            [NotNull] PftParallelWith node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftParallelWith_Construction_1()
        {
            PftParallelWith node = new PftParallelWith();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftParallelWith_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Parallel, 1, 1, "parallel");
            PftParallelWith node = new PftParallelWith(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftParallelWith_Execute_1()
        {
            PftParallelWith node = new PftParallelWith();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftParallelWith_ToString_1()
        {
            PftParallelWith node = new PftParallelWith();
            Assert.AreEqual("", node.ToString());
        }
    }
}
