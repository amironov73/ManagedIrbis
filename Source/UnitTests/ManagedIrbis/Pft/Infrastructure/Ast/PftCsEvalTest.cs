using AM;
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
    public class PftCsEvalTest
    {
        private void _Execute
            (
                [NotNull] PftCsEval node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            MarcRecord record = new MarcRecord();
            record.Fields.Add(new RecordField(100, "context.Write(null, \"Hello\");"));
            context.Record = record;
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftCsEval_Construction_1()
        {
            PftCsEval node = new PftCsEval();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftCsEval_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.CsEval, 1, 1, "cseval");
            PftCsEval node = new PftCsEval(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftCsEval_Execute_1()
        {
            PftCsEval node = new PftCsEval();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftCsEval_Execute_2()
        {
            // TODO придумать что-нибудь для AppVeyor
            if (!ContinuousIntegrationUtility.DetectAppVeyor())
            {
                PftCsEval node = new PftCsEval(new PftV(100));
                _Execute(node, "Hello");
            }
        }

        [TestMethod]
        public void PftCsEval_PrettyPrint_1()
        {
            PftCsEval node = new PftCsEval();
            node.Children.Add(new PftV(100));
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("cseval(v100)", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftCsEval_ToString_1()
        {
            PftCsEval node = new PftCsEval();
            Assert.AreEqual("cseval()", node.ToString());
        }

        [TestMethod]
        public void PftCsEval_ToString_2()
        {
            PftCsEval node = new PftCsEval();
            node.Children.Add(new PftV(100));
            Assert.AreEqual("cseval(v100)", node.ToString());
        }
    }
}
