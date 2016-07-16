using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis.Readers;

namespace UnitTests.ManagedClient.Readers
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
            Assert.AreEqual(first.Birthdate, second.Birthdate);
            Assert.AreEqual(first.Category, second.Category);
            Assert.AreEqual(first.Description, second.Description);
            Assert.AreEqual(first.Fio, second.Fio);
        }

        [TestMethod]
        public void TestReaderInfoSerialization()
        {
            ReaderInfo readerInfo = new ReaderInfo();
            _TestSerialization(readerInfo);

            readerInfo.Category = "студент";
            readerInfo.Fio = "Иванов Иван Иванович";
            _TestSerialization(readerInfo);
        }
    }
}
