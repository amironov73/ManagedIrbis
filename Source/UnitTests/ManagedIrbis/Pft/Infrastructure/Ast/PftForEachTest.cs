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
    public class PftForEachTest
    {
        private void _Execute
        (
            [NotNull] PftForEach node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftForEach_Construction_1()
        {
            PftForEach node = new PftForEach();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftForEach_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.ForEach, 1, 1, "foreach");
            PftForEach node = new PftForEach(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftForEach_Execute_1()
        {
            PftForEach node = new PftForEach();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftForEach_ToString_1()
        {
            PftForEach node = new PftForEach();
            Assert.AreEqual("", node.ToString());
        }
    }
}
