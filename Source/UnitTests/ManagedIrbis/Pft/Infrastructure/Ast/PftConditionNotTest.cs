using System.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable ObjectCreationAsStatement

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftConditionNotTest
    {
        private void _Execute
            (
                [NotNull] PftConditionNot node,
                bool expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            bool actual = node.Value;
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private PftConditionNot _GetNode()
        {
            PftConditionNot result = new PftConditionNot
            {
                InnerCondition = new PftTrue()
            };

            return result;
        }

        [TestMethod]
        public void PftConditionNot_Construction_1()
        {
            PftConditionNot node = new PftConditionNot();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
        }

        [TestMethod]
        public void PftConditionNot_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Not, 1, 1, "not");
            PftConditionNot node = new PftConditionNot(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftConditionNot_Clone_1()
        {
            PftConditionNot first = new PftConditionNot();
            PftConditionNot second = (PftConditionNot) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftConditionNot_Clone_2()
        {
            PftConditionNot first = _GetNode();
            PftConditionNot second = (PftConditionNot) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        private void _TestCompile
            (
                [NotNull] PftConditionNot node
            )
        {
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftConditionNot_Compile_1()
        {
            PftConditionNot node = _GetNode();
            _TestCompile(node);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftConditionNot_Compile_2()
        {
            PftConditionNot node = new PftConditionNot();
            _TestCompile(node);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftConditionNot_Execute_1()
        {
            PftConditionNot node = new PftConditionNot();
            _Execute(node, false);
        }

        [TestMethod]
        public void PftConditionNot_Execute_2()
        {
            PftConditionNot node = _GetNode();
            _Execute(node, false);
        }

        [TestMethod]
        public void PftConditionNot_Optimize_1()
        {
            PftConditionNot node = _GetNode();
            Assert.AreSame(node, node.Optimize());
        }

        [TestMethod]
        public void PftConditionNot_PrettyPrint_1()
        {
            PftConditionNot node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("not true ", printer.ToString());
        }

        private void _TestSerialization
            (
                [NotNull] PftConditionNot first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftConditionNot second = (PftConditionNot) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftConditionNot_Serialization_1()
        {
            PftConditionNot node = new PftConditionNot();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftConditionNot_ToString_1()
        {
            PftConditionNot node = new PftConditionNot();
            Assert.AreEqual(" not ", node.ToString());

            node = _GetNode();
            Assert.AreEqual(" not true", node.ToString());
        }
    }
}
