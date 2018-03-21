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
    public class XrfFile64Test
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetFileName()
        {
            return Path.Combine
                (
                    Irbis64RootPath,
                    "Datai/IBIS/ibis.xrf"
                );
        }

        [NotNull]
        private string _CreateDatabase()
        {
            Random random = new Random();
            string directory = Path.Combine
            (
                Path.GetTempPath(),
                random.Next().ToInvariantString()
            );
            Directory.CreateDirectory(directory);
            string path = Path.Combine(directory, "database");
            DirectUtility.CreateDatabase64(path);
            string result = path + ".xrf";

            return result;
        }

        [TestMethod]
        public void XrfFile64_Construction_1()
        {
            string fileName = _GetFileName();
            DirectAccessMode mode = DirectAccessMode.ReadOnly;
            using (XrfFile64 file = new XrfFile64(fileName, mode))
            {
                Assert.AreSame(fileName, file.FileName);
                Assert.AreEqual(mode, file.Mode);
            }
        }

        [TestMethod]
        public void XrfFile64_ReadRecord_1()
        {
            string fileName = _GetFileName();
            DirectAccessMode mode = DirectAccessMode.ReadOnly;
            using (XrfFile64 file = new XrfFile64(fileName, mode))
            {
                XrfRecord64 record = file.ReadRecord(1);
                Assert.AreEqual(1, record.Mfn);
                Assert.AreEqual(22951100L, record.Offset);
                Assert.AreEqual(0, (int)record.Status);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void XrfFile64_ReadRecord_2()
        {
            string fileName = _GetFileName();
            DirectAccessMode mode = DirectAccessMode.ReadOnly;
            using (XrfFile64 file = new XrfFile64(fileName, mode))
            {
                file.ReadRecord(111111);
            }
        }

        [TestMethod]
        public void XrfFile64_ReopenFile_1()
        {
            string fileName = _CreateDatabase();
            using (XrfFile64 file = new XrfFile64(fileName, DirectAccessMode.ReadOnly))
            {
                Assert.AreEqual(DirectAccessMode.ReadOnly, file.Mode);
                file.ReopenFile(DirectAccessMode.Exclusive);
                Assert.AreEqual(DirectAccessMode.Exclusive, file.Mode);
            }
        }

        [TestMethod]
        public void XrfFile64_WriteRecord_1()
        {
            string fileName = _CreateDatabase();
            using (XrfFile64 file = new XrfFile64(fileName, DirectAccessMode.Exclusive))
            {
                XrfRecord64 record1 = new XrfRecord64
                {
                    Mfn = 1,
                    Offset = 12345678L
                };
                file.WriteRecord(record1);

                file.LockRecord(1, true);

                XrfRecord64 record2 = file.ReadRecord(1);
                Assert.IsTrue(record2.Locked);
            }
        }
    }
}
