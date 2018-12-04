using System;
using System.Collections.Generic;

using AM.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class LocalListTest
    {
        [TestMethod]
        public void LocalList_Construction_1()
        {
            LocalList<int> list = new LocalList<int>();
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void LocalList_Construction_2()
        {
            int capacity = 10;
            LocalList<int> list = new LocalList<int>(capacity);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void LocalList_Add_1()
        {
            LocalList<int> list = new LocalList<int>();
            Assert.AreEqual(0, list.Count);
            list.Add(1);
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public void LocalList_Add_2()
        {
            LocalList<int> list = new LocalList<int>();
            Assert.AreEqual(0, list.Count);
            list.Add(1);
            list.Add(2);
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void LocalList_Add_3()
        {
            LocalList<int> list = new LocalList<int>();
            Assert.AreEqual(0, list.Count);
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            Assert.AreEqual(5, list.Count);
        }

        [TestMethod]
        public void LocalList_AddRange_1()
        {
            LocalList<int> list = new LocalList<int>();
            Assert.AreEqual(0, list.Count);
            int[] range = {1, 2};
            list.AddRange(range);
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void LocalList_GetEnumerator_1()
        {
            LocalList<int> list = new LocalList<int>();
            LocalList<int>.Enumerator enumerator = list.GetEnumerator();
            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void LocalList_GetEnumerator_2()
        {
            LocalList<int> list = new LocalList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            LocalList<int>.Enumerator enumerator = list.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(1, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(2, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(3, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void LocalList_GetEnumerator_3()
        {
            LocalList<int> list = new LocalList<int>();
            int sum = 0;
            foreach (int i in list)
            {
                sum += i;
            }
            Assert.AreEqual(0, sum);
        }

        [TestMethod]
        public void LocalList_GetEnumerator_4()
        {
            LocalList<int> list = new LocalList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            int sum = 0;
            foreach (int i in list)
            {
                sum += i;
            }
            Assert.AreEqual(6, sum);
        }

        [TestMethod]
        public void LocalList_Clear_1()
        {
            LocalList<int> list = new LocalList<int>();
            list.Add(1);
            list.Clear();
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void LocalList_Contains_1()
        {
            LocalList<int> list = new LocalList<int>();
            Assert.IsFalse(list.Contains(1));
            list.Add(1);
            Assert.IsTrue(list.Contains(1));
            Assert.IsFalse(list.Contains(2));
        }

        [TestMethod]
        public void LocalList_CopyTo_1()
        {
            LocalList<int> list = new LocalList<int>();
            int[] array = new int[3];
            list.CopyTo(array, 0);
            Assert.AreEqual(0, array[0]);
            Assert.AreEqual(0, array[1]);
            Assert.AreEqual(0, array[2]);
        }

        [TestMethod]
        public void LocalList_CopyTo_2()
        {
            LocalList<int> list = new LocalList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            int[] array = new int[3];
            list.CopyTo(array, 0);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
        }

        [TestMethod]
        public void LocalList_Remove_1()
        {
            LocalList<int> list = new LocalList<int>();
            Assert.IsFalse(list.Remove(2));
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Assert.IsTrue(list.Remove(2));
            Assert.IsFalse(list.Remove(2));
            Assert.IsFalse(list.Contains(2));
        }

        [TestMethod]
        public void LocalList_Count_1()
        {
            LocalList<int> list = new LocalList<int>();
            Assert.AreEqual(0, list.Count);
            list.Add(1);
            Assert.AreEqual(1, list.Count);
            list.Add(2);
            Assert.AreEqual(2, list.Count);
            list.Add(3);
            Assert.AreEqual(3, list.Count);
        }

        [TestMethod]
        public void LocalList_IsReadOnly_1()
        {
            LocalList<int> list = new LocalList<int>();
            Assert.IsFalse(list.IsReadOnly);
        }

        [TestMethod]
        public void LocalList_IndexOf_1()
        {
            LocalList<int> list = new LocalList<int>();
            Assert.IsTrue(list.IndexOf(2) < 0);
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Assert.AreEqual(1, list.IndexOf(2));
        }

        [TestMethod]
        public void LocalList_Insert_1()
        {
            LocalList<int> list = new LocalList<int>();
            Assert.AreEqual(0, list.Count);
            list.Insert(0, 3);
            Assert.AreEqual(1, list.Count);
            list.Insert(0, 2);
            Assert.AreEqual(2, list.Count);
            list.Insert(0, 1);
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(2, list[1]);
        }

        [TestMethod]
        public void LocalList_RemoveAt_1()
        {
            LocalList<int> list = new LocalList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Assert.IsTrue(list.Contains(2));
            list.RemoveAt(1);
            Assert.IsFalse(list.Contains(2));
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void LocalList_Item_1()
        {
            LocalList<int> list = new LocalList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        public void LocalList_Item_2()
        {
            LocalList<int> list = new LocalList<int>();
            list.Add(0);
            list.Add(0);
            list.Add(0);
            list[0] = 1;
            list[1] = 2;
            list[2] = 3;
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void LocalList_Item_3()
        {
            LocalList<int> list = new LocalList<int>();
            Assert.AreEqual(0, list[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void LocalList_Item_4()
        {
            LocalList<int> list = new LocalList<int>();
            list[0] = 0;
        }

        [TestMethod]
        public void LocalList_ToArray_1()
        {
            LocalList<int> list = new LocalList<int>();
            int[] array = list.ToArray();
            Assert.IsNotNull(array);
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        public void LocalList_ToArray_2()
        {
            LocalList<int> list = new LocalList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            int[] array = list.ToArray();
            Assert.IsNotNull(array);
            Assert.AreEqual(3, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
        }

        [TestMethod]
        public void LocalList_ToArray_3()
        {
            LocalList<int> list = new LocalList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            int[] array = list.ToArray();
            Assert.IsNotNull(array);
            Assert.AreEqual(4, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
            Assert.AreEqual(4, array[3]);
        }

        [TestMethod]
        public void LocalList_ToList_1()
        {
            LocalList<int> source = new LocalList<int>();
            List<int> list = source.ToList();
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void LocalList_ToList_2()
        {
            LocalList<int> source = new LocalList<int>();
            source.Add(1);
            source.Add(2);
            source.Add(3);
            List<int> list = source.ToList();
            Assert.AreEqual(3, list.Count);
        }

        [TestMethod]
        public void LocalList_ToUniqueList_1()
        {
            LocalList<int> source = new LocalList<int>();
            source.Add(1);
            source.Add(2);
            source.Add(2);
            source.Add(3);
            List<int> list = source.ToUniqueList();
            Assert.AreEqual(3, list.Count);
        }
    }
}
