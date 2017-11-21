using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Morphology;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Morphology
{
    [TestClass]
    public class MorphologyEntryTest
    {
        [NotNull]
        private MorphologyEntry _GetEntry()
        {
            return new MorphologyEntry
            {
                Mfn = 2502,
                MainTerm = "акварель",
                Dictionary = "базовый",
                Forms = new[]
                {
                    "акварели",
                    "акварелью",
                    "акварелей",
                    "акварелям",
                    "акварелями",
                    "акварелях"
                },
                Language = "ru"
            };
        }

        [NotNull]
        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord
            {
                Mfn = 2502
            };
            result.Fields.Add(new RecordField(10, "акварель"));
            result.Fields.Add(new RecordField(11, "базовый"));
            result.Fields.Add(new RecordField(12, "ru"));
            result.Fields.Add(new RecordField(20, "акварели"));
            result.Fields.Add(new RecordField(20, "акварелью"));
            result.Fields.Add(new RecordField(20, "акварелей"));
            result.Fields.Add(new RecordField(20, "акварелями"));
            result.Fields.Add(new RecordField(20, "акварелях"));

            return result;
        }

        [TestMethod]
        public void MorphologyEntry_Construction_1()
        {
            MorphologyEntry entry = new MorphologyEntry();
            Assert.AreEqual(0, entry.Mfn);
            Assert.IsNull(entry.MainTerm);
            Assert.IsNull(entry.Forms);
            Assert.IsNull(entry.Language);
            Assert.IsNull(entry.Dictionary);
        }

        [TestMethod]
        public void MorphologyEntry_Parse_1()
        {
            MarcRecord record = _GetRecord();
            MorphologyEntry entry = MorphologyEntry.Parse(record);
            Assert.AreEqual(record.Mfn, entry.Mfn);
            Assert.AreEqual(record.FM(10), entry.MainTerm);
            Assert.AreEqual(record.FM(11), entry.Dictionary);
            Assert.AreEqual(record.FM(12), entry.Language);
            Assert.IsNotNull(entry.Forms);
            Assert.AreEqual(record.Fields.GetFieldCount(20), entry.Forms.Length);
            for (int i = 0; i < record.Fields.GetFieldCount(20); i++)
            {
                Assert.AreEqual(record.Fields.GetField(20, i).GetFieldValue(), entry.Forms[i]);
            }
        }

        [TestMethod]
        public void MorphologyEntry_Encode_1()
        {
            MorphologyEntry entry = _GetEntry();
            MarcRecord record = entry.Encode();
            Assert.AreEqual(entry.Mfn, record.Mfn);
            Assert.AreEqual(entry.MainTerm, record.FM(10));
            Assert.AreEqual(entry.Dictionary, record.FM(11));
            Assert.AreEqual(entry.Language, record.FM(12));
            Assert.IsNotNull(entry.Forms);
            Assert.AreEqual(entry.Forms.Length, record.Fields.GetFieldCount(20));
            for (int i = 0; i < entry.Forms.Length; i++)
            {
                Assert.AreEqual(entry.Forms[i], record.Fields.GetField(20, i).GetFieldValue());
            }
        }

        private void _TestSerialization
            (
                [NotNull] MorphologyEntry first
            )
        {
            byte[] bytes = first.SaveToMemory();
            MorphologyEntry second = bytes.RestoreObjectFromMemory<MorphologyEntry>();
            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.MainTerm, second.MainTerm);
            if (ReferenceEquals(first.Forms, null))
            {
                Assert.IsNull(second.Forms);
            }
            else
            {
                Assert.IsNotNull(second.Forms);
                Assert.AreEqual(first.Forms.Length, second.Forms.Length);
                for (int i = 0; i < first.Forms.Length; i++)
                {
                    Assert.AreEqual(first.Forms[i], second.Forms[i]);
                }
            }
            Assert.AreEqual(first.Language, second.Language);
            Assert.AreEqual(first.Dictionary, second.Dictionary);
        }

        [TestMethod]
        public void MorphologyEntry_Serialization_1()
        {
            MorphologyEntry entry = new MorphologyEntry();
            _TestSerialization(entry);

            entry = _GetEntry();
            _TestSerialization(entry);
        }

        [TestMethod]
        public void MorphologyEntry_ToJson_1()
        {
            MorphologyEntry entry = new MorphologyEntry();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(entry));

            entry = _GetEntry();
            Assert.AreEqual("{'mfn':2502,'main':'акварель','dictionary':'базовый','language':'ru','forms':['акварели','акварелью','акварелей','акварелям','акварелями','акварелях']}", JsonUtility.SerializeShort(entry));
        }

        [TestMethod]
        public void MorphologyEntry_ToXml_1()
        {
            MorphologyEntry entry = new MorphologyEntry();
            Assert.AreEqual("<word />", XmlUtility.SerializeShort(entry));

            entry = _GetEntry();
            Assert.AreEqual("<word mfn=\"2502\" main=\"акварель\" dictionary=\"базовый\" language=\"ru\"><form>акварели</form><form>акварелью</form><form>акварелей</form><form>акварелям</form><form>акварелями</form><form>акварелях</form></word>", XmlUtility.SerializeShort(entry));
        }

        [TestMethod]
        public void MorphologyEntry_Verify_1()
        {
            MorphologyEntry entry = new MorphologyEntry();
            Assert.IsFalse(entry.Verify(false));

            entry = _GetEntry();
            Assert.IsTrue(entry.Verify(false));
        }

        [TestMethod]
        public void MorphologyEntry_ToString_1()
        {
            MorphologyEntry entry = new MorphologyEntry();
            Assert.AreEqual("(null): (none)", entry.ToString());

            entry = _GetEntry();
            Assert.AreEqual("акварель: акварели, акварелью, акварелей, акварелям, акварелями, акварелях", entry.ToString());
        }
    }
}
