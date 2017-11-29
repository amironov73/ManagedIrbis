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
    public class PftIncludeTest
    {
        private void _Execute
        (
            [NotNull] PftInclude node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftInclude_Construction_1()
        {
            PftInclude node = new PftInclude();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftInclude_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.At, 1, 1, "include");
            PftInclude node = new PftInclude(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftInclude_Execute_1()
        {
            PftInclude node = new PftInclude();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftInclude_ToString_1()
        {
            PftInclude node = new PftInclude();
            Assert.AreEqual("include()", node.ToString());
        }
    }
}
