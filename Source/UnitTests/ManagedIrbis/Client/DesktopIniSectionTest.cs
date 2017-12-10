using System.IO;

using AM.IO;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class DesktopIniSectionTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetFileName()
        {
            return Path.Combine(TestDataPath, "cirbisc.ini");
        }

        [NotNull]
        private IniFile _GetIniFile()
        {
            return new IniFile(_GetFileName(), IrbisEncoding.Ansi, false);
        }

        [TestMethod]
        public void DesktopIniSection_Construction_1()
        {
            DesktopIniSection section = new DesktopIniSection();
            Assert.AreEqual(DesktopIniSection.SectionName, section.Section.Name);
        }

        [TestMethod]
        public void DesktopIniSection_Construction_2()
        {
            IniFile iniFile = _GetIniFile();
            DesktopIniSection section = new DesktopIniSection(iniFile);
            Assert.AreEqual(DesktopIniSection.SectionName, section.Section.Name);
            Assert.AreSame(iniFile, section.Section.Owner);
            iniFile.Dispose();
        }

        [TestMethod]
        public void DesktopIniSection_Construction_3()
        {
            IniFile iniFile = _GetIniFile();
            IniFile.Section iniSection = iniFile.GetSection(DesktopIniSection.SectionName);
            Assert.IsNotNull(iniSection);
            DesktopIniSection section = new DesktopIniSection(iniSection);
            Assert.AreSame(iniSection, section.Section);
            Assert.AreEqual(DesktopIniSection.SectionName, section.Section.Name);
            Assert.AreSame(iniFile, section.Section.Owner);
            iniFile.Dispose();
        }

        [TestMethod]
        public void DesktopIniSection_AutoService_1()
        {
            DesktopIniSection section = new DesktopIniSection();
            Assert.IsTrue(section.AutoService);
            section.AutoService = false;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nAutoService=0\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_DBContext_1()
        {
            DesktopIniSection section = new DesktopIniSection();
            Assert.IsTrue(section.DBContext);
            section.DBContext = false;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nDBContext=0\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_DBContextFloating_1()
        {
            DesktopIniSection section = new DesktopIniSection();
            Assert.IsFalse(section.DBContextFloating);
            section.DBContextFloating = true;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nDBContextFloating=1\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_DBOpen_1()
        {
            DesktopIniSection section = new DesktopIniSection();
            Assert.IsTrue(section.DBOpen);
            section.DBOpen = false;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nDBOpen=0\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_DBOpenFloating_1()
        {
            DesktopIniSection section = new DesktopIniSection();
            Assert.IsFalse(section.DBOpenFloating);
            section.DBOpenFloating = true;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nDBOpenFloating=1\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_Entry_1()
        {
            DesktopIniSection section = new DesktopIniSection();
            Assert.IsTrue(section.Entry);
            section.Entry = false;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nEntry=0\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_EntryFloating_1()
        {
            DesktopIniSection section = new DesktopIniSection();
            Assert.IsFalse(section.EntryFloating);
            section.EntryFloating = true;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nEntryFloating=1\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_MainMenu_1()
        {
            DesktopIniSection section = new DesktopIniSection();
            Assert.IsTrue(section.MainMenu);
            section.MainMenu = false;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nMainMenu=0\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_MainMenuFloating_1()
        {
            DesktopIniSection section = new DesktopIniSection();
            Assert.IsFalse(section.MainMenuFloating);
            section.MainMenuFloating = true;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nMainMenuFloating=1\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_Search_1()
        {
            DesktopIniSection section = new DesktopIniSection();
            Assert.IsTrue(section.Search);
            section.Search = false;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nSearch=0\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_SearchFloating_1()
        {
            DesktopIniSection section = new DesktopIniSection();
            Assert.IsFalse(section.SearchFloating);
            section.SearchFloating = true;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nSearchFloating=1\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_Spelling_1()
        {
            DesktopIniSection section = new DesktopIniSection();
            Assert.IsTrue(section.Spelling);
            section.Spelling = false;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nSpelling=0\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_UserMode_1()
        {
            DesktopIniSection section = new DesktopIniSection();
            Assert.IsTrue(section.UserMode);
            section.UserMode = false;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nUserMode=0\n", actual);
        }

        [TestMethod]
        public void DesktopIniSection_UserModeFloating_1()
        {
            DesktopIniSection section = new DesktopIniSection();
            Assert.IsFalse(section.UserModeFloating);
            section.UserModeFloating = true;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[DESKTOP]\nUserModeFloating=1\n", actual);
        }
    }
}
