using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class StringUtiltityTest
    {
        [TestMethod]
        public void TestStringUtility()
        {
            Assert.AreEqual("a", "a".ToVisibleString());
            Assert.AreEqual("(null)", StringUtility.ToVisibleString(null));
            Assert.AreEqual("(empty)", String.Empty.ToVisibleString());
        }
    }
}
