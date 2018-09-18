using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Server;

namespace UnitTests.ManagedIrbis.Server
{
    [TestClass]
    public class ServerCacheTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void ServerCache_Construction_1()
        {
            ServerCache cache = new ServerCache();
            Assert.AreEqual(0, cache.MemoryUsage);
            Assert.AreEqual(ServerCache.DefaultLimit, cache.MemoryLimit);
        }

        [TestMethod]
        public void ServerCache_GetFile_1()
        {
            string fileName = Path.Combine(Irbis64RootPath, "client_ini.mnu");
            ServerCache cache = new ServerCache();
            byte[] content = cache.GetFile(fileName);
            Assert.IsNotNull(content);
            Assert.AreEqual(299, content.Length);
            Assert.AreEqual(299, cache.MemoryUsage);
            cache.Clear();
            Assert.AreEqual(0, cache.MemoryUsage);
        }
    }
}
