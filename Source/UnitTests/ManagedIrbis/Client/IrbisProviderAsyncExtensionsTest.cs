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
    public class IrbisProviderAsyncExtensionsTest
    {
        [NotNull]
        public IrbisProvider _GetProvider()
        {
            return new NullProvider();
        }

        [TestMethod]
        public void IrbisProviderAsyncExtensions_ConfigureAsync_1()
        {
            IrbisProvider provider = _GetProvider();
            provider.ConfigureAsync("some configuration").Wait();
            provider.Dispose();
        }

        [TestMethod]
        public void IrbisProviderAsyncExtensions_DisposeAsync_1()
        {
            IrbisProvider provider = _GetProvider();
            provider.DisposeAsync().Wait();
        }

        [TestMethod]
        public void IrbisProviderAsyncExtensions_FormatRecordAsync_1()
        {
            IrbisProvider provider = _GetProvider();
            MarcRecord record = new MarcRecord();
            string format = "format";
            string actual = provider.FormatRecordAsync(record, format).Result;
            Assert.AreEqual("", actual);
            provider.Dispose();
        }

        [TestMethod]
        public void IrbisProviderAsyncExtensions_FormatRecordsAsync_1()
        {
            IrbisProvider provider = _GetProvider();
            int[] mfns = {1, 2, 3};
            string format = "format";
            string[] actual = provider.FormatRecordsAsync(mfns, format).Result;
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Length);
            provider.Dispose();
        }

        [TestMethod]
        public void IrbisProviderAsyncExtensions_GetMaxMfnAsync_1()
        {
            IrbisProvider provider = _GetProvider();
            int actual = provider.GetMaxMfnAsync().Result;
            Assert.AreEqual(0, actual);
            provider.Dispose();
        }

        [TestMethod]
        public void IrbisProviderAsyncExtensions_GetUserIniFileAsync_1()
        {
            IrbisProvider provider = _GetProvider();
            IniFile iniFile = provider.GetUserIniFileAsync().Result;
            Assert.IsNotNull(iniFile);
            provider.Dispose();
        }

        [TestMethod]
        public void IrbisProviderAsyncExtensions_ListDatabasesAsync_1()
        {
            IrbisProvider provider = _GetProvider();
            DatabaseInfo[] databases = provider.ListDatabasesAsync().Result;
            Assert.IsNotNull(databases);
            provider.Dispose();
        }

        [TestMethod]
        public void IrbisProviderAsyncExtensions_ReadFileAsync_1()
        {
            IrbisProvider provider = _GetProvider();
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "brief.pft"
                );
            string actual = provider.ReadFileAsync(specification).Result;
            Assert.IsNull(actual);
            provider.Dispose();
        }

        [TestMethod]
        public void IrbisProviderAsyncExtensions_ReadIniFileAsync_1()
        {
            IrbisProvider provider = _GetProvider();
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "ibis.ini"
                );
            IniFile iniFile = provider.ReadIniFileAsync(specification).Result;
            Assert.IsNull(iniFile);
            provider.Dispose();
        }

        [TestMethod]
        public void IrbisProviderAsyncExtensions_ReadMenuFileAsync_1()
        {
            IrbisProvider provider = _GetProvider();
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "oomr.mnu"
                );
            MenuFile menuFile = provider.ReadMenuFileAsync(specification).Result;
            Assert.IsNull(menuFile);
            provider.Dispose();
        }

        [TestMethod]
        public void IrbisProviderAsyncExtensions_ReadRecordAsync_1()
        {
            IrbisProvider provider = _GetProvider();
            MarcRecord record = provider.ReadRecordAsync(1).Result;
            Assert.IsNull(record);
            provider.Dispose();
        }

        [TestMethod]
        public void IrbisProviderAsyncExtensions_ReadRecordVersionAsync_1()
        {
            IrbisProvider provider = _GetProvider();
            MarcRecord record = provider.ReadRecordVersionAsync(1, 2).Result;
            Assert.IsNull(record);
            provider.Dispose();
        }

        [TestMethod]
        public void IrbisProviderAsyncExtensions_ReadSearchScenariosAsync_1()
        {
            IrbisProvider provider = _GetProvider();
            SearchScenario[] scenarios = provider.ReadSearchScenariosAsync().Result;
            Assert.IsNotNull(scenarios);
            Assert.AreEqual(0, scenarios.Length);
            provider.Dispose();
        }

        [TestMethod]
        public void IrbisProviderAsyncExtensions_ReadTermsAsync_1()
        {
            IrbisProvider provider = _GetProvider();
            TermParameters parameters = new TermParameters
            {
                Database = "IBIS",
                NumberOfTerms = 10,
                StartTerm = "A=AUTHOR"
            };
            TermInfo[] terms = provider.ReadTermsAsync(parameters).Result;
            Assert.IsNotNull(terms);
            Assert.AreEqual(0, terms.Length);
            provider.Dispose();
        }

        [TestMethod]
        public void IrbisProviderAsyncExtensions_SearchAsync_1()
        {
            IrbisProvider provider = _GetProvider();
            int[] found = provider.SearchAsync("expression").Result;
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Length);
            provider.Dispose();
        }
    }
}
