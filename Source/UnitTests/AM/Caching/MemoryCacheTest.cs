using System;
using System.Threading;

using AM.Caching;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Caching
{
    [TestClass]
    public class MemoryCacheTest
    {
        [NotNull]
        public MemoryCache<int, string> _GetCache()
        {
            MemoryCache<int, string> result = new MemoryCache<int, string>();

            result
                .Add(1, "one")
                .Add(2, "two")
                .Add(3, "three");

            return result;
        }

        [TestMethod]
        public void MemoryCache_Add_Get_1()
        {
            MemoryCache<int, string> cache = _GetCache();

            Assert.IsNull(cache.Get(4));
            Assert.AreEqual("three", cache.Get(3));
        }

        [TestMethod]
        public void MemoryCache_Construction_1()
        {
            MemoryCache<int, string> cache = new MemoryCache<int, string>();
            Assert.AreEqual(MemoryCache<int, string>.DefaultLifetime, cache.Lifetime);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MemoryCache_Add_1()
        {
            MemoryCache<int, string> cache = new MemoryCache<int, string>();
            cache.Add(111, null);
        }

        [TestMethod]
        public void MemoryCache_Cleanup_1()
        {
            MemoryCache<int, string> cache = _GetCache();
            cache.Cleanup();
            Assert.AreEqual("one", cache.Get(1));
        }

        [TestMethod]
        public void MemoryCache_Clear_1()
        {
            MemoryCache<int, string> cache = _GetCache();
            cache.Clear();
            Assert.IsNull(cache.Get(1));
        }

        [TestMethod]
        public void MemoryCache_ContainsKey_1()
        {
            MemoryCache<int, string> cache = _GetCache();
            Assert.IsTrue(cache.ContainsKey(1));
            Assert.IsFalse(cache.ContainsKey(111));
        }

        [TestMethod]
        public void MemoryCache_Remove_1()
        {
            MemoryCache<int, string> cache = _GetCache();
            Assert.IsNotNull(cache.Get(1));
            cache.Remove(1);
            Assert.IsNull(cache.Get(1));
        }

        [TestMethod]
        public void MemoryCache_RemoveDeadItems_1()
        {
            MemoryCache<int, string> cache = _GetCache();
            // GC.Collect();
            cache.RemoveDeadItems();
            Assert.AreEqual("one", cache.Get(1));
        }

        [TestMethod]
        public void MemoryCache_RemoveOldItems_1()
        {
            MemoryCache<int, string> cache = _GetCache();
            Thread.Sleep(50);
            cache.RemoveOldItems(DateTime.Now);
            Assert.IsNull(cache.Get(1));
        }

        [TestMethod]
        public void MemoryCache_RemoveUnusedItems_1()
        {
            MemoryCache<int, string> cache = _GetCache();
            Assert.AreEqual("one", cache.Get(1));
            Thread.Sleep(50);
            cache.RemoveUnusedItems(DateTime.Now);
            Assert.IsNull(cache.Get(1));
        }

        [TestMethod]
        public void MemoryCache_UserData_1()
        {
            const string expected = "User data";
            MemoryCache<int, string> cache = new MemoryCache<int, string>();
            Assert.IsNull(cache.UserData);
            cache.UserData = expected;
            Assert.AreEqual(expected, cache.UserData);
        }

        [TestMethod]
        public void MemoryCache_GetOrRetrieve_1()
        {
            const string expected = "111";
            MemoryCache<int, string> cache = _GetCache();
            cache.Requester += key => expected;
            Assert.AreEqual(expected, cache.GetOrRequest(111));
        }
    }
}
