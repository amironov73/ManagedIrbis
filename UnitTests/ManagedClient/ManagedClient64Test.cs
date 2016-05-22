using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedClient;

namespace UnitTests.ManagedClient
{
    [TestClass]
    public class ManagedClient64Test
    {
        [TestMethod]
        public void TestManagedClient64Constructor()
        {
            using (ManagedClient64 client = new ManagedClient64())
            {
                Assert.IsNotNull(client);
            }
        }

        private void _TestSerialization
            (
                ManagedClient64 first
            )
        {
            byte[] bytes = first.SaveToMemory();

            ManagedClient64 second = bytes
                .RestoreObjectFromMemory<ManagedClient64>();

            Assert.IsNotNull(second);
        }

        [TestMethod]
        public void TestManagedClient64Serialization()
        {
            ManagedClient64 client = new ManagedClient64();
            _TestSerialization(client);
        }
    }
}
