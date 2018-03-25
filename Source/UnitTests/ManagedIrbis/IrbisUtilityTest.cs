using System;

using AM;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable MustUseReturnValue

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisUtilityTest
    {
        [TestMethod]
        public void IrbisUtility_EncodePercentString_1()
        {
            byte[] bytes = { 0x01, 0x5E, 0x34, 0xBC, 0x00, 0x00, 0x00, 0x00, 0x02, 0xA3, 0x5D, 0xF3 };
            string expected = "%01%5E4%BC%00%00%00%00%02%A3%5D%F3";
            string actual = IrbisUtility.EncodePercentString(bytes);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IrbisUtility_EncodePercentString_2()
        {
            Assert.AreEqual(string.Empty, IrbisUtility.EncodePercentString(null));
            Assert.AreEqual(string.Empty, IrbisUtility.EncodePercentString(EmptyArray<byte>.Value));
        }

        [TestMethod]
        public void IrbisUtility_DecodePercentString_1()
        {
            string text = "%01%5E4%BC%00%00%00%00%02%A3%5D%F3";
            byte[] expected = { 0x01, 0x5E, 0x34, 0xBC, 0x00, 0x00, 0x00, 0x00, 0x02, 0xA3, 0x5D, 0xF3 };
            byte[] actual = IrbisUtility.DecodePercentString(text);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IrbisUtility_DecodePercentString_2()
        {
            Assert.AreEqual(0, IrbisUtility.DecodePercentString(null).Length);
            Assert.AreEqual(0, IrbisUtility.DecodePercentString(string.Empty).Length);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void IrbisUtility_DecodePercentString_3()
        {
            string text = "%01%5";
            IrbisUtility.DecodePercentString(text);
        }
    }
}
