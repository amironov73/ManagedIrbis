using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;

namespace UnitTests.ManagedClient
{
    [TestClass]
    public class IrbisVersionTest
    {
        [TestMethod]
        public void TestIrbisVersionConstructor()
        {
            IrbisVersion version = new IrbisVersion();
            Assert.AreEqual(null, version.Organization);
            Assert.AreEqual(null, version.Version);
            Assert.AreEqual(0, version.MaxClients);
            Assert.AreEqual(0, version.ConnectedClients);
        }

        private void _TestSerialization
            (
                IrbisVersion first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IrbisVersion second = bytes
                .RestoreObjectFromMemory<IrbisVersion>();

            Assert.AreEqual(first.Organization, second.Organization);
            Assert.AreEqual(first.Version, second.Version);
            Assert.AreEqual(first.MaxClients, second.MaxClients);
            Assert.AreEqual(first.ConnectedClients, second.ConnectedClients);
        }

        [TestMethod]
        public void TestIrbisVersionSerialization()
        {
            IrbisVersion version = new IrbisVersion();
            _TestSerialization(version);

            version.Organization = "ISTU";
            version.Version = "64.2015.1";
            version.MaxClients = 100000;
            version.ConnectedClients = 10;
            _TestSerialization(version);
        }
    }
}
