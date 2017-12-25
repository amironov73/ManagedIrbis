using System.IO;

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftLTest
        : Common.CommonUnitTest
    {
        private void _Execute
            (
                [NotNull] PftL node,
                double expected
            )
        {
            using (IrbisProvider provider = GetProvider())
            {
                PftContext context = new PftContext(null);
                context.SetProvider(provider);
                node.Execute(context);
            }

            double actual = node.Value;
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        public PftL _GetNode()
        {
            return new PftL
            {
                Children =
                {
                    new PftUnconditionalLiteral("K=ATLAS")
                }
            };
        }

        [TestMethod]
        public void PftL_Construction_1()
        {
            PftL node = new PftL();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(0.0, node.Value);
        }

        [TestMethod]
        public void PftL_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.L, 1, 1, "l");
            PftL node = new PftL(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(0.0, node.Value);
        }

        [TestMethod]
        public void PftL_Compile_1()
        {
            PftL node = _GetNode();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftL_Execute_1()
        {
            PftL node = new PftL();
            _Execute(node, 0);
        }

        [TestMethod]
        public void PftL_Execute_2()
        {
            PftL node = _GetNode();
            _Execute(node, 27);
        }

        [TestMethod]
        public void PftL_PrettyPrint_1()
        {
            PftL node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("l('K=ATLAS')", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                [NotNull] PftL first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftL second = (PftL)PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftRef_Serialization_1()
        {
            PftL node = new PftL();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftL_ToString_1()
        {
            PftL node = new PftL();
            Assert.AreEqual("l()", node.ToString());

            node = _GetNode();
            Assert.AreEqual("l('K=ATLAS')", node.ToString());
        }
    }
}
