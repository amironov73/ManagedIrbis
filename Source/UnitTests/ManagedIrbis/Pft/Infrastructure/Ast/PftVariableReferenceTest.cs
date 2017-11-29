using System;

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
    public class PftVariableReferenceTest
    {
        private void _Execute
        (
            [NotNull] PftVariableReference node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftVariableReference_Construction_1()
        {
            PftVariableReference node = new PftVariableReference();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftVariableReference_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Variable, 1, 1, "$x");
            PftVariableReference node = new PftVariableReference(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftVariableReference_Execute_1()
        {
            PftVariableReference node = new PftVariableReference();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftVariableReference_ToString_1()
        {
            PftVariableReference node = new PftVariableReference
            {
                Name = "name"
            };
            Assert.AreEqual("$name", node.ToString());
        }
    }
}
