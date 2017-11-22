using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Client;
using ManagedIrbis.Search;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class SearchContextTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void SearchContext_Construction_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                SearchManager manager = new SearchManager(provider);
                SearchContext context = new SearchContext(manager, provider);
                Assert.AreSame(provider, context.Provider);
                Assert.AreSame(manager, context.Manager);
            }
        }
    }
}
