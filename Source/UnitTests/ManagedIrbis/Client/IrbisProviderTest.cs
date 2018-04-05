using System;
using System.ComponentModel.Design;
using System.IO;

using AM;
using AM.IO;

using ManagedIrbis;
using ManagedIrbis.Authentication;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Pft;
using ManagedIrbis.Search;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable PossibleNullReferenceException

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class IrbisProviderTest
        : Common.CommonUnitTest
    {
        class MyProvider
            : IrbisProvider
        {
            public MyProvider(string testDataPath)
            {
                TestDataPath = testDataPath;
            }

            public string TestDataPath { get; set; }

            public override string ReadFile
                (
                    FileSpecification specification
                )
            {
                string fileName = Path.Combine(TestDataPath, specification.FileName);
                string result = File.ReadAllText(fileName, IrbisEncoding.Ansi);

                return result;
            }
        }

        [TestMethod]
        public void IrbisProvider_Construction_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                Assert.IsNull(provider.BusyState);
                Assert.IsTrue(provider.Connected);
                Assert.IsNotNull(provider.Database);
                Assert.IsNull(provider.CredentialsResolver);

                provider.Database = "ISTU";
                Assert.AreEqual("ISTU", provider.Database);
            }
        }

        [TestMethod]
        public void IrbisProvider_CredentialsResolver_1()
        {
            Mock<ICredentialsResolver> mock = new Mock<ICredentialsResolver>();
            ICredentialsResolver resolver = mock.Object;

            using (IrbisProvider provider = new NullProvider())
            {
                provider.CredentialsResolver = resolver;
                Assert.AreSame(resolver, provider.CredentialsResolver);
            }
        }

        //[TestMethod]
        //public void IrbisProvider_Services_1()
        //{
        //    using (IrbisProvider provider = new NullProvider())
        //    {
        //        IServiceProvider services = new ServiceContainer();
        //        provider.Services = new NonNullValue<IServiceProvider> (services);
        //        Assert.AreSame(services, provider.Services.Value);
        //    }
        //}

        [TestMethod]
        public void IrbisProvider_AcquireFormatter_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                IPftFormatter formatter = provider.AcquireFormatter();
                Assert.IsNull(formatter);
                provider.ReleaseFormatter(formatter);
            }
        }

        [TestMethod]
        public void IrbisProvider_Configure_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                provider.Configure("configurationString");
            }
        }

        [TestMethod]
        public void IrbisProvider_ExactSearchLinks_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                TermLink[] links = provider.ExactSearchLinks("term");
                Assert.IsNotNull(links);
                Assert.AreEqual(0, links.Length);
            }
        }

        [TestMethod]
        public void IrbisProvider_ExactSearchTrimLinks_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                TermLink[] links = provider.ExactSearchTrimLinks("term", 10);
                Assert.IsNotNull(links);
                Assert.AreEqual(0, links.Length);
            }
        }

        [TestMethod]
        public void IrbisProvider_FileExist_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                FileSpecification specification = new FileSpecification
                    (
                        path: IrbisPath.MasterFile,
                        database: "IBIS",
                        fileName: "someFile.txt"
                    );
                Assert.IsFalse(provider.FileExist(specification));
            }
        }

        [TestMethod]
        public void IrbisProvider_FormatRecord_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                MarcRecord record = new MarcRecord();
                Assert.AreEqual(string.Empty, provider.FormatRecord(record, "format"));
            }
        }

        [TestMethod]
        public void IrbisProvider_FormatRecords_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                int[] mfns = { 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144,
                    233, 377, 610, 987, 1597, 2584, 4181, 6765, 10946, 17711 };
                string[] formatted = provider.FormatRecords(mfns, "format");
                Assert.IsNotNull(formatted);
                Assert.AreEqual(0, formatted.Length);
            }
        }

        [TestMethod]
        public void IrbisProvider_GetAlphabetTable_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                IrbisAlphabetTable table = provider.GetAlphabetTable();
                Assert.IsNotNull(table);
                Assert.AreEqual(182, table.Characters.Length);
            }
        }

        [TestMethod]
        public void IrbisProvider_GetCatalogState_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                CatalogState state = provider.GetCatalogState("IBIS");
                Assert.IsNotNull(state);
            }
        }

        [TestMethod]
        public void IrbisProvider_GetGeneration_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                string generation = provider.GetGeneration();
                Assert.AreEqual("64", generation);
            }
        }

        [TestMethod]
        public void IrbisProvider_GetFileSearchPath_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                string[] path = provider.GetFileSearchPath();
                Assert.IsNotNull(path);
                Assert.AreEqual(0, path.Length);
            }
        }

        [TestMethod]
        public void IrbisProvider_GetStopWords_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                IrbisStopWords words = provider.GetStopWords();
                Assert.IsNotNull(words);
                Assert.IsFalse(words.IsStopWord("about"));
                Assert.IsFalse(words.IsStopWord("IRBIS"));
            }
        }

        [TestMethod]
        public void IrbisProvider_GetUpperCaseTable_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                IrbisUpperCaseTable table = provider.GetUpperCaseTable();
                Assert.IsNotNull(table);
                Assert.AreEqual("SOME TEXT", table.ToUpper("some text"));
            }
        }

        [TestMethod]
        public void IrbisProvider_GetUserIniFile_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                IniFile iniFile = provider.GetUserIniFile();
                Assert.IsNotNull(iniFile);
            }
        }

        [TestMethod]
        public void IrbisProvider_ListDatabases_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                DatabaseInfo[] databases = provider.ListDatabases();
                Assert.IsNotNull(databases);
                Assert.AreEqual(0, databases.Length);
            }
        }

        [TestMethod]
        public void IrbisProvider_GetMaxMfn_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                int maxMfn = provider.GetMaxMfn();
                Assert.AreEqual(0, maxMfn);
            }
        }

        [TestMethod]
        public void IrbisProvider_NoOp_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                provider.NoOp();
                Assert.IsNotNull(provider);
            }
        }

        [TestMethod]
        public void IrbisProvider_ReadFile_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                FileSpecification specification = new FileSpecification
                    (
                        path: IrbisPath.MasterFile,
                        database: "IBIS",
                        fileName: "someFile.txt"
                    );
                string content = provider.ReadFile(specification);
                Assert.IsNull(content);
            }
        }

        [TestMethod]
        public void IrbisProvider_ReadIniFile_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                FileSpecification specification = new FileSpecification
                    (
                        path: IrbisPath.MasterFile,
                        database: "IBIS",
                        fileName: "some.ini"
                    );
                IniFile iniFile = provider.ReadIniFile(specification);
                Assert.IsNull(iniFile);
            }
        }

        [TestMethod]
        public void IrbisProvider_ReadIniFile_2()
        {
            using (IrbisProvider provider = new MyProvider(TestDataPath))
            {
                FileSpecification specification = new FileSpecification
                    (
                        path: IrbisPath.MasterFile,
                        database: "IBIS",
                        fileName: "IniFile1.ini"
                    );
                IniFile iniFile = provider.ReadIniFile(specification);
                Assert.IsNotNull(iniFile);
            }
        }

        [TestMethod]
        public void IrbisProvider_ReadMenuFile_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                FileSpecification specification = new FileSpecification
                    (
                        path: IrbisPath.MasterFile,
                        database: "IBIS",
                        fileName: "someFile.mnu"
                    );
                MenuFile menuFile = provider.ReadMenuFile(specification);
                Assert.IsNull(menuFile);
            }
        }

        [TestMethod]
        public void IrbisProvider_ReadMenuFile_2()
        {
            using (IrbisProvider provider = new MyProvider(TestDataPath))
            {
                FileSpecification specification = new FileSpecification
                (
                    path: IrbisPath.MasterFile,
                    database: "IBIS",
                    fileName: "ORG.MNU"
                );
                MenuFile menu = provider.ReadMenuFile(specification);
                Assert.IsNotNull(menu);
                string expected = "RU";
                string actual = menu.GetString("1");
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void IrbisProvider_ReadRecord_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                MarcRecord record = provider.ReadRecord(1);
                Assert.IsNull(record);
            }
        }

        [TestMethod]
        public void IrbisProvider_ReadRecordVersion_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                MarcRecord record = provider.ReadRecordVersion(1, 2);
                Assert.IsNull(record);
            }
        }

        [TestMethod]
        public void IrbisProvider_ReadSearchScenarios_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                SearchScenario[] scenarios = provider.ReadSearchScenarios();
                Assert.IsNotNull(scenarios);
                Assert.AreEqual(0, scenarios.Length);
            }
        }

        [TestMethod]
        public void IrbisProvider_ReadTerms_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                TermParameters parameters = new TermParameters
                {
                    Database = "IBIS",
                    StartTerm = "term",
                    NumberOfTerms = 10
                };
                TermInfo[] terms = provider.ReadTerms(parameters);
                Assert.IsNotNull(terms);
                Assert.AreEqual(0, terms.Length);
            }
        }

        [TestMethod]
        public void IrbisProvider_Reconnect_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                provider.Reconnect();
                Assert.IsNotNull(provider);
            }
        }

        [TestMethod]
        public void IrbisProvider_Search_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                int[] found = provider.Search("expression");
                Assert.IsNotNull(found);
                Assert.AreEqual(0, found.Length);
            }
        }

        [TestMethod]
        public void IrbisProvider_WriteRecord_1()
        {
            using (IrbisProvider provider = new NullProvider())
            {
                MarcRecord record = new MarcRecord();
                provider.WriteRecord(record);
            }
        }
    }
}
