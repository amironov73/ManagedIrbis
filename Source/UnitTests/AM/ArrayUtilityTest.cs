using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Collections;

// ReSharper disable ForCanBeConvertedToForeach

namespace UnitTests.AM
{
    [TestClass]
    public class ArrayUtilityTest
    {
        class MyClass : ICloneable
        {
            public int Value { get; set; }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }

        [TestMethod]
        public void ArrayUtility_ChangeType_1()
        {
            string[] source = { "1", "2", "3" };
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
        public void ArrayUtility_ChangeType_2()
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
        public void ArrayUtility_Compare_1()
        {
            int[] first = { 1, 2, 3 };
            int[] second = { 1, 3, 4 };
            Assert.IsTrue(ArrayUtility.Compare(first, second) < 0);

            first = new int[0];
            second = new int[0];
            Assert.IsTrue(ArrayUtility.Compare(first, second) == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ArrayUtility_Compare_2()
        {
            int[] first = { };
            int[] second = { 1, 3, 4 };
            Assert.IsTrue(ArrayUtility.Compare(first, second) < 0);
        }

        [TestMethod]
        public void ArrayUtility_Convert_1()
        {
            int[] source = { 1, 2, 3 };
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
        public void ArrayUtility_Create_1()
        {
            int[] array = ArrayUtility.Create(10, 235);
            Assert.AreEqual(10, array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(235, array[i]);
            }
        }

        [TestMethod]
        public void ArrayUtility_GetOccurrence_1()
        {
            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Assert.AreEqual(1, array.GetOccurrence(0));
            Assert.AreEqual(1, array.GetOccurrence(0, 0));
            Assert.AreEqual(0, array.GetOccurrence(10));
            Assert.AreEqual(1, array.GetOccurrence(10, 1));
        }

        [TestMethod]
        public void ArrayUtility_GetSpan_1()
        {
            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] actual = array.GetSpan(3, 4);
            int[] expected = { 4, 5, 6, 7 };
            CollectionAssert.AreEqual(expected, actual);

            actual = array.GetSpan(9, 5);
            expected = new[] { 10 };
            CollectionAssert.AreEqual(expected, actual);

            actual = array.GetSpan(3, 0);
            expected = new int[0];
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ArrayUtility_GetSpan_2()
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
        public void ArrayUtility_GetSpan_3()
        {
            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] actual = array.GetSpan(300);
            int[] expected = new int[0];
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ArrayUtility_GetSpan_4()
        {
            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] actual = array.GetSpan(300, 10);
            int[] expected = new int[0];
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ArrayUtility_IsNullOrEmpty_1()
        {
            Assert.IsTrue(((int[])null).IsNullOrEmpty());
            Assert.IsTrue(new int[0].IsNullOrEmpty());
            Assert.IsFalse(new int[1].IsNullOrEmpty());
        }

        [TestMethod]
        public void ArrayUtility_IsNullOrEmpty_2()
        {
            Assert.IsTrue(ArrayUtility.IsNullOrEmpty(null));
            Assert.IsTrue(ArrayUtility.IsNullOrEmpty(new int[0]));
            Assert.IsFalse(ArrayUtility.IsNullOrEmpty(new int[1]));

        }

        [TestMethod]
        public void ArrayUtility_Merge_1()
        {
            int[] array1 = { 1, 2, 3 };
            int[] array2 = { 5, 6, 7 };
            int[] result = ArrayUtility.Merge(array1, array2);
            Assert.AreEqual(6, result.Length);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(3, result[2]);
            Assert.AreEqual(5, result[3]);
            Assert.AreEqual(6, result[4]);
            Assert.AreEqual(7, result[5]);
        }

        [TestMethod]
        public void ArrayUtility_Merge_2()
        {
            int[] array1 = { 1, 2, 3 };
            int[] result = ArrayUtility.Merge(array1);
            Assert.AreEqual(array1.Length, result.Length);
            Assert.AreEqual(array1[0], result[0]);
            Assert.AreEqual(array1[1], result[1]);
            Assert.AreEqual(array1[2], result[2]);
        }

        [TestMethod]
        public void ArrayUtility_Merge_3()
        {
            int[] result = ArrayUtility.Merge<int>();
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArrayUtility_Merge_4()
        {
            ArrayUtility.Merge<int>(null, null);
        }

        [TestMethod]
        public void ArrayUtility_ToString_1()
        {
            int[] array = { 1, 2, 3 };
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

        [TestMethod]
        public void ArrayUtility_Clone_1()
        {
            MyClass[] source = new MyClass[3];
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = new MyClass { Value = i };
            }
            MyClass[] target = ArrayUtility.Clone(source);
            Assert.AreEqual(source.Length, target.Length);
            for (int i = 0; i < source.Length; i++)
            {
                Assert.AreEqual(source[i].Value, target[i].Value);
            }
        }

        [TestMethod]
        public void ArrayUtility_Coincide_1()
        {
            int[] first = { 1, 2, 3, 4, 5, 6, 7 };
            int[] second = { 3, 4, 5, 6, 7, 8, 9 };
            Assert.IsTrue(ArrayUtility.Coincide(first, 3, second, 1, 2));
            Assert.IsFalse(ArrayUtility.Coincide(first, 3, second, 2, 2));
        }

        [TestMethod]
        public void ArrayUtility_SplitArray_1()
        {
            int[] mainArray = {1, 2, 3, 4, 5, 6, 7};
            int[][] splitted = ArrayUtility.SplitArray(mainArray, 1);
            Assert.AreEqual(1, splitted.Length);
            Assert.AreEqual(mainArray.Length, splitted[0].Length);

            splitted = ArrayUtility.SplitArray(mainArray, 2);
            Assert.AreEqual(2, splitted.Length);
            Assert.AreEqual(4, splitted[0].Length);
            Assert.AreEqual(3, splitted[1].Length);

            splitted = ArrayUtility.SplitArray(mainArray, 3);
            Assert.AreEqual(3, splitted.Length);
            Assert.AreEqual(3, splitted[0].Length);
            Assert.AreEqual(3, splitted[1].Length);
            Assert.AreEqual(1, splitted[2].Length);

            splitted = ArrayUtility.SplitArray(mainArray, 4);
            Assert.AreEqual(4, splitted.Length);
            Assert.AreEqual(2, splitted[0].Length);
            Assert.AreEqual(2, splitted[1].Length);
            Assert.AreEqual(2, splitted[2].Length);
            Assert.AreEqual(1, splitted[3].Length);

            splitted = ArrayUtility.SplitArray(mainArray, 7);
            Assert.AreEqual(7, splitted.Length);
            Assert.AreEqual(1, splitted[0].Length);
            Assert.AreEqual(1, splitted[1].Length);
            Assert.AreEqual(1, splitted[2].Length);
            Assert.AreEqual(1, splitted[3].Length);
            Assert.AreEqual(1, splitted[4].Length);
            Assert.AreEqual(1, splitted[5].Length);
            Assert.AreEqual(1, splitted[6].Length);

            splitted = ArrayUtility.SplitArray(mainArray, 8);
            Assert.AreEqual(8, splitted.Length);
            Assert.AreEqual(1, splitted[0].Length);
            Assert.AreEqual(1, splitted[1].Length);
            Assert.AreEqual(1, splitted[2].Length);
            Assert.AreEqual(1, splitted[3].Length);
            Assert.AreEqual(1, splitted[4].Length);
            Assert.AreEqual(1, splitted[5].Length);
            Assert.AreEqual(1, splitted[6].Length);
            Assert.AreEqual(0, splitted[7].Length);
        }
    }
}
