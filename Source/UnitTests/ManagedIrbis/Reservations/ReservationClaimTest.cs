using System;

using AM.Collections;
using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Readers;
using ManagedIrbis.Reservations;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace UnitTests.ManagedIrbis.Reservations
{
    [TestClass]
    public class ReservationClaimTest
    {
        [TestMethod]
        public void ReservationClaim_Construction_1()
        {
            ReservationClaim claim = new ReservationClaim();
            Assert.IsNull(claim.Ticket);
            Assert.IsNull(claim.DateString);
            Assert.IsNull(claim.TimeString);
            Assert.IsNull(claim.DurationString);
            Assert.IsNull(claim.Status);
            Assert.IsNull(claim.Field);
            Assert.IsNull(claim.Reader);
            Assert.IsNull(claim.UserData);
        }

        private void _TestSerialization
            (
                [NotNull] ReservationClaim first
            )
        {
            byte[] bytes = first.SaveToMemory();
            ReservationClaim second = bytes.RestoreObjectFromMemory<ReservationClaim>();
            Assert.AreEqual(first.Ticket, second.Ticket);
            Assert.AreEqual(first.DateString, second.DateString);
            Assert.AreEqual(first.TimeString, second.TimeString);
            Assert.AreEqual(first.DurationString, second.DurationString);
            Assert.AreEqual(first.Status, second.Status);
            Assert.IsNull(second.Field);
            Assert.IsNull(second.Reader);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void ReservationClaim_Serialization_1()
        {
            ReservationClaim claim = new ReservationClaim();
            _TestSerialization(claim);

            claim.UserData = "User data";
            _TestSerialization(claim);

            claim.Reader = new ReaderInfo();
            _TestSerialization(claim);

            claim = new ReservationClaim
            {
                Ticket = "111",
                DateString = "20170901",
                TimeString = "130000",
                DurationString = "010000",
                Status = "1"
            };
            _TestSerialization(claim);
        }

        [TestMethod]
        public void ReservationClaim_ParseField_1()
        {
            RecordField field = new RecordField(ReservationClaim.Tag)
                .AddSubField('a', "111")
                .AddSubField('b', "20170901")
                .AddSubField('c', "130000")
                .AddSubField('d', "010000")
                .AddSubField('z', "1");
            ReservationClaim claim = ReservationClaim.ParseField(field);
            Assert.AreEqual("111", claim.Ticket);
            Assert.AreEqual("20170901", claim.DateString);
            Assert.AreEqual("130000", claim.TimeString);
        }

        [TestMethod]
        public void ReservationClaim_ParseRecord_1()
        {
            RecordField field1 = new RecordField(ReservationClaim.Tag)
                .AddSubField('a', "111")
                .AddSubField('b', "20170901")
                .AddSubField('c', "130000")
                .AddSubField('d', "010000")
                .AddSubField('z', "1");
            RecordField field2 = new RecordField(ReservationClaim.Tag)
                .AddSubField('a', "222")
                .AddSubField('b', "20170902")
                .AddSubField('c', "140000")
                .AddSubField('d', "010000");
            MarcRecord record = new MarcRecord();
            record.Fields.Add(field1);
            record.Fields.Add(field2);
            NonNullCollection<ReservationClaim> claims
                = ReservationClaim.ParseRecord(record);
            Assert.AreEqual(2, claims.Count);
            Assert.AreSame(field1, claims[0].Field);
            Assert.AreEqual("111", claims[0].Ticket);
            Assert.AreEqual("20170901", claims[0].DateString);
            Assert.AreEqual("130000", claims[0].TimeString);
            Assert.AreEqual("010000", claims[0].DurationString);
            Assert.AreEqual("1", claims[0].Status);
            Assert.AreSame(field2, claims[1].Field);
            Assert.AreEqual("222", claims[1].Ticket);
            Assert.AreEqual("20170902", claims[1].DateString);
            Assert.AreEqual("140000", claims[1].TimeString);
            Assert.AreEqual("010000", claims[1].DurationString);
            Assert.IsNull(claims[1].Status);
        }

        [TestMethod]
        public void ReservationClaim_ToField_1()
        {
            ReservationClaim claim = new ReservationClaim();
            RecordField field = claim.ToField();
            Assert.AreEqual(ReservationClaim.Tag, field.Tag);
            Assert.AreEqual(0, field.SubFields.Count);
        }

        [TestMethod]
        public void ReservationClaim_ToField_2()
        {
            ReservationClaim claim = new ReservationClaim
            {
                Ticket = "111",
                DateString = "20170901",
                TimeString = "130000",
                DurationString = "010000",
                Status = "1"
            };
            RecordField field = claim.ToField();
            Assert.AreEqual(ReservationClaim.Tag, field.Tag);
            Assert.AreEqual(5, field.SubFields.Count);
            Assert.AreEqual("111", field.GetFirstSubFieldValue('a'));
            Assert.AreEqual("20170901", field.GetFirstSubFieldValue('b'));
            Assert.AreEqual("130000", field.GetFirstSubFieldValue('c'));
            Assert.AreEqual("010000", field.GetFirstSubFieldValue('d'));
            Assert.AreEqual("1", field.GetFirstSubFieldValue('z'));
        }

        [TestMethod]
        public void ReservationClaim_ToXml_1()
        {
            ReservationClaim claim = new ReservationClaim();
            Assert.AreEqual("<claim />", XmlUtility.SerializeShort(claim));
            claim = new ReservationClaim
            {
                Ticket = "111",
                DateString = "20170901",
                TimeString = "130000",
                DurationString = "010000",
                Status = "1"
            };
            Assert.AreEqual("<claim ticket=\"111\" date=\"20170901\" time=\"130000\" duration=\"010000\" status=\"1\" />", XmlUtility.SerializeShort(claim));
        }

        [TestMethod]
        public void ReservationClaim_ToJson_1()
        {
            ReservationClaim claim = new ReservationClaim();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(claim));
            claim = new ReservationClaim
            {
                Ticket = "111",
                DateString = "20170901",
                TimeString = "130000",
                DurationString = "010000",
                Status = "1"
            };
            Assert.AreEqual("{'ticket':'111','date':'20170901','time':'130000','duration':'010000','status':'1'}", JsonUtility.SerializeShort(claim));
        }

        [TestMethod]
        public void ReservationClaim_Verify_1()
        {
            ReservationClaim claim = new ReservationClaim();
            Assert.IsFalse(claim.Verify(false));
            claim = new ReservationClaim
            {
                Ticket = "111",
                DateString = "20170901",
                TimeString = "130000",
                DurationString = "010000",
                Status = "1"
            };
            Assert.IsTrue(claim.Verify(false));
            claim = new ReservationClaim
            {
                Ticket = "111",
                DateString = "20170901",
                TimeString = "130000",
                Status = "1"
            };
            Assert.IsFalse(claim.Verify(false));
        }

        [TestMethod]
        public void ReservationClaim_ToString_1()
        {
            ReservationClaim claim = new ReservationClaim();
            Assert.AreEqual("(null): (null) (null) [(null)]", claim.ToString());
            claim = new ReservationClaim
            {
                Ticket = "111",
                DateString = "20170901",
                TimeString = "130000",
                DurationString = "010000",
                Status = "1"
            };
            Assert.AreEqual("20170901: 130000 010000 [111]", claim.ToString());
        }

        [TestMethod]
        public void ReservationClaim_Date_1()
        {
            ReservationClaim claim = new ReservationClaim
            {
                DateString = "20170901"
            };
            Assert.AreEqual(new DateTime(2017, 9, 1), claim.Date);
        }

        [TestMethod]
        public void ReservationClaim_Date_2()
        {
            ReservationClaim claim = new ReservationClaim
            {
                Date = new DateTime(2017, 9, 1)
            };
            Assert.AreEqual("20170901", claim.DateString);
        }

        [TestMethod]
        public void ReservationClaim_Time_1()
        {
            ReservationClaim claim = new ReservationClaim
            {
                TimeString = "130000"
            };
            Assert.AreEqual(new TimeSpan(13, 0, 0), claim.Time);
        }

        [TestMethod]
        public void ReservationClaim_Time_2()
        {
            ReservationClaim claim = new ReservationClaim
            {
                Time = new TimeSpan(13, 0, 0)
            };
            Assert.AreEqual("130000", claim.TimeString);
        }

        [TestMethod]
        public void ReservationClaim_BeginDateTime_1()
        {
            ReservationClaim claim = new ReservationClaim
            {
                DateString = "20170901",
                TimeString = "130000"
            };
            Assert.AreEqual(new DateTime(2017, 9, 1, 13, 0, 0), claim.BeginDateTime);
        }

        [TestMethod]
        public void ReservationClaim_BeginDateTime_2()
        {
            ReservationClaim claim = new ReservationClaim
            {
                BeginDateTime = new DateTime(2017, 9, 1, 13, 0, 0)
            };
            Assert.AreEqual("20170901", claim.DateString);
            Assert.AreEqual("130000", claim.TimeString);
        }

        [TestMethod]
        public void ReservationClaim_Duration_1()
        {
            ReservationClaim claim = new ReservationClaim
            {
                DurationString = "010000"
            };
            Assert.AreEqual(new TimeSpan(1, 0, 0), claim.Duration);
        }

        [TestMethod]
        public void ReservationClaim_Duration_2()
        {
            ReservationClaim claim = new ReservationClaim
            {
                Duration = new TimeSpan(1, 0, 0)
            };
            Assert.AreEqual("010000", claim.DurationString);
        }

        [TestMethod]
        public void ReservationClaim_EndDateTime_1()
        {
            ReservationClaim claim = new ReservationClaim
            {
                DateString = "20170901",
                TimeString = "130000",
                DurationString = "010000"
            };
            Assert.AreEqual(new DateTime(2017, 9, 1, 14, 0, 0), claim.EndDateTime);
        }

        [TestMethod]
        public void ReservationClaim_EndDateTime_2()
        {
            ReservationClaim claim = new ReservationClaim
            {
                BeginDateTime = new DateTime(2017, 9, 1, 13, 0, 0),
                Duration = new TimeSpan(1, 0, 0)
            };
            Assert.AreEqual("20170901", claim.DateString);
            Assert.AreEqual("130000", claim.TimeString);
            Assert.AreEqual("010000", claim.DurationString);
        }

        [TestMethod]
        public void ReservationClaim_EndDateTime_3()
        {
            ReservationClaim claim = new ReservationClaim
            {
                BeginDateTime = new DateTime(2017, 9, 1, 13, 0, 0),
                EndDateTime = new DateTime(2017, 9, 1, 14, 0, 0)
            };
            Assert.AreEqual("20170901", claim.DateString);
            Assert.AreEqual("130000", claim.TimeString);
            Assert.AreEqual(new TimeSpan(1, 0, 0), claim.Duration);
            Assert.AreEqual("010000", claim.DurationString);
        }

        [TestMethod]
        public void ReservationClaim_ApplyToField_1()
        {
            RecordField field = new RecordField(ReservationClaim.Tag)
                .AddSubField('a', "20000101")
                .AddSubField('b', "083000");
            ReservationClaim claim = new ReservationClaim
            {
                Ticket = "111",
                DateString = "20170901",
                TimeString = "130000",
                DurationString = "010000",
                Status = "1"
            };
            claim.ApplyToField(field);
            Assert.AreEqual("111", field.GetFirstSubFieldValue('a'));
            Assert.AreEqual("20170901", field.GetFirstSubFieldValue('b'));
            Assert.AreEqual("130000", field.GetFirstSubFieldValue('c'));
            Assert.AreEqual("010000", field.GetFirstSubFieldValue('d'));
            Assert.AreEqual("1", field.GetFirstSubFieldValue('z'));
        }
    }
}
