using AM.Caching;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Caching
{
    [TestClass]
    public class CacheItemTest
    {
        [NotNull]
        private CacheItem<int, string> _GetCacheItem()
        {
            return new CacheItem<int, string>(1, "one");
        }

        [TestMethod]
        public void CacheItem_Construction_1()
        {
            CacheItem<int, string> item = _GetCacheItem();
            Assert.AreEqual(item.Created, item.LastUsed);
            Assert.AreEqual(1, item.Key);
            Assert.AreEqual("one", item.Value);
        }

        [TestMethod]
        public void CacheItem_ToString_1()
        {
            CacheItem<int, string> item = _GetCacheItem();
            Assert.AreEqual("1: one", item.ToString());
        }
    }
}