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
    public class PftProcedureDefinitionTest
    {
        private void _Execute
        (
            [NotNull] PftProcedureDefinition node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftProcedureDefinition_Construction_1()
        {
            PftProcedureDefinition node = new PftProcedureDefinition();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftProcedureDefinition_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Proc, 1, 1, "proc");
            PftProcedureDefinition node = new PftProcedureDefinition(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftProcedureDefinition_Execute_1()
        {
            PftProcedureDefinition node = new PftProcedureDefinition();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftProcedureDefinition_ToString_1()
        {
            PftProcedureDefinition node = new PftProcedureDefinition();
            Assert.AreEqual("", node.ToString());
        }
    }
}
