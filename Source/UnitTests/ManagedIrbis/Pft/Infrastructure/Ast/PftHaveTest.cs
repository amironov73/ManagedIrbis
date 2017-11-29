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
    public class PftHaveTest
    {
        private void _Execute
        (
            [NotNull] PftHave node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftHave_Construction_1()
        {
            PftHave node = new PftHave();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftHave_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Have, 1, 1, "have");
            PftHave node = new PftHave(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftHave_Execute_1()
        {
            PftHave node = new PftHave();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftHave_ToString_1()
        {
            PftHave node = new PftHave();
            Assert.AreEqual("", node.ToString());
        }
    }
}
