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
    public class PftBreakTest
    {
        private void _Execute
        (
            [NotNull] PftBreak node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftBreak_Construction_1()
        {
            PftBreak node = new PftBreak();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftBreak_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Break, 1, 1, "break");
            PftBreak node = new PftBreak(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftBreak_Execute_1()
        {
            try
            {
                PftBreak node = new PftBreak();
                _Execute(node, "");
            }
            catch (Exception exception)
            {
                Assert.AreEqual("PftBreakException", exception.GetType().Name);
            }
        }

        [TestMethod]
        public void PftBreak_ToString_1()
        {
            PftBreak node = new PftBreak();
            Assert.AreEqual("break", node.ToString());
        }
    }
}
