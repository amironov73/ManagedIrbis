using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class TimeSpanUtilityTest
    {
        [TestMethod]
        public void TimeSpanUtility_OneDay_1()
        {
            TimeSpan span = TimeSpanUtility.OneDay;
            Assert.AreEqual(span.Days, 1);
            Assert.AreEqual(span.Hours, 0);
            Assert.AreEqual(span.Minutes, 0);
            Assert.AreEqual(span.Seconds, 0);
            Assert.AreEqual(span.Milliseconds, 0);
        }

        [TestMethod]
        public void TimeSpanUtility_OneHour_1()
        {
            TimeSpan span = TimeSpanUtility.OneHour;
            Assert.AreEqual(span.Days, 0);
            Assert.AreEqual(span.Hours, 1);
            Assert.AreEqual(span.Minutes, 0);
            Assert.AreEqual(span.Seconds, 0);
            Assert.AreEqual(span.Milliseconds, 0);
        }

        [TestMethod]
        public void TimeSpanUtility_OneMinute_1()
        {
            TimeSpan span = TimeSpanUtility.OneMinute;
            Assert.AreEqual(span.Days, 0);
            Assert.AreEqual(span.Hours, 0);
            Assert.AreEqual(span.Minutes, 1);
            Assert.AreEqual(span.Seconds, 0);
            Assert.AreEqual(span.Milliseconds, 0);
        }

        [TestMethod]
        public void TimeSpanUtility_OneSecond_1()
        {
            TimeSpan span = TimeSpanUtility.OneSecond;
            Assert.AreEqual(span.Days, 0);
            Assert.AreEqual(span.Hours, 0);
            Assert.AreEqual(span.Minutes, 0);
            Assert.AreEqual(span.Seconds, 1);
            Assert.AreEqual(span.Milliseconds, 0);
        }

        [TestMethod]
        public void TimeSpanUtility_IsZero_1()
        {
            Assert.IsTrue(TimeSpan.Zero.IsZero());
            Assert.IsFalse(TimeSpanUtility.OneMinute.IsZero());
        }

        [TestMethod]
        public void TimeSpanUtility_IsZeroOrLess_1()
        {
            Assert.IsTrue(TimeSpan.Zero.IsZeroOrLess());
            Assert.IsFalse(TimeSpanUtility.OneMinute.IsZeroOrLess());
            Assert.IsTrue(new TimeSpan(-1000).IsZeroOrLess());
        }

        [TestMethod]
        public void TimeSpanUtility_LessThanZero_1()
        {
            Assert.IsFalse(TimeSpan.Zero.LessThanZero());
            Assert.IsFalse(TimeSpanUtility.OneMinute.LessThanZero());
            Assert.IsTrue(new TimeSpan(-1000).LessThanZero());
        }

        [TestMethod]
        public void TimeSpanUtility_ToString_1()
        {
            TimeSpan span = new TimeSpan(0,0,0,1,450);
            Assert.AreEqual("1", span.ToWholeSecondsString());
            Assert.AreEqual("1.45", span.ToSecondString());
            Assert.AreEqual("00:01", span.ToMinuteString());
            Assert.AreEqual("00:00:01", span.ToHourString());
            Assert.AreEqual("00 d 00 h 00 m 01 s", span.ToDayString());
        }

        [TestMethod]
        public void TimeSpanUtility_ToAutoString_1()
        {
            TimeSpan span = new TimeSpan(0, 0, 0, 1, 450);
            Assert.AreEqual("1.45", span.ToAutoString());

            span = new TimeSpan(0, 0, 1, 45, 450);
            Assert.AreEqual("01:45", span.ToAutoString());

            span = new TimeSpan(0, 1, 40, 45, 450);
            Assert.AreEqual("01:40:45", span.ToAutoString());

            span = new TimeSpan(1, 2, 40, 45, 450);
            Assert.AreEqual("01 d 02 h 40 m 45 s", span.ToAutoString());

            span = TimeSpan.Zero;
            Assert.AreEqual("0.00", span.ToAutoString());

            span = new TimeSpan(-10000);
            Assert.AreEqual("0.00", span.ToAutoString());
        }
    }
}
