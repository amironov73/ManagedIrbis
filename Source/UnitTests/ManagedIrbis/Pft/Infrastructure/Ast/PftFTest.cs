using System;
using System.Collections.Generic;
using System.IO;
using AM.Text;

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

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftFTest
    {
        private void _Execute
            (
                [NotNull] PftF node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private PftF _GetNode()
        {
            return new PftF
            {
                Argument1 = new PftNumericLiteral(Math.PI),
                Argument2 = new PftNumericLiteral(10),
                Argument3 = new PftNumericLiteral(8)
            };
        }

        [TestMethod]
        public void PftF_Construction_1()
        {
            PftF node = new PftF();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftF_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.F, 1, 1, "f");
            PftF node = new PftF(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftF_Clone_1()
        {
            PftF first = new PftF();
            PftF second = (PftF) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftF_Clone_2()
        {
            PftF first = _GetNode();
            PftF second = (PftF) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftF_Compile_1()
        {
            PftF node = _GetNode();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftF_Compile_2()
        {
            PftF node = new PftF();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftF_Compile_3()
        {
            PftF node = _GetNode();
            node.Argument3 = null;
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftF_Compile_4()
        {
            PftF node = _GetNode();
            node.Argument3 = null;
            node.Argument2 = null;
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void PftF_Execute_1()
        {
            PftF node = new PftF();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftF_Execute_2()
        {
            PftF node = _GetNode();
            _Execute(node, "3.14159265");

            node.Argument3 = new PftNumericLiteral(3);
            _Execute(node, "     3.142");

            node.Argument3 = new PftNumericLiteral(0);
            _Execute(node, "         3");

            node.Argument2 = new PftNumericLiteral(3);
            _Execute(node, "  3");

            node.Argument2 = new PftNumericLiteral(0);
            _Execute(node, "3");

            node.Argument1 = new PftNumericLiteral(123.45);
            _Execute(node, "123");
        }

        [TestMethod]
        public void PftF_GetNodeInfo_1()
        {
            PftF node = _GetNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.IsNotNull(info);
            Assert.AreEqual("F", info.Name);
        }

        private void _TestSerialization
            (
                [NotNull] PftF first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftF second = (PftF) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftF_Serialization_1()
        {
            PftF node = new PftF();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftF_ToString_1()
        {
            PftF node = new PftF();
            Assert.AreEqual("f()", node.ToString());
        }

        [TestMethod]
        public void PftF_ToString_2()
        {
            PftF node = _GetNode();
            Assert.AreEqual("f(3.14159265358979,10,8)", node.ToString());
        }

        [TestMethod]
        public void PftF_ToString_3()
        {
            PftF node = _GetNode();
            node.Argument3 = null;
            Assert.AreEqual("f(3.14159265358979,10)", node.ToString());
        }
    }
}
