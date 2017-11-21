using System;

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
    public class SynonymEntryTest
    {
        [NotNull]
        private SynonymEntry _GetEntry()
        {
            return new SynonymEntry
            {
                Mfn = 8,
                MainWord = "абориген",
                Synonyms = new[]
                {
                    "житель",
                    "туземец"
                },
                Language = "rus",
                Worksheet = "SYNON"
            };
        }

        [NotNull]
        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord
            {
                Mfn = 8
            };
            result.Fields.Add(new RecordField(10, "абориген"));
            result.Fields.Add(new RecordField(11, "житель"));
            result.Fields.Add(new RecordField(11, "туземец"));
            result.Fields.Add(new RecordField(12, "rus"));
            result.Fields.Add(new RecordField(920, "SYNON"));

            return result;
        }

        [TestMethod]
        public void SynonymEntry_Construction_1()
        {
            SynonymEntry entry = new SynonymEntry();
            Assert.AreEqual(0, entry.Mfn);
            Assert.IsNull(entry.MainWord);
            Assert.IsNull(entry.Synonyms);
            Assert.IsNull(entry.Language);
            Assert.IsNull(entry.Worksheet);
        }

        [TestMethod]
        public void SynonymEntry_Parse_1()
        {
            MarcRecord record = _GetRecord();
            SynonymEntry entry = SynonymEntry.Parse(record);
            Assert.AreEqual(record.Mfn, entry.Mfn);
            Assert.AreEqual(record.FM(10), entry.MainWord);
            Assert.IsNotNull(entry.Synonyms);
            Assert.AreEqual(record.Fields.GetFieldCount(11), entry.Synonyms.Length);
            Assert.AreEqual(record.Fields.GetField(11, 0).GetFieldValue(), entry.Synonyms[0]);
            Assert.AreEqual(record.Fields.GetField(11, 1).GetFieldValue(), entry.Synonyms[1]);
            Assert.AreEqual(record.FM(12), entry.Language);
            Assert.AreEqual(record.FM(920), entry.Worksheet);
        }

        [TestMethod]
        public void SynonymEntry_Encode_1()
        {
            SynonymEntry entry = _GetEntry();
            MarcRecord record = entry.Encode();
            Assert.AreEqual(entry.Mfn, record.Mfn);
            Assert.AreEqual(entry.MainWord, record.FM(10));
            Assert.IsNotNull(entry.Synonyms);
            Assert.AreEqual(entry.Synonyms.Length, record.Fields.GetFieldCount(11));
            Assert.AreEqual(entry.Synonyms[0], record.Fields.GetField(11, 0).GetFieldValue());
            Assert.AreEqual(entry.Synonyms[1], record.Fields.GetField(11, 1).GetFieldValue());
            Assert.AreEqual(entry.Language, record.FM(12));
            Assert.AreEqual(entry.Worksheet, record.FM(920));
        }

        private void _TestSerialization
            (
                [NotNull] SynonymEntry first
            )
        {
            byte[] bytes = first.SaveToMemory();
            SynonymEntry second = bytes.RestoreObjectFromMemory<SynonymEntry>();
            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.MainWord, second.MainWord);
            if (ReferenceEquals(first.Synonyms, null))
            {
                Assert.IsNull(second.Synonyms);
            }
            else
            {
                Assert.IsNotNull(second.Synonyms);
                Assert.AreEqual(first.Synonyms.Length, second.Synonyms.Length);
                for (int i = 0; i < first.Synonyms.Length; i++)
                {
                    Assert.AreEqual(first.Synonyms[i], second.Synonyms[i]);
                }
            }
            Assert.AreEqual(first.Language, second.Language);
            Assert.AreEqual(first.Worksheet, second.Worksheet);
        }

        [TestMethod]
        public void SynonymEntry_Serialization_1()
        {
            SynonymEntry entry = new SynonymEntry();
            _TestSerialization(entry);

            entry = _GetEntry();
            _TestSerialization(entry);
        }

        [TestMethod]
        public void SynonymEntry_ToJson_1()
        {
            SynonymEntry entry = new SynonymEntry();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(entry));

            entry = _GetEntry();
            Assert.AreEqual("{'mfn':8,'main':'абориген','synonyms':['житель','туземец'],'language':'rus','worksheet':'SYNON'}", JsonUtility.SerializeShort(entry));
        }

        [TestMethod]
        public void SynonymEntry_ToXml_1()
        {
            SynonymEntry entry = new SynonymEntry();
            Assert.AreEqual("<word />", XmlUtility.SerializeShort(entry));

            entry = _GetEntry();
            Assert.AreEqual("<word mfn=\"8\" main=\"абориген\" language=\"rus\" worksheet=\"SYNON\"><synonym>житель</synonym><synonym>туземец</synonym></word>", XmlUtility.SerializeShort(entry));
        }

        [TestMethod]
        public void SynonymEntry_ToString_1()
        {
            SynonymEntry entry = new SynonymEntry();
            Assert.AreEqual("(null): (none)", entry.ToString());

            entry = _GetEntry();
            Assert.AreEqual("абориген: житель, туземец", entry.ToString());
        }
    }
}
