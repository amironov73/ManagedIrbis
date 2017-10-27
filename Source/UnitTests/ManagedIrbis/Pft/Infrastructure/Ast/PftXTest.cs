using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftXTest
    {
        [TestMethod]
        public void PftX_Construction_1()
        {
            PftX node = new PftX();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(0, node.Shift);
        }

        [TestMethod]
        public void PftX_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.X, 1, 1, "5");
            PftX node = new PftX(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
            Assert.AreEqual(5, node.Shift);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftX_Construction_2a_Exception()
        {
            PftToken token = new PftToken(PftTokenKind.X, 1, 1, "q");
            PftX node = new PftX(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
            Assert.AreEqual(5, node.Shift);
        }

        [TestMethod]
        public void PftX_Construction_3()
        {
            PftX node = new PftX(5);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(5, node.Shift);
        }

        [TestMethod]
        public void PftX_Compile_1()
        {
            PftCompiler compiler = new PftCompiler();
            PftProgram program = new PftProgram();
            program.Children.Add(new PftX(5));
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftX_Execute_1()
        {
            PftContext context = new PftContext(null);
            PftX node = new PftX(5);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual("     ", actual);
        }

        [TestMethod]
        public void PftX_Execute_2()
        {
            MarcRecord record = new MarcRecord();
            record.Fields.Add(new RecordField(1, "Hello"));
            PftContext context = new PftContext(null)
            {
                Record = record
            };
            PftV v = new PftV("v1");
            PftX node = new PftX(5);
            v.LeftHand.Add(node);
            v.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual("     Hello", actual);
        }

        [TestMethod]
        public void PftX_PrettyPrint_1()
        {
            PftPrettyPrinter printer = new PftPrettyPrinter();
            PftProgram program = new PftProgram();
            program.Children.Add(new PftX(5));
            program.PrettyPrint(printer);
            Assert.AreEqual("x5", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                [NotNull] PftX first
            )
        {
            PftProgram ethalon = new PftProgram();
            ethalon.Children.Add(first);
            byte[] bytes = PftSerializer.ToMemory(ethalon);

            PftProgram deserialized = (PftProgram)PftSerializer.FromMemory(bytes);
            PftX second = (PftX)deserialized.Children[0];

            Assert.AreEqual(first.Shift, second.Shift);

            PftSerializationUtility.VerifyDeserializedProgram
                (
                    ethalon,
                    deserialized
                );

            try
            {
                second.Shift = 6;
                PftSerializationUtility.VerifyDeserializedProgram
                    (
                        ethalon,
                        deserialized
                    );
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void PftX_Serialization_1()
        {
            PftX node = new PftX();
            _TestSerialization(node);

            node = new PftX(5);
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftX_Shift_1()
        {
            PftX node = new PftX(5);
            Assert.AreEqual(5, node.Shift);

            node.Shift = 6;
            Assert.AreEqual(6, node.Shift);
        }

        [TestMethod]
        public void PftTrue_ToString_1()
        {
            PftX node = new PftX(5);
            Assert.AreEqual("x5", node.ToString());
        }
    }
}
