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
    public class PftFieldAssignmentTest
    {
        private void _Execute
        (
            [NotNull] PftFieldAssignment node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftFieldAssignment_Construction_1()
        {
            PftFieldAssignment node = new PftFieldAssignment();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftFieldAssignment_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.V, 1, 1, "v200");
            PftFieldAssignment node = new PftFieldAssignment(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PftFieldAssignment_Execute_1()
        {
            PftFieldAssignment node = new PftFieldAssignment();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftFieldAssignment_ToString_1()
        {
            PftFieldAssignment node = new PftFieldAssignment();
            Assert.AreEqual("", node.ToString());
        }
    }
}
