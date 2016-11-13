using System;
using System.IO;
using ManagedIrbis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis.Worksheet;

namespace UnitTests.ManagedIrbis.Worksheet
{
    [TestClass]
    public class WsFileTest
        : Common.CommonUnitTest
    {
        private void _TestSerialization
            (
                WsFile first
            )
        {
            byte[] bytes = first.SaveToMemory();

            WsFile second = bytes
                .RestoreObjectFromMemory<WsFile>();

            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Pages.Count, second.Pages.Count);
            for (int i = 0; i < first.Pages.Count; i++)
            {
                Assert.AreEqual(first.Pages[i].Name, second.Pages[i].Name);
                Assert.AreEqual(first.Pages[i].Items.Count, second.Pages[i].Items.Count);
            }
        }

        [TestMethod]
        public void TestWssFileReadLocalFile1()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "pazk31.ws"
                );
            WsFile file = WsFile.ReadLocalFile(fileName);

            Assert.IsNotNull(file.Pages);
            Assert.AreEqual("pazk31.ws", file.Name);
            Assert.AreEqual(9, file.Pages.Count);

            _TestSerialization(file);
        }

        [TestMethod]
        public void TestWssFileReadLocalFile2()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "pazk42.ws"
                );
            WsFile file = WsFile.ReadLocalFile
                (
                    fileName,
                    IrbisEncoding.Ansi
                );
            file = WsFile.FixupLocalFile
                (
                    fileName,
                    IrbisEncoding.Ansi,
                    file
                );

            Assert.IsNotNull(file.Pages);
            Assert.AreEqual("pazk42.ws", file.Name);
            Assert.AreEqual(12, file.Pages.Count);

            _TestSerialization(file);
        }
    }
}
