using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;
using ManagedIrbis;
using ManagedIrbis.Fields;

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class RevisionInfoTest
    {
        [TestMethod]
        public void RevisionInfo_Constructor_1()
        {
            RevisionInfo revision = new RevisionInfo();
            Assert.IsNull(revision.Stage);
            Assert.IsNull(revision.Date);
            Assert.IsNull(revision.Name);
            Assert.IsNull(revision.UserData);
        }

        private void _TestSerialization
            (
                RevisionInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();

            RevisionInfo second = bytes
                    .RestoreObjectFromMemory<RevisionInfo>();
            Assert.AreEqual(first.Date, second.Date);
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Stage, second.Stage);
        }

        [TestMethod]
        public void RevisionInfo_Serialization_1()
        {
            var revisionInfo = new RevisionInfo();
            _TestSerialization(revisionInfo);
        }

        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();
            result.Fields.Add(new RecordField("907", "^CПК^A20051010^BТНП^101"));
            result.Fields.Add(new RecordField("907", "^CКР^A20051108^BАЕВ^101"));
            result.Fields.Add(new RecordField("907", "^C^A20130127^B"));
            result.Fields.Add(new RecordField("907", "^CКР^A20160516^Bреутовасс"));
            result.Fields.Add(new RecordField("907", "^CКР^A20160823^Bсуханованн"));

            return result;
        }

        [TestMethod]
        public void RevisionInfo_Parse_1()
        {
            MarcRecord record = _GetRecord();
            RevisionInfo[] actual = RevisionInfo.Parse(record);
            Assert.AreEqual(5, actual.Length);
            Assert.AreEqual("20051010", actual[0].Date);
            Assert.AreEqual("ТНП", actual[0].Name);
            Assert.AreEqual("ПК", actual[0].Stage);
        }

        [TestMethod]
        public void RevisionInfo_ToField_1()
        {
            RevisionInfo revision = new RevisionInfo
            {
                Date = "20051010",
                Name = "ТНП",
                Stage = "ПК"
            };
            RecordField actual = revision.ToField();
            Assert.AreEqual("20051010", actual.GetFirstSubFieldValue('a'));
            Assert.AreEqual("ТНП", actual.GetFirstSubFieldValue('b'));
            Assert.AreEqual("ПК", actual.GetFirstSubFieldValue('c'));
        }

        [TestMethod]
        public void RevisionInfo_ToString_1()
        {
            RevisionInfo revision = new RevisionInfo
            {
                Date = "20051010",
                Name = "ТНП",
                Stage = "ПК"
            };
            const string expected = "Stage: ПК, Date: 20051010, Name: ТНП";
            string actual = revision.ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}
