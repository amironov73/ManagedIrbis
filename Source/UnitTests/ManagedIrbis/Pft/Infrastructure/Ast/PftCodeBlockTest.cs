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
    public class PftCodeBlockTest
    {
        private void _Execute
        (
            [NotNull] PftCodeBlock node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftCodeBlock_Construction_1()
        {
            PftCodeBlock node = new PftCodeBlock();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftCodeBlock_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.TripleCurly, 1, 1, "{{{");
            PftCodeBlock node = new PftCodeBlock(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftCodeBlock_Execute_1()
        {
            PftCodeBlock node = new PftCodeBlock();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftCodeBlock_ToString_1()
        {
            PftCodeBlock node = new PftCodeBlock();
            Assert.AreEqual("", node.ToString());
        }
    }
}
