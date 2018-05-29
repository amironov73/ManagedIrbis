using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ObjectCreationAsStatement

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftEvalTest
    {
        private void _Execute
            (
                [NotNull] PftEval node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            MarcRecord record = new MarcRecord();
            record.Fields.Add(new RecordField(100, "field100"));
            context.Record = record;
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftEval_Construction_1()
        {
            PftEval node = new PftEval();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftEval_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Eval, 1, 1, "eval");
            PftEval node = new PftEval(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftEval_Execute_1()
        {
            PftEval node = new PftEval();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftEval_Execute_2()
        {
            PftEval node = new PftEval(new PftUnconditionalLiteral("v100"));
            _Execute(node, "field100");
        }

        [TestMethod]
        public void PftEval_PrettyPrint_1()
        {
            PftEval node = new PftEval();
            node.Children.Add(new PftUnconditionalLiteral("v100"));
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("eval('v100')", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftEval_ToString_1()
        {
            PftEval node = new PftEval();
            Assert.AreEqual("eval()", node.ToString());
        }

        [TestMethod]
        public void PftEval_ToString_2()
        {
            PftEval node = new PftEval();
            node.Children.Add(new PftUnconditionalLiteral("v100"));
            Assert.AreEqual("eval('v100')", node.ToString());
        }
    }
}
