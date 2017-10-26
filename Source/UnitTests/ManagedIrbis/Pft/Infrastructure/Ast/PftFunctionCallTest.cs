using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftFunctionCallTest
    {
        [NotNull]
        private PftProgram _GetProgram
            (
                [NotNull] string name,
                params PftNode[] nodes
            )
        {
            PftProgram result = new PftProgram();
            PftFunctionCall call = new PftFunctionCall(name);
            call.Arguments.AddRange(nodes);
            result.Children.Add(call);

            return result;
        }

        private void _Execute
            (
                [NotNull] string name,
                [NotNull] string expected,
                params PftNode[] nodes
            )
        {
            PftProgram program = new PftProgram();
            PftFunctionCall call = new PftFunctionCall(name);
            call.Arguments.AddRange(nodes);
            program.Children.Add(call);

            PftContext context = new PftContext(null)
            {
                Record = new MarcRecord()
            };
            program.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftFunctionCall_Construction_1()
        {
            PftFunctionCall node = new PftFunctionCall();
            Assert.IsNull(node.Name);
            Assert.IsNotNull(node.Arguments);
            Assert.AreEqual(0, node.Arguments.Count);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftFunctionCall_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Identifier, 1, 1, "name");
            PftFunctionCall node = new PftFunctionCall(token);
            Assert.AreEqual("name", node.Name);
            Assert.IsNotNull(node.Arguments);
            Assert.AreEqual(0, node.Arguments.Count);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftFunctionCall_Construction_3()
        {
            const string name = "name";
            PftFunctionCall node = new PftFunctionCall(name);
            Assert.AreEqual(name, node.Name);
            Assert.IsNotNull(node.Arguments);
            Assert.AreEqual(0, node.Arguments.Count);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftFunctionCall_bold_1()
        {
            _Execute
                (
                    "bold",
                    ""
                );
            _Execute
                (
                    "bold",
                    "<b>hello</b>",
                    new PftUnconditionalLiteral("hello")
                );
            _Execute
                (
                    "bold",
                    "<b>1</b>",
                    new PftNumericLiteral(1.0)
                );
        }

        [TestMethod]
        public void PftFunctionCall_chr_1()
        {
            _Execute
                (
                    "chr",
                    ""
                );
            _Execute
                (
                    "chr",
                    "0",
                    new PftNumericLiteral(48)
                );
            _Execute
                (
                    "chr",
                    "0",
                    new PftUnconditionalLiteral("48")
                );
        }

        [TestMethod]
        public void PftFunctionCall_insert_1()
        {
            _Execute
                (
                    "insert",
                    ""
                );
            _Execute
                (
                    "insert",
                    "Happy New Year!",
                    new PftUnconditionalLiteral("Happy Year!"),
                    new PftNumericLiteral(5),
                    new PftUnconditionalLiteral(" New")
                );
        }

        [TestMethod]
        public void PftFunctionCall_italic_1()
        {
            _Execute
                (
                    "italic",
                    ""
                );
            _Execute
                (
                    "italic",
                    "<i>hello</i>",
                    new PftUnconditionalLiteral("hello")
                );
            _Execute
                (
                    "italic",
                    "<i>1</i>",
                    new PftNumericLiteral(1.0)
                );
        }

        [TestMethod]
        public void PftFunctionCall_len_1()
        {
            _Execute
                (
                    "len",
                    "0"
                );
            _Execute
                (
                    "len",
                    "5",
                    new PftUnconditionalLiteral("Hello")
                );
            _Execute
                (
                    "len",
                    "2",
                    new PftNumericLiteral(10)
                );
        }

        [TestMethod]
        public void PftFunctionCall_padLeft_1()
        {
            _Execute
                (
                    "padLeft",
                    ""
                );
            _Execute
                (
                    "padLeft",
                    "=========================Hello",
                    new PftUnconditionalLiteral("Hello"),
                    new PftNumericLiteral(30),
                    new PftUnconditionalLiteral("=")
                );
            _Execute
                (
                    "padLeft",
                    "                         Hello",
                    new PftUnconditionalLiteral("Hello"),
                    new PftNumericLiteral(30)
                );
            _Execute
                (
                    "padLeft",
                    "============================10",
                    new PftNumericLiteral(10),
                    new PftNumericLiteral(30),
                    new PftUnconditionalLiteral("=")
                );
            _Execute
                (
                    "padLeft",
                    "                            10",
                    new PftNumericLiteral(10),
                    new PftNumericLiteral(30)
                );
        }

        [TestMethod]
        public void PftFunctionCall_padRight_1()
        {
            _Execute
                (
                    "padRight",
                    ""
                );
            _Execute
                (
                    "padRight",
                    "Hello=========================",
                    new PftUnconditionalLiteral("Hello"),
                    new PftNumericLiteral(30),
                    new PftUnconditionalLiteral("=")
                );
            _Execute
                (
                    "padRight",
                    "Hello                         ",
                    new PftUnconditionalLiteral("Hello"),
                    new PftNumericLiteral(30)
                );
            _Execute
                (
                    "padRight",
                    "10============================",
                    new PftNumericLiteral(10),
                    new PftNumericLiteral(30),
                    new PftUnconditionalLiteral("=")
                );
            _Execute
                (
                    "padRight",
                    "10                            ",
                    new PftNumericLiteral(10),
                    new PftNumericLiteral(30)
                );
        }

        [TestMethod]
        public void PftFunctionCall_remove_1()
        {
            _Execute
                (
                    "remove",
                    ""
                );
            _Execute
                (
                    "remove",
                    "Happy New Year!",
                    new PftUnconditionalLiteral("Happy Twice New Year!"),
                    new PftNumericLiteral(5),
                    new PftNumericLiteral(6)
                );
        }

        [TestMethod]
        public void PftFunctionCall_replace_1()
        {
            _Execute
                (
                    "replace",
                    ""
                );
            _Execute
                (
                    "replace",
                    "Happy Birthday!",
                    new PftUnconditionalLiteral("Happy New Year!"),
                    new PftUnconditionalLiteral("New Year"),
                    new PftUnconditionalLiteral("Birthday")
                );
        }

        [TestMethod]
        public void PftFunctionCall_size_1()
        {
            _Execute
                (
                    "size",
                    "0"
                );
            _Execute
                (
                    "size",
                    "1",
                    new PftUnconditionalLiteral("Happy New Year!")
                );
        }
    }
}
