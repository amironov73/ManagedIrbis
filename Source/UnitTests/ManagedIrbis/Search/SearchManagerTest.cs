using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Search;

using Newtonsoft.Json.Linq;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class SearchManagerTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void SearchManager_Construction_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                SearchManager manager = new SearchManager(provider);
                Assert.AreSame(provider, manager.Provider);
            }
        }

        [TestMethod]
        public void SearchManager_LoadSearchScenarios_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        "IBIS",
                        "ibis.ini"
                    );
                SearchManager manager = new SearchManager(provider);
                SearchScenario[] scenarios
                    = manager.LoadSearchScenarios(specification);
                Assert.AreEqual(73, scenarios.Length);
            }
        }

        [TestMethod]
        public void SearchManager_Search_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                SearchManager manager = new SearchManager(provider);
                FoundLine[] found = manager.Search
                    (
                        "IBIS",
                        "K=атланты",
                        "K="
                    );
                Assert.AreEqual(1, found.Length);

                found = manager.Search
                    (
                        "IBIS",
                        "K=неттакогослова",
                        "K="
                    );
                Assert.AreEqual(0, found.Length);
            }
        }
    }
}
