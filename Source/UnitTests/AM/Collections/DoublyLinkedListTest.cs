using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class DoublyLinkedListTest
    {
        [TestMethod]
        public void DoublyLinkedList_Construction_1()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>();

            Assert.AreEqual(0, list.Count);
            Assert.AreEqual(null, list.FirstNode);
            Assert.AreEqual(null, list.LastNode);
            Assert.IsFalse(list.IsReadOnly);
        }

        [TestMethod]
        public void DoublyLinkedList_Add_1()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };

            // ReSharper disable PossibleNullReferenceException
            Assert.AreEqual(1, list.FirstNode.Value);
            Assert.AreEqual(2, list.FirstNode.Next.Value);
            Assert.AreEqual(3, list.LastNode.Value);
            Assert.AreEqual(2, list.LastNode.Previous.Value);
            // ReSharper restore PossibleNullReferenceException

            Assert.AreEqual(3, list.Count);
        }

        [TestMethod]
        public void DoublyLinkedList_IndexOf_1()
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
        public void DoublyLinkedList_Insert_1()
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
        public void DoublyLinkedList_Insert_2()
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
        public void DoublyLinkedList_Remove_1()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };

            Assert.IsTrue(list.Remove(2));
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(3, list[1]);
        }

        [TestMethod]
        public void DoublyLinkedList_Remove_2()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };

            Assert.IsFalse(list.Remove(22));
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        public void DoublyLinkedList_RemoveAt_1()
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
        public void DoublyLinkedList_RemoveAt_2()
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
        public void DoublyLinkedList_RemoveAt_3()
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
        public void DoublyLinkedList_RemoveAt_4()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };

            list.RemoveAt(100);
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        public void DoublyLinkedList_Contains_1()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };

            Assert.IsTrue(list.Contains(1));
            Assert.IsTrue(list.Contains(2));
            Assert.IsTrue(list.Contains(3));
            Assert.IsFalse(list.Contains(4));
        }

        [TestMethod]
        public void DoublyLinkedList_CopyTo_1()
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
        public void DoublyLinkedList_Node_1()
        {
            DoublyLinkedList<int>.Node node
                = new DoublyLinkedList<int>.Node { Value = 10 };
            Assert.IsNull(node.Next);
            Assert.IsNull(node.Previous);
            Assert.AreEqual(10, node.Value);
            Assert.AreEqual("10", node.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void DoublyLinkedList_Indexer_1()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };
            Assert.AreEqual(-1, list[-1]);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void DoublyLinkedList_Indexer_2()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };
            list[-1] = -1;
            Assert.AreEqual(-1, list[-1]);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void DoublyLinkedList_Indexer_3()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };
            list[100] = 100;
            Assert.AreEqual(100, list[100]);
        }

        [TestMethod]
        public void DoublyLinkedList_Indexer_4()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };
            list[0] = -1;
            Assert.AreEqual(-1, list[0]);
        }

        [TestMethod]
        public void DoublyLinkedList_Clear_1()
        {
            DoublyLinkedList<int> list = new DoublyLinkedList<int>
            {
                1, 2, 3
            };
            Assert.AreEqual(3, list.Count);

            list.Clear();
            Assert.AreEqual(0, list.Count);

            list.Clear();
            Assert.AreEqual(0, list.Count);
        }
    }
}
