using System;
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

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable ObjectCreationAsStatement

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftF2Test
    {
        private void _Execute
            (
                [NotNull] PftF2 node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private PftF2 _GetNode()
        {
            return new PftF2
            {
                Number = new PftNumericLiteral(Math.PI),
                Format =
                {
                    new PftUnconditionalLiteral("F2")
                }
            };
        }

        [TestMethod]
        public void PftF2_Construction_1()
        {
            PftF2 node = new PftF2();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.Number);
            Assert.IsNotNull(node.Format);
        }

        [TestMethod]
        public void PftF2_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.F2, 1, 1, "f2");
            PftF2 node = new PftF2(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.Number);
            Assert.IsNotNull(node.Format);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftF2_Clone_1()
        {
            PftF2 first = new PftF2();
            PftF2 second = (PftF2) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftF2_Clone_2()
        {
            PftF2 first = _GetNode();
            PftF2 second = (PftF2) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        private void _TestCompile
            (
                [NotNull] PftF2 node
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
        public void PftF2_Compile_1()
        {
            PftF2 node = _GetNode();
            _TestCompile(node);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftF2_Compile_2()
        {
            PftF2 node = new PftF2();
            _TestCompile(node);
        }

        [TestMethod]
        public void PftF2_Execute_1()
        {
            PftF2 node = new PftF2();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftF2_Execute_2()
        {
            PftF2 node = _GetNode();
            _Execute(node, "3.14");

            node.Format.Clear();
            _Execute(node, "3.14159265358979");
        }

        [TestMethod]
        public void PftF2_Execute_3()
        {
            PftF2 node = new PftF2
            {
                Number = new PftNumericLiteral(Math.PI),
                Format =
                {
                    new PftUnconditionalLiteral("F"),
                    new PftUnconditionalLiteral("2")
                }
            };
            _Execute(node, "3.14");
        }

        [TestMethod]
        public void PftF2_Execute_4()
        {
            PftF2 node = new PftF2
            {
                Number = new PftNumericLiteral(Math.PI),
                Format =
                {
                    new PftUnconditionalLiteral("F"),
                    new PftComma(),
                    new PftUnconditionalLiteral("2")
                }
            };
            _Execute(node, "3.14");
        }

        [TestMethod]
        public void PftF2_GetNodeInfo_1()
        {
            PftF2 node = _GetNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("F2", info.Name);
        }

        private void _TestSerialization
            (
                [NotNull] PftF2 first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftF2 second = (PftF2) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftF2_Serialization_1()
        {
            PftF2 node = new PftF2();
            _TestSerialization(node);

            node = new PftF2();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftF2_ToString_1()
        {
            PftF2 node = new PftF2();
            Assert.AreEqual("f2(,)", node.ToString());
        }

        [TestMethod]
        public void PftF2_ToString_2()
        {
            PftF2 node = _GetNode();
            Assert.AreEqual("f2(3.14159265358979,'F2')", node.ToString());

            node.Format.Clear();
            Assert.AreEqual("f2(3.14159265358979,)", node.ToString());
        }

        [TestMethod]
        public void PftF2_ToString_3()
        {
            PftF2 node = new PftF2
            {
                Number = new PftNumericLiteral(Math.PI),
                Format =
                {
                    new PftUnconditionalLiteral("F"),
                    new PftUnconditionalLiteral("2")
                }
            };
            Assert.AreEqual("f2(3.14159265358979,'F' '2')", node.ToString());
        }

        [TestMethod]
        public void PftF2_ToString_4()
        {
            PftF2 node = new PftF2
            {
                Number = new PftNumericLiteral(Math.PI),
                Format =
                {
                    new PftUnconditionalLiteral("F"),
                    new PftComma(),
                    new PftUnconditionalLiteral("2")
                }
            };
            Assert.AreEqual("f2(3.14159265358979,'F' , '2')", node.ToString());
        }
    }
}

