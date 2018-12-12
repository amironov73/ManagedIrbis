using System;

using UnsafeAM.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.UnsafeAM.Collections
{
    [TestClass]
    public class ChunkArrayTest
    {
        [TestMethod]
        public void ChunkArray_Construction_1()
        {
            ChunkArray<int> array = new ChunkArray<int>(0);
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        public void ChunkArray_Construction_2()
        {
            ChunkArray<int> array = new ChunkArray<int>(1);
            Assert.AreEqual(1, array.Length);
        }

        [TestMethod]
        public void ChunkArray_Construction_3()
        {
            ChunkArray<int> array = new ChunkArray<int>(2);
            Assert.AreEqual(2, array.Length);
        }

        [TestMethod]
        public void ChunkArray_Construction_4()
        {
            ChunkArray<int> array = new ChunkArray<int>(1025);
            Assert.AreEqual(1025, array.Length);
        }

        [TestMethod]
        public void ChunkArray_CopyTo_1()
        {
            ChunkArray<int> source = new ChunkArray<int>(0);
            int[] destination = new int[3];
            source.CopyTo(destination, 0);
            Assert.AreEqual(0, destination[0]);
            Assert.AreEqual(0, destination[1]);
            Assert.AreEqual(0, destination[2]);
        }

        [TestMethod]
        public void ChunkArray_CopyTo_2()
        {
            ChunkArray<int> source = new ChunkArray<int>(3);
            source[0] = 1;
            source[1] = 2;
            source[2] = 3;
            int[] destination = new int[3];
            source.CopyTo(destination, 0);
            Assert.AreEqual(1, destination[0]);
            Assert.AreEqual(2, destination[1]);
            Assert.AreEqual(3, destination[2]);
        }

        [TestMethod]
        public void ChunkArray_CopyTo_3()
        {
            ChunkArray<int> source = new ChunkArray<int>(1027);
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = i;
            }
            int[] destination = new int[source.Length];
            source.CopyTo(destination, 0);
            for (int i = 0; i < source.Length; i++)
            {
                Assert.AreEqual(i, destination[i]);
            }
        }

        [TestMethod]
        public void ChunkArray_IndexOf_1()
        {
            ChunkArray<int> array = new ChunkArray<int>(0);
            Assert.IsTrue(array.IndexOf(1) < 0);
            Assert.IsTrue(array.IndexOf(2) < 0);
        }

        [TestMethod]
        public void ChunkArray_IndexOf_2()
        {
            ChunkArray<int> array = new ChunkArray<int>(1);
            array[0] = 1;
            Assert.AreEqual(0, array.IndexOf(1));
            Assert.IsTrue(array.IndexOf(2) < 0);
        }

        [TestMethod]
        public void ChunkArray_IndexOf_3()
        {
            ChunkArray<int> array = new ChunkArray<int>(1027);
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i;
            }
            Assert.AreEqual(1, array.IndexOf(1));
            Assert.AreEqual(2, array.IndexOf(2));
            Assert.AreEqual(1026, array.IndexOf(1026));
            Assert.IsTrue(array.IndexOf(2027) < 0);
        }

        [TestMethod]
        public void ChunkArray_Item_1()
        {
            ChunkArray<int> array = new ChunkArray<int>(3);
            array[0] = 1;
            array[1] = 2;
            array[2] = 3;
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
        }

        [TestMethod]
        public void ChunkArray_Item_2()
        {
            ChunkArray<int> array = new ChunkArray<int>(1027);
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i;
            }
            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(i, array[i]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void ChunkArray_Item_3()
        {
            ChunkArray<int> array = new ChunkArray<int>(3);
            int value = array[4];
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void ChunkArray_Item_4()
        {
            ChunkArray<int> array = new ChunkArray<int>(3);
            array[4] = 3;
        }
    }
}
