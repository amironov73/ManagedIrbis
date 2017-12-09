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
    public class EntryIniSectionTest
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
        public void EntryIniSection_Construction_1()
        {
            EntryIniSection section = new EntryIniSection();
            Assert.AreEqual(EntryIniSection.SectionName, section.Section.Name);
        }

        [TestMethod]
        public void EntryIniSection_Construction_2()
        {
            IniFile iniFile = _GetIniFile();
            EntryIniSection section = new EntryIniSection(iniFile);
            Assert.AreEqual(EntryIniSection.SectionName, section.Section.Name);
            Assert.AreSame(iniFile, section.Section.Owner);
            iniFile.Dispose();
        }

        [TestMethod]
        public void EntryIniSection_Construction_3()
        {
            IniFile iniFile = _GetIniFile();
            IniFile.Section iniSection = iniFile.GetSection(EntryIniSection.SectionName);
            Assert.IsNotNull(iniSection);
            EntryIniSection section = new EntryIniSection(iniSection);
            Assert.AreSame(iniSection, section.Section);
            Assert.AreEqual(EntryIniSection.SectionName, section.Section.Name);
            Assert.AreSame(iniFile, section.Section.Owner);
            iniFile.Dispose();
        }

        [TestMethod]
        public void EntryIniSection_DbnFlc_1()
        {
            EntryIniSection section = new EntryIniSection();
            Assert.AreEqual("DBNFLC", section.DbnFlc);
            section.DbnFlc = "12345";
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[Entry]\nDBNFLC=12345\n", actual);
        }

        [TestMethod]
        public void EntryIniSection_DefFieldNumb_1()
        {
            EntryIniSection section = new EntryIniSection();
            Assert.AreEqual(10, section.DefFieldNumb);
            section.DefFieldNumb = 12345;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[Entry]\nDefFieldNumb=12345\n", actual);
        }

        [TestMethod]
        public void EntryIniSection_MaxAddFields_1()
        {
            EntryIniSection section = new EntryIniSection();
            Assert.AreEqual(10, section.MaxAddFields);
            section.MaxAddFields = 12345;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[Entry]\nMaxAddFields=12345\n", actual);
        }

        [TestMethod]
        public void EntryIniSection_RecordUpdate_1()
        {
            EntryIniSection section = new EntryIniSection();
            Assert.AreEqual(true, section.RecordUpdate);
            section.RecordUpdate = false;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[Entry]\nRECUPDIF=0\n", actual);
        }
    }
}
