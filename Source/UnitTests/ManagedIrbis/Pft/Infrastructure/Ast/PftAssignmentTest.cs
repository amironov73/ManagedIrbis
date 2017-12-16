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
    public class PftAssignmentTest
    {
        private void _Execute
            (
                [NotNull] PftAssignment node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            PftVariable variable = context.Variables.GetExistingVariable(node.Name);
            string actual = variable.StringValue.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        private void _Execute
            (
                [NotNull] PftAssignment node,
                double expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            PftVariable variable = context.Variables.GetExistingVariable(node.Name);
            double actual = variable.NumericValue;
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private PftAssignment _GetNumericAssignment()
        {
            return new PftAssignment
            {
                IsNumeric = true,
                Name = "x",
                Children =
                {
                    new PftNumericExpression
                    {
                        LeftOperand = new PftNumericLiteral(1),
                        Operation = "+",
                        RightOperand = new PftNumericExpression
                        {
                            LeftOperand = new PftNumericLiteral(2),
                            Operation = "*",
                            RightOperand = new PftNumericLiteral(3)
                        }
                    }
                }
            };
        }

        [NotNull]
        private PftAssignment _GetStringAssignment()
        {
            return new PftAssignment
            {
                IsNumeric = false,
                Name = "x",
                Children =
                {
                    new PftUnconditionalLiteral("Hello,"),
                    new PftComma(),
                    new PftUnconditionalLiteral(" world!")
                }
            };
        }

        [TestMethod]
        public void PftAssignment_Construction_1()
        {
            PftAssignment node = new PftAssignment();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNull(node.Name);
            Assert.AreEqual(0, node.Children.Count);
        }

        [TestMethod]
        public void PftAssignment_Construction_2()
        {
            string name = "x";
            PftToken token = new PftToken(PftTokenKind.Equals, 1, 1, name);
            PftAssignment node = new PftAssignment(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.AreSame(name, node.Name);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftAssignment_Construction_3()
        {
            string name = "x";
            PftAssignment node = new PftAssignment(name);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.AreSame(name, node.Name);
            Assert.AreEqual(0, node.Children.Count);
        }

        [TestMethod]
        public void PftAssignment_Clone_1()
        {
            PftAssignment first = new PftAssignment();
            PftAssignment second = (PftAssignment) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftAssignment_Clone_2()
        {
            PftAssignment first = _GetNumericAssignment();
            PftAssignment second = (PftAssignment)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftAssignment_Clone_3()
        {
            PftAssignment first = _GetStringAssignment();
            PftAssignment second = (PftAssignment)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftWhile_Compile_1()
        {
            PftProgram program = new PftProgram();
            PftAssignment node = _GetNumericAssignment();
            program.Children.Add(node);
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftWhile_Compile_2()
        {
            PftProgram program = new PftProgram();
            PftAssignment node = _GetStringAssignment();
            program.Children.Add(node);
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftAssignment_Execute_1()
        {
            PftAssignment node = new PftAssignment();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftAssignment_Execute_2()
        {
            PftAssignment node = _GetNumericAssignment();
            _Execute(node, 7);
        }

        [TestMethod]
        public void PftAssignment_Execute_3()
        {
            PftAssignment node = _GetStringAssignment();
            _Execute(node, "Hello, world!");
        }

        [TestMethod]
        public void PftAssignment_GetNodeInfo_1()
        {
            PftAssignment node = _GetNumericAssignment();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("Assignment", info.Name);
        }

        [TestMethod]
        public void PftAssignment_GetNodeInfo_2()
        {
            PftAssignment node = _GetStringAssignment();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("Assignment", info.Name);
        }

        [TestMethod]
        public void PftAssignment_PrettyPrint_1()
        {
            PftAssignment node = _GetNumericAssignment();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("$x=1 + 2 * 3;", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftAssignment_PrettyPrint_2()
        {
            PftAssignment node = _GetStringAssignment();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("$x='Hello,', ' world!';", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                [NotNull] PftAssignment first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftAssignment second = (PftAssignment) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftAssignment_Serialization_1()
        {
            PftAssignment node = new PftAssignment();
            _TestSerialization(node);

            node = _GetNumericAssignment();
            _TestSerialization(node);

            node = _GetStringAssignment();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftAssignment_ToString_1()
        {
            PftAssignment node = new PftAssignment();
            Assert.AreEqual("$=;", node.ToString());
        }

        [TestMethod]
        public void PftAssignment_ToString_2()
        {
            PftAssignment node = _GetNumericAssignment();
            Assert.AreEqual("$x=1+2*3;", node.ToString());
        }

        [TestMethod]
        public void PftAssignment_ToString_3()
        {
            PftAssignment node = _GetStringAssignment();
            Assert.AreEqual("$x='Hello,' , ' world!';", node.ToString());
        }
    }
}
