using System;
using System.IO;
using System.Text;

using AM;
using AM.Logging;
using AM.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Logging
{
    [TestClass]
    public class FileLoggerTest
    {
        [TestMethod]
        public void FileLogger_Construction_1()
        {
            string fileName = Path.GetTempFileName();
            FileLogger logger = new FileLogger(fileName);
            Assert.AreSame(fileName, logger.FileName);
        }

        [TestMethod]
        public void FileLogger_Debug_1()
        {
            string message = "Message";
            string fileName = Path.GetTempFileName();
            FileLogger logger = new FileLogger(fileName);
            logger.Debug(message);
            string expected = message + NewLine.Unix;
            string actual = File.ReadAllText(fileName).DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FileLogger_Error_1()
        {
            string message = "Message";
            string fileName = Path.GetTempFileName();
            FileLogger logger = new FileLogger(fileName);
            logger.Error(message);
            string expected = message + NewLine.Unix;
            string actual = File.ReadAllText(fileName).DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FileLogger_Fatal_1()
        {
            string message = "Message";
            string fileName = Path.GetTempFileName();
            FileLogger logger = new FileLogger(fileName);
            logger.Fatal(message);
            string expected = message + NewLine.Unix;
            string actual = File.ReadAllText(fileName).DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FileLogger_Info_1()
        {
            string message = "Message";
            string fileName = Path.GetTempFileName();
            FileLogger logger = new FileLogger(fileName);
            logger.Info(message);
            string expected = message + NewLine.Unix;
            string actual = File.ReadAllText(fileName).DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FileLogger_Trace_1()
        {
            string message = "Message";
            string fileName = Path.GetTempFileName();
            FileLogger logger = new FileLogger(fileName);
            logger.Trace(message);
            string expected = message + NewLine.Unix;
            string actual = File.ReadAllText(fileName).DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FileLogger_Warn_1()
        {
            string message = "Message";
            string fileName = Path.GetTempFileName();
            FileLogger logger = new FileLogger(fileName);
            logger.Warn(message);
            string expected = message + NewLine.Unix;
            string actual = File.ReadAllText(fileName).DosToUnix();
            Assert.AreEqual(expected, actual);
        }

    }
}
