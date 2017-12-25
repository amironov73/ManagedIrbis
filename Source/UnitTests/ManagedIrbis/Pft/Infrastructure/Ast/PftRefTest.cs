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
    public class PftRefTest
        : Common.CommonUnitTest
    {
        private void _Execute
            (
                [NotNull] PftRef node,
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

        [NotNull]
        private PftRef _GetNode()
        {
            return new PftRef
            {
                Mfn = new PftNumericLiteral(3),
                Format =
                {
                    new PftV(200, 'a'),
                    new PftV(200, 'e')
                    {
                        LeftHand = { new PftConditionalLiteral(" : ", false) }
                    },
                    new PftV(200, 'f')
                    {
                        LeftHand = { new PftConditionalLiteral(" / ", false) }
                    }
                }
            };
        }

        [TestMethod]
        public void PftRef_Construction_1()
        {
            PftRef node = new PftRef();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.Mfn);
            Assert.IsNotNull(node.Format);
            Assert.AreEqual(0, node.Format.Count);
        }

        [TestMethod]
        public void PftRef_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Ref, 1, 1, "");
            PftRef node = new PftRef(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.Mfn);
            Assert.IsNotNull(node.Format);
            Assert.AreEqual(0, node.Format.Count);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftRef_Clone_1()
        {
            PftRef first = new PftRef();
            PftRef second = (PftRef)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftRef_Clone_2()
        {
            PftRef first = _GetNode();
            PftRef second = (PftRef)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftRef_Compile_1()
        {
            PftRef node = _GetNode();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftRef_Compile_2()
        {
            PftRef node = new PftRef();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftRef_Compile_3()
        {
            PftRef node = new PftRef
            {
                Mfn = new PftNumericLiteral(3)
            };
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftRef_Execute_1()
        {
            PftRef node = new PftRef();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftRef_Execute_2()
        {
            PftRef node = _GetNode();
            _Execute(node, "Энергетическая и информационная электроника : Сб. / ред. В. А. Лабунцов [и др.]");
        }

        [TestMethod]
        public void PftRef_GetNodeInfo_1()
        {
            PftRef node = _GetNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("Ref", info.Name);
        }

        [TestMethod]
        public void PftRef_Optimize_1()
        {
            PftRef node = _GetNode();
            Assert.AreSame(node, node.Optimize());
        }

        [TestMethod]
        public void PftRef_Optimize_2()
        {
            PftRef node = new PftRef();
            Assert.IsNull(node.Optimize());
        }

        [TestMethod]
        public void PftRef_Optimize_3()
        {
            PftRef node = new PftRef
            {
                Mfn = new PftNumericLiteral(3)
            };
            Assert.IsNull(node.Optimize());
        }

        [TestMethod]
        public void PftRef_PrettyPrint_1()
        {
            PftRef node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("ref(3, v200^a \" : \"v200^e \" / \"v200^f)", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                [NotNull] PftRef first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftRef second = (PftRef)PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftRef_Serialization_1()
        {
            PftRef node = new PftRef();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftRef_ToString_1()
        {
            PftRef node = new PftRef();
            Assert.AreEqual("ref(,)", node.ToString());
        }

        [TestMethod]
        public void PftRef_ToString_2()
        {
            PftRef node = _GetNode();
            Assert.AreEqual("ref(3,v200^a \" : \"v200^e \" / \"v200^f)", node.ToString());
        }
    }
}
