using System.IO;

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

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftATest
        : Common.CommonUnitTest
    {
        private void _Execute
        (
            [NotNull] MarcRecord record,
            [NotNull] PftA node,
            bool expected
        )
        {
            PftContext context = new PftContext(null)
            {
                Record = record
            };
            context.Globals.Add(100, "global100");
            node.Execute(context);
            bool actual = node.Value;
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private PftA _GetVNode()
        {
            return new PftA(200, 'a');
        }

        [NotNull]
        private PftA _GetGNode(int index)
        {
            return new PftA
            {
                Field = new PftG(index)
            };
        }

        [NotNull]
        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();

            RecordField field = new RecordField(700);
            field.AddSubField('a', "Иванов");
            field.AddSubField('b', "И. И.");
            result.Fields.Add(field);

            field = new RecordField(701);
            field.AddSubField('a', "Петров");
            field.AddSubField('b', "П. П.");
            result.Fields.Add(field);

            field = new RecordField(200);
            field.AddSubField('a', "Заглавие");
            field.AddSubField('e', "подзаголовочное");
            field.AddSubField('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new RecordField(300, "Первое примечание");
            result.Fields.Add(field);
            field = new RecordField(300, "Второе примечание");
            result.Fields.Add(field);
            field = new RecordField(300, "Третье примечание");
            result.Fields.Add(field);

            return result;
        }

        [TestMethod]
        public void PftA_Construction_1()
        {
            PftA node = new PftA();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.Field);
        }

        [TestMethod]
        public void PftA_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.P, 1, 1, "p");
            PftA node = new PftA(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.Field);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftA_Construction_3()
        {
            string text = "v200^a";
            PftA node = new PftA(text);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNotNull(node.Field);
            Assert.AreEqual(text, node.Field.ToString());
        }

        [TestMethod]
        public void PftA_Construction_4()
        {
            PftA node = new PftA(200);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNotNull(node.Field);
            Assert.AreEqual("v200", node.Field.ToString());
        }

        [TestMethod]
        public void PftA_Construction_5()
        {
            PftA node = new PftA(200, 'a');
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNotNull(node.Field);
            Assert.AreEqual("v200^a", node.Field.ToString());
        }

        [TestMethod]
        public void PftA_Clone_1()
        {
            PftA first = new PftA();
            PftA second = (PftA)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftA_Clone_2()
        {
            PftA first = _GetVNode();
            PftA second = (PftA)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftA_Compile_1()
        {
            PftA node = _GetVNode();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftA_Compile_2()
        {
            PftA node = _GetGNode(100);
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftA_Compile_3()
        {
            PftA node = new PftA();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftA_Execute_1()
        {
            MarcRecord record = _GetRecord();
            PftA node = new PftA();
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftA_Execute_2()
        {
            MarcRecord record = _GetRecord();
            PftA node = _GetVNode();
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftA_Execute_3()
        {
            MarcRecord record = _GetRecord();
            PftA node = _GetGNode(100);
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftA_Execute_3a()
        {
            MarcRecord record = _GetRecord();
            PftA node = _GetGNode(100);
            Assert.IsNotNull(node.Field);
            node.Field.SubField = 'a';
            _Execute(record, node, true);
        }

        [TestMethod]
        public void PftA_Execute_3b()
        {
            MarcRecord record = _GetRecord();
            PftA node = _GetGNode(100);
            Assert.IsNotNull(node.Field);
            node.Field.SubField = '*';
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftA_Execute_3c()
        {
            MarcRecord record = _GetRecord();
            PftA node = _GetGNode(100);
            Assert.IsNotNull(node.Field);
            node.Field.Offset = 1;
            node.Field.Length = 2;
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftA_Execute_4()
        {
            MarcRecord record = _GetRecord();
            PftA node = _GetGNode(101);
            _Execute(record, node, true);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftA_Execute_5()
        {
            MarcRecord record = _GetRecord();
            PftA node = new PftA
            {
                Field = new PftField
                {
                    Command = 'd',
                    Tag = "100"
                }
            };
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftA_GetNodeInfo_1()
        {
            PftA node = _GetVNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("A", info.Name);
        }

        [TestMethod]
        public void PftA_GetNodeInfo_2()
        {
            PftA node = _GetGNode(100);
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("A", info.Name);
        }

        private void _TestSerialization
            (
                [NotNull] PftA first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftA second = (PftA)PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftRef_Serialization_1()
        {
            PftA node = new PftA();
            _TestSerialization(node);

            node = _GetVNode();
            _TestSerialization(node);

            node = _GetGNode(100);
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftA_ToString_1()
        {
            PftA node = new PftA();
            Assert.AreEqual("a()", node.ToString());

            node = _GetVNode();
            Assert.AreEqual("a(v200^a)", node.ToString());

            node = _GetGNode(100);
            Assert.AreEqual("a(g100)", node.ToString());
        }
    }
}
