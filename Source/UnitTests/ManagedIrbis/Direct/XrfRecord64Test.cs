using System;
using System.IO;

using AM;
using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UseObjectOrCollectionInitializer

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class XrfRecord64Test
    {
        [TestMethod]
        public void XrfRecord64_Construction_1()
        {
            XrfRecord64 record = new XrfRecord64();
            Assert.AreEqual(0, record.Mfn);
            Assert.AreEqual(0L, record.Offset);
            Assert.AreEqual(0, (int)record.Status);
            Assert.IsFalse(record.Deleted);
            Assert.IsFalse(record.Locked);
        }

        [TestMethod]
        public void XrfRecord64_Properties_1()
        {
            XrfRecord64 record = new XrfRecord64();
            record.Mfn = 123;
            Assert.AreEqual(123, record.Mfn);
            record.Offset = 23456;
            Assert.AreEqual(23456L, record.Offset);
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
            XrfRecord64 record = new XrfRecord64();
            record.Status = RecordStatus.Last;
            Assert.IsFalse(record.Locked);
            record.Locked = true;
            Assert.IsTrue(record.Locked);
            Assert.AreEqual(RecordStatus.Last|RecordStatus.Locked, record.Status);
            record.Locked = false;
            Assert.IsFalse(record.Locked);
            Assert.AreEqual(RecordStatus.Last, record.Status);
        }

        [TestMethod]
        public void XrfRecord64_ToString_1()
        {
            XrfRecord64 record = new XrfRecord64();
            Assert.AreEqual("MFN: 0, Offset: 0, Status: 0", record.ToString());

            record = new XrfRecord64
            {
                Mfn = 123,
                Offset = 2345,
                Status = RecordStatus.Last
            };
            Assert.AreEqual("MFN: 123, Offset: 2345, Status: Last", record.ToString());
        }
    }
}
