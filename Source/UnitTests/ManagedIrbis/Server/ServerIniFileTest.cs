using System.IO;
using System.Net;

using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Server;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Server
{
    [TestClass]
    public class ServerIniFileTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private ServerIniFile _GetFile()
        {
            string fileName = Path.Combine(Irbis64RootPath, "irbis_server.ini");
            IniFile iniFile = new IniFile(fileName, IrbisEncoding.Ansi, false);
            ServerIniFile result = new ServerIniFile(iniFile);

            return result;
        }

        [TestMethod]
        public void ServerIniFile_Construction_1()
        {
            IniFile iniFile = new IniFile();
            ServerIniFile file = new ServerIniFile(iniFile);
            Assert.AreSame(iniFile, file.Ini);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_AlphabetTablePath_1()
        {
            ServerIniFile file = _GetFile();
            Assert.AreEqual(@"D:\IRBIS64_2015\isisacw", file.AlphabetTablePath);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_ClientList_1()
        {
            ServerIniFile file = _GetFile();
            Assert.AreEqual("client_m.mnu", file.ClientList);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_ClientTimeLive_1()
        {
            ServerIniFile file = _GetFile();
            Assert.AreEqual(30, file.ClientTimeLive);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_DataPath_1()
        {
            ServerIniFile file = _GetFile();
            Assert.AreEqual(@"D:\IRBIS64_2015\DATAI\", file.DataPath);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_EncryptPasswords_1()
        {
            ServerIniFile file = _GetFile();
            Assert.IsFalse(file.EncryptPasswords);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_FormatsCacheable_1()
        {
            ServerIniFile file = _GetFile();
            Assert.IsFalse(file.FormatsCacheable);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_IPAddress_1()
        {
            ServerIniFile file = _GetFile();
            IPAddress expected = IPAddress.Loopback;
            Assert.AreEqual(expected, file.IPAddress);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_IPPort_1()
        {
            ServerIniFile file = _GetFile();
            Assert.AreEqual(6666, file.IPPort);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_KeepProcessAlive_1()
        {
            ServerIniFile file = _GetFile();
            Assert.IsTrue(file.KeepProcessAlive);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_LocalIPPort_1()
        {
            ServerIniFile file = _GetFile();
            Assert.AreEqual(0, file.LocalIPPort);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_MaxLogFileSize_1()
        {
            ServerIniFile file = _GetFile();
            Assert.AreEqual(1000000, file.MaxLogFileSize);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_MaxProcessCount_1()
        {
            ServerIniFile file = _GetFile();
            Assert.AreEqual(100, file.MaxProcessCount);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_MaxProcessRequests_1()
        {
            ServerIniFile file = _GetFile();
            Assert.AreEqual(1000, file.MaxProcessRequests);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_MaxServers_1()
        {
            ServerIniFile file = _GetFile();
            Assert.AreEqual(30, file.MaxServers);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_ProcessThreadsMonitor_1()
        {
            ServerIniFile file = _GetFile();
            Assert.AreEqual(60, file.ProcessThreadsMonitor);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_ProcessTimeLive_1()
        {
            ServerIniFile file = _GetFile();
            Assert.AreEqual(15, file.ProcessTimeLive);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_RecognizeClientAddress_1()
        {
            ServerIniFile file = _GetFile();
            Assert.IsFalse(file.RecognizeClientAddress);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_RegisterWindowsMessage_1()
        {
            ServerIniFile file = _GetFile();
            Assert.IsTrue(file.RegisterWindowsMessage);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_ServiceName_1()
        {
            ServerIniFile file = _GetFile();
            Assert.AreEqual("irbis_server.exe", file.ServiceName);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_SuppressExceptions_1()
        {
            ServerIniFile file = _GetFile();
            Assert.IsTrue(file.SuppressExceptions);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_SystemPath_1()
        {
            ServerIniFile file = _GetFile();
            Assert.AreEqual(@"D:\IRBIS64_2015\", file.SystemPath);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_ThreadsAvailable_1()
        {
            ServerIniFile file = _GetFile();
            Assert.IsTrue(file.ThreadsAvailable);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_ThreadsAvailableRead_1()
        {
            ServerIniFile file = _GetFile();
            Assert.IsTrue(file.ThreadsAvailableRead);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_ThreadsAvailableWrite_1()
        {
            ServerIniFile file = _GetFile();
            Assert.IsTrue(file.ThreadsAvailableWrite);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_UpperCaseTable_1()
        {
            ServerIniFile file = _GetFile();
            Assert.AreEqual(@"D:\IRBIS64_2015\isisucw", file.UpperCaseTable);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_WorkDir_1()
        {
            ServerIniFile file = _GetFile();
            Assert.AreEqual(@"D:\IRBIS64_2015\workdir", file.WorkDir);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_GetValue_1()
        {
            ServerIniFile file = _GetFile();
            string actual = file.GetValue("DBNNAMECAT", "dbnam2.mnu");
            Assert.AreEqual("dbnam1.mnu", actual);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_GetValue_2()
        {
            ServerIniFile file = _GetFile();
            int actual = file.GetValue("MappingFileSize", 10);
            Assert.AreEqual(20, actual);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_SetValue_1()
        {
            ServerIniFile file = _GetFile();
            string key = "DBNNAMECAT";
            string expected = "dbnam4.mnu";
            file.SetValue(key, expected);
            string actual = file.GetValue(key, "dbnam3.mnu");
            Assert.AreEqual(expected, actual);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_SetValue_2()
        {
            ServerIniFile file = _GetFile();
            string key = "MappingFileSize";
            int expected = 30;
            file.SetValue(key, expected);
            int actual = file.GetValue(key, 20);
            Assert.AreEqual(expected, actual);
            file.Dispose();
        }

        [TestMethod]
        public void ServerIniFile_Dispose_1()
        {
            ServerIniFile file = _GetFile();
            file.Dispose();
        }
    }
}
