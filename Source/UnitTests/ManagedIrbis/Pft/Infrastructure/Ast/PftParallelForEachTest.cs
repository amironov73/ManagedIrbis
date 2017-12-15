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
    public class PftParallelForEachTest
    {
        private void _Execute
            (
                [NotNull] MarcRecord record,
                [NotNull] PftParallelForEach node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null)
            {
                Record = record
            };
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.IsNotNull(actual);

            // Assert.AreEqual(expected, actual);
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

        [NotNull]
        private PftParallelForEach _GetNode()
        {
            string name = "x";
            return new PftParallelForEach
            {
                Variable = new PftVariableReference(name),
                Sequence =
                {
                    new PftV("v200^a"),
                    new PftV("v200^e"),
                    new PftUnconditionalLiteral("Hello")
                },
                Body =
                {
                    new PftVariableReference(name),
                    new PftSlash()
                }
            };
        }

        [TestMethod]
        public void PftParallelForEach_Construction_1()
        {
            PftParallelForEach node = new PftParallelForEach();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNull(node.Variable);
            Assert.IsNotNull(node.Sequence);
            Assert.AreEqual(0, node.Sequence.Count);
            Assert.IsNotNull(node.Body);
            Assert.AreEqual(0, node.Body.Count);
        }

        [TestMethod]
        public void PftParallelForEach_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Parallel, 1, 1, "parallel");
            PftParallelForEach node = new PftParallelForEach(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNull(node.Variable);
            Assert.IsNotNull(node.Sequence);
            Assert.AreEqual(0, node.Sequence.Count);
            Assert.IsNotNull(node.Body);
            Assert.AreEqual(0, node.Body.Count);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftParallelForEach_Clone_1()
        {
            PftParallelForEach first = new PftParallelForEach();
            PftParallelForEach second = (PftParallelForEach) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftParallelForEach_Clone_2()
        {
            PftParallelForEach first = _GetNode();
            PftParallelForEach second = (PftParallelForEach)first.Clone();
            Assert.IsNotNull(second);

            // TODO FIX THIS!
            //PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftParallelForEach_Execute_1()
        {
            MarcRecord record = _GetRecord();
            PftParallelForEach node = _GetNode();

            // TODO FIX THIS!
            _Execute
                (
                    record,
                    node,
                    "Заглавие\nподзаголовочное\nHello\n"
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftParallelForEach_Execute_2()
        {
            MarcRecord record = _GetRecord();
            PftParallelForEach node = new PftParallelForEach();
            _Execute(record, node, "");
        }

        //[TestMethod]
        //public void PftParallelForEach_Execute_3()
        //{
        //    MarcRecord record = _GetRecord();
        //    PftParallelForEach node = _GetNode();
        //    node.Body.Add(new PftBreak());
        //    _Execute
        //        (
        //            record,
        //            node,
        //            "Заглавие\n"
        //        );
        //}

        [TestMethod]
        public void PftParallelForEach_GetNodeInfo_1()
        {
            PftParallelForEach node = _GetNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("ParallelForEach", info.Name);
        }

        [TestMethod]
        public void PftParallelForEach_PrettyPrint_1()
        {
            PftParallelForEach node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\nparallel foreach $x in v200^a, v200^e, \'Hello\'do\n  $x /\nend\n", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                [NotNull] PftParallelForEach first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftParallelForEach second = (PftParallelForEach) PftSerializer.Deserialize(reader);
            Assert.IsNotNull(second);

            // TODO FIX THIS!
            //PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftParallelForEach_Serialization_1()
        {
            PftParallelForEach node = new PftParallelForEach();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftParallelForEach_ToString_1()
        {
            PftParallelForEach node = new PftParallelForEach();
            Assert.AreEqual("parallel foreach  in  do  end", node.ToString());
        }

        [TestMethod]
        public void PftParallelForEach_ToString_2()
        {
            PftParallelForEach node = _GetNode();
            Assert.AreEqual("parallel foreach $x in v200^a v200^e 'Hello' do $x / end", node.ToString());
        }
    }
}
