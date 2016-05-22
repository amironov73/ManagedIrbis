using System;
using AM.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Ranges;

namespace UnitTests.AM.Text.Ranges
{
    [TestClass]
    public class NumberRangeTest
    {
        [TestMethod]
        public void TestNumberRangeConstructor()
        {
            NumberRange range = new NumberRange("10", "15");
            Assert.AreEqual("10-15", range.ToString());
            Assert.AreEqual(true, range.Contains("12"));
            Assert.AreEqual(false, range.Contains("18"));
        }

        [TestMethod]
        public void TestNumberRangeParse()
        {
            NumberRange range = NumberRange.Parse("10-15");
            Assert.AreEqual(range.Start, new NumberText("10"));
            Assert.AreEqual(range.Stop, new NumberText("15"));
            Assert.AreEqual(true, range.Contains("12"));
            Assert.AreEqual(false, range.Contains("18"));
        }
    }
}
