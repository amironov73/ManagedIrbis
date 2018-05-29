using AM;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ObjectCreationAsStatement

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftCodeBlockTest
    {
        private void _Execute
            (
                [NotNull] PftCodeBlock node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftCodeBlock_Construction_1()
        {
            PftCodeBlock node = new PftCodeBlock();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftCodeBlock_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.TripleCurly, 1, 1, "{{{");
            PftCodeBlock node = new PftCodeBlock(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftCodeBlock_Construction_3()
        {
            PftToken token = new PftToken(PftTokenKind.TripleCurly, 1, 1, "");
            new PftCodeBlock(token);
        }

        [TestMethod]
        public void PftCodeBlock_Execute_1()
        {
            PftCodeBlock node = new PftCodeBlock();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftCodeBlock_Execute_2()
        {
            // TODO придумать что-нибудь для AppVeyor
            if (!ContinuousIntegrationUtility.DetectAppVeyor())
            {
                PftCodeBlock node = new PftCodeBlock
                {
                    Text = "context.Write(null, \"Hello\");"
                };
                _Execute(node, "Hello");
            }
        }

        [TestMethod]
        public void PftCodeBlock_Execute_3()
        {
            // TODO придумать что-нибудь для AppVeyor
            if (!ContinuousIntegrationUtility.DetectAppVeyor())
            {
                PftCodeBlock node = new PftCodeBlock
                {
                    Text = "context.Write(null, \"Hello\")"
                };
                PftContext context = new PftContext(null);
                node.Execute(context);
                string actual = context.Text.DosToUnix();
                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.Contains(";"));
            }
        }

        [TestMethod]
        public void PftCodeBlock_PrettyPrint_1()
        {
            PftCodeBlock node = new PftCodeBlock
            {
                Text = "context.Write(null, \"Hello\");"
            };
            node.Children.Add(new PftUnconditionalLiteral("Hello"));
            node.Children.Add(new PftUnconditionalLiteral("world"));
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\n{{{context.Write(null, \"Hello\");}}}\n", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftCodeBlock_ToString_1()
        {
            PftCodeBlock node = new PftCodeBlock();
            Assert.AreEqual("{{{}}}", node.ToString());
        }

        [TestMethod]
        public void PftCodeBlock_ToString_2()
        {
            PftCodeBlock node = new PftCodeBlock
            {
                Text = "context.Write(null, \"Hello\");"
            };
            Assert.AreEqual("{{{context.Write(null, \"Hello\");}}}", node.ToString());
        }
    }
}
