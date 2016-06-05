using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class ConversionUtilityTest
    {
        [TestMethod]
        public void TestConversionUtilityCanConvertTo()
        {
            Assert.IsTrue
                (
                    ConversionUtility.CanConvertTo<bool>(1)
                );
            Assert.IsTrue
                (
                    ConversionUtility.CanConvertTo<int>("1")
                );
        }

        [TestMethod]
        public void TestConversionUtilityConvertTo()
        {
            Assert.AreEqual
                (
                    true,
                    ConversionUtility.ConvertTo<bool>(1)
                );
            Assert.AreEqual
                (
                    1,
                    ConversionUtility.ConvertTo<int>("1")
                );
        }

        [TestMethod]
        public void TestConversionUtilityToBoolean()
        {
            Assert.AreEqual
                (
                    true,
                    ConversionUtility.ToBoolean(1)
                );
            Assert.AreEqual
                (
                    true,
                    ConversionUtility.ToBoolean("true")
                );
            Assert.AreEqual
                (
                    true,
                    ConversionUtility.ToBoolean("yes")
                );
        }
    }
}
