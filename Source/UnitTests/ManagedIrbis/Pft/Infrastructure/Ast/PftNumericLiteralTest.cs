using System;
using System.IO;

using AM;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ObjectCreationAsStatement

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftNumericLiteralTest
    {
        private void _Execute
            (
                [NotNull] PftNumericLiteral node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = node.Value.ToInvariantString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftNumericLiteral_Construction_1()
        {
            PftNumericLiteral node = new PftNumericLiteral();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(0.0, node.Value);
        }

        [TestMethod]
        public void PftNumericLiteral_Construction_2()
        {
            double value = 123.45;
            PftNumericLiteral node = new PftNumericLiteral(value);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(value, node.Value);
        }

        [TestMethod]
        public void PftNumericLiteral_Construction_3()
        {
            PftToken token = new PftToken(PftTokenKind.Number, 1, 1, "123.45");
            PftNumericLiteral node = new PftNumericLiteral(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(123.45, node.Value);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void PftNumericLiteral_Construction_4()
        {
            PftToken token = new PftToken(PftTokenKind.Number, 1, 1, "123#45");
            new PftNumericLiteral(token);
        }

        [TestMethod]
        public void PftNumericLiteral_Construction_5()
        {
            PftToken token = new PftToken(PftTokenKind.Number, 1, 1, "123,45");
            PftNumericLiteral node = new PftNumericLiteral(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(12345, node.Value);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PftNumericLiteral_CompareNode_1()
        {
            PftNumericLiteral left = new PftNumericLiteral(123.45);
            PftNumericLiteral right = new PftNumericLiteral(124.35);
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        public void PftNumericLiteral_Compile_1()
        {
            PftNumericLiteral node = new PftNumericLiteral(123.45);
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftNumericLiteral_Clone_1()
        {
            PftNumericLiteral left = new PftNumericLiteral(123.45);
            PftNumericLiteral right = (PftNumericLiteral)left.Clone();
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        public void PftNumericLiteral_Execute_1()
        {
            PftNumericLiteral node = new PftNumericLiteral();
            _Execute(node, "0");
        }

        [TestMethod]
        public void PftNumericLiteral_Execute_2()
        {
            PftNumericLiteral node = new PftNumericLiteral(123.45);
            _Execute(node, "123.45");
        }

        [TestMethod]
        public void PftNumericLiteral_Execute_3()
        {
            PftNumericLiteral node = new PftNumericLiteral(-123.45);
            _Execute(node, "-123.45");
        }

        [TestMethod]
        public void PftNumericLiteral_PrettyPrint_1()
        {
            PftNumericLiteral node = new PftNumericLiteral(123.45);
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("123.45", printer.ToString());
        }

        private void _TestSerialization
            (
                [NotNull] PftNumericLiteral first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftNumericLiteral second = (PftNumericLiteral) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftNumericLiteral_Serialization_1()
        {
            PftNumericLiteral node = new PftNumericLiteral();
            _TestSerialization(node);

            node = new PftNumericLiteral(123.45);
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftNumericLiteral_ToString_1()
        {
            PftNumericLiteral node = new PftNumericLiteral();
            Assert.AreEqual("0", node.ToString());

            node = new PftNumericLiteral(123.45);
            Assert.AreEqual("123.45", node.ToString());
        }
    }
}
