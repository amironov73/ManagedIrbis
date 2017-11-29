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
    public class PftParallelGroupTest
    {
        private void _Execute
        (
            [NotNull] PftParallelGroup node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftParallelGroup_Construction_1()
        {
            PftParallelGroup node = new PftParallelGroup();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftParallelGroup_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Parallel, 1, 1, "parallel");
            PftParallelGroup node = new PftParallelGroup(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftParallelGroup_Execute_1()
        {
            PftParallelGroup node = new PftParallelGroup();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftParallelGroup_ToString_1()
        {
            PftParallelGroup node = new PftParallelGroup();
            Assert.AreEqual("", node.ToString());
        }
    }
}
