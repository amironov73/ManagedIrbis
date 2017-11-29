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
    public class PftBangTest
    {
        private void _Execute
        (
            [NotNull] PftBang node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftBang_Construction_1()
        {
            PftBang node = new PftBang();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
        }

        [TestMethod]
        public void PftBang_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Bang, 1, 1, "!");
            PftBang node = new PftBang(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftBang_Execute_1()
        {
            PftBang node = new PftBang();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftBang_ToString_1()
        {
            PftBang node = new PftBang();
            Assert.AreEqual("!", node.ToString());
        }
    }
}
