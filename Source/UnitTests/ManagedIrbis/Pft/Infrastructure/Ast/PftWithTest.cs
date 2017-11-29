using System;

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftWithTest
    {
        private void _Execute
        (
            [NotNull] PftWith node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftWith_Construction_1()
        {
            PftWith node = new PftWith();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftWith_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.With, 1, 1, "with");
            PftWith node = new PftWith(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void PftWith_Execute_1()
        {
            PftWith node = new PftWith();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftWith_ToString_1()
        {
            PftWith node = new PftWith();
            Assert.AreEqual("", node.ToString());
        }
    }
}
