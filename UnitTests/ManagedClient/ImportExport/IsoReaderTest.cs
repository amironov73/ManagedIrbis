using System;
using System.IO;
using System.Linq;
using ManagedClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient.ImportExport;

namespace UnitTests.ManagedClient.ImportExport
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
