using System.IO;

using AM;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;

namespace UnitTests.ManagedIrbis.Infrastructure
{
    [TestClass]
    public class EfficientRecordSerializerTest
        : Common.CommonUnitTest
    {
        private void _TestSerialization
            (
                [NotNull] MarcRecord firstRecord
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            EfficientRecordSerializer.Serialize(writer, firstRecord);
            writer.Close();

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            MarcRecord secondRecord = EfficientRecordSerializer.Deserialize(reader);
            Assert.AreEqual(firstRecord.Fields.Count, secondRecord.Fields.Count);
            for (int fieldIndex = 0; fieldIndex < firstRecord.Fields.Count; fieldIndex++)
            {
                RecordField firstField = firstRecord.Fields[fieldIndex];
                RecordField secondField = secondRecord.Fields[fieldIndex];
                Assert.AreEqual(firstField.Tag, secondField.Tag);
                Assert.AreEqual(firstField.Value, secondField.Value);
                Assert.AreEqual(firstField.SubFields.Count, secondField.SubFields.Count);
                for (int subIndex = 0; subIndex < firstField.SubFields.Count; subIndex++)
                {
                    SubField firstSubField = firstField.SubFields[subIndex];
                    SubField secondSubField = secondField.SubFields[subIndex];
                    Assert.AreEqual(firstSubField.Code, secondSubField.Code);
                    Assert.AreEqual(firstSubField.Value, secondSubField.Value);
                }
            }
        }

        [TestMethod]
        public void EfficientRecordSerializer_Serialization_1()
        {
            MarcRecord record = new MarcRecord();
            _TestSerialization(record);

            using (IrbisProvider provider = GetProvider())
            {
                for (int mfn = 1; mfn < 10; mfn++)
                {
                    record = provider.ReadRecord(mfn).ThrowIfNull();
                    _TestSerialization(record);
                }
            }
        }
    }
}
