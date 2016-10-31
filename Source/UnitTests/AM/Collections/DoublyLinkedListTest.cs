using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class DoublyLinkedListTest
    {
        [TestMethod]
        public void DoublyLinkedList_Construction()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>();

            Assert.AreEqual(0, list.Count);
            Assert.AreEqual(null, list.FirstNode);
            Assert.AreEqual(null, list.LastNode);
        }

        [TestMethod]
        public void DoublyLinkedList_Add()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };

            Assert.AreEqual(1, list.FirstNode.Value);
            Assert.AreEqual(2, list.FirstNode.Next.Value);
            Assert.AreEqual(3, list.LastNode.Value);
            Assert.AreEqual(2, list.LastNode.Previous.Value);

            Assert.AreEqual(3, list.Count);

            Assert.AreEqual(list, list.FirstNode.List);
            Assert.AreEqual(list, list.LastNode.List);
        }

        [TestMethod]
        public void DoublyLinkedList_IndexOf()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };

            Assert.AreEqual(0, list.IndexOf(1));
            Assert.AreEqual(1, list.IndexOf(2));
            Assert.AreEqual(2, list.IndexOf(3));
            Assert.AreEqual(-1, list.IndexOf(4));
        }

        [TestMethod]
        public void DoublyLinkedList_Insert1()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };

            list.Insert(1, 4);
            Assert.AreEqual(4, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(4, list[1]);
            Assert.AreEqual(2, list[2]);
            Assert.AreEqual(3, list[3]);
        }

        [TestMethod]
        public void DoublyLinkedList_Insert2()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };

            list.Insert(0, 4);
            Assert.AreEqual(4, list.Count);
            Assert.AreEqual(4, list[0]);
            Assert.AreEqual(1, list[1]);
            Assert.AreEqual(2, list[2]);
            Assert.AreEqual(3, list[3]);
        }

        [TestMethod]
        public void DoublyLinkedList_Remove1()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };

            list.RemoveAt(1);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(3, list[1]);
        }

        [TestMethod]
        public void DoublyLinkedList_Remove2()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };

            list.RemoveAt(0);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(2, list[0]);
            Assert.AreEqual(3, list[1]);
        }

        [TestMethod]
        public void DoublyLinkedList_Remove3()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };

            list.RemoveAt(2);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
        }

        [TestMethod]
        public void DoublyLinkedList_Contains()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };

            Assert.AreEqual(true, list.Contains(1));
            Assert.AreEqual(true, list.Contains(2));
            Assert.AreEqual(true, list.Contains(3));
            Assert.AreEqual(false, list.Contains(4));
        }

        [TestMethod]
        public void DoublyLinkedList_CopyTo()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };

            int[] array = new int[3];
            list.CopyTo(array, 0);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
        }

        [TestMethod]
        public void DoublyLinkedList_Remove()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };

            Assert.AreEqual(true, list.Remove(2));
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(3, list[1]);
        }
    }
}
