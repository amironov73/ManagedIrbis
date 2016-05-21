using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedClient;

namespace UnitTests.ManagedClient
{
    [TestClass]
    public class IrbisRecordTest
    {
        [TestMethod]
        public void TestIrbisRecordConstruction()
        {
            IrbisRecord record = new IrbisRecord();

            Assert.IsNotNull(record);
            Assert.IsNotNull(record.Fields);
            Assert.IsNull(record.Database);
            Assert.IsNotNull(record.Description);
            Assert.AreEqual(0, record.Version);
        }

        private void _TestSerialization
            (
                IrbisRecord record1
            )
        {
            byte[] bytes = record1.SaveToMemory();

            IrbisRecord record2 = bytes
                .RestoreObjectFromMemory<IrbisRecord>();
            Assert.IsNotNull(record2);
            Assert.AreEqual
                (
                    0,
                    IrbisRecord.Compare
                    (
                        record1,
                        record2
                    )
                );
        }

        [TestMethod]
        public void TestIrbisRecordSerialization()
        {
            IrbisRecord record = new IrbisRecord();
            _TestSerialization(record);
            record.Fields.Add(new RecordField("200"));
            _TestSerialization(record);
            record.Fields.Add(new RecordField("300", "Hello"));
            _TestSerialization(record);
        }
    }
}
