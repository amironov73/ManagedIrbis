using System.IO;

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftProgramTest
    {
        private void _Execute
            (
                [NotNull] MarcRecord record,
                [NotNull] PftProgram program,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null)
            {
                Record = record
            };
            program.Execute(context);
            string actual = context.Output.Text.DosToUnix();
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
        private PftProgram _GetProgram()
        {
            return new PftProgram
            {
                Children =
                {
                    new PftUnconditionalLiteral("Hello"),
                    new PftSlash(),
                    new PftGroup
                    {
                        Children =
                        {
                            new PftConditionalStatement
                            {
                                Condition = new PftP("v200^a"),
                                ThenBranch =
                                {
                                    new PftV("v200^a"),
                                    new PftComma(),
                                    new PftSlash()
                                }
                            }
                        }
                    }
                }
            };
        }

        [TestMethod]
        public void PftProgram_Construction_1()
        {
            PftProgram program = new PftProgram();
            Assert.IsNotNull(program.Procedures);
            Assert.AreEqual(0, program.Procedures.Registry.Count);
            Assert.IsTrue(program.RequiresConnection);
            Assert.IsFalse(program.ConstantExpression);
        }

        [TestMethod]
        public void PftProgram_DumpToText_1()
        {
            PftProgram program = _GetProgram();
            string expected = "Program\n UnconditionalLiteral: Hello\n Slash\n Group\n" +
                              "  ConditionalStatement\n   Condition\n    P\n     Field\n" +
                              "      V: v200^a\n   Then\n    V: v200^a\n    Comma\n    Slash\n";
            string actual = program.DumpToText().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftProgram_Execute_1()
        {
            MarcRecord record = _GetRecord();
            PftProgram program = new PftProgram();
            _Execute(record, program, "");
        }

        [TestMethod]
        public void PftProgram_Execute_2()
        {
            MarcRecord record = _GetRecord();
            PftProgram program = _GetProgram();
            _Execute(record, program, "Hello\nЗаглавие\n");
        }

        [TestMethod]
        public void PftProgram_Execute_3()
        {
            MarcRecord record = _GetRecord();
            PftProgram program = new PftProgram
            {
                Children =
                {
                    new PftUnconditionalLiteral("Hello"),
                    new PftBreak(),
                    new PftUnconditionalLiteral("world")
                }
            };
            _Execute(record, program, "Hello");
        }

        [TestMethod]
        public void PftProgram_Execute_4()
        {
            MarcRecord record = _GetRecord();
            PftProgram program = new PftProgram
            {
                Children =
                {
                    new PftUnconditionalLiteral("Hello"),
                    new PftFunctionCall("exit"),
                    new PftUnconditionalLiteral("world")
                }
            };
            _Execute(record, program, "Hello");
        }

        private void _TestSerialization
            (
                [NotNull] PftProgram first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Save(first, writer);
            byte[] bytes = stream.ToArray();

            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftProgram second = (PftProgram) PftSerializer.Read(reader);
            Assert.IsNotNull(second);

            // TODO FIX THIS!
            //PftSerializationUtility.VerifyDeserializedProgram(first, second);
        }

        [TestMethod]
        public void PftProgram_Serialization_1()
        {
            PftProgram program = new PftProgram();
            _TestSerialization(program);

            program = _GetProgram();
            _TestSerialization(program);
        }

        [TestMethod]
        public void PftProgram_ToString_1()
        {
            PftProgram program = new PftProgram();
            Assert.AreEqual("", program.ToString());

            program = _GetProgram();
            Assert.AreEqual("\'Hello\' /\n(\n  if p(v200^a)\n  then v200^a, /\n  fi,\n)\n", program.ToString().DosToUnix());
        }
    }
}
