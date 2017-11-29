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
    public class PftLocalTest
    {
        private void _Execute
        (
            [NotNull] PftLocal node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftLocal_Construction_1()
        {
            PftLocal node = new PftLocal();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftLocal_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Local, 1, 1, "local");
            PftLocal node = new PftLocal(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftLocal_Execute_1()
        {
            PftLocal node = new PftLocal();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftLocal_ToString_1()
        {
            PftLocal node = new PftLocal();
            Assert.AreEqual("local do  end", node.ToString());
        }
    }
}
