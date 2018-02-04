using System;
using System.IO;

using AM;
using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class MappedAccess64Test
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetMasterPath()
        {
            return Path.Combine
            (
                TestDataPath,
                "Irbis64\\Datai\\IBIS\\ibis.mst"
            );
        }

        [NotNull]
        private MappedAccess64 _GetAccess()
        {
            return new MappedAccess64(_GetMasterPath());
        }

        [TestMethod]
        public void MappedAccess64_Construction_1()
        {
            string fileName = _GetMasterPath();
            string database = Path.GetFileNameWithoutExtension(fileName);
            MappedAccess64 access = new MappedAccess64(fileName);
            Assert.AreEqual(database, access.Database);
            Assert.IsNotNull(access.Mst);
            Assert.IsNotNull(access.Xrf);
            //Assert.IsNotNull(access.InvertedFile);
            access.Dispose();
        }

        [TestMethod]
        public void MappedAccess_GetMaxMfn_1()
        {
            using (MappedAccess64 access = _GetAccess())
            {
                int actual = access.GetMaxMfn();
                Assert.AreEqual(332, actual);
            }
        }

        [TestMethod]
        public void MappedAccess_ReadRawRecord_1()
        {
            using (MappedAccess64 access = _GetAccess())
            {
                MstRecord64 record = access.ReadRawRecord(1);
                Assert.IsNotNull(record);
                Assert.IsNotNull(record.Dictionary);
                Assert.AreEqual(100, record.Dictionary.Count);
                Assert.IsNotNull(record.Leader);
                Assert.AreEqual(0L, record.Offset);
                Assert.IsFalse(record.Deleted);
            }
        }

        [TestMethod]
        public void MappedAccess_ReadRawRecord_2()
        {
            using (MappedAccess64 access = _GetAccess())
            {
                MstRecord64 record = access.ReadRawRecord(1001);
                Assert.IsNull(record);
            }
        }

        [TestMethod]
        public void MappedAccess_ReadRecord_1()
        {
            using (MappedAccess64 access = _GetAccess())
            {
                MarcRecord record = access.ReadRecord(1);
                Assert.IsNotNull(record);
                Assert.AreEqual(100, record.Fields.Count);
                Assert.AreEqual(2, record.Version);
                Assert.IsFalse(record.Deleted);
            }
        }

        [TestMethod]
        public void MappedAccess_ReadRecord_2()
        {
            using (MappedAccess64 access = _GetAccess())
            {
                MarcRecord record = access.ReadRecord(1001);
                Assert.IsNull(record);
            }
        }

        [TestMethod]
        public void MappedAccess_ReadAllRecordVersions_1()
        {
            using (MappedAccess64 access = _GetAccess())
            {
                MarcRecord[] versions = access.ReadAllRecordVersions(1);
                Assert.IsNotNull(versions);
                Assert.AreEqual(2, versions.Length);
            }
        }
    }
}
