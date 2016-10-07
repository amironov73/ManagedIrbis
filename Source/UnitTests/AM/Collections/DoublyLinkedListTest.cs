using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class DoublyLinkedListTest
    {
        [TestMethod]
        public void TestDoublyLinkedListConstruction()
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
    }
}
