using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class FuncUtilityTest
    {
        [TestMethod]
        public void TestMemoize()
        {
            int counter = 0;

            Func<int, int> original = arg =>
            {
                counter++;
                return arg * 2;
            };
            Func<int, int> memoized = original.Memoize();

            int expected = memoized(1);
            int actual = memoized(1);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(1, counter);

            actual = memoized(1);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(1, counter);
        }

    }
}
