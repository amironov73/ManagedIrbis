using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Globalization;

namespace UnitTests.AM.Globalization
{
    [TestClass]
    public class RussianStringComparerTest
    {
        private void _TestCompare1
            (
                int expected,
                string str1,
                string str2
            )
        {
            RussianStringComparer comparer
                = new RussianStringComparer(true, false);

            int actual = comparer.Compare(str1, str2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestRussianStringComparer1()
        {
            _TestCompare1(0, "ежик", "ёжик");
        }

        private void _TestCompare2
            (
                int expected,
                string str1,
                string str2
            )
        {
            RussianStringComparer comparer
                = new RussianStringComparer(true, true);

            int actual = comparer.Compare(str1, str2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestRussianStringComparer2()
        {
            _TestCompare2(0, "ежик", "ЁЖИК");
        }
    }
}
