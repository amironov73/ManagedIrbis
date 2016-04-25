using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient;

namespace UnitTests
{
    [TestClass]
    public class IrbisDateTest
    {
        [TestMethod]
        public void TestIrbisDate()
        {
            string date1 = "20160101";
            IrbisDate date2 = date1;
            string date3 = date2;
            Assert.AreEqual(date1, date3);
        }
    }
}
