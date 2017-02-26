using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Runtime;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class MarcRecordTest
    {
        [TestMethod]
        public void MarcRecord_Constructor_1()
        {
            MarcRecord record = new MarcRecord();

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
                MarcRecord record1
            )
        {
            byte[] bytes = record1.SaveToMemory();

            MarcRecord record2 = bytes
                .RestoreObjectFromMemory<MarcRecord>();
            Assert.IsNotNull(record2);
            Assert.AreEqual
                (
                    0,
                    MarcRecord.Compare
                    (
                        record1,
                        record2
                    )
                );
        }

        [TestMethod]
        public void MarcRecord_Serialization_1()
        {
            MarcRecord record = new MarcRecord();
            _TestSerialization(record);
            record.Fields.Add(new RecordField("200"));
            _TestSerialization(record);
            record.Fields.Add(new RecordField("300", "Hello"));
            _TestSerialization(record);
        }

        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();

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
        public void MarcRecord_FM_1()
        {
            MarcRecord record = _GetRecord();

            Assert.AreEqual("Иванов", record.FM("700", 'a'));
            Assert.AreEqual("Первое примечание", record.FM("300"));
        }

        [TestMethod]
        public void MarcRecord_FMA_1()
        {
            MarcRecord record = _GetRecord();

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
        public void MarcRecord_FR_1()
        {
            MarcRecord record = _GetRecord();

            string actual = record.FR("v200^a, \" : \"v200^e");
            string expected = "Заглавие : подзаголовочное";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MarcRecord_Modified_1()
        {
            MarcRecord record = _GetRecord();
            record.Modified = false;

            Assert.IsFalse(record.Modified);
            record.Fields[0].SubFields[1].Value = "О. О.";
            Assert.IsTrue(record.Modified);
        }

        [TestMethod]
        public void MarcRecord_Modified_2()
        {
            MarcRecord record = _GetRecord();
            record.Modified = false;

            Assert.IsFalse(record.Modified);
            record.Fields.Add(new RecordField("300", "Четвертое примечание"));
            Assert.IsTrue(record.Modified);
        }

        [TestMethod]
        public void MarcRecord_Modified_3()
        {
            MarcRecord record = _GetRecord();
            record.Modified = false;

            Assert.IsFalse(record.Modified);
            record.Fields.RemoveAt(record.Fields.Count - 1);
            Assert.IsTrue(record.Modified);
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void MarcRecord_ReadOnly_1()
        {
            MarcRecord record = _GetRecord().AsReadOnly();

            record.Fields.Add(new RecordField());
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void MarcRecord_ReadOnly_2()
        {
            MarcRecord record = _GetRecord().AsReadOnly();

            record.Status |= RecordStatus.LogicallyDeleted;
        }

        [TestMethod]
        public void MarcRecord_ToJson_1()
        {
            MarcRecord record = _GetRecord();

            string actual = record.ToJson()
                .Replace("\r", "").Replace("\n", "")
                .Replace("\"", "'");
            const string expected = "{'fields':[{'tag':'700','subfields':[{'code':'a','value':'Иванов'},{'code':'b','value':'И. И.'}]},{'tag':'701','subfields':[{'code':'a','value':'Петров'},{'code':'b','value':'П. П.'}]},{'tag':'200','subfields':[{'code':'a','value':'Заглавие'},{'code':'e','value':'подзаголовочное'},{'code':'f','value':'И. И. Иванов, П. П. Петров'}]},{'tag':'300','value':'Первое примечание'},{'tag':'300','value':'Второе примечание'},{'tag':'300','value':'Третье примечание'}]}";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MarcRecord_Renumbering_1()
        {
            MarcRecord record = _GetRecord();

            string[] tags = record.Fields
                .Select(field => field.Tag.ToUpper())
                .Distinct()
                .ToArray();

            foreach (string tag in tags)
            {
                RecordField[] fields = record.Fields.GetField(tag);
                for (int i = 0; i < fields.Length; i++)
                {
                    RecordField field = fields[i];
                    Assert.AreEqual(i + 1, field.Repeat);
                    Assert.AreEqual(record, field.Record);

                    foreach (SubField subField in field.SubFields)
                    {
                        Assert.AreEqual(field, subField.Field);
                    }
                }
            }
        }

        [TestMethod]
        public void MarcRecord_Verify_1()
        {
            MarcRecord record = _GetRecord();

            Assert.IsTrue(record.Verify(false));
        }

        [TestMethod]
        public void MarcRecord_ToString_1()
        {
            MarcRecord record = _GetRecord();

            Assert.IsNotNull(record.ToString());
        }

        [TestMethod]
        public void MarcRecord_Compare_1()
        {
            MarcRecord left = new MarcRecord();
            MarcRecord right = new MarcRecord();
            right.Fields.Add(new RecordField("300", "Примечание"));
            Assert.IsTrue
                (

                    MarcRecord.Compare(left, right) < 0
                );
        }

        [TestMethod]
        public void MarcRecord_Compare_2()
        {
            MarcRecord left = new MarcRecord();
            left.Fields.Add(new RecordField("300", "Примечание1"));
            MarcRecord right = new MarcRecord();
            right.Fields.Add(new RecordField("300", "Примечание2"));
            Assert.IsTrue
                (

                    MarcRecord.Compare(left, right) < 0
                );
        }

        [TestMethod]
        public void MarcRecord_HostName_1()
        {
            const string miron = "mironxp";
            MarcRecord record = new MarcRecord();
            record.HostName = miron;
            Assert.AreEqual(miron, record.HostName);
        }

        [TestMethod]
        public void MarcRecord_Deleted_1()
        {
            MarcRecord record = new MarcRecord();
            Assert.IsFalse(record.Deleted);
            Assert.AreEqual
                (
                    (RecordStatus)0,
                    record.Status & RecordStatus.LogicallyDeleted
                );
            record.Deleted = true;
            Assert.IsTrue(record.Deleted);
            Assert.AreEqual
                (
                    RecordStatus.LogicallyDeleted,
                    record.Status & RecordStatus.LogicallyDeleted
                );
            record.Deleted = false;
            Assert.AreEqual
                (
                    (RecordStatus)0,
                    record.Status & RecordStatus.LogicallyDeleted
                );
            Assert.IsFalse(record.Deleted);
        }
    }
}
