using System;
using System.IO;
using System.Security.Cryptography;
using AM;
using AM.IO;
using AM.Text;
using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class MstRecord64Test
    {
        [TestMethod]
        public void MstRecord64_Construction_1()
        {
            MstRecord64 record = new MstRecord64();
            Assert.IsNotNull(record.Dictionary);
            Assert.IsNotNull(record.Leader);
            Assert.AreEqual(0L, record.Offset);
            Assert.IsFalse(record.Deleted);
        }

        [TestMethod]
        public void MstRecord64_Properties_1()
        {
            MstRecord64 record = new MstRecord64();
            Assert.IsFalse(record.Deleted);
            Assert.AreEqual(0L, record.Offset);
            record.Offset = 123L;
            Assert.AreEqual(123L, record.Offset);
        }

        [TestMethod]
        public void MstRecord64_DecodeRecord_1()
        {
            MstRecord64 mst = new MstRecord64
            {
                Leader = new MstRecordLeader64 { Mfn = 123, Nvf = 1 }
            };
            mst.Dictionary.Add(new MstDictionaryEntry64()
            {
                Tag = 100,
                Bytes = new byte[0],
                Length = 0,
                Text = "Hello"
            });
            MarcRecord decoded = mst.DecodeRecord();
            Assert.IsNotNull(decoded);
            Assert.AreEqual(123, decoded.Mfn);
            Assert.AreEqual(1, decoded.Fields.Count);
            Assert.AreEqual(100, decoded.Fields[0].Tag);
            Assert.AreEqual("Hello", decoded.Fields[0].Value);
            Assert.AreEqual(0, decoded.Fields[0].SubFields.Count);
        }

        [TestMethod]
        public void MstRecord_EncodeRecord_1()
        {
            MarcRecord record = new MarcRecord {Mfn = 123, Version = 10}
                .AddField(100, "Hello")
                .AddField(new RecordField(200, new SubField('a', "Title"), new SubField('e', "SubTitle")));
            MstRecord64 encoded = MstRecord64.EncodeRecord(record);
            Assert.IsNotNull(encoded);
            Assert.AreEqual(record.Mfn, encoded.Leader.Mfn);
            Assert.AreEqual((int)record.Status, encoded.Leader.Status);
            Assert.AreEqual(record.Version, encoded.Leader.Version);
            Assert.AreEqual(2, encoded.Dictionary.Count);
            Assert.AreEqual(100, encoded.Dictionary[0].Tag);
            Assert.AreEqual(200, encoded.Dictionary[1].Tag);
        }

        [TestMethod]
        public void MstRecord_Prepare_Write_1()
        {
            MstRecord64 record = new MstRecord64
            {
                Leader = new MstRecordLeader64{Mfn = 123, Status = 111, Version = 10}
            };
            record.Dictionary.Add(new MstDictionaryEntry64
            {
                Tag = 100,
                Text = "Hello"
            });
            record.Dictionary.Add(new MstDictionaryEntry64
            {
                Tag = 200,
                Text = "^ATitile^ESubTitle"
            });
            record.Prepare();
            Assert.AreEqual(2, record.Leader.Nvf);
            MemoryStream stream = new MemoryStream();
            record.Write(stream);
            byte[] bytes = stream.ToArray();
            Assert.AreEqual(79, bytes.Length);
        }

        [TestMethod]
        public void MstRecord64_ToString_1()
        {
            MstRecord64 record = new MstRecord64();
            Assert.AreEqual("Leader: Mfn: 0, Length: 0, Previous: 0, Base: 0, Nvf: 0, Status: 0, Version: 0\nDictionary: ", record.ToString().DosToUnix());

            record.Leader = new MstRecordLeader64
            {
                Mfn = 123
            };
            record.Dictionary.Add(new MstDictionaryEntry64
            {
                Bytes = new byte[10],
                Length = 10,
                Position = 111,
                Tag = 100,
                Text = "Hello"
            });
            Assert.AreEqual("Leader: Mfn: 123, Length: 0, Previous: 0, Base: 0, Nvf: 0, Status: 0, Version: 0\nDictionary: Tag: 100, Position: 111, Length: 10, Text: Hello\n", record.ToString().DosToUnix());
        }
    }
}
