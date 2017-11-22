using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Search;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class SearchResultTest
    {
        [TestMethod]
        public void SearchResult_Construction_1()
        {
            SearchResult result = new SearchResult();
            Assert.AreEqual(0, result.FoundCount);
            Assert.IsNull(result.Query);
        }

        [TestMethod]
        public void SearchResult_Properties_1()
        {
            SearchResult result = new SearchResult();
            result.FoundCount = 123;
            Assert.AreEqual(123, result.FoundCount);
            result.Query = "Query";
            Assert.AreEqual("Query", result.Query);
        }
    }
}
