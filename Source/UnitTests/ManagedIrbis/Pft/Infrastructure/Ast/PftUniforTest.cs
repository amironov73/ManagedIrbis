using System;
using System.IO;

using AM.Text;

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
    public class PftUniforTest
    {
        private void _Execute
            (
                [NotNull] PftUnifor node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private PftUnifor _GetNode()
        {
            return new PftUnifor("unifor")
            {
                Children =
                {
                    new PftUnconditionalLiteral("+9V")
                }
            };
        }

        [TestMethod]
        public void PftUnifor_Construction_1()
        {
            PftUnifor node = new PftUnifor();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.Name);
        }

        [TestMethod]
        public void PftUnifor_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Unifor, 1, 1, "unifor");
            PftUnifor node = new PftUnifor(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual("unifor", node.Name);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftUnifor_Construction_3()
        {
            PftUnifor node = new PftUnifor("unifor");
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual("unifor", node.Name);
        }

        [TestMethod]
        public void PftUnifor_Clone_1()
        {
            PftUnifor first = new PftUnifor();
            PftUnifor second = (PftUnifor)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftUnifor_Clone_2()
        {
            PftUnifor first = _GetNode();
            PftUnifor second = (PftUnifor)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSerializationException))]
        public void PftUnifor_Clone_3()
        {
            PftUnifor first = _GetNode();
            PftUnifor second = (PftUnifor)first.Clone();
            second.Name = "umarci";
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftUnifor_Compile_1()
        {
            PftUnifor node = _GetNode();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftUnifor_Execute_1()
        {
            PftUnifor node = new PftUnifor();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftUnifor_Execute_2()
        {
            PftUnifor node = _GetNode();
            _Execute(node, "64");
        }

        [TestMethod]
        public void PftUnifor_GetNodeInfo_1()
        {
            PftUnifor node = _GetNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("FormatExit", info.Name);
        }

        [TestMethod]
        public void PftUnifor_Optimize_1()
        {
            PftUnifor node = new PftUnifor();
            Assert.IsNull(node.Optimize());
        }

        [TestMethod]
        public void PftUnifor_Optimize_2()
        {
            PftUnifor node = _GetNode();
            Assert.AreSame(node, node.Optimize());
        }

        private void _TestSerialization
            (
                [NotNull] PftUnifor first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftUnifor second = (PftUnifor)PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftUnifor_Serialization_1()
        {
            PftUnifor node = new PftUnifor();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftUnifor_ToString_1()
        {
            PftUnifor node = new PftUnifor();
            Assert.AreEqual("&()", node.ToString());

            node = _GetNode();
            Assert.AreEqual("&unifor('+9V')", node.ToString());
        }
    }
}
