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
    public class PftUniforTest
    {
        private void _Execute
        (
            [NotNull] PftUnifor node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUnifor_Construction_1()
        {
            PftUnifor node = new PftUnifor();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftUnifor_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Unifor, 1, 1, "&unifor");
            PftUnifor node = new PftUnifor(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftUnifor_Execute_1()
        {
            PftUnifor node = new PftUnifor();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftUnifor_ToString_1()
        {
            PftUnifor node = new PftUnifor();
            Assert.AreEqual("&()", node.ToString());
        }
    }
}
