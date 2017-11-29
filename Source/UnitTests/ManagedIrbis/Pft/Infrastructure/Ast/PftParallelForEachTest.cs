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
    public class PftParallelForEachTest
    {
        private void _Execute
        (
            [NotNull] PftParallelForEach node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftParallelForEach_Construction_1()
        {
            PftParallelForEach node = new PftParallelForEach();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftParallelForEach_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Parallel, 1, 1, "parallel");
            PftParallelForEach node = new PftParallelForEach(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftParallelForEach_Execute_1()
        {
            PftParallelForEach node = new PftParallelForEach();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftParallelForEach_ToString_1()
        {
            PftParallelForEach node = new PftParallelForEach();
            Assert.AreEqual("", node.ToString());
        }
    }
}
