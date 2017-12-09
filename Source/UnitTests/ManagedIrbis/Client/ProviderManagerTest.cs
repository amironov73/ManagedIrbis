using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AM.Runtime;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class ProviderManagerTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void ProviderManager_Construction_1()
        {
            Dictionary<string, Type> registry = ProviderManager.Registry;
            Assert.IsNotNull(registry);
            Assert.AreEqual(5, registry.Count);
        }

        [TestMethod]
        public void ProviderManager_GetProvider_1()
        {
            IrbisProvider provider = ProviderManager.GetProvider
                (
                    ProviderManager.Null,
                    false
                );
            Assert.IsNotNull(provider);

            provider = ProviderManager.GetProvider
                (
                    "NoSuchProvider",
                    false
                );
            Assert.IsNull(provider);

            const string NullTypeProvider = "NullTypeProvider";
            ProviderManager.Registry[NullTypeProvider] = null;
            provider = ProviderManager.GetProvider
                (
                    NullTypeProvider,
                    false
                );
            Assert.IsNull(provider);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void ProviderManager_GetProvider_2()
        {
            ProviderManager.GetProvider
                (
                    "NoSuchProvider",
                    true
                );
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void ProviderManager_GetProvider_3()
        {
            const string NullTypeProvider = "NullTypeProvider";
            ProviderManager.Registry[NullTypeProvider] = null;
            ProviderManager.GetProvider
                (
                    NullTypeProvider,
                    true
                );
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void ProviderManager_GetPreconfiguredProvider_1()
        {
            ProviderManager.GetPreconfiguredProvider();
        }

        [TestMethod]
        public void ProviderManager_GetAndConfigureProvider_1()
        {
            string connectionString = "Provider=Null;Some garbage;";
            IrbisProvider provider = ProviderManager
                .GetAndConfigureProvider(connectionString);
            Assert.IsNotNull(provider);
        }

        [TestMethod]
        public void ProviderManager_GetAndConfigureProvider_2()
        {
            string connectionString = "Some garbage;";
            ProviderManager.Registry[ProviderManager.Default]
                = typeof(NullProvider);
            IrbisProvider provider = ProviderManager
                .GetAndConfigureProvider(connectionString);
            Assert.IsNotNull(provider);
        }
    }
}
