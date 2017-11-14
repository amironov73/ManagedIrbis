using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

// ReSharper disable ConvertToLocalFunction

namespace UnitTests.AM
{
    [TestClass]
    public class FuncUtilityTest
    {
        [TestMethod]
        public void FuncUtility_Memoize_1()
        {
            int counter = 0;

            Func<int, int> original = arg =>
            {
                counter++;

                return arg * 2;
            };
            Func<int, int> memoized = original.Memoize();

            int expected = original(1);
            int actual = memoized(1);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(2, counter);

            actual = memoized(1);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(2, counter);
        }

        [TestMethod]
        public void FuncUtility_RetryOnFail_1()
        {
            int counter = 0;

            Func<int> function = () =>
            {
                counter++;
                if (counter < 3)
                {
                    throw new Exception();
                }

                return counter;
            };

            int actual = FuncUtility.RetryOnFault(function, 3);
            Assert.AreEqual(3, actual);
            Assert.AreEqual(3, counter);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FuncUtility_RetryOnFail_2()
        {
            int counter = 0;

            Func<int> function = () =>
            {
                counter++;
                if (counter < 3)
                {
                    throw new Exception();
                }

                return counter;
            };

            int actual = FuncUtility.RetryOnFault(function, 2);
            Assert.AreEqual(3, actual);
            Assert.AreEqual(3, counter);
        }
    }
}
