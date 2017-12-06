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
                    "Datai\\IBIS\\ibis.xrf"
                );
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
    }
}
