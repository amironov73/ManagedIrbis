using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Caching;

namespace UnitTests.AM.Caching
{
    [TestClass]
    public class MemoryCacheTest
    {
        [TestMethod]
        public void TestMemoryCache()
        {
            MemoryCache<int,string> cache = new MemoryCache<int, string>();

            cache
                .Add(1, "one")
                .Add(2, "two")
                .Add(3, "three");

            Assert.IsNull(cache.Get(4));
            Assert.AreEqual("three", cache.Get(3));
        }
    }
}
