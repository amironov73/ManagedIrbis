using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Runtime;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
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

            record.Fields.Add(new RecordField());

            Assert.AreEqual(record, record.Fields[0].Record);
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

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void TestIrbisRecordReadOnly()
        {
            IrbisRecord record = _GetRecord().AsReadOnly();

            record.Fields.Add(new RecordField());
        }

        [TestMethod]
        public void TestIrbisRecordToJson()
        {
            IrbisRecord record = _GetRecord();

            string actual = record.ToJson()
                .Replace("\r", "").Replace("\n", "")
                .Replace("\"", "'");
            const string expected = "{'fields':[{'tag':'700','subfields':[{'code':'a','value':'Иванов'},{'code':'b','value':'И. И.'}]},{'tag':'701','subfields':[{'code':'a','value':'Петров'},{'code':'b','value':'П. П.'}]},{'tag':'200','subfields':[{'code':'a','value':'Заглавие'},{'code':'e','value':'подзаголовочное'},{'code':'f','value':'И. И. Иванов, П. П. Петров'}]},{'tag':'300','value':'Первое примечание'},{'tag':'300','value':'Второе примечание'},{'tag':'300','value':'Третье примечание'}]}";

            Assert.AreEqual(expected, actual);
        }
    }
}
