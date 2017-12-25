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

// ReSharper disable UseNullPropagation

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftProcedureDefinitionTest
    {
        private void _Execute
            (
                [NotNull] MarcRecord record,
                [NotNull] PftProcedureDefinition node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null)
            {
                Record = record
            };

            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(string.Empty, actual);

            if (!ReferenceEquals(node.Procedure, null))
            {
                node.Procedure.Execute(context, string.Empty);
            }
            actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
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
        private PftProcedureDefinition _GetNode()
        {
            return new PftProcedureDefinition
            {
                Procedure = new PftProcedure
                {
                    Name = "ShowRecord",
                    Body =
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
                }
            };
        }

        [TestMethod]
        public void PftProcedureDefinition_Construction_1()
        {
            PftProcedureDefinition node = new PftProcedureDefinition();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.Procedure);
        }

        [TestMethod]
        public void PftProcedureDefinition_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Proc, 1, 1, "proc");
            PftProcedureDefinition node = new PftProcedureDefinition(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftProcedureDefinition_Children_1()
        {
            PftProcedureDefinition node = _GetNode();
            Assert.AreEqual(3, node.Children.Count);
        }

        [TestMethod]
        public void PftProcedureDefinition_Clone_1()
        {
            PftProcedureDefinition first = new PftProcedureDefinition();
            PftProcedureDefinition second = (PftProcedureDefinition)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftProcedureDefinition_Clone_2()
        {
            PftProcedureDefinition first = _GetNode();
            PftProcedureDefinition second = (PftProcedureDefinition)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftProcedureDefinition_Execute_1()
        {
            MarcRecord record = _GetRecord();
            PftProcedureDefinition node = new PftProcedureDefinition();
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftProcedureDefinition_Execute_2()
        {
            MarcRecord record = _GetRecord();
            PftProcedureDefinition node = _GetNode();
            _Execute(record, node, "Заглавие : подзаголовочное / И. И. Иванов, П. П. Петров");
        }

        [TestMethod]
        public void PftProcedureDefinition_GetNodeInfo_1()
        {
            PftProcedureDefinition node = _GetNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("Procedure", info.Name);
        }

        private void _TestSerialization
            (
                [NotNull] PftProcedureDefinition first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftProcedureDefinition second
                = (PftProcedureDefinition)PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftProcedureDefinition_Serialization_1()
        {
            PftProcedureDefinition node = new PftProcedureDefinition();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftProcedureDefinition_ToString_1()
        {
            PftProcedureDefinition node = new PftProcedureDefinition();
            Assert.AreEqual("", node.ToString());
        }
    }
}
