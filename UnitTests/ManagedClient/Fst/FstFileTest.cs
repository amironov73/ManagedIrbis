using System;
using System.IO;
using ManagedIrbis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis.Fst;

namespace UnitTests.ManagedClient.Fst
{
    [TestClass]
    public class FstFileTest
        : Common.CommonUnitTest
    {
        private void _TestSerialization
            (
                FstFile first
            )
        {
            byte[] bytes = first.SaveToMemory();

            FstFile second = bytes
                .RestoreObjectFromMemory<FstFile>();

            Assert.AreEqual(first.FileName, second.FileName);
            Assert.AreEqual(first.Lines.Count, second.Lines.Count);
        }

        [TestMethod]
        public void TestFstFileSerialization()
        {
            FstFile fst = new FstFile();

            _TestSerialization(fst);
        }

        [TestMethod]
        public void TestFstFileParseLocalFile()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "QueryToRec.fst"
                );
            FstFile fst = FstFile.ParseLocalFile(fileName, IrbisEncoding.Ansi);

            Assert.AreEqual(5, fst.Lines.Count);

            _TestSerialization(fst);
        }
    }
}
