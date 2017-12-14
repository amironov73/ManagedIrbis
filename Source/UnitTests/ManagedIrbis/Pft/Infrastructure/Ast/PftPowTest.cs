using System.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable ObjectCreationAsStatement

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftPowTest
    {
        private void _Execute
            (
                [NotNull] PftPow node,
                double expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            double actual = node.Value;
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private PftPow _GetNode()
        {
            return new PftPow
            {
                X = new PftNumericLiteral(2),
                Y = new PftNumericLiteral(3)
            };
        }

        [TestMethod]
        public void PftPow_Construction_1()
        {
            PftPow node = new PftPow();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftPow_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Pow, 1, 1, "pow");
            PftPow node = new PftPow(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftPow_Clone_1()
        {
            PftPow first = new PftPow();
            PftPow second = (PftPow) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftPow_Clone_2()
        {
            PftPow first = _GetNode();
            PftPow second = (PftPow) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftPow_Compile_1()
        {
            PftPow node = _GetNode();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftPow_Compile_2()
        {
            PftPow node = new PftPow();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftPow_Execute_1()
        {
            PftPow node = new PftPow();
            _Execute(node, 0);

            node = _GetNode();
            _Execute(node, 8);
        }

        [TestMethod]
        public void PftPow_GetNodeInfo_1()
        {
            PftPow node = _GetNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.IsNotNull(info);
            Assert.AreEqual("Pow", info.Name);
        }

        [TestMethod]
        public void PftRsum_PrettyPrint_1()
        {
            PftPow node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("pow(2, 3)", printer.ToString());
        }

        private void _TestSerialization
            (
                [NotNull] PftPow first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftPow second = (PftPow) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftComparison_Serialization_1()
        {
            PftPow node = new PftPow();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftPow_ToString_1()
        {
            PftPow node = new PftPow();
            Assert.AreEqual("pow(,)", node.ToString());

            node = _GetNode();
            Assert.AreEqual("pow(2,3)", node.ToString());
        }
    }
}

