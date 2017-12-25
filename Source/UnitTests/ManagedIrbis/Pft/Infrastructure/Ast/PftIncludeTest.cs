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
    public class PftIncludeTest
        : Common.CommonUnitTest
    {
        private void _Execute
            (
                [NotNull] PftInclude node,
                [NotNull] string expected
            )
        {
            using (IrbisProvider provider = GetProvider())
            {
                PftContext context = new PftContext(null);
                context.SetProvider(provider);
                node.Execute(context);
                string actual = context.Text.DosToUnix();
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void PftInclude_Construction_1()
        {
            PftInclude node = new PftInclude();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.Program);
        }

        [TestMethod]
        public void PftInclude_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.At, 1, 1, "include");
            PftInclude node = new PftInclude(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.Program);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftInclude_Construction_3()
        {
            string name = "name";
            PftInclude node = new PftInclude(name);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.Program);
            Assert.AreSame(name, node.Text);
        }

        [TestMethod]
        public void PftInclude_Children_1()
        {
            PftInclude node = new PftInclude
            {
                Program = new PftProgram()
            };
            Assert.AreEqual(1, node.Children.Count);
        }

        [TestMethod]
        public void PftInclide_Clone_1()
        {
            PftInclude first = new PftInclude();
            PftInclude second = (PftInclude)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftInclide_Clone_2()
        {
            PftInclude first = new PftInclude
            {
                Program = new PftProgram()
            };
            PftInclude second = (PftInclude)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftInclude_Clone_3()
        {
            PftInclude first = new PftInclude("fileName");
            PftInclude second = (PftInclude)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftInclude_Compile_1()
        {
            PftNode node = new PftInclude("_test_hello.pft");
            using (IrbisProvider provider = GetProvider())
            {
                PftCompiler compiler = new PftCompiler();
                compiler.SetProvider(provider);
                PftProgram program = new PftProgram();
                program.Children.Add(node);
                compiler.CompileProgram(program);
            }
        }

        [TestMethod]
        public void PftInclude_Compile_2()
        {
            PftNode node = new PftInclude("_test_hello");
            using (IrbisProvider provider = GetProvider())
            {
                PftCompiler compiler = new PftCompiler();
                compiler.SetProvider(provider);
                PftProgram program = new PftProgram();
                program.Children.Add(node);
                compiler.CompileProgram(program);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftInclude_Compile_3()
        {
            PftNode node = new PftInclude();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftInclude_Compile_4()
        {
            PftNode node = new PftInclude("_test_error.pft");
            using (IrbisProvider provider = GetProvider())
            {
                PftCompiler compiler = new PftCompiler();
                compiler.SetProvider(provider);
                PftProgram program = new PftProgram();
                program.Children.Add(node);
                compiler.CompileProgram(program);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void PftInclude_Execute_1()
        {
            PftInclude node = new PftInclude();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftInclude_Execute_2()
        {
            PftInclude node = new PftInclude("_test_hello.pft");
            _Execute(node, "Hello");
        }

        [TestMethod]
        public void PftInclude_Execute_3()
        {
            PftInclude node = new PftInclude("_test_hello");
            _Execute(node, "Hello");
        }

        [TestMethod]
        public void PftInclude_Execute_4()
        {
            PftInclude node = new PftInclude("empty.pft");
            _Execute(node, "");
        }

        [TestMethod]
        public void PftInclude_Execute_5()
        {
            PftInclude node = new PftInclude("empty2.pft");
            _Execute(node, "");
        }

        [TestMethod]
        public void PftInclude_GetNodeInfo_1()
        {
            PftInclude node = new PftInclude("fileName");
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("Include", info.Name);
        }

        [TestMethod]
        public void PftInclude_GetNodeInfo_2()
        {
            PftInclude node = new PftInclude("fileName")
            {
                Program = new PftProgram()
            };
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("Include", info.Name);
        }

        private void _TestSerialization
            (
                [NotNull] PftInclude first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftInclude second = (PftInclude)PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftInclude_Serialization_1()
        {
            PftInclude node = new PftInclude();
            _TestSerialization(node);

            node = new PftInclude("fileName");
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftInclude_ToString_1()
        {
            PftInclude node = new PftInclude();
            Assert.AreEqual("include()", node.ToString());

            node = new PftInclude("fileName");
            Assert.AreEqual("include(fileName)", node.ToString());
        }
    }
}
