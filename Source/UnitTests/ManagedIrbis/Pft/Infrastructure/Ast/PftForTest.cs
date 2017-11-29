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
    public class PftForTest
    {
        private void _Execute
        (
            [NotNull] PftFor node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftFor_Construction_1()
        {
            PftFor node = new PftFor();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftFor_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.For, 1, 1, "for");
            PftFor node = new PftFor(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        // Вечный цикл. Зависает
        //[TestMethod]
        //public void PftFor_Execute_1()
        //{
        //    PftFor node = new PftFor();
        //    _Execute(node, "");
        //}

        [TestMethod]
        public void PftFor_ToString_1()
        {
            PftFor node = new PftFor();
            Assert.AreEqual("", node.ToString());
        }
    }
}
