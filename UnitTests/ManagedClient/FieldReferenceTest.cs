using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedClient;

namespace UnitTests.ManagedClient
{
    [TestClass]
    public class FieldReferenceTest
    {
        private void _TestSerialization
            (
                FieldReference first
            )
        {
            byte[] bytes = first.SaveToMemory();


            FieldReference second = bytes
                .RestoreObjectFromMemory<FieldReference>();

            Assert.AreEqual(first.Command, second.Command);
            Assert.AreEqual(first.ConditionalPrefix, second.ConditionalPrefix);
            Assert.AreEqual(first.ConditionalSuffix, second.ConditionalSuffix);
            Assert.AreEqual(first.EmbeddedTag, second.EmbeddedTag);
            Assert.AreEqual(first.FieldTag, second.FieldTag);
            Assert.AreEqual(first.Length, second.Length);
            Assert.AreEqual(first.IndexFrom, second.IndexFrom);
            Assert.AreEqual(first.IndexTo, second.IndexTo);
            Assert.AreEqual(first.Offset, second.Offset);
            Assert.AreEqual(first.PlusPrefix, second.PlusPrefix);
            Assert.AreEqual(first.PlusSuffix, second.PlusSuffix);
            Assert.AreEqual(first.RepeatablePrefix, second.RepeatablePrefix);
            Assert.AreEqual(first.RepeatableSuffix, second.RepeatableSuffix);
            Assert.AreEqual(first.SubField, second.SubField);
        }

        [TestMethod]
        public void TestFieldReferenceSerialization()
        {
            FieldReference reference = new FieldReference();
            _TestSerialization(reference);

            reference.FieldTag = "200";
            reference.SubField = 'a';
            _TestSerialization(reference);
        }

        [TestMethod]
        public void TestFieldReferenceToSourceCode()
        {
            FieldReference reference = new FieldReference("200", 'a');
            Assert.AreEqual("v200^a", reference.ToSourceCode());

            reference.RepeatableSuffix = ",";
            reference.PlusSuffix = true;
            Assert.AreEqual("v200^a+|,|", reference.ToSourceCode());

            reference = new FieldReference("200", 'a')
            {
                IndexFrom = 2
            };
            Assert.AreEqual("v200^a[2-]", reference.ToSourceCode());
            reference.IndexTo = 2;
            Assert.AreEqual("v200^a[2]", reference.ToSourceCode());
            reference.IndexTo = 3;
            Assert.AreEqual("v200^a[2-3]", reference.ToSourceCode());
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestFieldReferenceToSourceCodeException()
        {
            FieldReference reference = new FieldReference();
            reference.ToSourceCode();
        }

        private IrbisRecord _GetRecord()
        {
            IrbisRecord result = new IrbisRecord();

            RecordField field = new RecordField("700");
            field.AddSubField('a', "Иванов");
            field.AddSubField('b', "И. И.");
            result.Fields.Add(field);

            field = new RecordField("701");
            field.AddSubField('a', "Петров");
            field.AddSubField('b', "П. П.");
            result.Fields.Add(field);

            field = new RecordField("200");
            field.AddSubField('a', "Заглавие");
            field.AddSubField('e', "подзаголовочное");
            field.AddSubField('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new RecordField("300", "Первое примечание");
            result.Fields.Add(field);
            field = new RecordField("300", "Второе примечание");
            result.Fields.Add(field);
            field = new RecordField("300", "Третье примечание");
            result.Fields.Add(field);

            return result;
        }

        [TestMethod]
        public void TestFieldReferenceFormatSingle()
        {
            IrbisRecord record = _GetRecord();
            FieldReference reference = new FieldReference("200", 'a');

            string actual = reference.FormatSingle(record);
            Assert.AreEqual(record.FM(reference.FieldTag, reference.SubField),
                actual);
        }
    }
}
