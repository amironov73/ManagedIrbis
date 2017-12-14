using System.IO;
using AM.Text;

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

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable ObjectCreationAsStatement

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftRsumTest
    {
        private void _Execute
            (
                [NotNull] PftRsum node,
                double expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            double actual = node.Value;
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private PftRsum _GetNode()
        {
            PftRsum result = new PftRsum
            {
                Name = "rsum"
            };
            result.Children.Add(new PftUnconditionalLiteral("1;"));
            result.Children.Add(new PftUnconditionalLiteral("2;"));
            result.Children.Add(new PftUnconditionalLiteral("3;"));

            return result;
        }

        [TestMethod]
        public void PftRsum_Construction_1()
        {
            PftRsum node = new PftRsum();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.Name);
        }

        [TestMethod]
        public void PftRsum_Construction_2()
        {
            string name = "rsum";
            PftToken token = new PftToken(PftTokenKind.Rsum, 1, 1, name);
            PftRsum node = new PftRsum(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreSame(name, node.Name);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftRsum_Construction_3()
        {
            string name = "rsum";
            PftRsum node = new PftRsum(name);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreSame(name, node.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftRsum_Construction_4()
        {
            string name = null;
            PftToken token = new PftToken(PftTokenKind.Rsum, 1, 1, name);
            new PftRsum(token);
        }

        [TestMethod]
        public void PftRsum_Clone_1()
        {
            PftRsum first = new PftRsum();
            PftRsum second = (PftRsum) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftRsum_Clone_2()
        {
            PftRsum first = _GetNode();
            PftRsum second = (PftRsum) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSerializationException))]
        public void PftRsum_Clone_3()
        {
            PftRsum first = _GetNode();
            PftRsum second = (PftRsum) first.Clone();
            second.Name = "@@@";
            PftSerializationUtility.CompareNodes(first, second);
        }

        private void _TestCompile
            (
                [NotNull] PftRsum node
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
        public void PftRsum_Compile_1()
        {
            PftRsum node = _GetNode();
            node.Name = "rsum";
            _TestCompile(node);

            node = _GetNode();
            node.Name = "rmin";
            _TestCompile(node);

            node = _GetNode();
            node.Name = "rmax";
            _TestCompile(node);

            node = _GetNode();
            node.Name = "ravr";
            _TestCompile(node);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftRsum_Compile_2()
        {
            PftRsum node = _GetNode();
            node.Name = "@@@";
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftRsum_Execute_1()
        {
            PftRsum node = new PftRsum();
            _Execute(node, 0);

            node = _GetNode();

            node.Name = "rsum";
            _Execute(node, 6);

            node.Name = "rmin";
            _Execute(node, 1);

            node.Name = "rmax";
            _Execute(node, 3);

            node.Name = "ravr";
            _Execute(node, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftRsum_Execute_2()
        {
            PftRsum node = new PftRsum();
            _Execute(node, 0);

            node = _GetNode();
            node.Name = "@@@";
            _Execute(node, 6);
        }

        [TestMethod]
        public void PftRsum_PrettyPrint_1()
        {
            PftRsum node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("rsum('1;''2;''3;')", printer.ToString());
        }

        private void _TestSerialization
            (
                [NotNull] PftRsum first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftRsum second = (PftRsum) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftRsum_Serialization_1()
        {
            PftRsum node = new PftRsum();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftRsum_ToString_1()
        {
            PftRsum node = new PftRsum();
            Assert.AreEqual("()", node.ToString());
        }

        [TestMethod]
        public void PftRsum_ToString_2()
        {
            PftRsum node = _GetNode();
            Assert.AreEqual("rsum('1;' '2;' '3;')", node.ToString());
        }
    }
}
