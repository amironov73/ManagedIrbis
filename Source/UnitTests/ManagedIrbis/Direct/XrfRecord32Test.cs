using ManagedIrbis;
using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UseObjectOrCollectionInitializer

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class XrfRecord32Test
    {
        [TestMethod]
        public void XrfRecord32_Construction_1()
        {
            XrfRecord32 record = new XrfRecord32();
            Assert.AreEqual(0, record.AbsoluteOffset);
            Assert.AreEqual(0, record.BlockNumber);
            Assert.AreEqual(0, record.BlockOffset);
            Assert.AreEqual(0, (int)record.Status);
            Assert.IsFalse(record.Deleted);
            Assert.IsFalse(record.Locked);
        }

        [TestMethod]
        public void XrfRecord32_Properties_1()
        {
            XrfRecord32 record = new XrfRecord32();
            record.AbsoluteOffset = 123;
            Assert.AreEqual(123, record.AbsoluteOffset);
            record.BlockNumber = 23456;
            Assert.AreEqual(23456, record.BlockNumber);
            record.BlockOffset = 12345;
            Assert.AreEqual(12345, record.BlockOffset);
            record.Status = RecordStatus.LogicallyDeleted;
            Assert.AreEqual(RecordStatus.LogicallyDeleted, record.Status);
            Assert.IsTrue(record.Deleted);
            Assert.IsFalse(record.Locked);
            record.Status = RecordStatus.Locked;
            Assert.IsTrue(record.Locked);
            Assert.IsFalse(record.Deleted);
        }

        [TestMethod]
        public void XrfRecord_Locked_1()
        {
            XrfRecord32 record = new XrfRecord32();
            record.Status = RecordStatus.Last;
            Assert.IsFalse(record.Locked);
            record.Locked = true;
            Assert.IsTrue(record.Locked);
            Assert.AreEqual(RecordStatus.Last | RecordStatus.Locked, record.Status);
            record.Locked = false;
            Assert.IsFalse(record.Locked);
            Assert.AreEqual(RecordStatus.Last, record.Status);
        }

        [TestMethod]
        public void XrfRecord32_ToString_1()
        {
            XrfRecord32 record = new XrfRecord32();
            Assert.AreEqual("Offset: 0, Status: 0", record.ToString());

            record = new XrfRecord32
            {
                AbsoluteOffset = 123,
                Status = RecordStatus.Last
            };
            Assert.AreEqual("Offset: 123, Status: Last", record.ToString());
        }
    }
}
