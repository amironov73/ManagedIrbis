using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Direct;

// ReSharper disable UseObjectOrCollectionInitializer

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class FoundRecordTest
    {
        [TestMethod]
        public void FoundRecord_Construction_1()
        {
            FoundRecord record = new FoundRecord();
            Assert.AreEqual(0, record.Mfn);
            Assert.AreEqual(0L, record.Position);
            Assert.AreEqual(0, record.Length);
            Assert.AreEqual(0, record.FieldCount);
            Assert.AreEqual(0, record.Version);
            Assert.AreEqual(0, record.Flags);
        }

        [TestMethod]
        public void FoundRecord_Properties_1()
        {
            FoundRecord record = new FoundRecord();
            record.Mfn = 123;
            Assert.AreEqual(123, record.Mfn);
            record.Position = 234;
            Assert.AreEqual(234L, record.Position);
            record.Length = 345;
            Assert.AreEqual(345, record.Length);
            record.FieldCount = 456;
            Assert.AreEqual(456, record.FieldCount);
            record.Version = 567;
            Assert.AreEqual(567, record.Version);
            record.Flags = 678;
            Assert.AreEqual(678, record.Flags);
        }

        [TestMethod]
        public void FoundRecord_ToString_1()
        {
            FoundRecord record = new FoundRecord
            {
                Mfn = 123,
                Version = 456
            };
            Assert.AreEqual("[123] v456", record.ToString());
        }
    }
}
