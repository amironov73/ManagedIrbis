using System;
using System.IO;

using AM.PlatformAbstraction;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class ExternalPftCacheTest
    {
        [NotNull]
        private string _GetPath()
        {
            string result = Path.Combine
                (
                    Path.GetTempPath(),
                    "AnotherPftCache"
                );

            if (!Directory.Exists(result))
            {
                Directory.CreateDirectory(result);
            }

            return result;
        }

        [NotNull]
        private byte[] _GetDll()
        {
            return new byte[] { 1, 2, 3 };
        }

        [NotNull]
        private byte[] _GetPft()
        {
            return new byte[] { 3, 2, 1 };
        }

        [TestMethod]
        public void ExternalPftCache_Construction_1()
        {
            ExternalPftCache cache = new ExternalPftCache();
            Assert.AreEqual(cache.RootDirectory, cache.GetDefaultRootDirectory());
        }

        [TestMethod]
        public void ExternalPftCache_Construction_2()
        {
            string path = _GetPath();
            ExternalPftCache cache = new ExternalPftCache(path);
            Assert.AreEqual(path, cache.RootDirectory);
        }

        [TestMethod]
        public void ExternalPftCache_AddDll_1()
        {
            const string scriptText = "someScript";
            ExternalPftCache cache = new ExternalPftCache();
            byte[] image = _GetDll();
            cache.AddDll(scriptText, image);
            string fileName = cache.GetPath
                (
                    scriptText,
                    ExternalPftCache.DLL
                );
            Assert.IsNotNull(fileName);
            byte[] actual = File.ReadAllBytes(fileName);
            CollectionAssert.AreEqual(image, actual);
        }

        [TestMethod]
        public void ExternalPftCache_AddSerializedPft_1()
        {
            const string scriptText = "someScript";
            ExternalPftCache cache = new ExternalPftCache();
            byte[] image = _GetPft();
            cache.AddSerializedPft(scriptText, image);
            string fileName = cache.GetPath
                (
                    scriptText,
                    ExternalPftCache.AST
                );
            Assert.IsNotNull(fileName);
            byte[] actual = File.ReadAllBytes(fileName);
            CollectionAssert.AreEqual(image, actual);
        }

        [TestMethod]
        public void ExternalPftCache_Clear_1()
        {
            ExternalPftCache cache = new ExternalPftCache();
            cache.Clear();
            string[] files = Directory.GetFiles(cache.RootDirectory);
            Assert.AreEqual(0, files.Length);
        }

        [TestMethod]
        public void ExternalPftCache_GetDll_1()
        {
            const string scriptText = "no such script";
            ExternalPftCache cache = new ExternalPftCache();
            cache.Clear();
            Func<PftContext, PftPacket> func = cache.GetDll(scriptText);
            Assert.IsNull(func);
        }

        [TestMethod]
        [ExpectedException(typeof(BadImageFormatException))]
        public void ExternalPftCache_GetDll_2()
        {
            const string scriptText = "no such script";
            ExternalPftCache cache = new ExternalPftCache();
            byte[] image = _GetDll();
            cache.AddDll(scriptText, image);
            cache.GetDll(scriptText);
        }

        [TestMethod]
        public void ExternalPftCache_GetSerializedPft_1()
        {
            const string scriptText = "no such script";
            ExternalPftCache cache = new ExternalPftCache();
            cache.Clear();
            PftNode node = cache.GetSerializedPft(scriptText);
            Assert.IsNull(node);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void ExternalPftCache_GetSerializedPft_2()
        {
            const string scriptText = "no such script";
            ExternalPftCache cache = new ExternalPftCache();
            byte[] image = _GetPft();
            cache.AddSerializedPft(scriptText, image);
            cache.GetSerializedPft(scriptText);
        }

        [TestMethod]
        public void ExternalPftCache_ToString_1()
        {
            ExternalPftCache cache = new ExternalPftCache();
            Assert.AreEqual(cache.RootDirectory, cache.ToString());
        }
    }
}

