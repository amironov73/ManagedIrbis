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
    public class XrfFile32Test
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetFileName()
        {
            return Path.Combine
            (
                Irbis32RootPath,
                "ibis.xrf"
            );
        }

        [TestMethod]
        public void XrfFile32_Construction_1()
        {
            string fileName = _GetFileName();
            using (XrfFile32 file = new XrfFile32(fileName))
            {
                Assert.AreSame(fileName, file.FileName);
            }
        }

        [TestMethod]
        public void XrfFile32_ReadRecord_1()
        {
            string fileName = _GetFileName();
            using (XrfFile32 file = new XrfFile32(fileName))
            {
                XrfRecord32 record = file.ReadRecord(1);
                Assert.AreEqual(3780548, record.AbsoluteOffset);
                Assert.AreEqual(7384, record.BlockNumber);
                Assert.AreEqual(452, record.BlockOffset);
                Assert.AreEqual(0, (int)record.Status);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void XrfFile32_ReadRecord_2()
        {
            string fileName = _GetFileName();
            using (XrfFile32 file = new XrfFile32(fileName))
            {
                file.ReadRecord(111111);
            }
        }
    }
}
