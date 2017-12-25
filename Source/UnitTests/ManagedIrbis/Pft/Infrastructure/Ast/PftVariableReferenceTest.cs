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
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftVariableReferenceTest
    {
        private void _Execute
            (
                [NotNull] PftVariableReference node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            context.Variables.SetVariable("t", "^aSubfieldA^bSubfieldB");
            context.Variables.SetVariable("x", "VariableX");
            context.Variables.SetVariable("y", 3.14);
            context.Variables.SetVariable("z", "VariableZ1\nVariableZ2");
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftVariableReference_Construction_1()
        {
            PftVariableReference node = new PftVariableReference();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.Name);
            Assert.AreEqual(0.0, node.Value);
            Assert.AreEqual(IndexKind.None, node.Index.Kind);
            Assert.AreEqual('\0', node.SubFieldCode);
        }

        [TestMethod]
        public void PftVariableReference_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Variable, 1, 1, "$x");
            PftVariableReference node = new PftVariableReference(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual("x", node.Name);
            Assert.AreEqual(0.0, node.Value);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
            Assert.AreEqual(IndexKind.None, node.Index.Kind);
            Assert.AreEqual('\0', node.SubFieldCode);
        }

        [TestMethod]
        public void PftVariableReference_Construction_3()
        {
            string name = "name";
            PftVariableReference node = new PftVariableReference(name);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreSame(name, node.Name);
            Assert.AreEqual(0.0, node.Value);
            Assert.AreEqual(IndexKind.None, node.Index.Kind);
            Assert.AreEqual('\0', node.SubFieldCode);
        }

        [TestMethod]
        public void PftVariableReference_Construction_4()
        {
            string name = "name";
            PftVariableReference node = new PftVariableReference(name, 5);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreSame(name, node.Name);
            Assert.AreEqual(0.0, node.Value);
            Assert.AreEqual(IndexKind.Literal, node.Index.Kind);
            Assert.AreEqual(5, node.Index.Literal);
            Assert.AreEqual('\0', node.SubFieldCode);
        }

        [TestMethod]
        public void PftVariableReference_Construction_5()
        {
            string name = "name";
            PftVariableReference node = new PftVariableReference(name, 'a');
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreSame(name, node.Name);
            Assert.AreEqual(0.0, node.Value);
            Assert.AreEqual(IndexKind.None, node.Index.Kind);
            Assert.AreEqual('a', node.SubFieldCode);
        }

        [TestMethod]
        public void PftVariableReference_Clone_1()
        {
            PftVariableReference first = new PftVariableReference();
            PftVariableReference second = (PftVariableReference)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftVariableReference_Clone_2()
        {
            PftVariableReference first = new PftVariableReference("x", 5);
            PftVariableReference second = (PftVariableReference)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSerializationException))]
        public void PftVariableReference_Clone_3()
        {
            PftVariableReference first = new PftVariableReference("x", 5);
            PftVariableReference second = (PftVariableReference)first.Clone();
            second.SubFieldCode = 'a';
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftVariableReference_Compile_1()
        {
            PftNode node = new PftVariableReference("x");
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftVariableReference_Compile_2()
        {
            PftNode node = new PftVariableReference();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftVariableReference_Execute_1()
        {
            PftVariableReference node = new PftVariableReference();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftVariableReference_Execute_2()
        {
            PftVariableReference node = new PftVariableReference("x");
            _Execute(node, "VariableX");
        }

        [TestMethod]
        public void PftVariableReference_Execute_3()
        {
            PftVariableReference node = new PftVariableReference("y");
            _Execute(node, "3.14");
        }

        [TestMethod]
        public void PftVariableReference_Execute_4()
        {
            PftVariableReference node = new PftVariableReference("z", 1);
            _Execute(node, "VariableZ1");

            node = new PftVariableReference("z", 2);
            _Execute(node, "VariableZ2");

            node = new PftVariableReference("z");
            _Execute(node, "VariableZ1\nVariableZ2");
        }

        [TestMethod]
        public void PftVariableReference_Execute_5()
        {
            PftVariableReference node = new PftVariableReference("t", 'a');
            _Execute(node, "SubfieldA");

            node = new PftVariableReference("t", 'b');
            _Execute(node, "SubfieldB");

            node = new PftVariableReference("t", 'c');
            _Execute(node, "");
        }

        [TestMethod]
        public void PftVariableReference_Execute_6()
        {
            PftVariableReference node = new PftVariableReference("t", 'a')
            {
                Index = IndexSpecification.GetLiteral(1)
            };
            _Execute(node, "SubfieldA");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void PftVariableReference_Execute_7()
        {
            PftVariableReference node = new PftVariableReference("noSuchVariable");
            _Execute(node, "");
        }

        [TestMethod]
        public void PftVariableReference_GetNodeInfo_1()
        {
            PftVariableReference node = new PftVariableReference("x");
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("VariableReference", info.Name);
        }

        [TestMethod]
        public void PftVariableReference_GetNodeInfo_2()
        {
            PftVariableReference node = new PftVariableReference("x", 2);
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("VariableReference", info.Name);
        }

        [TestMethod]
        public void PftVariableReference_PrettyPrint_1()
        {
            PftVariableReference node = new PftVariableReference("x");
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("$x", printer.ToString());
        }

        [TestMethod]
        public void PftVariableReference_PrettyPrint_2()
        {
            PftVariableReference node = new PftVariableReference("x", 5);
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("$x[5]", printer.ToString());
        }

        private void _TestSerialization
            (
                [NotNull] PftVariableReference first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftVariableReference second = (PftVariableReference)PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftVariableReference_Serialization_1()
        {
            PftVariableReference node = new PftVariableReference();
            _TestSerialization(node);

            node = new PftVariableReference("x");
            _TestSerialization(node);

            // TODO Fix this!
            //node = new PftVariableReference("x", 5);
            //_TestSerialization(node);

            node = new PftVariableReference("x", 'a');
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftVariableReference_ToString_1()
        {
            PftVariableReference node = new PftVariableReference("x");
            Assert.AreEqual("$x", node.ToString());

            node = new PftVariableReference("x", 5);
            Assert.AreEqual("$x[5]", node.ToString());

            node = new PftVariableReference("x", 'a');
            Assert.AreEqual("$x^a", node.ToString());
        }
    }
}
