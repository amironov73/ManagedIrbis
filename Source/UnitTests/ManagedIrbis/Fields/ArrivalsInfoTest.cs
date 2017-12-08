using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Fields;

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class ArrivalsInfoTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private RecordField _GetField()
        {
            return new RecordField(ArrivalsInfo.Tag)
                .AddSubField('1', "62")
                .AddSubField('a', "62")
                .AddSubField('3', "59");
        }

        [NotNull]
        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();
            result.Fields.Add(RecordField.Parse(88, "^A2017/112^Y112^B20171205^CТов. накл. №№ 3, 4, 5, 6, 7 от 17.11.2017 г.^DК8^E64^F82^G61115"));
            result.Fields.Add(RecordField.Parse(920, "KSU"));
            result.Fields.Add(RecordField.Parse(907, "^C^A20171205^Bklipovatv"));
            result.Fields.Add(RecordField.Parse(907, "^CПК^A20171205^Bklipovatv"));
            result.Fields.Add(RecordField.Parse(44, "^IФ404^H05^J62^K82^+61115.00^A79^O62^P82^S82^882^G3"));
            result.Fields.Add(RecordField.Parse(744, "^IФ404^H05^A62^B82^+61115.00^L59^M78^S82^682^O4^J3^43"));
            result.Fields.Add(RecordField.Parse(145, "^A62^B82^C61115.00^G62^H82^I61115.00^J59^K79^P59^Q79"));
            result.Fields.Add(RecordField.Parse(147, "^C3^F3"));
            result.Fields.Add(RecordField.Parse(148, "^B59^C78^D4^G10"));
            result.Fields.Add(RecordField.Parse(149, "^C62^D59^F82^G79^L61115.00^M62^N82^O61115.00^T82^V82^182"));
            result.Fields.Add(RecordField.Parse(151, "^882"));
            result.Fields.Add(RecordField.Parse(45, "^A79^382^462^582^661115.00^.62^(59^G3"));
            result.Fields.Add(RecordField.Parse(47, "^882"));
            result.Fields.Add(RecordField.Parse(18, "^162^259^359^762"));
            result.Fields.Add(RecordField.Parse(17, "^162^A62^359"));
            result.Fields.Add(RecordField.Parse(910, "^261115.00"));
            result.Fields.Add(RecordField.Parse(20, "^A288.00^B62.00^E82.00^F82.00^G62.00"));
            result.Fields.Add(RecordField.Parse(910, "^1И83156"));
            result.Fields.Add(RecordField.Parse(910, "^1Б62490"));
            result.Fields.Add(RecordField.Parse(910, "^1CD4093"));
            result.Fields.Add(RecordField.Parse(910, "И83210"));
            result.Fields.Add(RecordField.Parse(910, "Б62513"));
            result.Fields.Add(RecordField.Parse(910, "CD4095"));

            return result;
        }

        [NotNull]
        public ArrivalsInfo _GetArrivals()
        {
            return new ArrivalsInfo
            {
                OnBalanceWithoutPeriodicals = "62",
                TotalWithoutPeriodicals = "62",
                Educational = "59",
                UnknownSubFields = new SubField[0]
            };
        }

        [TestMethod]
        public void ArrivalsInfo_Construciton_1()
        {
            ArrivalsInfo arrivals = new ArrivalsInfo();
            Assert.IsNull(arrivals.OnBalanceWithoutPeriodicals);
            Assert.IsNull(arrivals.OffBalanceWithoutPeriodicals);
            Assert.IsNull(arrivals.TotalWithoutPeriodicals);
            Assert.IsNull(arrivals.OffBalanceWithPeriodicals);
            Assert.IsNull(arrivals.Educational);
            Assert.IsNull(arrivals.UnknownSubFields);
            Assert.IsNull(arrivals.Field);
            Assert.IsNull(arrivals.UserData);
        }

        [TestMethod]
        public void ArrivalsInfo_ParseField_1()
        {
            RecordField field = _GetField();
            ArrivalsInfo actual = ArrivalsInfo.ParseField(field);
            ArrivalsInfo expected = _GetArrivals();
            _Compare(expected, actual);
            Assert.IsNotNull(actual.UnknownSubFields);
            Assert.AreEqual(0, actual.UnknownSubFields.Length);
            Assert.AreSame(field, actual.Field);
            Assert.IsNull(actual.UserData);
        }

        [TestMethod]
        public void ArrivalsInfo_ParseRecord_1()
        {
            MarcRecord record = _GetRecord();
            ArrivalsInfo[] arrivals = ArrivalsInfo.ParseRecord(record);
            Assert.AreEqual(1, arrivals.Length);
        }

        [TestMethod]
        public void ArrivalsInfo_ToField_1()
        {
            ArrivalsInfo arrivals = _GetArrivals();
            RecordField actual = arrivals.ToField();
            RecordField expected = _GetField();
            CompareFields(expected, actual);
        }

        [TestMethod]
        public void ArrivalsInfo_ApplyToField_1()
        {
            ArrivalsInfo arrivals = _GetArrivals();
            RecordField actual = new RecordField(ArrivalsInfo.Tag)
                .AddSubField('1', "100")
                .AddSubField('a', "200")
                .AddSubField('3', "90");
            arrivals.ApplyToField(actual);
            RecordField expected = _GetField();
            CompareFields(expected, actual);
        }

        private void _Compare
            (
                [NotNull] ArrivalsInfo first,
                [NotNull] ArrivalsInfo second
            )
        {
            Assert.AreEqual(first.OnBalanceWithoutPeriodicals, second.OnBalanceWithoutPeriodicals);
            Assert.AreEqual(first.OffBalanceWithoutPeriodicals, second.OffBalanceWithoutPeriodicals);
            Assert.AreEqual(first.TotalWithoutPeriodicals, second.TotalWithoutPeriodicals);
            Assert.AreEqual(first.OnBalanceWithoutPeriodicals, second.OnBalanceWithoutPeriodicals);
            Assert.AreEqual(first.Educational, second.Educational);
            if (ReferenceEquals(first.UnknownSubFields, null))
            {
                Assert.IsNull(second.UnknownSubFields);
            }
            else
            {
                Assert.IsNotNull(second.UnknownSubFields);
                Assert.AreEqual(first.UnknownSubFields.Length, second.UnknownSubFields.Length);
                for (int i = 0; i < first.UnknownSubFields.Length; i++)
                {
                    Assert.AreEqual(first.UnknownSubFields[i].Code, second.UnknownSubFields[i].Code);
                    Assert.AreEqual(first.UnknownSubFields[i].Value, second.UnknownSubFields[i].Value);
                }
            }
        }

        private void _TestSerialization
            (
                [NotNull] ArrivalsInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            ArrivalsInfo second = bytes.RestoreObjectFromMemory<ArrivalsInfo>();
            _Compare(first, second);
            Assert.IsNull(second.Field);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void ArrivalsInfo_Serialization_1()
        {
            ArrivalsInfo arrivals = new ArrivalsInfo();
            _TestSerialization(arrivals);

            arrivals = _GetArrivals();
            arrivals.Field = new RecordField();
            arrivals.UserData = "User data";
            _TestSerialization(arrivals);
        }

        [TestMethod]
        public void ArrivalsInfo_Verify_1()
        {
            ArrivalsInfo arrivals = new ArrivalsInfo();
            Assert.IsFalse(arrivals.Verify(false));

            arrivals = _GetArrivals();
            Assert.IsTrue(arrivals.Verify(false));
        }

        [TestMethod]
        public void ArrivalsInfo_ToXml_1()
        {
            ArrivalsInfo arrivals = new ArrivalsInfo();
            Assert.AreEqual("<arrivals />", XmlUtility.SerializeShort(arrivals));

            arrivals = _GetArrivals();
            Assert.AreEqual("<arrivals onBalanceWithoutPeriodicals=\"62\" totalWithoutPeriodicals=\"62\" educational=\"59\" />", XmlUtility.SerializeShort(arrivals));
        }

        [TestMethod]
        public void ArrivalsInfo_ToJson_1()
        {
            ArrivalsInfo arrivals = new ArrivalsInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(arrivals));

            arrivals = _GetArrivals();
            Assert.AreEqual("{'onBalanceWithoutPeriodicals':'62','totalWithoutPeriodicals':'62','educational':'59'}", JsonUtility.SerializeShort(arrivals));
        }

        [TestMethod]
        public void ArrivalsInfo_ToString_1()
        {
            ArrivalsInfo arrivals = new ArrivalsInfo();
            Assert.AreEqual("OnBalanceWithoutPeriodicals: (null), OffBalanceWithoutPeriodicals: (null), TotalWithoutPeriodicals: (null), OffBalanceWithPeriodicals: (null), Educational: (null)", arrivals.ToString());

            arrivals = _GetArrivals();
            Assert.AreEqual("OnBalanceWithoutPeriodicals: 62, OffBalanceWithoutPeriodicals: (null), TotalWithoutPeriodicals: 62, OffBalanceWithPeriodicals: (null), Educational: 59", arrivals.ToString());

        }
    }
}
