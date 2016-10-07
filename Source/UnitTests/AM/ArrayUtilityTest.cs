using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class ArrayUtilityTest
    {
        [TestMethod]
        public void TestArraySpan1()
        {
            int[] array = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            int[] actual = array.GetSpan(3, 4);
            int[] expected = {4, 5, 6, 7};
            CollectionAssert.AreEqual(expected, actual);

            actual = array.GetSpan(9, 5);
            expected = new[] {10};
            CollectionAssert.AreEqual(expected, actual);

            actual = array.GetSpan(3, 0);
            expected= new int[0];
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestArraySpan2()
        {
            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] actual = array.GetSpan(3);
            int[] expected = { 4, 5, 6, 7, 8, 9, 10 };
            CollectionAssert.AreEqual(expected, actual);

            actual = array.GetSpan(9);
            expected = new[] { 10 };
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
