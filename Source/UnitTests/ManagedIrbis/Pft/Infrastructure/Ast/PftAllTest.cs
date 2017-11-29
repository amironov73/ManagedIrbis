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
    public class PftAllTest
    {
        private void _Execute
        (
            [NotNull] PftAll node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftAll_Construction_1()
        {
            PftAll node = new PftAll();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftAll_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.All, 1, 1, "all");
            PftAll node = new PftAll(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftAll_Execute_1()
        {
            PftAll node = new PftAll();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftAll_ToString_1()
        {
            PftAll node = new PftAll();
            Assert.AreEqual("", node.ToString());
        }
    }
}
