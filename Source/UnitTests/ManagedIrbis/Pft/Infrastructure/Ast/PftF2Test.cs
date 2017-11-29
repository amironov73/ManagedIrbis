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
    public class PftF2Test
    {
        private void _Execute
        (
            [NotNull] PftF2 node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftF2_Construction_1()
        {
            PftF2 node = new PftF2();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftF2_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.F2, 1, 1, "f2");
            PftF2 node = new PftF2(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftF2_Execute_1()
        {
            PftF2 node = new PftF2();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftF2_ToString_1()
        {
            PftF2 node = new PftF2();
            Assert.AreEqual("f2(,)", node.ToString());
        }
    }
}
