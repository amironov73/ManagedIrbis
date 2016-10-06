using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis.Readers;

namespace UnitTests.ManagedIrbis.Readers
{
    [TestClass]
    public class IriProfileTest
    {
        private void _TestSerialization
            (
                IriProfile first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IriProfile second = bytes
                .RestoreObjectFromMemory<IriProfile>();

            Assert.AreEqual(first.Active, second.Active);
            Assert.AreEqual(first.Database, second.Database);
            Assert.AreEqual(first.Title, second.Title);
            Assert.AreEqual(first.Query, second.Query);
        }

        [TestMethod]
        public void TestIriProfileSerialization()
        {
            IriProfile iriProfile = new IriProfile();
            _TestSerialization(iriProfile);

            iriProfile.Title = "abc";
            iriProfile.Query = "bca";
            _TestSerialization(iriProfile);
        }
    }
}
