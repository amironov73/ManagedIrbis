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
    public class PftConditionParenthesisTest
    {
        private void _Execute
            (
                [NotNull] PftConditionParenthesis node,
                bool expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            bool actual = node.Value;
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private PftConditionParenthesis _GetNode()
        {
            PftConditionParenthesis result = new PftConditionParenthesis
            {
                InnerCondition = new PftTrue()
            };

            return result;
        }

        [TestMethod]
        public void PftConditionParenthesis_Construction_1()
        {
            PftConditionParenthesis node = new PftConditionParenthesis();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
        }

        [TestMethod]
        public void PftConditionParenthesis_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.LeftParenthesis, 1, 1, "(");
            PftConditionParenthesis node = new PftConditionParenthesis(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftConditionParenthesis_Clone_1()
        {
            PftConditionParenthesis first = new PftConditionParenthesis();
            PftConditionParenthesis second = (PftConditionParenthesis) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftConditionParenthesis_Clone_2()
        {
            PftConditionParenthesis first = _GetNode();
            PftConditionParenthesis second = (PftConditionParenthesis) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        private void _TestCompile
            (
                [NotNull] PftConditionParenthesis node
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
        public void PftConditionParenthesis_Compile_1()
        {
            PftConditionParenthesis node = _GetNode();
            _TestCompile(node);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftConditionParenthesis_Compile_2()
        {
            PftConditionParenthesis node = new PftConditionParenthesis();
            _TestCompile(node);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftConditionParenthesis_Execute_1()
        {
            PftConditionParenthesis node = new PftConditionParenthesis();
            _Execute(node, false);
        }

        [TestMethod]
        public void PftConditionParenthesis_Execute_2()
        {
            PftConditionParenthesis node = _GetNode();
            _Execute(node, true);
        }

        [TestMethod]
        public void PftConditionParenthesis_GetNodeInfo_1()
        {
            PftConditionParenthesis node = _GetNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.IsNotNull(info);
            Assert.AreEqual("ConditionParenthesis", info.Name);
        }

        [TestMethod]
        public void PftConditionParenthesis_Optimize_1()
        {
            PftConditionParenthesis node = _GetNode();
            Assert.AreNotSame(node, node.Optimize());
        }

        [TestMethod]
        public void PftConditionParenthesis_Optimize_2()
        {
            PftConditionParenthesis node = new PftConditionParenthesis();
            Assert.AreSame(node, node.Optimize());
        }

        [TestMethod]
        public void PftConditionParenthesis_PrettyPrint_1()
        {
            PftConditionParenthesis node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("(true)", printer.ToString());
        }

        private void _TestSerialization
            (
                [NotNull] PftConditionParenthesis first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftConditionParenthesis second = (PftConditionParenthesis) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftConditionParenthesis_Serialization_1()
        {
            PftConditionParenthesis node = new PftConditionParenthesis();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftConditionParenthesis_ToString_1()
        {
            PftConditionParenthesis node = new PftConditionParenthesis();
            Assert.AreEqual("()", node.ToString());

            node = _GetNode();
            Assert.AreEqual("(true)", node.ToString());
        }
    }
}
