using System;

using AM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM
{
    [TestClass]
    public class DateTimeUtilityTest
    {
        [TestMethod]
        public void DateTimeUtility_Between_1()
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

        [TestMethod]
        public void DateTimeUtility_MaxDate_1()
        {
            Assert.AreEqual
                (
                    new DateTime(2017, 1, 1),
                    DateTimeUtility.MaxDate
                        (
                            new DateTime(2017, 1, 1)
                        )
                );

            Assert.AreEqual
                (
                    new DateTime(2017, 1, 1),
                    DateTimeUtility.MaxDate
                        (
                            new DateTime(2016, 1, 1),
                            new DateTime(2017, 1, 1)
                        )
                );
        }

        [TestMethod]
        public void DateTimeUtility_MinDate_1()
        {
            Assert.AreEqual
                (
                    new DateTime(2016, 1, 1),
                    DateTimeUtility.MaxDate
                        (
                            new DateTime(2016, 1, 1)
                        )
                );

            Assert.AreEqual
                (
                    new DateTime(2016, 1, 1),
                    DateTimeUtility.MinDate
                        (
                            new DateTime(2016, 1, 1),
                            new DateTime(2017, 1, 1)
                        )
                );
        }
    }
}
