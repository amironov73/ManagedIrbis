using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class SetTest
    {
        [TestMethod]
        public void Set_Construction1()
        {
            Set<int> set = new Set<int>();
            Assert.AreEqual(0, set.Count);
            Assert.AreEqual(true, set.IsEmpty);
        }

        [TestMethod]
        public void Set_Construction2()
        {
            Set<int> set = new Set<int>(100);
            Assert.AreEqual(0, set.Count);
            Assert.AreEqual(true, set.IsEmpty);
        }

        [TestMethod]
        public void Set_Construction3()
        {
            Set<int> first = new Set<int> { 1, 2 };
            Set<int> second = new Set<int>(first);
            Assert.AreEqual(first.Count, second.Count);
            Assert.AreEqual(first.IsEmpty, second.IsEmpty);
        }

        [TestMethod]
        public void Set_Construction4()
        {
            int[] array = { 1, 2, 3 };
            Set<int> set = new Set<int>(array);
            Assert.AreEqual(array.Length, set.Count);
        }

        [TestMethod]
        public void Set_Add()
        {
            Set<int> set = new Set<int>();
            Assert.AreEqual(0, set.Count);
            set.Add(1);
            Assert.AreEqual(1, set.Count);
            set.Add(1);
            Assert.AreEqual(1, set.Count);
        }

        [TestMethod]
        public void Set_AddRange()
        {
            Set<int> set = new Set<int>();
            Assert.AreEqual(0, set.Count);
            set.AddRange(new[] { 1, 2, 3 });
            Assert.AreEqual(3, set.Count);
            set.AddRange(new[] { 3, 4, 5 });
            Assert.AreEqual(5, set.Count);
        }

        [TestMethod]
        public void Set_ConvertAll()
        {
            Set<int> first = new Set<int>(new[] { 1, 2, 3 });
            Set<string> second = first.ConvertAll
                (
                    item => item.ToString()
                );
            Assert.AreEqual(first.Count, second.Count);
        }

        [TestMethod]
        public void Set_TrueForAll()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            Assert.AreEqual
                (
                    true,
                    set.TrueForAll
                    (
                        item => item > 0
                    )
                );
            Assert.AreEqual
                (
                    false,
                    set.TrueForAll
                    (
                        item => item > 2
                    )
                );
        }

        [TestMethod]
        public void Set_FindAdd()
        {
            Set<int> first = new Set<int>(new[] { 1, 2, 3 });
            Set<int> second = first.FindAll
                (
                    item => item > 1
                );
            Assert.AreEqual(2, second.Count);

            second = first.FindAll
                (
                    item => item > 3
                );
            Assert.AreEqual(true, second.IsEmpty);
        }

        [TestMethod]
        public void Set_ForEach()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            int counter = 0;
            set.ForEach
                (
                    item => counter = counter + item
                );
            Assert.AreEqual(6, counter);
        }

        [TestMethod]
        public void Set_Clear()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            Assert.AreEqual(3, set.Count);
            Assert.AreEqual(false, set.IsEmpty);

            set.Clear();
            Assert.AreEqual(0, set.Count);
            Assert.AreEqual(true, set.IsEmpty);
        }

        [TestMethod]
        public void Set_Contains()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            Assert.AreEqual(true, set.Contains(1));
            Assert.AreEqual(true, set.Contains(2));
            Assert.AreEqual(true, set.Contains(3));
            Assert.AreEqual(false, set.Contains(4));
        }

        [TestMethod]
        public void Set_CopyTo()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            int[] array = new int[6];
            set.CopyTo(array, 0);
            Assert.AreEqual(true, array.Contains(1));
            Assert.AreEqual(true, array.Contains(2));
            Assert.AreEqual(true, array.Contains(3));
            Assert.AreEqual(false, array.Contains(4));
        }

        [TestMethod]
        public void Set_Remove1()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            Assert.AreEqual(3, set.Count);
            Assert.AreEqual(true, set.Contains(2));

            set.Remove(2);
            Assert.AreEqual(2, set.Count);
            Assert.AreEqual(true, set.Contains(1));
            Assert.AreEqual(false, set.Contains(2));
            Assert.AreEqual(true, set.Contains(3));
        }

        [TestMethod]
        public void Set_Remove2()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            Assert.AreEqual(3, set.Count);
            Assert.AreEqual(true, set.Contains(2));
            Assert.AreEqual(true, set.Contains(3));

            set.Remove(2, 3);
            Assert.AreEqual(1, set.Count);
            Assert.AreEqual(true, set.Contains(1));
            Assert.AreEqual(false, set.Contains(2));
            Assert.AreEqual(false, set.Contains(3));
        }

        [TestMethod]
        public void Set_GetEnumerator()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            int[] items = set.ToArray();
            Assert.AreEqual(set.Count, items.Length);
            Assert.AreEqual(true, items.Contains(1));
            Assert.AreEqual(true, items.Contains(2));
            Assert.AreEqual(true, items.Contains(3));
        }

        [TestMethod]
        public void Set_Clone()
        {
            Set<int> first = new Set<int>(new[] { 1, 2, 3 });
            Set<int> second = (Set<int>) first.Clone();
            Assert.AreEqual(first.Count, second.Count);
        }

        [TestMethod]
        public void Set_Empty()
        {
            Set<int> set = Set<int>.Empty;
            Assert.AreEqual(0, set.Count);
            Assert.AreEqual(true, set.IsEmpty);
        }
    }
}
