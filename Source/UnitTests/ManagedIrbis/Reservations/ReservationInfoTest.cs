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
    public class ReservationInfoTest
    {
        [TestMethod]
        public void ReservationInfo_Construction()
        {
            ReservationInfo info = new ReservationInfo();
            Assert.IsNull(info.Room);
            Assert.IsNull(info.Number);
            Assert.IsNull(info.Status);
            Assert.IsNull(info.Description);
            Assert.IsNotNull(info.Claims);
            Assert.AreEqual(0, info.Claims.Count);
            Assert.IsNotNull(info.History);
            Assert.AreEqual(0, info.History.Count);
            Assert.IsNull(info.Record);
            Assert.IsNull(info.UserData);
        }

        private void _TestSerialization
            (
                [NotNull] ReservationInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            ReservationInfo second = bytes.RestoreObjectFromMemory<ReservationInfo>();
            Assert.AreEqual(first.Room, second.Room);
            Assert.AreEqual(first.Number, second.Number);
            Assert.AreEqual(first.Status, second.Status);
            Assert.AreEqual(first.Description, second.Description);
            Assert.AreEqual(first.Claims.Count, second.Claims.Count);
            for (int i = 0; i < first.Claims.Count; i++)
            {
                Assert.AreEqual(first.Claims[i].Ticket, second.Claims[i].Ticket);
                Assert.AreEqual(first.Claims[i].DateString, second.Claims[i].DateString);
                Assert.AreEqual(first.Claims[i].TimeString, second.Claims[i].TimeString);
                Assert.AreEqual(first.Claims[i].DurationString, second.Claims[i].DurationString);
                Assert.AreEqual(first.Claims[i].Status, second.Claims[i].Status);
            }
            Assert.AreEqual(first.History.Count, second.History.Count);
            for (int i = 0; i < first.History.Count; i++)
            {
                Assert.AreEqual(first.History[i].Ticket, second.History[i].Ticket);
                Assert.AreEqual(first.History[i].Name, second.History[i].Name);
                Assert.AreEqual(first.History[i].DateString, second.History[i].DateString);
                Assert.AreEqual(first.History[i].BeginTimeString, second.History[i].BeginTimeString);
                Assert.AreEqual(first.History[i].EndTimeString, second.History[i].EndTimeString);
            }
            Assert.IsNull(second.Record);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void ReservationInfo_Serialization_1()
        {
            ReservationInfo info = new ReservationInfo();
            _TestSerialization(info);

            info.Record = new MarcRecord();
            info.UserData = "User data";
            _TestSerialization(info);

            info.Room = "Room1";
            info.Number = "01";
            info.Status = "0";
            info.Description = "Description";
            info.Claims.Add(new ReservationClaim
            {
                Ticket = "111",
                DateString = "20170902",
                TimeString = "130000",
                DurationString = "010000",
                Status = "1"
            });
            info.History.Add(new HistoryInfo
            {
                Ticket = "111",
                Name = "Reader",
                DateString = "20170901",
                BeginTimeString = "130000",
                EndTimeString = "140000"
            });
            _TestSerialization(info);
        }

        [TestMethod]
        public void ReservationInfo_ParseRecord_1()
        {
            MarcRecord record = new MarcRecord();
            record.Fields.Add(new RecordField(10, "Room1"));
            record.Fields.Add(new RecordField(11, "01"));
            record.Fields.Add(new RecordField(12, "0"));
            record.Fields.Add(new RecordField(13, "Description"));
            RecordField field20 = new RecordField(20)
                .AddSubField('a', "111")
                .AddSubField('b', "20170902")
                .AddSubField('c', "130000")
                .AddSubField('d', "010000")
                .AddSubField('z', "1");
            record.Fields.Add(field20);
            RecordField field30 = new RecordField(30)
                .AddSubField('a', "20170901")
                .AddSubField('b', "130000")
                .AddSubField('c', "140000")
                .AddSubField('d', "111");
            record.Fields.Add(field30);

            ReservationInfo info = ReservationInfo.ParseRecord(record);
            Assert.AreSame(record, info.Record);
            Assert.AreEqual("Room1", info.Room);
            Assert.AreEqual("01", info.Number);
            Assert.AreEqual("0", info.Status);
            Assert.AreEqual("Description", info.Description);
            Assert.AreEqual(1, info.Claims.Count);
            Assert.AreSame(field20, info.Claims[0].Field);
            Assert.AreEqual("111", info.Claims[0].Ticket);
            Assert.AreEqual("20170902", info.Claims[0].DateString);
            Assert.AreEqual("130000", info.Claims[0].TimeString);
            Assert.AreEqual("010000", info.Claims[0].DurationString);
            Assert.AreEqual("1", info.Claims[0].Status);
            Assert.IsNull(info.Claims[0].Reader);
            Assert.IsNull(info.Claims[0].UserData);
            Assert.AreEqual(1, info.History.Count);
            Assert.AreSame(field30, info.History[0].Field);
            Assert.AreEqual("111", info.History[0].Ticket);
            Assert.AreEqual("20170901", info.History[0].DateString);
            Assert.AreEqual("130000", info.History[0].BeginTimeString);
            Assert.AreEqual("140000", info.History[0].EndTimeString);
            Assert.IsNull(info.History[0].Name);
            Assert.IsNull(info.History[0].UserData);
            Assert.IsNull(record.UserData);
        }

        private ReservationInfo _GetInfo()
        {
            ReservationInfo result = new ReservationInfo
            {
                Room = "Room1",
                Number = "01",
                Status = "0",
                Description = "Description",
                Claims =
                {
                    new ReservationClaim
                    {
                        Ticket = "111",
                        DateString = "20170902",
                        TimeString = "130000",
                        DurationString = "010000",
                        Status = "1"
                    }
                },
                History =
                {
                    new HistoryInfo
                    {
                        Ticket = "111",
                        DateString = "20170901",
                        BeginTimeString = "130000",
                        EndTimeString = "140000"
                    }
                }
            };

            return result;
        }

        [TestMethod]
        public void ReservationInfo_ToRecord_1()
        {
            ReservationInfo info = _GetInfo();
            MarcRecord record = info.ToRecord();
            Assert.AreEqual("Room1", info.Room);
            Assert.AreEqual("01", info.Number);
            Assert.AreEqual("0", info.Status);
            Assert.AreEqual("Description", info.Description);
            Assert.AreEqual(1, record.Fields.GetFieldCount(20));
            RecordField field20 = record.Fields.GetFirstField(20);
            Assert.IsNotNull(field20);
            Assert.AreEqual("111", field20.GetFirstSubFieldValue('a'));
            Assert.AreEqual("20170902", field20.GetFirstSubFieldValue('b'));
            Assert.AreEqual("130000", field20.GetFirstSubFieldValue('c'));
            Assert.AreEqual("010000", field20.GetFirstSubFieldValue('d'));
            Assert.AreEqual("1", field20.GetFirstSubFieldValue('z'));
            Assert.AreEqual(1, record.Fields.GetFieldCount(30));
            RecordField field30 = record.Fields.GetFirstField(30);
            Assert.IsNotNull(field30);
            Assert.AreEqual("20170901", field30.GetFirstSubFieldValue('a'));
            Assert.AreEqual("130000", field30.GetFirstSubFieldValue('b'));
            Assert.AreEqual("140000", field30.GetFirstSubFieldValue('c'));
            Assert.AreEqual("111", field30.GetFirstSubFieldValue('d'));
        }

        [TestMethod]
        public void ReservationInfo_ToXml_1()
        {
            ReservationInfo info = new ReservationInfo();
            Assert.AreEqual("<reservation />", XmlUtility.SerializeShort(info));

            info = _GetInfo();
            Assert.AreEqual("<reservation room=\"Room1\" number=\"01\" status=\"0\" description=\"Description\"><claim ticket=\"111\" date=\"20170902\" time=\"130000\" duration=\"010000\" status=\"1\" /><history><date>20170901</date><beginTime>130000</beginTime><endTime>140000</endTime><ticket>111</ticket></history></reservation>", XmlUtility.SerializeShort(info));
        }

        [TestMethod]
        public void ReservationInfo_ToJson_1()
        {
            ReservationInfo info = new ReservationInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(info));

            info = _GetInfo();
            Assert.AreEqual("{'room':'Room1','number':'01','status':'0','description':'Description','claims':[{'ticket':'111','date':'20170902','time':'130000','duration':'010000','status':'1'}],'history':[{'date':'20170901','beginTime':'130000','endTime':'140000','ticket':'111'}]}", JsonUtility.SerializeShort(info));
        }

        [TestMethod]
        public void ReservationInfo_Verify_1()
        {
            ReservationInfo info = new ReservationInfo();
            Assert.IsFalse(info.Verify(false));

            info = _GetInfo();
            Assert.IsTrue(info.Verify(false));
        }

        [TestMethod]
        public void ReservationInfo_ToString_1()
        {
            ReservationInfo info = new ReservationInfo();
            Assert.AreEqual("(null): (null)", info.ToString());

            info = _GetInfo();
            Assert.AreEqual("[Room1] 01: 0", info.ToString());
        }

        [TestMethod]
        public void ReservationInfo_ApplyToRecord_1()
        {
            MarcRecord record = new MarcRecord();
            record.Fields.Add(new RecordField(10, "Room2"));
            record.Fields.Add(new RecordField(11, "02"));
            record.Fields.Add(new RecordField(12, "1"));
            record.Fields.Add(new RecordField(13, "No description"));
            ReservationInfo info = _GetInfo();
            info.ApplyToRecord(record);
            Assert.AreEqual("Room1", record.FM(10));
            Assert.AreEqual("01", record.FM(11));
            Assert.AreEqual("0", record.FM(12));
            Assert.AreEqual("Description", record.FM(13));
            Assert.AreEqual(1, record.Fields.GetFieldCount(20));
            Assert.AreEqual(1, record.Fields.GetFieldCount(30));
        }
    }
}
