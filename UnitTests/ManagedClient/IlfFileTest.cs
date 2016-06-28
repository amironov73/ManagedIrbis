using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient;

namespace UnitTests.ManagedClient
{
    [TestClass]
    public class IlfFileTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void TestIlfFileReadLocalFile()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "MARS_WSS.ILF"
                );

            IlfFile library = IlfFile.ReadLocalFile
                (
                    fileName,
                    IrbisEncoding.Ansi
                );

            Assert.AreEqual(190, library.Entries.Count);
            Assert.AreEqual(library.EntryCount, library.Entries.Count);

            for (int i = 0; i < library.EntryCount; i++)
            {
                IlfFile.Entry entry = library.Entries[i];
                Assert.AreEqual(i + 1, entry.Number);
                Assert.IsNotNull(entry.Name);
                Assert.IsNotNull(entry.Data);
            }

            Assert.IsNotNull(library.GetFile("PR"));
            Assert.IsNotNull(library.GetFile("pr"));
            Assert.IsNull(library.GetFile("ZR"));

            Assert.IsTrue
                (
                    library.Verify(false)
                );
        }
    }
}
