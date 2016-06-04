using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedClient;

namespace UnitTests
{
    [TestClass]
    public class RecordFieldTest
    {
        [TestMethod]
        public void TestRecordFieldConstruction()
        {
            RecordField field = new RecordField();
            Assert.AreEqual(RecordField.NoTag, field.Tag);
            Assert.AreEqual(null, field.Value);
            Assert.AreEqual(0,field.SubFields.Count);
        }

        private void _TestSerialization
            (
                RecordField field1
            )
        {
            byte[] bytes = field1.SaveToMemory();

            RecordField field2 = bytes
                .RestoreObjectFromMemory<RecordField>();

            Assert.AreEqual
                (
                    0,
                    RecordField.Compare
                    (
                        field1,
                        field2
                    )
                );
        }

        [TestMethod]
        public void TestRecordFieldSerialization()
        {
            _TestSerialization
                (
                    new RecordField()
                );
            _TestSerialization
                (
                    new RecordField("100")
                );
            _TestSerialization
                (
                    new RecordField("199", "Hello")
                );
            _TestSerialization
                (
                    new RecordField
                        (
                            "200",
                            new SubField('a', "Hello")
                        )
                );
        }

        private RecordField _GetField()
        {
            RecordField result = new RecordField("200", "Значение");

            result.AddSubField('a', "Заглавие");
            result.AddSubField('e', "подзаголовочные");
            result.AddSubField('f', "об ответственности");

            return result;
        }

        [TestMethod]
        public void TestRecordFieldToString()
        {
            RecordField field = _GetField();

            string actual = field.ToString();
            int result = string.CompareOrdinal
                (
                    "200#Значение^aЗаглавие^eподзаголовочные^fоб ответственности",
                    actual
                );

            Assert.AreEqual
                (
                    0,
                    result
                );
        }

        [TestMethod]
        public void TestRecordFieldToText()
        {
            RecordField field = _GetField();

            string actual = field.ToText();
            Assert.AreEqual
                (
                    "Значение^aЗаглавие^eподзаголовочные^fоб ответственности",
                    actual
                );
        }
    }
}
