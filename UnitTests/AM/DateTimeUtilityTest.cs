using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class DateTimeUtilityTest
    {
        [TestMethod]
        public void TestDateTime_Between()
        {
            Assert.IsTrue
                (
                    new DateTime(2010, 1, 1).Between
                    (
                        new DateTime(2000, 1, 1),
                        new DateTime(2016, 1, 1)
                    )
                );

            Assert.IsFalse
                (
                    new DateTime(2017, 1, 1).Between
                    (
                        new DateTime(2000, 1, 1),
                        new DateTime(2016, 1, 1)
                    )
                );
        }
    }
}
