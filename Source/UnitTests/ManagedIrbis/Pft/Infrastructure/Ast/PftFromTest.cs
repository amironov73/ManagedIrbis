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
    public class PftFromTest
    {
        private void _Execute
        (
            [NotNull] PftFrom node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftFrom_Construction_1()
        {
            PftFrom node = new PftFrom();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftFrom_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.From, 1, 1, "from");
            PftFrom node = new PftFrom(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftFrom_Execute_1()
        {
            PftFrom node = new PftFrom();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftFrom_ToString_1()
        {
            PftFrom node = new PftFrom();
            Assert.AreEqual("from  in  select  end", node.ToString());
        }
    }
}
