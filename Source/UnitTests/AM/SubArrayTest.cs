using System;
using System.Collections.Generic;
using AM;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM
{
    [TestClass]
    public class SubArrayTest
    {
        [NotNull]
        private int[] _GetArray()
        {
            return new[] {1, 2, 3, 4, 5, 6, 7};
        }

        [TestMethod]
        public void SubArray_Construction_1()
        {
            int[] array = _GetArray();
            SubArray<int> subArray = new SubArray<int>(array, 2, 3);
            Assert.AreSame(array, subArray.Array);
            Assert.AreEqual(2, subArray.Offset);
            Assert.AreEqual(3, subArray.Length);

            subArray = new SubArray<int>(array, 5, 3);
            Assert.AreEqual(2, subArray.Length);

            subArray = new SubArray<int>(array, 7, 3);
            Assert.AreEqual(0, subArray.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SubArray_Construction_1a()
        {
            int[] array = _GetArray();
            SubArray<int> subArray = new SubArray<int>(array, -2, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SubArray_Construction_1b()
        {
            int[] array = _GetArray();
            SubArray<int> subArray = new SubArray<int>(array, 20, 3);
        }

        [TestMethod]
        public void SubArray_Construction_2()
        {
            int[] array = _GetArray();
            SubArray<int> subArray = new SubArray<int>(array, 2);
            Assert.AreSame(array, subArray.Array);
            Assert.AreEqual(2, subArray.Offset);
            Assert.AreEqual(5, subArray.Length);

            subArray = new SubArray<int>(array, 5);
            Assert.AreEqual(2, subArray.Length);

            subArray = new SubArray<int>(array, 7);
            Assert.AreEqual(0, subArray.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SubArray_Construction_2a()
        {
            int[] array = _GetArray();
            SubArray<int> subArray = new SubArray<int>(array, -2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SubArray_Construction_2b()
        {
            int[] array = _GetArray();
            SubArray<int> subArray = new SubArray<int>(array, 20);
        }

        [TestMethod]
        public void SubArray_Indexer_1()
        {
            int[] array = _GetArray();
            SubArray<int> subArray = new SubArray<int>(array, 3);

            for (int i = 0; i < subArray.Length; i++)
            {
                Assert.AreEqual(array[subArray.Offset + i], subArray[i]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void SubArray_Indexer_1a()
        {
            int[] array = _GetArray();
            SubArray<int> subArray = new SubArray<int>(array, 3);
            int value = subArray[-100];
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void SubArray_Indexer_1b()
        {
            int[] array = _GetArray();
            SubArray<int> subArray = new SubArray<int>(array, 3);
            int value = subArray[100];
        }

        [TestMethod]
        public void SubArray_Indexer_2()
        {
            int[] array = _GetArray();
            SubArray<int> subArray = new SubArray<int>(array, 3);

            for (int i = 0; i < subArray.Length; i++)
            {
                int x = i * i;
                subArray[i] = x;
                Assert.AreEqual(x, subArray[i]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void SubArray_Indexer_2a()
        {
            int[] array = _GetArray();
            SubArray<int> subArray = new SubArray<int>(array, 3);
            subArray[-100] = -100;
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void SubArray_Indexer_2b()
        {
            int[] array = _GetArray();
            SubArray<int> subArray = new SubArray<int>(array, 3);
            subArray[100] = 100;
        }

        [TestMethod]
        public void SubArray_ToArray_1()
        {
            int[] source = _GetArray();
            SubArray<int> subArray = new SubArray<int>(source, 3);
            int[] target = subArray.ToArray();
            Assert.AreEqual(subArray.Length, target.Length);
            for (int i = 0; i < subArray.Length; i++)
            {
                Assert.AreEqual(subArray[i], target[i]);
            }
        }

        [TestMethod]
        public void SubArray_Contains_1()
        {
            int[] source = _GetArray();
            SubArray<int> subArray = new SubArray<int>(source, 2, 2);
            Assert.IsTrue(subArray.Contains(3));
            Assert.IsFalse(subArray.Contains(33));
            Assert.IsFalse(subArray.Contains(1));
            Assert.IsFalse(subArray.Contains(7));
        }

        [TestMethod]
        public void SubArray_CopyTo_1()
        {
            int[] source = _GetArray();
            SubArray<int> subArray = new SubArray<int>(source, 2, 2);
            int[] target = new int[2];
            subArray.CopyTo(target, 0);
            Assert.AreEqual(3, target[0]);
            Assert.AreEqual(4, target[1]);
        }

        [TestMethod]
        public void SubArray_IndexOf_1()
        {
            int[] source = _GetArray();
            SubArray<int> subArray = new SubArray<int>(source, 2, 2);
            Assert.AreEqual(0, subArray.IndexOf(3));
            Assert.AreEqual(1, subArray.IndexOf(4));
            Assert.AreEqual(-1, subArray.IndexOf(1));
            Assert.AreEqual(-1, subArray.IndexOf(0));
            Assert.AreEqual(-1, subArray.IndexOf(9));
        }

        [TestMethod]
        public void SubArray_GetEnumerator_1()
        {
            int[] source = _GetArray();
            SubArray<int> subArray = new SubArray<int>(source, 2, 2);
            IEnumerator<int> enumerator = subArray.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(3, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(4, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();
        }

        [TestMethod]
        public void SubArray_Equals_1()
        {
            int[] source = _GetArray();
            SubArray<int> sub1 = new SubArray<int>(source, 2, 2);
            SubArray<int> sub2 = new SubArray<int>(source, 2, 2);
            Assert.IsTrue(sub1.Equals(sub2));

            sub2 = new SubArray<int>(source, 2, 3);
            Assert.IsFalse(sub1.Equals(sub2));
        }

        [TestMethod]
        public void SubArray_Equals_2()
        {
            int[] source = _GetArray();
            SubArray<int> sub1 = new SubArray<int>(source, 2, 2);
            SubArray<int> sub2 = new SubArray<int>(source, 2, 2);
            Assert.IsTrue(sub1.Equals((object)sub2));

            sub2 = new SubArray<int>(source, 2, 3);
            Assert.IsFalse(sub1.Equals((object)sub2));

            // ReSharper disable once RedundantCast
            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.IsFalse(sub1.Equals((object)"Hello"));
        }

        [TestMethod]
        public void SubArray_ToString_1()
        {
            int[] source = _GetArray();
            SubArray<int> subArray = new SubArray<int>(source, 2, 2);
            Assert.AreEqual("3, 4", subArray.ToString());

            subArray = new SubArray<int>(source, 2, 1);
            Assert.AreEqual("3", subArray.ToString());

            subArray = new SubArray<int>(source, 2, 0);
            Assert.AreEqual("", subArray.ToString());
        }
    }
}
