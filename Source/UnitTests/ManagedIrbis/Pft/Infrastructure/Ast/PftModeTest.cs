using System;
using System.IO;

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

// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable ObjectCreationAsStatement

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftModeTest
    {
        private void _Execute
            (
                [NotNull] PftMode node,
                PftFieldOutputMode mode,
                bool upper
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            Assert.AreEqual(mode, context.FieldOutputMode);
            Assert.AreEqual(upper, context.UpperMode);
        }

        [TestMethod]
        public void PftMode_Construction_1()
        {
            PftMode node = new PftMode();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(PftFieldOutputMode.PreviewMode, node.OutputMode);
            Assert.IsFalse(node.UpperMode);
        }

        [TestMethod]
        public void PftMode_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Mpl, 1, 1, "mpl");
            PftMode node = new PftMode(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(PftFieldOutputMode.PreviewMode, node.OutputMode);
            Assert.IsFalse(node.UpperMode);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftMode_Construction_3()
        {
            PftToken token = new PftToken(PftTokenKind.Mpl, 1, 1, "@");
            new PftMode(token);
        }

        [TestMethod]
        public void PftMode_Construction_4()
        {
            PftMode node = new PftMode("mdu");
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(PftFieldOutputMode.DataMode, node.OutputMode);
            Assert.IsTrue(node.UpperMode);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftMode_Construction_5()
        {
            new PftMode("@");
        }

        private void _TestCompile
            (
                [NotNull] PftMode node
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
        public void PftMode_Compile_1()
        {
            PftMode node = new PftMode();
            _TestCompile(node);
        }

        [TestMethod]
        public void PftMode_Execute_1()
        {
            PftMode node = new PftMode();
            _Execute(node, PftFieldOutputMode.PreviewMode, false);

            node = new PftMode("mhu");
            _Execute(node, PftFieldOutputMode.HeaderMode, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftMode_ParseText_1()
        {
            PftMode mode = new PftMode();
            mode.ParseText("@");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftMode_ParseText_2()
        {
            PftMode mode = new PftMode();
            mode.ParseText("mqq");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftMode_ParseText_3()
        {
            PftMode mode = new PftMode();
            mode.ParseText("mpq");
        }

        [TestMethod]
        public void PftMode_ParseText_4()
        {
            PftMode mode = new PftMode();
            mode.ParseText("MHU");
            Assert.AreEqual(PftFieldOutputMode.HeaderMode, mode.OutputMode);
            Assert.IsTrue(mode.UpperMode);

            mode.ParseText("MDL");
            Assert.AreEqual(PftFieldOutputMode.DataMode, mode.OutputMode);
            Assert.IsFalse(mode.UpperMode);
        }

        private void _TestSerialization
            (
                [NotNull] PftMode first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftMode second = (PftMode) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftMode_Serialization_1()
        {
            PftMode node = new PftMode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftMode_ToString_1()
        {
            PftMode node = new PftMode();
            Assert.AreEqual("mpl", node.ToString());

            node = new PftMode
            {
                OutputMode = PftFieldOutputMode.HeaderMode,
                UpperMode = true
            };
            Assert.AreEqual("mhu", node.ToString());

            node = new PftMode
            {
                OutputMode = PftFieldOutputMode.DataMode,
                UpperMode = false
            };
            Assert.AreEqual("mdl", node.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PftMode_ToString_2()
        {
            PftMode node = new PftMode
            {
                OutputMode = (PftFieldOutputMode)'q',
                UpperMode = true
            };
            node.ToString();
        }
    }
}
