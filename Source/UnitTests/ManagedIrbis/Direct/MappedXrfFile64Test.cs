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
    public class MappedXrfFile64Test
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
        public void MappedXrfFile64_ReadRecord_1()
        {
            string fileName = _GetFileName();
            using (MappedXrfFile64 file = new MappedXrfFile64(fileName))
            {
                Assert.AreSame(fileName, file.FileName);
                XrfRecord64 record = file.ReadRecord(1);
                Assert.AreEqual(1, record.Mfn);
                Assert.AreEqual(22951100L, record.Offset);
                Assert.AreEqual(0, (int)record.Status);
            }
        }
    }
}
