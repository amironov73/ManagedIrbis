using System.IO;

using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class RemoteCatalogerIniFileTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private RemoteCatalogerIniFile _GetFile()
        {
            string fileName = Path.Combine(Irbis64RootPath, "irbisc.ini");
            IniFile iniFile = new IniFile(fileName, IrbisEncoding.Ansi, false);
            RemoteCatalogerIniFile result = new RemoteCatalogerIniFile(iniFile);

            return result;
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_Construction_1()
        {
            IniFile iniFile = new IniFile();
            RemoteCatalogerIniFile file = new RemoteCatalogerIniFile(iniFile);
            Assert.AreSame(iniFile, file.Ini);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_AutoinFile_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("autoin.gbl", file.AutoinFile);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_AutoMerge_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.IsFalse(file.AutoMerge);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_BriefPft_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("BRIEF", file.BriefPft);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_ClientTimeLive_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual(15, file.ClientTimeLive);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_CopyMnu_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("FSTW.MNU", file.CopyMnu);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_CountTag_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("999", file.CountTag);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_DatabaseList_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("dbnam2.mnu", file.DatabaseList);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_DbnFlc_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("DBNFLC", file.DbnFlc);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_DefaultDb_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("ISTU", file.DefaultDb);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_EmptyDbn_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("BLANK", file.EmptyDbn);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_ExemplarTag_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("910", file.ExemplarTag);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_ExportMenu_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("EXPORTW.MNU", file.ExportMenu);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_FormatMenu_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("FMT31.MNU", file.FormatMenu);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_HelpDbn_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("HELP", file.HelpDbn);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_ImportMenu_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("IMPORTW.MNU", file.ImportMenu);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_IndexPrefix_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("I=", file.IndexPrefix);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_IndexTag_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("903", file.IndexTag);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_IriMenu_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("iri.mnu", file.IriMenu);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_MaxBriefPortion_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual(10, file.MaxBriefPortion);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_MaxMarked_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual(10, file.MaxMarked);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_PftMenu_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("PFTw.MNU", file.PftMenu);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_PftOpt_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("PFTw_H.OPT", file.PftOpt);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_SearchIni_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual(string.Empty, file.SearchIni);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_StandardDbn_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("IBIS", file.StandardDbn);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_WorkDirectory_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("c:\\irbiswrk\\", file.WorkDirectory);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_WsOpt_1()
        {
            RemoteCatalogerIniFile file = _GetFile();
            Assert.AreEqual("WS31.OPT", file.WsOpt);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_SetValue_1()
        {
            IniFile iniFile = new IniFile();
            RemoteCatalogerIniFile file = new RemoteCatalogerIniFile(iniFile);
            string main = RemoteCatalogerIniFile.Main;
            string key = "PRMARCFORMAT";
            string expected = "MARC";
            file.SetValue(main, key, expected);
            string actual = file.GetValue(main, key, "!");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoteCatalogerIniFile_SetValue_2()
        {
            IniFile iniFile = new IniFile();
            RemoteCatalogerIniFile file = new RemoteCatalogerIniFile(iniFile);
            string main = RemoteCatalogerIniFile.Main;
            string key = "KKKFONTSIZE";
            int expected = 12;
            file.SetValue(main, key, expected);
            int actual = file.GetValue(main, key, 0);
            Assert.AreEqual(expected, actual);
        }
    }
}
