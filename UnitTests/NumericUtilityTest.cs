using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests
{
    [TestClass]
    public class NumericUtilityTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(true, "1".IsPositiveInteger());
            Assert.AreEqual(false, "-1".IsPositiveInteger());
            Assert.AreEqual(false, "0".IsPositiveInteger());
        }
    }
}
