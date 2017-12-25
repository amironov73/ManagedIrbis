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
    public class PftPTest
    {
        private void _Execute
            (
                [NotNull] MarcRecord record,
                [NotNull] PftP node,
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
        private PftP _GetVNode()
        {
            return new PftP(200, 'a');
        }

        [NotNull]
        private PftP _GetGNode(int index)
        {
            return new PftP
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
        public void PftP_Construction_1()
        {
            PftP node = new PftP();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.Field);
        }

        [TestMethod]
        public void PftP_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.P, 1, 1, "p");
            PftP node = new PftP(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.Field);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftP_Construction_3()
        {
            string text = "v200^a";
            PftP node = new PftP(text);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNotNull(node.Field);
            Assert.AreEqual(text, node.Field.ToString());
        }

        [TestMethod]
        public void PftP_Construction_4()
        {
            PftP node = new PftP(200);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNotNull(node.Field);
            Assert.AreEqual("v200", node.Field.ToString());
        }

        [TestMethod]
        public void PftP_Construction_5()
        {
            PftP node = new PftP(200, 'a');
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNotNull(node.Field);
            Assert.AreEqual("v200^a", node.Field.ToString());
        }

        [TestMethod]
        public void PftP_Clone_1()
        {
            PftP first = new PftP();
            PftP second = (PftP)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftP_Clone_2()
        {
            PftP first = _GetVNode();
            PftP second = (PftP)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftP_Compile_1()
        {
            PftP node = _GetVNode();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftP_Compile_2()
        {
            PftP node = _GetGNode(100);
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftP_Compile_3()
        {
            PftP node = new PftP();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftP_Execute_1()
        {
            MarcRecord record = _GetRecord();
            PftP node = new PftP();
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftP_Execute_2()
        {
            MarcRecord record = _GetRecord();
            PftP node = _GetVNode();
            _Execute(record, node, true);
        }

        [TestMethod]
        public void PftP_Execute_3()
        {
            MarcRecord record = _GetRecord();
            PftP node = _GetGNode(100);
            _Execute(record, node, true);
        }

        [TestMethod]
        public void PftP_Execute_3a()
        {
            MarcRecord record = _GetRecord();
            PftP node = _GetGNode(100);
            Assert.IsNotNull(node.Field);
            node.Field.SubField = 'a';
            _Execute(record, node, false);
        }

        [TestMethod]
        public void PftP_Execute_3b()
        {
            MarcRecord record = _GetRecord();
            PftP node = _GetGNode(100);
            Assert.IsNotNull(node.Field);
            node.Field.SubField = '*';
            _Execute(record, node, true);
        }

        [TestMethod]
        public void PftP_Execute_3c()
        {
            MarcRecord record = _GetRecord();
            PftP node = _GetGNode(100);
            Assert.IsNotNull(node.Field);
            node.Field.Offset = 1;
            node.Field.Length = 2;
            _Execute(record, node, true);
        }

        [TestMethod]
        public void PftP_Execute_4()
        {
            MarcRecord record = _GetRecord();
            PftP node = _GetGNode(101);
            _Execute(record, node, false);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftP_Execute_5()
        {
            MarcRecord record = _GetRecord();
            PftP node = new PftP
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
        public void PftP_GetNodeInfo_1()
        {
            PftP node = _GetVNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("P", info.Name);
        }

        [TestMethod]
        public void PftP_GetNodeInfo_2()
        {
            PftP node = _GetGNode(100);
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("P", info.Name);
        }

        private void _TestSerialization
            (
                [NotNull] PftP first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftP second = (PftP)PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftRef_Serialization_1()
        {
            PftP node = new PftP();
            _TestSerialization(node);

            node = _GetVNode();
            _TestSerialization(node);

            node = _GetGNode(100);
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftP_ToString_1()
        {
            PftP node = new PftP();
            Assert.AreEqual("p()", node.ToString());

            node = _GetVNode();
            Assert.AreEqual("p(v200^a)", node.ToString());

            node = _GetGNode(100);
            Assert.AreEqual("p(g100)", node.ToString());
        }
    }
}
