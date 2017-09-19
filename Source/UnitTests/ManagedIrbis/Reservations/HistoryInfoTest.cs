using System;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Reservations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Reservations
{
    [TestClass]
    public class HistoryInfoTest
    {
        [TestMethod]
        public void HistoryInfo_Construction()
        {
            HistoryInfo history = new HistoryInfo();
            Assert.IsNull(history.DateString);
            Assert.IsNull(history.BeginTimeString);
            Assert.IsNull(history.EndTimeString);
            Assert.IsNull(history.Ticket);
            Assert.IsNull(history.Name);
            Assert.IsNull(history.Field);
            Assert.IsNull(history.UserData);
        }

        private void _TestSerialization
            (
                [NotNull] HistoryInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            HistoryInfo second = bytes.RestoreObjectFromMemory<HistoryInfo>();
            Assert.AreEqual(first.DateString, second.DateString);
            Assert.AreEqual(first.BeginTimeString, second.BeginTimeString);
            Assert.AreEqual(first.EndTimeString, second.EndTimeString);
            Assert.AreEqual(first.Ticket, second.Ticket);
            Assert.AreEqual(first.Name, second.Name);
            Assert.IsNull(second.Field);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void HistoryInfo_Serialization_1()
        {
            HistoryInfo history = new HistoryInfo();
            _TestSerialization(history);

            history.UserData = "User data";
            _TestSerialization(history);

            history.Date = DateTime.Today;
            history.BeginTime = new TimeSpan(13, 0, 0);
            history.EndTime = new TimeSpan(14, 0, 0);
            history.Ticket = "111";
            history.Name = "Reader";
            _TestSerialization(history);
        }

        [TestMethod]
        public void HistoryInfo_ParseField_1()
        {
            RecordField field = new RecordField(HistoryInfo.Tag)
                .AddSubField('a', "20170901")
                .AddSubField('b', "130000")
                .AddSubField('c', "140000")
                .AddSubField('d', "111")
                .AddSubField('e', "Reader");
            HistoryInfo history = HistoryInfo.ParseField(field);
            Assert.AreSame(history.Field, field);
            Assert.AreEqual("20170901", history.DateString);
            Assert.AreEqual("130000", history.BeginTimeString);
            Assert.AreEqual("140000", history.EndTimeString);
            Assert.AreEqual("111", history.Ticket);
            Assert.AreEqual("Reader", history.Name);
        }

        [TestMethod]
        public void HistoryInfo_ParseRecord_1()
        {
            RecordField field1 = new RecordField(HistoryInfo.Tag)
                .AddSubField('a', "20170901")
                .AddSubField('b', "130000")
                .AddSubField('c', "140000")
                .AddSubField('d', "111")
                .AddSubField('e', "Reader1");
            RecordField field2 = new RecordField(HistoryInfo.Tag)
                .AddSubField('a', "20170902")
                .AddSubField('b', "140000")
                .AddSubField('c', "150000")
                .AddSubField('d', "222")
                .AddSubField('e', "Reader2");
            MarcRecord record = new MarcRecord();
            record.Fields.Add(field1);
            record.Fields.Add(field2);
            HistoryInfo[] history = HistoryInfo.ParseRecord(record);
            Assert.AreEqual(2, history.Length);
            Assert.AreSame(history[0].Field, field1);
            Assert.AreEqual("20170901", history[0].DateString);
            Assert.AreEqual("130000", history[0].BeginTimeString);
            Assert.AreEqual("140000", history[0].EndTimeString);
            Assert.AreEqual("111", history[0].Ticket);
            Assert.AreEqual("Reader1", history[0].Name);
            Assert.AreSame(history[1].Field, field2);
            Assert.AreEqual("20170902", history[1].DateString);
            Assert.AreEqual("140000", history[1].BeginTimeString);
            Assert.AreEqual("150000", history[1].EndTimeString);
            Assert.AreEqual("222", history[1].Ticket);
            Assert.AreEqual("Reader2", history[1].Name);
        }

        [TestMethod]
        public void HistoryInfo_ToField_1()
        {
            HistoryInfo history = new HistoryInfo();
            RecordField field = history.ToField();
            Assert.AreEqual(HistoryInfo.Tag, field.Tag);
            Assert.AreEqual(0, field.SubFields.Count);
        }

        [TestMethod]
        public void HistoryInfo_ToField_2()
        {
            HistoryInfo history = new HistoryInfo
            {
                DateString = "20170901",
                BeginTimeString = "130000",
                EndTimeString = "140000",
                Ticket = "111",
                Name = "Reader"
            };
            RecordField field = history.ToField();
            Assert.AreEqual(HistoryInfo.Tag, field.Tag);
            Assert.AreEqual(5, field.SubFields.Count);
            Assert.AreEqual("20170901", field.GetFirstSubFieldValue('a'));
            Assert.AreEqual("130000", field.GetFirstSubFieldValue('b'));
            Assert.AreEqual("140000", field.GetFirstSubFieldValue('c'));
            Assert.AreEqual("111", field.GetFirstSubFieldValue('d'));
            Assert.AreEqual("Reader", field.GetFirstSubFieldValue('e'));
        }

        [TestMethod]
        public void HistoryInfo_ToXml_1()
        {
            HistoryInfo history = new HistoryInfo();
            Assert.AreEqual("<history />", XmlUtility.SerializeShort(history));
            history = new HistoryInfo
            {
                DateString = "20170901",
                BeginTimeString = "130000",
                EndTimeString = "140000",
                Ticket = "111",
                Name = "Reader"
            };
            Assert.AreEqual("<history><date>20170901</date><beginTime>130000</beginTime><endTime>140000</endTime><ticket>111</ticket><name>Reader</name></history>", XmlUtility.SerializeShort(history));
        }

        [TestMethod]
        public void HistoryInfo_ToJson_1()
        {
            HistoryInfo history = new HistoryInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(history));
            history = new HistoryInfo
            {
                DateString = "20170901",
                BeginTimeString = "130000",
                EndTimeString = "140000",
                Ticket = "111",
                Name = "Reader"
            };
            Assert.AreEqual("{'date':'20170901','beginTime':'130000','endTime':'140000','ticket':'111','name':'Reader'}", JsonUtility.SerializeShort(history));
        }

        [TestMethod]
        public void HistoryInfo_Verify_1()
        {
            HistoryInfo history = new HistoryInfo();
            Assert.IsFalse(history.Verify(false));
            history = new HistoryInfo
            {
                DateString = "20170901",
                BeginTimeString = "130000",
                EndTimeString = "140000",
                Ticket = "111",
                Name = "Reader"
            };
            Assert.IsTrue(history.Verify(false));
            history = new HistoryInfo
            {
                DateString = "20170901",
                BeginTimeString = "140000",
                EndTimeString = "130000",
                Ticket = "111",
                Name = "Reader"
            };
            Assert.IsFalse(history.Verify(false));
        }

        [TestMethod]
        public void HistoryInfo_ToString_1()
        {
            HistoryInfo history = new HistoryInfo();
            Assert.AreEqual("(null): (null)-(null) [(null)] ((null))", history.ToString());
            history = new HistoryInfo
            {
                DateString = "20170901",
                BeginTimeString = "130000",
                EndTimeString = "140000",
                Ticket = "111",
                Name = "Reader"
            };
            Assert.AreEqual("20170901: 130000-140000 [111] (Reader)", history.ToString());
        }

        [TestMethod]
        public void HistoryInfo_Date_1()
        {
            HistoryInfo history = new HistoryInfo
            {
                DateString = "20170901"
            };
            Assert.AreEqual(new DateTime(2017, 9, 1), history.Date);
        }

        [TestMethod]
        public void HistoryInfo_Date_2()
        {
            HistoryInfo history = new HistoryInfo
            {
                Date = new DateTime(2017, 9, 1)
            };
            Assert.AreEqual("20170901", history.DateString);
        }

        [TestMethod]
        public void HistoryInfo_BeginDate_1()
        {
            HistoryInfo history = new HistoryInfo
            {
                DateString = "20170901",
                BeginTimeString = "130000"
            };
            Assert.AreEqual(new DateTime(2017, 9, 1, 13, 0, 0), history.BeginDate);
        }

        [TestMethod]
        public void HistoryInfo_BeginDate_2()
        {
            HistoryInfo history = new HistoryInfo
            {
                BeginDate = new DateTime(2017, 9, 1, 13, 0, 0),
            };
            Assert.AreEqual("20170901", history.DateString);
            Assert.AreEqual("130000", history.BeginTimeString);
        }

        [TestMethod]
        public void HistoryInfo_BeginTime_1()
        {
            HistoryInfo history = new HistoryInfo
            {
                BeginTimeString = "130000"
            };
            Assert.AreEqual(new TimeSpan(13, 0, 0), history.BeginTime);
        }

        [TestMethod]
        public void HistoryInfo_BeginTime_2()
        {
            HistoryInfo history = new HistoryInfo
            {
                BeginTime = new TimeSpan(13, 0, 0),
            };
            Assert.AreEqual("130000", history.BeginTimeString);
        }

        [TestMethod]
        public void HistoryInfo_EndDate_1()
        {
            HistoryInfo history = new HistoryInfo
            {
                DateString = "20170901",
                EndTimeString = "140000"
            };
            Assert.AreEqual(new DateTime(2017, 9, 1, 14, 0, 0), history.EndDate);
        }

        [TestMethod]
        public void HistoryInfo_EndDate_2()
        {
            HistoryInfo history = new HistoryInfo
            {
                EndDate = new DateTime(2017, 9, 1, 14, 0, 0),
            };
            Assert.AreEqual("20170901", history.DateString);
            Assert.AreEqual("140000", history.EndTimeString);
        }

        [TestMethod]
        public void HistoryInfo_EndTime_1()
        {
            HistoryInfo history = new HistoryInfo
            {
                EndTimeString = "140000"
            };
            Assert.AreEqual(new TimeSpan(14, 0, 0), history.EndTime);
        }

        [TestMethod]
        public void HistoryInfo_EndTime_2()
        {
            HistoryInfo history = new HistoryInfo
            {
                EndTime = new TimeSpan(14, 0, 0),
            };
            Assert.AreEqual("140000", history.EndTimeString);
        }

        [TestMethod]
        public void HistoryInfo_Duration_1()
        {
            HistoryInfo history = new HistoryInfo
            {
                BeginTimeString = "130000",
                EndTimeString = "140000"
            };
            Assert.AreEqual(new TimeSpan(1, 0, 0), history.Duration);
        }

        [TestMethod]
        public void HistoryInfo_Duration_2()
        {
            HistoryInfo history = new HistoryInfo
            {
                BeginTime = new TimeSpan(13, 0, 0),
                Duration = new TimeSpan(1, 0, 0),
            };
            Assert.AreEqual(new TimeSpan(14, 0, 0), history.EndTime);
        }

        [TestMethod]
        public void HistoryInfo_ApplyToField_1()
        {
            RecordField field = new RecordField(HistoryInfo.Tag)
                .AddSubField('a', "20000101")
                .AddSubField('b', "083000");
            HistoryInfo history = new HistoryInfo
            {
                DateString = "20170901",
                BeginTimeString = "130000",
                EndTimeString = "140000",
                Ticket = "111",
                Name = "Reader"
            };
            history.ApplyToField(field);
            Assert.AreEqual("20170901", field.GetFirstSubFieldValue('a'));
            Assert.AreEqual("130000", field.GetFirstSubFieldValue('b'));
            Assert.AreEqual("140000", field.GetFirstSubFieldValue('c'));
            Assert.AreEqual("111", field.GetFirstSubFieldValue('d'));
            Assert.AreEqual("Reader", field.GetFirstSubFieldValue('e'));
        }
    }
}
