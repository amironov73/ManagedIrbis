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
    public class PftCeilTest
    {
        private void _Execute
        (
            [NotNull] PftCeil node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftCeil_Construction_1()
        {
            PftCeil node = new PftCeil();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftCeil_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Ceil, 1, 1, "ceil");
            PftCeil node = new PftCeil(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftCeil_Execute_1()
        {
            PftCeil node = new PftCeil();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftCeil_ToString_1()
        {
            PftCeil node = new PftCeil();
            Assert.AreEqual("ceil()", node.ToString());
        }
    }
}
