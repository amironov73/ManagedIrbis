using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis.Worksheet;

namespace UnitTests.ManagedIrbis.Worksheet
{
    [TestClass]
    public class WssFileTest
        : Common.CommonUnitTest
    {
        private void _TestSerialization
            (
                WssFile first
            )
        {
            byte[] bytes = first.SaveToMemory();

            WssFile second = bytes
                .RestoreObjectFromMemory<WssFile>();

            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Items.Count, second.Items.Count);
            for (int i = 0; i < first.Items.Count; i++)
            {
                Assert.AreEqual(first.Items[i].Tag, second.Items[i].Tag);
                Assert.AreEqual(first.Items[i].Title, second.Items[i].Title);
            }
        }

        [TestMethod]
        public void TestWssFileSerialization()
        {
            WssFile file = new WssFile();
            _TestSerialization(file);
        }

        [TestMethod]
        public void TestWssFileReadLocalFile()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "951m.wss"
                );
            WssFile file = WssFile.ReadLocalFile(fileName);

            Assert.IsNotNull(file.Items);
            Assert.AreEqual("951m.wss", file.Name);
            Assert.AreEqual(5, file.Items.Count);

            _TestSerialization(file);
        }
    }
}
