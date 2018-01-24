using System;

using AM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM
{
    [TestClass]
    public class NumberRangeTest
    {
        private void _Check64(string line, params long[] array)
        {
            long[] parsed = NumberRange.ParseInt64(line);
            CollectionAssert.AreEqual(array, parsed);
        }

        private void _Check32(string line, params int[] array)
        {
            int[] parsed = NumberRange.ParseInt32(line);
            CollectionAssert.AreEqual(array, parsed);
        }

        [TestMethod]
        public void NumberRange_ParseInt32_1()
        {
            _Check32(" ");
            _Check32("0", 0);
            _Check32("1", 1);
            _Check32("1, 2", 1, 2);
            _Check32("1-5, 2", 1, 2, 3, 4, 5, 2);
            _Check32("1 - 3", 1, 2, 3);
            _Check32("3-1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NumberRange_ParseInt32_2()
        {
            _Check32("1-");
        }

        [TestMethod]
        public void NumberRange_ParseInt64_1()
        {
            _Check64(" ");
            _Check64("0", 0);
            _Check64("1", 1);
            _Check64("1, 2", 1, 2);
            _Check64("1-5, 2", 1, 2, 3, 4, 5, 2);
            _Check64("1 - 3", 1, 2, 3);
            _Check64("3-1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NumberRange_ParseInt64_2()
        {
            _Check64("1-");
        }
    }
}
