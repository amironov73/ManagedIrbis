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
    public class PftATest
        : Common.CommonUnitTest
    {
        private void _Execute
            (
                [NotNull] PftA node,
                [NotNull] string expected
            )
        {
            using (IrbisProvider provider = GetProvider())
            {
                MarcRecord record = provider.ReadRecord(1);
                Assert.IsNotNull(record);
                PftContext context = new PftContext(null)
                {
                    Record = record
                };
                node.Execute(context);
                string actual = context.Text.DosToUnix();
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void PftA_Construction_1()
        {
            PftA node = new PftA();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftA_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.A, 1, 1, "a");
            PftA node = new PftA(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftA_Execute_1()
        {
            PftA node = new PftA
            {
                Field = new PftV("v200")
            };
            _Execute(node, "");
        }

        [TestMethod]
        public void PftA_ToString_1()
        {
            PftA node = new PftA();
            Assert.AreEqual("a()", node.ToString());
        }
    }
}
