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
    public class DisplayIniSectionTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetFileName()
        {
            return Path.Combine(Irbis64RootPath, "irbisc.ini");
        }

        [NotNull]
        private IniFile _GetIniFile()
        {
            return new IniFile(_GetFileName(), IrbisEncoding.Ansi, false);
        }

        [TestMethod]
        public void DisplayIniSection_Construction_1()
        {
            DisplayIniSection section = new DisplayIniSection();
            Assert.AreEqual(DisplayIniSection.SectionName, section.Section.Name);
        }

        [TestMethod]
        public void DisplayIniSection_Construction_2()
        {
            IniFile iniFile = _GetIniFile();
            DisplayIniSection section = new DisplayIniSection(iniFile);
            Assert.AreEqual(DisplayIniSection.SectionName, section.Section.Name);
            Assert.AreSame(iniFile, section.Section.Owner);
            iniFile.Dispose();
        }

        [TestMethod]
        public void DisplayIniSection_Construction_3()
        {
            IniFile iniFile = _GetIniFile();
            IniFile.Section iniSection = iniFile.GetSection(DisplayIniSection.SectionName);
            Assert.IsNotNull(iniSection);
            DisplayIniSection section = new DisplayIniSection(iniSection);
            Assert.AreSame(iniSection, section.Section);
            Assert.AreEqual(DisplayIniSection.SectionName, section.Section.Name);
            Assert.AreSame(iniFile, section.Section.Owner);
            iniFile.Dispose();
        }

        [TestMethod]
        public void DisplayIniSection_MaxBriefPortion_1()
        {
            DisplayIniSection section = new DisplayIniSection();
            Assert.AreEqual(6, section.MaxBriefPortion);
            section.MaxBriefPortion = 12345;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[Display]\nMaxBriefPortion=12345\n", actual);
        }

        [TestMethod]
        public void DisplayIniSection_MaxMarked_1()
        {
            DisplayIniSection section = new DisplayIniSection();
            Assert.AreEqual(100, section.MaxMarked);
            section.MaxMarked = 12345;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[Display]\nMaxMarked=12345\n", actual);
        }
    }
}
