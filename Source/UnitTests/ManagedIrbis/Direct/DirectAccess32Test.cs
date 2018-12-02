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
    public class DirectAccess32Test
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetMasterPath()
        {
            return Path.Combine
                (
                    TestDataPath,
                    "Irbis32/ibis.mst"
                );
        }

        [NotNull]
        private DirectAccess32 _GetAccess()
        {
            return new DirectAccess32(_GetMasterPath(), DirectAccessMode.ReadOnly);
        }

        [TestMethod]
        public void DirectAccess32_Construction_1()
        {
            string fileName = _GetMasterPath();
            string database = Path.GetFileNameWithoutExtension(fileName);
            DirectAccess32 access = new DirectAccess32(fileName, DirectAccessMode.ReadOnly);
            Assert.AreEqual(database, access.Database);
            Assert.IsNotNull(access.Mst);
            Assert.IsNotNull(access.Xrf);
            Assert.IsNotNull(access.InvertedFile);
            access.Dispose();
        }

        [TestMethod]
        public void DirectAccess32_Construction_2()
        {
            string fileName = _GetMasterPath();
            string database = Path.GetFileNameWithoutExtension(fileName);
            DirectAccess32 access = new DirectAccess32(fileName, DirectAccessMode.Exclusive);
            Assert.AreEqual(database, access.Database);
            Assert.IsNotNull(access.Mst);
            Assert.IsNotNull(access.Xrf);
            Assert.IsNotNull(access.InvertedFile);
            access.Dispose();
        }

        [TestMethod]
        public void DirectAccess32_Construction_3()
        {
            string fileName = _GetMasterPath();
            string database = Path.GetFileNameWithoutExtension(fileName);
            DirectAccess32 access = new DirectAccess32(fileName, DirectAccessMode.Shared);
            Assert.AreEqual(database, access.Database);
            Assert.IsNotNull(access.Mst);
            Assert.IsNotNull(access.Xrf);
            Assert.IsNotNull(access.InvertedFile);
            access.Dispose();
        }

        [TestMethod]
        public void DirectAccess32_GetMaxMfn_1()
        {
            using (DirectAccess32 access = _GetAccess())
            {
                int actual = access.GetMaxMfn();
                Assert.AreEqual(269, actual);
            }
        }

        [TestMethod]
        public void DirectAccess_ReadRawRecord_1()
        {
            using (DirectAccess32 access = _GetAccess())
            {
                MarcRecord record = access.ReadRecord(1);
                Assert.IsNotNull(record);
                Assert.AreEqual(88, record.Fields.Count);
                Assert.IsFalse(record.Deleted);

                record = access.ReadRecord(1000000);
                Assert.IsNull(record);
            }
        }

        [TestMethod]
        public void DirectAccess32_ReadAllRecordVersions_1()
        {
            using (DirectAccess32 access = _GetAccess())
            {
                MarcRecord[] versions = access.ReadAllRecordVersions(1);
                Assert.AreEqual(1, versions.Length);
            }
        }

        [TestMethod]
        public void DirectAccess32_SearchSimple_1()
        {
            using (DirectAccess32 access = _GetAccess())
            {
                int[] found = access.SearchSimple("K=БАЗЫ");
                Assert.AreEqual(1, found.Length);
            }
        }

        [TestMethod]
        public void DirectAccess32_SearchReadSimple_1()
        {
            using (DirectAccess32 access = _GetAccess())
            {
                MarcRecord[] found = access.SearchReadSimple("K=БАЗЫ");
                Assert.AreEqual(1, found.Length);
            }
        }
    }
}
