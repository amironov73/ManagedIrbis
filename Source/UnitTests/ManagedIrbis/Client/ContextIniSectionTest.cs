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
    public class ContextIniSectionTest
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
        public void ContextIniSection_Construction_1()
        {
            ContextIniSection section = new ContextIniSection();
            Assert.AreEqual(ContextIniSection.SectionName, section.Section.Name);
        }

        [TestMethod]
        public void ContextIniSection_Construction_2()
        {
            IniFile iniFile = _GetIniFile();
            ContextIniSection section = new ContextIniSection(iniFile);
            Assert.AreEqual(ContextIniSection.SectionName, section.Section.Name);
            Assert.AreSame(iniFile, section.Section.Owner);
            iniFile.Dispose();
        }

        [TestMethod]
        public void ContextIniSection_Construction_3()
        {
            IniFile iniFile = _GetIniFile();
            IniFile.Section iniSection = iniFile.GetSection(ContextIniSection.SectionName);
            Assert.IsNotNull(iniSection);
            ContextIniSection section = new ContextIniSection(iniSection);
            Assert.AreSame(iniSection, section.Section);
            Assert.AreEqual(ContextIniSection.SectionName, section.Section.Name);
            Assert.AreSame(iniFile, section.Section.Owner);
            iniFile.Dispose();
        }

        [TestMethod]
        public void ContextIniSection_Database_1()
        {
            ContextIniSection section = new ContextIniSection();
            Assert.IsNull(section.Database);
            section.Database = "IBIS";
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[CONTEXT]\nDBN=IBIS\n", actual);
        }

        [TestMethod]
        public void ContextIniSection_DisplayFormat_1()
        {
            ContextIniSection section = new ContextIniSection();
            Assert.IsNull(section.DisplayFormat);
            section.DisplayFormat = "Оптимизированный";
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[CONTEXT]\nPFT=Оптимизированный\n", actual);
        }

        [TestMethod]
        public void ContextIniSection_Mfn_1()
        {
            ContextIniSection section = new ContextIniSection();
            Assert.AreEqual(0, section.Mfn);
            section.Mfn = 123;
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[CONTEXT]\nCURMFN=123\n", actual);
        }

        [TestMethod]
        public void ContextIniSection_Password_1()
        {
            ContextIniSection section = new ContextIniSection();
            Assert.IsNull(section.Password);
            section.Password = "Пароль";
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[CONTEXT]\nUserPassword=Пароль\n", actual);
        }

        [TestMethod]
        public void ContextIniSection_Query_1()
        {
            ContextIniSection section = new ContextIniSection();
            Assert.IsNull(section.Query);
            section.Query = "\"T=ЗАГЛАВИЕ\"";
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[CONTEXT]\nQUERY=\"T=ЗАГЛАВИЕ\"\n", actual);
        }

        [TestMethod]
        public void ContextIniSection_SearchPrefix_1()
        {
            ContextIniSection section = new ContextIniSection();
            Assert.IsNull(section.SearchPrefix);
            section.SearchPrefix = "T=";
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[CONTEXT]\nPREFIX=T=\n", actual);
        }

        [TestMethod]
        public void ContextIniSection_UserName_1()
        {
            ContextIniSection section = new ContextIniSection();
            Assert.IsNull(section.UserName);
            section.UserName = "Пользователь";
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[CONTEXT]\nUserName=Пользователь\n", actual);
        }

        [TestMethod]
        public void ContextIniSection_Worksheet_1()
        {
            ContextIniSection section = new ContextIniSection();
            Assert.IsNull(section.Worksheet);
            section.Worksheet = "ASP52MRS";
            string actual = section.ToString().DosToUnix();
            Assert.AreEqual("[CONTEXT]\nWS=ASP52MRS\n", actual);
        }
    }
}
