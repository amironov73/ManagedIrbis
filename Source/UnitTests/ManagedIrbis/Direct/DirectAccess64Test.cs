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
    public class DirectAccess64Test
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
        private DirectAccess64 _GetAccess()
        {
            return new DirectAccess64(_GetMasterPath(), DirectAccessMode.ReadOnly);
        }

        [TestMethod]
        public void DirectAccess64_Construction_1()
        {
            string fileName = _GetMasterPath();
            string database = Path.GetFileNameWithoutExtension(fileName);
            DirectAccess64 access = new DirectAccess64(fileName);
            Assert.AreEqual(database, access.Database);
            Assert.IsNotNull(access.Mst);
            Assert.IsNotNull(access.Xrf);
            Assert.IsNotNull(access.InvertedFile);
            access.Dispose();
        }

        [TestMethod]
        public void DirectAccess64_Construction_2()
        {
            string fileName = _GetMasterPath();
            string database = Path.GetFileNameWithoutExtension(fileName);
            DirectAccess64 access = new DirectAccess64
                (
                    fileName,
                    DirectAccessMode.Exclusive
                );
            Assert.AreEqual(database, access.Database);
            Assert.IsNotNull(access.Mst);
            Assert.IsNotNull(access.Xrf);
            Assert.IsNotNull(access.InvertedFile);
            access.Dispose();
        }

        [TestMethod]
        public void DirectAccess64_Construction_3()
        {
            string fileName = _GetMasterPath();
            string database = Path.GetFileNameWithoutExtension(fileName);
            DirectAccess64 access = new DirectAccess64
                (
                    fileName,
                    DirectAccessMode.Shared
                );
            Assert.AreEqual(database, access.Database);
            Assert.IsNotNull(access.Mst);
            Assert.IsNotNull(access.Xrf);
            Assert.IsNotNull(access.InvertedFile);
            access.Dispose();
        }

        [TestMethod]
        public void DirectAccess64_Construction_4()
        {
            string fileName = _GetMasterPath();
            string database = Path.GetFileNameWithoutExtension(fileName);
            DirectAccess64 access = new DirectAccess64
                (
                    fileName,
                    DirectAccessMode.ReadOnly
                );
            Assert.AreEqual(database, access.Database);
            Assert.IsNotNull(access.Mst);
            Assert.IsNotNull(access.Xrf);
            Assert.IsNotNull(access.InvertedFile);
            access.Dispose();
        }

        [TestMethod]
        public void DirectAccess_GetMaxMfn_1()
        {
            using (DirectAccess64 access = _GetAccess())
            {
                int actual = access.GetMaxMfn();
                Assert.AreEqual(332, actual);
            }
        }

        [TestMethod]
        public void DirectAccess_ReadRawRecord_1()
        {
            using (DirectAccess64 access = _GetAccess())
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
    }
}
