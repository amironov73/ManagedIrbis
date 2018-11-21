using System;
using System.Collections.Generic;
using AM.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class FrugalListTest
    {
        [TestMethod]
        public void FrugalList_Construction_1()
        {
            FrugalList<int> list = new FrugalList<int>();
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void FrugalList_Add_1()
        {
            FrugalList<int> list = new FrugalList<int>();
            Assert.AreEqual(0, list.Count);
            list.Add(1);
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public void FrugalList_Add_2()
        {
            FrugalList<int> list = new FrugalList<int>();
            Assert.AreEqual(0, list.Count);
            list.Add(1);
            list.Add(2);
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void FrugalList_Add_3()
        {
            FrugalList<int> list = new FrugalList<int>();
            Assert.AreEqual(0, list.Count);
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            Assert.AreEqual(5, list.Count);
        }

        [TestMethod]
        public void FrugalList_Clear_1()
        {
            FrugalList<int> list = new FrugalList<int>();
            Assert.AreEqual(0, list.Count);
            list.Add(1);
            list.Clear();
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void FrugalList_Contains_1()
        {
            FrugalList<int> list = new FrugalList<int>();
            Assert.IsFalse(list.Contains(1));
            list.Add(1);
            Assert.IsTrue(list.Contains(1));
            Assert.IsFalse(list.Contains(2));
        }

        [TestMethod]
        public void FrugalList_CopyTo_1()
        {
            FrugalList<int> list = new FrugalList<int>();
            int[] array = new int[3];
            list.CopyTo(array, 0);
            Assert.AreEqual(0, array[0]);
            Assert.AreEqual(0, array[1]);
            Assert.AreEqual(0, array[2]);
        }

        [TestMethod]
        public void FrugalList_CopyTo_2()
        {
            FrugalList<int> list = new FrugalList<int>();
            list.Add(1);
            int[] array = new int[3];
            list.CopyTo(array, 0);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(0, array[1]);
            Assert.AreEqual(0, array[2]);
        }

        [TestMethod]
        public void FrugalList_CopyTo_3()
        {
            FrugalList<int> list = new FrugalList<int>();
            list.Add(1);
            list.Add(2);
            int[] array = new int[3];
            list.CopyTo(array, 0);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(0, array[2]);
        }

        [TestMethod]
        public void FrugalList_CopyTo_4()
        {
            FrugalList<int> list = new FrugalList<int>();
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
        public void FrugalList_CopyTo_5()
        {
            FrugalList<int> list = new FrugalList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            int[] array = new int[4];
            list.CopyTo(array, 0);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
            Assert.AreEqual(4, array[3]);
        }

        [TestMethod]
        public void FrugalList_CopyTo_6()
        {
            FrugalList<int> list = new FrugalList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            int[] array = new int[5];
            list.CopyTo(array, 0);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
            Assert.AreEqual(4, array[3]);
            Assert.AreEqual(5, array[4]);
        }

        [TestMethod]
        public void FrugalList_Count_1()
        {
            FrugalList<int> list = new FrugalList<int>();
            Assert.AreEqual(0, list.Count);
            list.Add(1);
            Assert.AreEqual(1, list.Count);
            list.Add(2);
            Assert.AreEqual(2, list.Count);
            list.Add(3);
            Assert.AreEqual(3, list.Count);
        }

        [TestMethod]
        public void FrugalList_IsReadOnly_1()
        {
            FrugalList<int> list = new FrugalList<int>();
            Assert.IsFalse(list.IsReadOnly);
        }

        [TestMethod]
        public void FrugalList_IndexOf_1()
        {
            FrugalList<int> list = new FrugalList<int>();
            Assert.IsTrue(list.IndexOf(2) < 0);
            list.Add(1);
            Assert.IsTrue(list.IndexOf(2) < 0);
        }

        [TestMethod]
        public void FrugalList_IndexOf_2()
        {
            FrugalList<int> list = new FrugalList<int>();
            Assert.IsTrue(list.IndexOf(2) < 0);
            list.Add(1);
            list.Add(2);
            Assert.AreEqual(1, list.IndexOf(2));
            Assert.IsTrue(list.IndexOf(3) < 0);
        }

        [TestMethod]
        public void FrugalList_IndexOf_2a()
        {
            FrugalList<int> list = new FrugalList<int>();
            Assert.IsTrue(list.IndexOf(2) < 0);
            list.Add(2);
            list.Add(1);
            Assert.AreEqual(0, list.IndexOf(2));
        }

        [TestMethod]
        public void FrugalList_IndexOf_3()
        {
            FrugalList<int> list = new FrugalList<int>();
            Assert.IsTrue(list.IndexOf(2) < 0);
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Assert.AreEqual(1, list.IndexOf(2));
        }

        [TestMethod]
        public void FrugalList_IndexOf_4()
        {
            FrugalList<int> list = new FrugalList<int>();
            Assert.IsTrue(list.IndexOf(2) < 0);
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            Assert.AreEqual(1, list.IndexOf(2));
        }

        [TestMethod]
        public void FrugalList_Item_1()
        {
            FrugalList<int> list = new FrugalList<int>();
            list.Add(1);
            Assert.AreEqual(1, list[0]);
        }

        [TestMethod]
        public void FrugalList_Item_2()
        {
            FrugalList<int> list = new FrugalList<int>();
            list.Add(1);
            list.Add(2);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
        }

        [TestMethod]
        public void FrugalList_Item_3()
        {
            FrugalList<int> list = new FrugalList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        public void FrugalList_Item_4()
        {
            FrugalList<int> list = new FrugalList<int>();
            list.Add(0);
            list[0] = 1;
            Assert.AreEqual(1, list[0]);
        }

        [TestMethod]
        public void FrugalList_Item_5()
        {
            FrugalList<int> list = new FrugalList<int>();
            list.Add(0);
            list.Add(0);
            list[0] = 1;
            list[1] = 2;
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
        }

        [TestMethod]
        public void FrugalList_Item_6()
        {
            FrugalList<int> list = new FrugalList<int>();
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
        public void FrugalList_Item_7()
        {
            FrugalList<int> list = new FrugalList<int>();
            Assert.AreEqual(0, list[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void FrugalList_Item_8()
        {
            FrugalList<int> list = new FrugalList<int>();
            list[0] = 0;
        }

        [TestMethod]
        public void FrugalList_ToArray_1()
        {
            FrugalList<int> list = new FrugalList<int>();
            int[] array = list.ToArray();
            Assert.IsNotNull(array);
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        public void FrugalList_ToArray_2()
        {
            FrugalList<int> list = new FrugalList<int>();
            list.Add(1);
            int[] array = list.ToArray();
            Assert.IsNotNull(array);
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(1, array[0]);
        }

        [TestMethod]
        public void FrugalList_ToArray_3()
        {
            FrugalList<int> list = new FrugalList<int>();
            list.Add(1);
            list.Add(2);
            int[] array = list.ToArray();
            Assert.IsNotNull(array);
            Assert.AreEqual(2, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
        }

        [TestMethod]
        public void FrugalList_ToArray_4()
        {
            FrugalList<int> list = new FrugalList<int>();
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
        public void FrugalList_ToArray_5()
        {
            FrugalList<int> list = new FrugalList<int>();
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
        public void FrugalList_GetEnumerator_1()
        {
            FrugalList<int> list = new FrugalList<int>();
            IEnumerator<int> enumerator = list.GetEnumerator();
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();
        }

        [TestMethod]
        public void FrugalList_GetEnumerator_2()
        {
            FrugalList<int> list = new FrugalList<int>();
            list.Add(1);
            IEnumerator<int> enumerator = list.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(1, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();
        }

        [TestMethod]
        public void FrugalList_GetEnumerator_3()
        {
            FrugalList<int> list = new FrugalList<int>();
            list.Add(1);
            list.Add(2);
            IEnumerator<int> enumerator = list.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(1, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(2, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();
        }

        [TestMethod]
        public void FrugalList_GetEnumerator_4()
        {
            FrugalList<int> list = new FrugalList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            IEnumerator<int> enumerator = list.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(1, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(2, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(3, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();
        }

        [TestMethod]
        public void FrugalList_GetEnumerator_5()
        {
            FrugalList<int> list = new FrugalList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            IEnumerator<int> enumerator = list.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(1, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(2, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(3, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(4, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();
        }
    }
}
