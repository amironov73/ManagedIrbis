using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

using JetBrains.Annotations;

using Moq;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class InsistentFileTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetReadFileName()
        {
            return Path.Combine(TestDataPath, "record.txt");
        }

        [NotNull]
        private string _GetWriteFileName()
        {
            return Path.GetTempFileName();
        }

        [TestMethod]
        public void InsistentFile_OpenForExclusiveRead_1()
        {
            string fileName = _GetReadFileName();
            using (FileStream stream
                = InsistentFile.OpenForExclusiveRead(fileName))
            {
                byte[] content = new byte[10];
                int read = stream.Read(content, 0, content.Length);
                Assert.AreEqual(content.Length, read);
            }
        }

        [TestMethod]
        public void InsistentFile_OpenForSharedRead_1()
        {
            string fileName = _GetReadFileName();
            using (FileStream stream
                = InsistentFile.OpenForSharedRead(fileName))
            {
                byte[] content = new byte[10];
                int read = stream.Read(content, 0, content.Length);
                Assert.AreEqual(content.Length, read);
            }
        }

        [TestMethod]
        public void InsistentFile_OpenForExclusiveWrite_1()
        {
            int expected = 10;
            string fileName = _GetWriteFileName();
            using (FileStream stream
                = InsistentFile.OpenForExclusiveWrite(fileName))
            {
                byte[] content = new byte[expected];
                stream.Write(content, 0, expected);
            }
            int actual = (int)new FileInfo(fileName).Length;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void InsistentFile_OpenForSharedWrite_1()
        {
            int expected = 10;
            string fileName = _GetWriteFileName();
            using (FileStream stream
                = InsistentFile.OpenForSharedWrite(fileName))
            {
                byte[] content = new byte[expected];
                stream.Write(content, 0, expected);
            }
            int actual = (int)new FileInfo(fileName).Length;
            Assert.AreEqual(expected, actual);
        }
    }
}
