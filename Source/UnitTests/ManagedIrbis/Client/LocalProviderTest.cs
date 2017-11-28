using System.Linq;

using AM.IO;
using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Direct;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft;
using ManagedIrbis.Search;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class LocalProviderTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void LocalProvider_Construction_1()
        {
            using (LocalProvider provider = (LocalProvider) GetProvider())
            {
                Assert.IsFalse(provider.BusyState.Busy);
                Assert.IsTrue(provider.Connected);
                Assert.IsNotNull(provider.DataPath);
                Assert.IsNotNull(provider.RootPath);
                Assert.AreEqual("IBIS", provider.Database);
                Assert.AreEqual(DirectAccessMode.Exclusive, provider.Mode);
            }
        }

        [TestMethod]
        public void LocalProvider_Formatter_1()
        {
            using (LocalProvider provider = (LocalProvider) GetProvider())
            {
                PftFormatter formatter = (PftFormatter) provider.AcquireFormatter();
                Assert.IsNotNull(formatter);
                Assert.AreEqual(provider, formatter.Context.Provider);
                provider.ReleaseFormatter(formatter);
            }
        }

        [TestMethod]
        public void LocalProvider_FileExist_1()
        {
            using (LocalProvider provider = (LocalProvider) GetProvider())
            {
                FileSpecification specification
                    = new FileSpecification(IrbisPath.Data, "ibis.par");
                Assert.IsTrue(provider.FileExist(specification));

                specification = new FileSpecification(IrbisPath.Data, "nosuchfile");
                Assert.IsFalse(provider.FileExist(specification));
            }
        }

        [TestMethod]
        public void LocalProvider_FormatRecord_1()
        {
            using (LocalProvider provider = (LocalProvider) GetProvider())
            {
                MarcRecord record = provider.ReadRecord(1);
                Assert.IsNotNull(record);
                string format = "v200^a, \" : \"v200^e, \" / \"v200^f";
                string actual = provider.FormatRecord(record, format);
                Assert.AreEqual("Куда пойти учиться? : Информ. - реклам. справ " +
                    "/ З. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]",
                    actual);
            }
        }

        [TestMethod]
        public void LocalProvider_GetAlphabetTable_1()
        {
            using (LocalProvider provider = (LocalProvider) GetProvider())
            {
                IrbisAlphabetTable table = provider.GetAlphabetTable();
                Assert.AreEqual(182, table.Characters.Length);
            }
        }

        [TestMethod]
        public void LocalProvider_GetFileSearchPath_1()
        {
            using (LocalProvider provider = (LocalProvider)GetProvider())
            {
                string[] path = provider.GetFileSearchPath();
                Assert.AreEqual(3, path.Length);
            }
        }

        [TestMethod]
        public void LocalProvider_GetMaxMfn_1()
        {
            using (LocalProvider provider = (LocalProvider)GetProvider())
            {
                int actual = provider.GetMaxMfn();
                Assert.AreEqual(332, actual);
            }
        }

        [TestMethod]
        public void LocalProvider_GetStopWords_1()
        {
            using (LocalProvider provider = (LocalProvider)GetProvider())
            {
                IrbisStopWords words = provider.GetStopWords();
                Assert.IsTrue(words.IsStopWord("для"));
                Assert.IsFalse(words.IsStopWord("длина"));
            }
        }

        [TestMethod]
        public void LocalProvider_GetUserIniFile_1()
        {
            using (LocalProvider provider = (LocalProvider)GetProvider())
            {
                IniFile iniFile = provider.GetUserIniFile();
                Assert.IsNotNull(iniFile);
                string actual = iniFile["Main", "DBNNAMECAT"];
                Assert.AreEqual("dbnam2.mnu", actual);
            }
        }

        [TestMethod]
        public void LocalProvider_ListDatabases_1()
        {
            using (LocalProvider provider = (LocalProvider)GetProvider())
            {
                DatabaseInfo[] databases = provider.ListDatabases();
                Assert.AreEqual(4, databases.Length);
            }
        }

        [TestMethod]
        public void LocalProvider_ReadFile_1()
        {
            using (LocalProvider provider = (LocalProvider)GetProvider())
            {
                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        "IBIS",
                        "_test_hello.pft"
                    );
                string actual = provider.ReadFile(specification);
                Assert.AreEqual("'Hello'", actual);
            }
        }

        [TestMethod]
        public void LocalProvider_ReadRecord_1()
        {
            using (LocalProvider provider = (LocalProvider) GetProvider())
            {
                MarcRecord record = provider.ReadRecord(1);
                Assert.IsNotNull(record);
                string actual = record.FM(200, 'a');
                Assert.AreEqual("Куда пойти учиться?", actual);

                record = provider.ReadRecord(0);
                Assert.IsNull(record);
            }
        }

        [TestMethod]
        public void LocalProvider_ReadRecordVersion_1()
        {
            using (LocalProvider provider = (LocalProvider) GetProvider())
            {
                // Хронологически первая версия
                MarcRecord record = provider.ReadRecordVersion(5, -1);
                Assert.IsNotNull(record);
                Assert.AreEqual("0000000", record.FM(999));

                // Хронологически последняя версия
                record = provider.ReadRecordVersion(5, 0);
                Assert.IsNotNull(record);
                Assert.AreEqual("1", record.FM(999));
            }
        }

        [TestMethod]
        public void LocalProvider_ReadTerms_1()
        {
            using (LocalProvider provider = (LocalProvider) GetProvider())
            {
                TermParameters parameters = new TermParameters
                {
                    Database = "IBIS",
                    NumberOfTerms = 10,
                    StartTerm = "K=ACCESS"
                };
                TermInfo[] terms = provider.ReadTerms(parameters);
                Assert.AreEqual(10, terms.Length);
                Assert.AreEqual("K=ACCESS", terms[0].Text);
            }
        }

        [TestMethod]
        public void LocalProvider_ReadTerms_2()
        {
            using (LocalProvider provider = (LocalProvider) GetProvider())
            {
                TermParameters parameters = new TermParameters
                {
                    Database = "IBIS",
                    NumberOfTerms = 10,
                    StartTerm = "K=ACC"
                };
                TermInfo[] terms = provider.ReadTerms(parameters);
                Assert.AreEqual(10, terms.Length);
                Assert.AreEqual("K=ACCESS", terms[0].Text);
            }
        }

        [TestMethod]
        public void LocalProvider_Search_1()
        {
            using (LocalProvider provider = (LocalProvider) GetProvider())
            {
                int[] found = provider.Search("K=ACCESS");
                Assert.AreEqual(1, found.Length);
                MarcRecord record = provider.ReadRecord(found[0]);
                Assert.IsNotNull(record);
                string[] keywords = record.FMA(610);
                Assert.IsTrue(keywords.Contains("Access"));
            }
        }

        [TestMethod]
        public void LocalProvider_Search_2()
        {
            using (LocalProvider provider = (LocalProvider) GetProvider())
            {
                int[] found = provider.Search("K=ACCESS + K=ADAGIO");
                Assert.AreEqual(2, found.Length);
                MarcRecord record = provider.ReadRecord(found[0]);
                Assert.IsNotNull(record);
                string[] keywords = record.FMA(610);
                Assert.IsTrue(keywords.Contains("Access"));
                record = provider.ReadRecord(found[1]);
                Assert.IsNotNull(record);
                string[] titles = record.FMA(330, 'c');
                Assert.IsTrue(titles.Contains("Adagio из струнного квартета op. 11"));
            }
        }
    }
}

