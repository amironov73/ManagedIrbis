using System;

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
    public class PftAssignmentTest
    {
        private void _Execute
        (
            [NotNull] PftAssignment node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftAssignment_Construction_1()
        {
            PftAssignment node = new PftAssignment();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftAssignment_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Equals, 1, 1, "");
            PftAssignment node = new PftAssignment(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftAssignment_Execute_1()
        {
            PftAssignment node = new PftAssignment();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftAssignment_ToString_1()
        {
            PftAssignment node = new PftAssignment();
            Assert.AreEqual("$=", node.ToString());
        }
    }
}
