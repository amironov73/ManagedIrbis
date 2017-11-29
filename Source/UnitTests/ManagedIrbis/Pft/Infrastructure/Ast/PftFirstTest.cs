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
    public class PftFirstTest
    {
        private void _Execute
        (
            [NotNull] PftFirst node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftFirst_Construction_1()
        {
            PftFirst node = new PftFirst();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftFirst_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.First, 1, 1, "first");
            PftFirst node = new PftFirst(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftFirst_Execute_1()
        {
            PftFirst node = new PftFirst();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftFirst_ToString_1()
        {
            PftFirst node = new PftFirst();
            Assert.AreEqual("", node.ToString());
        }
    }
}
