using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Runtime;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class FieldReferenceTest
    {
        [TestMethod]
        public void FieldReference_Constructor_1()
        {
            FieldReference reference = new FieldReference();
            Assert.AreEqual('\0', reference.Command);
            Assert.IsNull(reference.Embedded);
            Assert.AreEqual(0, reference.Indent);
            Assert.AreEqual(0, reference.Offset);
            Assert.AreEqual(0, reference.Length);
            Assert.AreEqual(FieldReference.NoCode, reference.SubField);
            Assert.AreEqual(0, reference.Tag);
            Assert.IsNull(reference.TagSpecification);
            Assert.IsNull(reference.SubFieldSpecification);
        }

        [TestMethod]
        public void FieldReference_Constructor_2()
        {
            FieldReference reference = new FieldReference(200);
            Assert.AreEqual('\0', reference.Command);
            Assert.IsNull(reference.Embedded);
            Assert.AreEqual(0, reference.Indent);
            Assert.AreEqual(0, reference.Offset);
            Assert.AreEqual(0, reference.Length);
            Assert.AreEqual(FieldReference.NoCode, reference.SubField);
            Assert.AreEqual(200, reference.Tag);
            Assert.IsNull(reference.TagSpecification);
            Assert.IsNull(reference.SubFieldSpecification);
        }

        [TestMethod]
        public void FieldReference_Constructor_3()
        {
            FieldReference reference = new FieldReference(200, 'a');
            Assert.AreEqual('\0', reference.Command);
            Assert.IsNull(reference.Embedded);
            Assert.AreEqual(0, reference.Indent);
            Assert.AreEqual(0, reference.Offset);
            Assert.AreEqual(0, reference.Length);
            Assert.AreEqual('a', reference.SubField);
            Assert.AreEqual(200, reference.Tag);
            Assert.IsNull(reference.TagSpecification);
            Assert.IsNull(reference.SubFieldSpecification);
        }

        private void _TestSerialization
        (
            FieldReference first
        )
        {
            byte[] bytes = first.SaveToMemory();


            FieldReference second = bytes
                .RestoreObjectFromMemory<FieldReference>();

            Assert.AreEqual(first.Command, second.Command);
            Assert.AreEqual(first.Embedded, second.Embedded);
            Assert.AreEqual(first.Indent, second.Indent);
            Assert.AreEqual(first.Offset, second.Offset);
            Assert.AreEqual(first.Length, second.Length);
            Assert.AreEqual(first.SubField, second.SubField);
            Assert.AreEqual(first.Tag, second.Tag);
            Assert.AreEqual(first.TagSpecification, second.TagSpecification);
            Assert.AreEqual(first.FieldRepeat, second.FieldRepeat);
            Assert.AreEqual(first.SubFieldRepeat, second.SubFieldRepeat);
            Assert.AreEqual(first.SubFieldSpecification, second.SubFieldSpecification);
        }

        [TestMethod]
        public void FieldReference_Serialization_1()
        {
            FieldReference reference = new FieldReference();
            _TestSerialization(reference);

            reference.Tag = 200;
            reference.SubField = 'a';
            _TestSerialization(reference);
        }

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
        public void FieldReference_Format_1()
        {
            MarcRecord record = _GetRecord();
            FieldReference reference = new FieldReference(200, 'a');

            string actual = reference.Format(record);
            Assert.AreEqual
                (
                    record.FM
                        (
                            reference.Tag,
                            reference.SubField
                        ),
                    actual
                );
        }

        [TestMethod]
        public void FieldReference_Parse_1()
        {
            FieldReference reference = FieldReference.Parse("v200");
            Assert.AreEqual('v', reference.Command);
            Assert.AreEqual(200, reference.Tag);
            Assert.AreEqual(FieldReference.NoCode, reference.SubField);
            Assert.AreEqual(0, reference.Offset);
            Assert.AreEqual(0, reference.Length);

            reference = FieldReference.Parse("v200^a");
            Assert.AreEqual('v', reference.Command);
            Assert.AreEqual(200, reference.Tag);
            Assert.AreEqual('a', reference.SubField);
            Assert.AreEqual(0, reference.Offset);
            Assert.AreEqual(0, reference.Length);

            reference = FieldReference.Parse("v200^a*5.7");
            Assert.AreEqual('v', reference.Command);
            Assert.AreEqual(200, reference.Tag);
            Assert.AreEqual('a', reference.SubField);
            Assert.AreEqual(5, reference.Offset);
            Assert.AreEqual(7, reference.Length);
        }

        private void _TestParse
            (
                string expected
            )
        {
            FieldReference reference = FieldReference.Parse(expected);
            Assert.AreEqual(200, reference.Tag);
        }

        [TestMethod]
        public void FieldReference_Parse_2()
        {
            _TestParse("v200");
            _TestParse("v200^a");
            _TestParse("v200[1]");
            _TestParse("v200*5");
            _TestParse("v200.2");
        }

        [TestMethod]
        public void FieldReference_Verify_1()
        {
            FieldReference reference = new FieldReference();
            Assert.IsFalse(reference.Verify(false));

            reference = new FieldReference(200, 'a');
            Assert.IsTrue(reference.Verify(false));
        }
    }
}
