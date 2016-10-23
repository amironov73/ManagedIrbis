using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Collections;

namespace UnitTests.AM
{
    [TestClass]
    public class ArrayUtilityTest
    {
        [TestMethod]
        public void ArrayUtility_ChangeType1()
        {
            string[] source = {"1", "2", "3"};
            object[] target = ArrayUtility.ChangeType<string, object>(source);
            Assert.AreEqual(source.Length, target.Length);
            for (int i = 0; i < source.Length; i++)
            {
                Assert.IsTrue
                    (
                        ReferenceEquals(source[i], target[i])
                    );
            }
        }

        [TestMethod]
        public void ArrayUtility_ChangeType2()
        {
            string[] source = { "1", "2", "3" };
            object[] target = ArrayUtility.ChangeType<object>(source);
            Assert.AreEqual(source.Length, target.Length);
            for (int i = 0; i < source.Length; i++)
            {
                Assert.IsTrue
                    (
                        ReferenceEquals(source[i], target[i])
                    );
            }
        }

        [TestMethod]
        public void ArrayUtility_Compare()
        {
            int[] first = {1, 2, 3};
            int[] second = {1, 3, 4};
            Assert.IsTrue(ArrayUtility.Compare(first, second) < 0);

            first = new int[0];
            second = new int[0];
            Assert.IsTrue(ArrayUtility.Compare(first, second) == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ArrayUtility_Compare_Exception()
        {
            int[] first = {};
            int[] second = { 1, 3, 4 };
            Assert.IsTrue(ArrayUtility.Compare(first, second) < 0);
        }

        [TestMethod]
        public void ArrayUtility_Convert()
        {
            int[] source = {1, 2, 3};
            short[] target = ArrayUtility.Convert<int, short>(source);
            Assert.AreEqual(source.Length, target.Length);
            for (int i = 0; i < source.Length; i++)
            {
                Assert.IsTrue
                    (
                        (short)source[i] == target[i]
                    );
            }
        }

        [TestMethod]
        public void ArrayUtility_Create()
        {
            int[] array = ArrayUtility.Create(10, 235);
            Assert.AreEqual(10, array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(235, array[i]);
            }
        }

        [TestMethod]
        public void ArrayUtility_GetSpan1()
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
        public void ArrayUtility_GetSpan2()
        {
            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] actual = array.GetSpan(3);
            int[] expected = { 4, 5, 6, 7, 8, 9, 10 };
            CollectionAssert.AreEqual(expected, actual);

            actual = array.GetSpan(9);
            expected = new[] { 10 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ArrayUtility_GetSpan3()
        {
            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] actual = array.GetSpan(300);
            int[] expected = new int[0];
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ArrayUtility_IsNullOrEmpty()
        {
            Assert.IsTrue(((int[])null).IsNullOrEmpty());
            Assert.IsTrue(new int[0].IsNullOrEmpty());
            Assert.IsFalse(new int[1].IsNullOrEmpty());
        }

        [TestMethod]
        public void ArrayUtility_ToString()
        {
            int[] array = {1, 2, 3};
            string[] lines = ArrayUtility.ToString(array);
            Assert.AreEqual(array.Length, lines.Length);
            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual
                    (
                        array[i].ToString(),
                        lines[i]
                    );
            }
        }
    }
}
