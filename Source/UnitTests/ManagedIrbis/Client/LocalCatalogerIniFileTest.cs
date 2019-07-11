using System.IO;

using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Search;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class LocalCatalogerIniFileTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private LocalCatalogerIniFile _GetFile()
        {
            string fileName = Path.Combine(TestDataPath, "Cirbisc.ini");
            IniFile iniFile = new IniFile(fileName, IrbisEncoding.Ansi, false);
            LocalCatalogerIniFile result = new LocalCatalogerIniFile(iniFile);

            return result;
        }

        [TestMethod]
        public void LocalCatalogerIniFile_Construction_1()
        {
            IniFile iniFile = new IniFile();
            LocalCatalogerIniFile file = new LocalCatalogerIniFile(iniFile);
            Assert.AreSame(iniFile, file.Ini);
            Assert.IsNotNull(file.Main);
        }

        [TestMethod]
        public void LocalCatalogerIniFile_Organization_1()
        {
            LocalCatalogerIniFile file = _GetFile();
            Assert.AreEqual("Иркутский государственный технический университет", file.Organization);
        }

        [TestMethod]
        public void LocalCatalogerIniFile_ServerIP_1()
        {
            LocalCatalogerIniFile file = _GetFile();
            Assert.AreEqual("127.0.0.1", file.ServerIP);
        }

        [TestMethod]
        public void LocalCatalogerIniFile_ServerPort_1()
        {
            LocalCatalogerIniFile file = _GetFile();
            Assert.AreEqual(6666, file.ServerPort);
        }

        [TestMethod]
        public void LocalCatalogerIniFile_GetValue_1()
        {
            LocalCatalogerIniFile file = _GetFile();
            string actual = file.GetValue("Main", "FontName", "Arial");
            Assert.AreEqual("Arial", actual);
        }

        [TestMethod]
        public void LocalCatalogerIniFile_GetValue_2()
        {
            LocalCatalogerIniFile file = _GetFile();
            string actual = file.GetValue("Main", "FontCharSet", "204");
            Assert.AreEqual("204", actual);
        }

        [TestMethod]
        public void LocalCatalogerIniFile_Load_1()
        {
            string fileName = Path.Combine(TestDataPath, "Cirbisc.ini");
            LocalCatalogerIniFile file = LocalCatalogerIniFile.Load(fileName);
            Assert.IsNotNull(file);
        }
    }
}
