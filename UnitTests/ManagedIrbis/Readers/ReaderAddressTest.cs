using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis.Readers;

namespace UnitTests.ManagedIrbis.Readers
{
    [TestClass]
    public class ReaderAddressTest
    {
        private void _TestSerialization
            (
                ReaderAddress first
            )
        {
            byte[] bytes = first.SaveToMemory();

            ReaderAddress second = bytes
                .RestoreObjectFromMemory<ReaderAddress>();

            Assert.AreEqual(first.AdditionalData, second.AdditionalData);
            Assert.AreEqual(first.Apartment, second.Apartment);
            Assert.AreEqual(first.City, second.City);
        }

        [TestMethod]
        public void TestReaderAddressSerialization()
        {
            ReaderAddress readerAddress = new ReaderAddress();
            _TestSerialization(readerAddress);

            readerAddress.City = "Иркутск";
            _TestSerialization(readerAddress);
        }
    }
}
