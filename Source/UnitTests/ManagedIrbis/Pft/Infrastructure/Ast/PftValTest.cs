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
    public class PftValTest
    {
        private void _Execute
        (
            [NotNull] PftVal node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftVal_Construction_1()
        {
            PftVal node = new PftVal();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftVal_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Val, 1, 1, "val");
            PftVal node = new PftVal(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftVal_Execute_1()
        {
            PftVal node = new PftVal();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftVal_ToString_1()
        {
            PftVal node = new PftVal();
            Assert.AreEqual("val()", node.ToString());
        }
    }
}
