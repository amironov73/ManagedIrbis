using AM.IO;
using AM.PlatformAbstraction;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Pft;
using ManagedIrbis.Search;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ExpressionIsAlwaysNull

namespace UnitTests.ManagedIrbis.Client
{
    //
    // In fact, these test are for IrbisProvider class.
    //

    [TestClass]
    public class NullProviderTest
    {
        [TestMethod]
        public void NullProvider_Construction_1()
        {
            NullProvider provider = new NullProvider();
            Assert.IsNull(provider.BusyState);
            Assert.IsTrue(provider.Connected);
            Assert.AreEqual("IBIS", provider.Database);
            Assert.IsNotNull(provider.PlatformAbstraction);
            Assert.IsNotNull(provider.Services);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_AcquireFormatter_1()
        {
            NullProvider provider = new NullProvider();
            Assert.IsNull(provider.AcquireFormatter());
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_Configure_1()
        {
            NullProvider provider = new NullProvider();
            provider.Configure("some configuration");
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_ExactSearchLinks_1()
        {
            NullProvider provider = new NullProvider();
            TermLink[] links = provider.ExactSearchLinks("term");
            Assert.IsNotNull(links);
            Assert.AreEqual(0, links.Length);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_ExactSearchTrimLinks_1()
        {
            NullProvider provider = new NullProvider();
            TermLink[] links = provider.ExactSearchTrimLinks("term", 100);
            Assert.IsNotNull(links);
            Assert.AreEqual(0, links.Length);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_FileExist_1()
        {
            NullProvider provider = new NullProvider();
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "BRIEF.PFT"
                );
            Assert.IsFalse(provider.FileExist(specification));
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_FormatRecord_1()
        {
            NullProvider provider = new NullProvider();
            MarcRecord record = new MarcRecord();
            string format = "v200^a";
            Assert.AreEqual(string.Empty, provider.FormatRecord(record, format));
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_FormatRecords_1()
        {
            NullProvider provider = new NullProvider();
            int[] mfns = {1, 2, 3};
            string format = "v200^a";
            string[] expected = new string[0];
            string[] actual = provider.FormatRecords(mfns, format);
            CollectionAssert.AreEqual(expected, actual);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_GetAlphabetTable_1()
        {
            NullProvider provider = new NullProvider();
            IrbisAlphabetTable table = provider.GetAlphabetTable();
            Assert.AreEqual(182, table.Characters.Length);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_GetCatalogState_1()
        {
            NullProvider provider = new NullProvider()
            {
                PlatformAbstraction = new TestingPlatformAbstraction()
            };
            string database = "IBIS";
            CatalogState catalogState = provider.GetCatalogState(database);
            Assert.IsNotNull(catalogState);
            Assert.AreEqual(0, catalogState.Id);
            Assert.AreEqual(0, catalogState.MaxMfn);
            Assert.AreEqual(database, catalogState.Database);
            Assert.AreEqual(provider.PlatformAbstraction.Today(), catalogState.Date);
            Assert.IsNotNull(catalogState.Records);
            Assert.AreEqual(0, catalogState.Records.Length);
            Assert.IsNull(catalogState.LogicallyDeleted);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_GetGeneration_1()
        {
            NullProvider provider = new NullProvider();
            Assert.AreEqual("64", provider.GetGeneration());
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_GetMaxMfn_1()
        {
            NullProvider provider = new NullProvider();
            Assert.AreEqual(0, provider.GetMaxMfn());
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_GetFileSearchPath_1()
        {
            NullProvider provider = new NullProvider();
            string[] path = provider.GetFileSearchPath();
            Assert.IsNotNull(path);
            Assert.AreEqual(0, path.Length);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_GetStopWords_1()
        {
            NullProvider provider = new NullProvider();
            IrbisStopWords words = provider.GetStopWords();
            Assert.IsNotNull(words);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_GetUpperCaseTable_1()
        {
            NullProvider provider = new NullProvider();
            IrbisUpperCaseTable table = provider.GetUpperCaseTable();
            Assert.IsNotNull(table);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_GetUserIniFile_1()
        {
            NullProvider provider = new NullProvider();
            IniFile iniFile = provider.GetUserIniFile();
            Assert.IsNotNull(iniFile);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_ListDatabases_1()
        {
            NullProvider provider = new NullProvider();
            DatabaseInfo[] databases = provider.ListDatabases();
            Assert.IsNotNull(databases);
            Assert.AreEqual(0, databases.Length);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_NoOp_1()
        {
            NullProvider provider = new NullProvider();
            provider.NoOp();
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_ReadFile_1()
        {
            NullProvider provider = new NullProvider();
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "BRIEF.PFT"
                );
            string content = provider.ReadFile(specification);
            Assert.IsNull(content);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_ReadIniFile_1()
        {
            NullProvider provider = new NullProvider();
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "ibis.ini"
                );
            IniFile iniFile = provider.ReadIniFile(specification);
            Assert.IsNull(iniFile);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_ReadMenuFile_1()
        {
            NullProvider provider = new NullProvider();
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "oomr.mnu"
                );
            MenuFile menuFile = provider.ReadMenuFile(specification);
            Assert.IsNull(menuFile);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_ReadRecord_1()
        {
            NullProvider provider = new NullProvider();
            MarcRecord record = provider.ReadRecord(1);
            Assert.IsNull(record);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_ReadRecordVersion_1()
        {
            NullProvider provider = new NullProvider();
            MarcRecord record = provider.ReadRecordVersion(1, 10);
            Assert.IsNull(record);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_ReadSearchScenarios_1()
        {
            NullProvider provider = new NullProvider();
            SearchScenario[] scenarios = provider.ReadSearchScenarios();
            Assert.IsNotNull(scenarios);
            Assert.AreEqual(0, scenarios.Length);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_ReadTerms_1()
        {
            NullProvider provider = new NullProvider();
            TermParameters parameters = new TermParameters
            {
                Database = "IBIS",
                NumberOfTerms = 10,
                StartTerm = "A=AUTHOR"
            };
            TermInfo[] terms = provider.ReadTerms(parameters);
            Assert.IsNotNull(terms);
            Assert.AreEqual(0, terms.Length);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_Reconnect_1()
        {
            NullProvider provider = new NullProvider();
            provider.Reconnect();
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_ReleaseFormatter_1()
        {
            NullProvider provider = new NullProvider();
            IPftFormatter formatter = null;
            provider.ReleaseFormatter(formatter);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_Search_1()
        {
            NullProvider provider = new NullProvider();
            int[] found = provider.Search("some query");
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Length);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_WriteRecord_1()
        {
            NullProvider provider = new NullProvider();
            MarcRecord record = new MarcRecord();
            provider.WriteRecord(record);
            provider.Dispose();
        }

        [TestMethod]
        public void NullProvider_Dispose_1()
        {
            NullProvider provider = new NullProvider();
            provider.Dispose();
        }
    }
}
