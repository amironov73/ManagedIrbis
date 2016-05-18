using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class TimeSpanUtilityTest
    {
        [TestMethod]
        public void TestTimeSpanUtility()
        {
            TimeSpan span = new TimeSpan(0,0,0,1,450);
            Assert.AreEqual("1", span.ToWholeSecondsString());
            Assert.AreEqual("1.45", span.ToSecondString());
            Assert.AreEqual("00:01", span.ToMinuteString());
            Assert.AreEqual("00:00:01", span.ToHourString());
            Assert.AreEqual("00 d 00 h 00 m 01 s", span.ToDayString());
            Assert.AreEqual("1.45", span.ToAutoString());
        }
    }
}
