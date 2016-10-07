using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis.Readers;

namespace UnitTests.ManagedIrbis.Readers
{
    [TestClass]
    public class ReaderInfoTest
    {
        private void _TestSerialization
            (
                ReaderInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();

            ReaderInfo second = bytes
                .RestoreObjectFromMemory<ReaderInfo>();

            Assert.AreEqual(first.Age, second.Age);
            Assert.AreEqual(first.DateOfBirth, second.DateOfBirth);
            Assert.AreEqual(first.Category, second.Category);
            Assert.AreEqual(first.Description, second.Description);
            Assert.AreEqual(first.FullName, second.FullName);
        }

        [TestMethod]
        public void TestReaderInfoSerialization()
        {
            ReaderInfo readerInfo = new ReaderInfo();
            _TestSerialization(readerInfo);

            readerInfo.Category = "студент";
            readerInfo.FullName = "Иванов Иван Иванович";
            _TestSerialization(readerInfo);
        }
    }
}
