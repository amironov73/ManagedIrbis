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
    public class PftBlankTest
    {
        private void _Execute
        (
            [NotNull] PftBlank node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftBlank_Construction_1()
        {
            PftBlank node = new PftBlank();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftBlank_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Blank, 1, 1, "blank");
            PftBlank node = new PftBlank(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftBlank_Execute_1()
        {
            PftBlank node = new PftBlank();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftBlank_ToString_1()
        {
            PftBlank node = new PftBlank();
            Assert.AreEqual("blank()", node.ToString());
        }
    }
}
