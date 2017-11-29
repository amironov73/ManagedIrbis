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
    public class PftSignTest
    {
        private void _Execute
        (
            [NotNull] PftSign node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftSign_Construction_1()
        {
            PftSign node = new PftSign();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftSign_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Sign, 1, 1, "sign");
            PftSign node = new PftSign(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftSign_Execute_1()
        {
            PftSign node = new PftSign();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftSign_ToString_1()
        {
            PftSign node = new PftSign();
            Assert.AreEqual("sign()", node.ToString());
        }
    }
}
