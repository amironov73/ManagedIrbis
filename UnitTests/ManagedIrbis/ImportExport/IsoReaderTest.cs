using System;
using System.IO;
using System.Linq;
using ManagedIrbis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.ImportExport;

namespace UnitTests.ManagedIrbis.ImportExport
{
    [TestClass]
    public class IsoReaderTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void TestIsoReader()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "TEST1.ISO"
                );

            IrbisRecord[] records;
            using (IsoReader reader
                = new IsoReader(fileName, IrbisEncoding.Ansi))
            {
                records = reader.ToArray();
            }

            Assert.AreEqual(82, records.Length);
        }
    }
}
