using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedClient;

namespace UnitTests.ManagedClient
{
    [TestClass]
    public class IrbisRecordTest
    {
        [TestMethod]
        public void TestIrbisRecordConstruction()
        {
            IrbisRecord record = new IrbisRecord();

            Assert.IsNotNull(record);
            Assert.IsNotNull(record.Fields);
            Assert.IsNull(record.Database);
            Assert.IsNull(record.Description);
            Assert.AreEqual(0, record.Version);
        }

        private void _TestSerialization
            (
                IrbisRecord record1
            )
        {
            byte[] bytes = record1.SaveToMemory();

            IrbisRecord record2 = bytes
                .RestoreObjectFromMemory<IrbisRecord>();
            Assert.IsNotNull(record2);
            Assert.AreEqual
                (
                    0,
                    IrbisRecord.Compare
                    (
                        record1,
                        record2
                    )
                );
        }

        [TestMethod]
        public void TestIrbisRecordSerialization()
        {
            IrbisRecord record = new IrbisRecord();
            _TestSerialization(record);
            record.Fields.Add(new RecordField("200"));
            _TestSerialization(record);
            record.Fields.Add(new RecordField("300", "Hello"));
            _TestSerialization(record);
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
        public void TestIrbisRecordFM()
        {
            IrbisRecord record = _GetRecord();

            Assert.AreEqual("Иванов", record.FM("700", 'a'));
            Assert.AreEqual("Первое примечание", record.FM("300"));
        }

        [TestMethod]
        public void TestIrbisRecordFMA()
        {
            IrbisRecord record = _GetRecord();

            string[] actual = record.FMA("700", 'a');
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("Иванов", actual[0]);

            actual = record.FMA("300");
            Assert.AreEqual(3, actual.Length);
            Assert.AreEqual("Первое примечание", actual[0]);
            Assert.AreEqual("Второе примечание", actual[1]);
            Assert.AreEqual("Третье примечание", actual[2]);
        }

        [TestMethod]
        public void TestIrbisRecordFR()
        {
            IrbisRecord record = _GetRecord();

            string actual = record.FR("\"Автор: \"v700^a");
            Assert.AreEqual("Автор: Иванов", actual);
        }

        [TestMethod]
        public void TestIrbisRecordFRA()
        {
            IrbisRecord record = _GetRecord();

            string[] actual = record.FRA("\"Автор: \"v700^a");
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("Автор: Иванов", actual[0]);
        }
    }
}
