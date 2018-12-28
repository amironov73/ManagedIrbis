using System;
using System.IO;

using AM;
using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Direct;
using ManagedIrbis.Search;
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
                    "Irbis64/Datai/IBIS/ibis.mst"
                );
        }

        [NotNull]
        private DirectAccess64 _GetAccess()
        {
            return new DirectAccess64(_GetMasterPath(), DirectAccessMode.ReadOnly);
        }

        [NotNull]
        private DirectAccess64 _GetWriteableAccess()
        {
            Random random = new Random();
            string directory =Path.Combine
                (
                    Path.GetTempPath(),
                    random.Next().ToInvariantString()
                );
            Directory.CreateDirectory(directory);
            string databasePath = Path.Combine(directory, "database");
            DirectUtility.CreateDatabase64(databasePath);
            string masterPath = databasePath + ".mst";

            return new DirectAccess64(masterPath, DirectAccessMode.Exclusive);
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
        public void DirectAccess64_GetMaxMfn_1()
        {
            using (DirectAccess64 access = _GetAccess())
            {
                int actual = access.GetMaxMfn();
                Assert.AreEqual(332, actual);
            }
        }

        [TestMethod]
        public void DirectAccess64_ReadRawRecord_1()
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

                record = access.ReadRawRecord(1000000);
                Assert.IsNull(record);
            }
        }

        [TestMethod]
        public void DirectAccess64_GetDatabaseInfo_1()
        {
            using (DirectAccess64 access = _GetAccess())
            {
                DatabaseInfo info = access.GetDatabaseInfo();
                Assert.AreEqual(332, info.MaxMfn);
                Assert.IsFalse(info.DatabaseLocked);
                Assert.IsNotNull(info.LogicallyDeletedRecords);
                Assert.AreEqual(0, info.LogicallyDeletedRecords.Length);
                Assert.IsNotNull(info.PhysicallyDeletedRecords);
                Assert.AreEqual(0, info.PhysicallyDeletedRecords.Length);
                Assert.IsNotNull(info.NonActualizedRecords);
                Assert.AreEqual(0, info.NonActualizedRecords.Length);
                Assert.IsNotNull(info.LockedRecords);
                Assert.AreEqual(0, info.LockedRecords.Length);
            }
        }

        [TestMethod]
        public void DirectAccess64_ReadRecord_1()
        {
            using (DirectAccess64 access = _GetAccess())
            {
                MarcRecord record = access.ReadRecord(1);
                Assert.IsNotNull(record);
                Assert.AreEqual(1, record.Mfn);
                Assert.AreEqual(100, record.Fields.Count);

                record = access.ReadRecord(1000000);
                Assert.IsNull(record);
            }
        }

        [TestMethod]
        public void DirectAccess64_ReadAllRecordVersions_1()
        {
            using (DirectAccess64 access = _GetAccess())
            {
                MarcRecord[] records = access.ReadAllRecordVersions(1);
                Assert.AreEqual(2, records.Length);

                records = access.ReadAllRecordVersions(1000000);
                Assert.AreEqual(0, records.Length);
            }
        }

        [TestMethod]
        public void DirectAccess64_ReadLinks_1()
        {
            using (DirectAccess64 access = _GetAccess())
            {
                TermLink[] links = access.ReadLinks("K=БИБЛИОТЕКА");
                Assert.AreEqual(3, links.Length);
            }
        }

        [TestMethod]
        public void DirectAccess64_ReadTerms_1()
        {
            using (DirectAccess64 access = _GetAccess())
            {
                TermParameters parameters = new TermParameters
                {
                    Database = "IBIS",
                    NumberOfTerms = 10,
                    StartTerm = "K=БИБЛИОТЕКА"
                };
                TermInfo[] terms = access.ReadTerms(parameters);
                Assert.AreEqual(10, terms.Length);
            }
        }

        [TestMethod]
        public void DirectAccess64_ReopenFiles_1()
        {
            using (DirectAccess64 access = _GetAccess())
            {
                access.ReopenFiles(DirectAccessMode.Shared);
            }
        }

        [TestMethod]
        public void DirectAccess64_SearchSimple_1()
        {
            using (DirectAccess64 access = _GetAccess())
            {
                int[] found = access.SearchSimple("K=БИБЛИОТЕКА");
                Assert.AreEqual(3, found.Length);
            }
        }

        [TestMethod]
        public void DirectAccess64_SearchReadSimple_1()
        {
            using (DirectAccess64 access = _GetAccess())
            {
                MarcRecord[] found = access.SearchReadSimple("K=БИБЛИОТЕКА");
                Assert.AreEqual(3, found.Length);
            }
        }

        //[TestMethod]
        //public void DirectAccess64_WriteRawRecord_1()
        //{
        //    MstRecord64 record = new MstRecord64
        //    {
        //        Leader = new MstRecordLeader64()
        //    };
        //    record.Dictionary.Add(new MstDictionaryEntry64 {Tag = 100, Text = "Hello"});
        //    record.Dictionary.Add(new MstDictionaryEntry64 {Tag = 200, Text = "^ATitile^ESubTitle"});
        //    record.Prepare();
        //    using (DirectAccess64 access = _GetWriteableAccess())
        //    {
        //        access.WriteRawRecord(record);
        //    }
        //}

        //[TestMethod]
        //public void DirectAccess64_WriteRecord_1()
        //{
        //    MarcRecord record = new MarcRecord();
        //    record.AddField(100, "Helllo");
        //    record.AddField(new RecordField(200, new SubField('a', "Title"),
        //        new SubField('e', "SubTitle")));
        //    using (DirectAccess64 access = _GetWriteableAccess())
        //    {
        //        access.WriteRecord(record);
        //    }
        //}
    }
}
