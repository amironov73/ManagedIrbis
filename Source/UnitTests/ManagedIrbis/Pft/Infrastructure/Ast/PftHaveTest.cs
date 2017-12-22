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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftHaveTest
    {
        private void _Execute
            (
                [NotNull] PftHave node,
                bool expected
            )
        {
            PftContext context = new PftContext(null);
            context.Variables.SetVariable("first", "second");
            context.Variables.SetVariable("second", "first");
            context.Variables.SetVariable("third", 123.45);
            node.Execute(context);
            bool actual = node.Value;
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private PftHave _GetVariableNode()
        {
            return new PftHave("name", true);
        }

        [NotNull]
        private PftHave _GetFunctionNode()
        {
            return new PftHave("name", false);
        }

        [TestMethod]
        public void PftHave_Construction_1()
        {
            PftHave node = new PftHave();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.Variable);
            Assert.IsNull(node.Identifier);
            Assert.IsFalse(node.Value);
        }

        [TestMethod]
        public void PftHave_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Have, 1, 1, "have");
            PftHave node = new PftHave(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.Variable);
            Assert.IsNull(node.Identifier);
            Assert.IsFalse(node.Value);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftHave_Construction_3()
        {
            string name = "name";
            PftHave node = new PftHave(name, true);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.Identifier);
            Assert.IsNotNull(node.Variable);
            Assert.AreEqual(name, node.Variable.Name);
            Assert.IsFalse(node.Value);
        }

        [TestMethod]
        public void PftHave_Construction_4()
        {
            string name = "name";
            PftHave node = new PftHave(name, false);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(name, node.Identifier);
            Assert.IsNull(node.Variable);
            Assert.IsFalse(node.Value);
        }

        [TestMethod]
        public void PftHave_Clone_1()
        {
            PftHave first = new PftHave();
            PftHave second = (PftHave) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftConditionalStatement_Clone_2()
        {
            PftHave first = _GetVariableNode();
            PftHave second = (PftHave) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftConditionalStatement_Clone_3()
        {
            PftHave first = _GetFunctionNode();
            PftHave second = (PftHave) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftHave_Execute_1()
        {
            PftHave node = new PftHave();
            _Execute(node, false);
        }

        [TestMethod]
        public void PftHave_Execute_2()
        {
            PftHave node = _GetVariableNode();
            _Execute(node, false);
        }

        [TestMethod]
        public void PftHave_Execute_3()
        {
            PftHave node = _GetFunctionNode();
            _Execute(node, false);
        }

        [TestMethod]
        public void PftHave_GetNodeInfo_1()
        {
            PftHave node = _GetVariableNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.IsNotNull(info);
            Assert.AreEqual("Have", info.Name);
        }

        [TestMethod]
        public void PftHave_GetNodeInfo_2()
        {
            PftHave node = _GetFunctionNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.IsNotNull(info);
            Assert.AreEqual("Have", info.Name);
        }

        private void _TestSerialization
            (
                [NotNull] PftHave first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftHave second = (PftHave) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftConditionalStatement_Serialization_1()
        {
            PftHave node = new PftHave();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftConditionalStatement_Serialization_2()
        {
            PftHave node = _GetVariableNode();
            _TestSerialization(node);

            node = _GetFunctionNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftHave_ToString_1()
        {
            PftHave node = new PftHave();
            Assert.AreEqual("have()", node.ToString());
        }

        [TestMethod]
        public void PftHave_ToString_2()
        {
            PftHave node = _GetVariableNode();
            Assert.AreEqual("have($name)", node.ToString());

            node = _GetFunctionNode();
            Assert.AreEqual("have(name)", node.ToString());
        }
    }
}
