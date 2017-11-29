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
    public class PftFloorTest
    {
        private void _Execute
        (
            [NotNull] PftFloor node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftFloor_Construction_1()
        {
            PftFloor node = new PftFloor();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftFloor_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Floor, 1, 1, "floor");
            PftFloor node = new PftFloor(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftFloor_Execute_1()
        {
            PftFloor node = new PftFloor();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftFloor_ToString_1()
        {
            PftFloor node = new PftFloor();
            Assert.AreEqual("floor()", node.ToString());
        }
    }
}
