using System;
using System.Collections;
using System.Collections.Generic;
using AM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class PairTest
    {
        [TestMethod]
        public void Pair_Construction1()
        {
            Pair<int, string> pair = new Pair<int, string>();
            Assert.AreEqual(0, pair.First);
            Assert.AreEqual(null, pair.Second);
        }

        [TestMethod]
        public void Pair_Construction2()
        {
            Pair<int, string> firstPair = new Pair<int, string>(1, "Hello");
            Pair<int, string> secondPair = new Pair<int, string>(firstPair);
            Assert.AreEqual(firstPair.First, secondPair.First);
            Assert.AreEqual(firstPair.Second, secondPair.Second);
        }

        [TestMethod]
        public void Pair_Construction3()
        {
            Pair<int, string> pair = new Pair<int, string>(1);
            Assert.AreEqual(1, pair.First);
            Assert.AreEqual(null, pair.Second);
        }

        [TestMethod]
        public void Pair_Construction4()
        {
            Pair<int, string> pair = new Pair<int, string>(1, "Hello");
            Assert.AreEqual(1, pair.First);
            Assert.AreEqual("Hello", pair.Second);
        }

        [TestMethod]
        public void Pair_Construction5()
        {
            Pair<int, string> pair = new Pair<int, string>(1, "Hello", true);
            Assert.AreEqual(1, pair.First);
            Assert.AreEqual("Hello", pair.Second);
            Assert.AreEqual(true, pair.ReadOnly);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Pair_Add()
        {
            IList pair = new Pair<int, string>();
            pair.Add("hello");
        }

        [TestMethod]
        public void Pair_Contains()
        {
            IList pair = new Pair<int, string>(1, "Hello");
            Assert.AreEqual(true, pair.Contains(1));
            Assert.AreEqual(true, pair.Contains("Hello"));
            Assert.AreEqual(false, pair.Contains("World"));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Pair_Clear()
        {
            IList pair = new Pair<int, string>();
            pair.Clear();
        }

        [TestMethod]
        public void Pair_IndexOf()
        {
            IList pair = new Pair<int, string>(1, "Hello");
            Assert.AreEqual(0, pair.IndexOf(1));
            Assert.AreEqual(1, pair.IndexOf("Hello"));
            Assert.AreEqual(-1, pair.IndexOf("World"));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Pair_Insert()
        {
            IList pair = new Pair<int, string>();
            pair.Insert(0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Pair_Remove()
        {
            IList pair = new Pair<int, string>();
            pair.Remove(1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Pair_RemoveAt()
        {
            IList pair = new Pair<int, string>();
            pair.RemoveAt(1);
        }

        [TestMethod]
        public void Pair_Indexer1()
        {
            IList pair = new Pair<int, string>(1, "Hello");
            Assert.AreEqual(1, pair[0]);
            Assert.AreEqual("Hello", pair[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Pair_Indexer2()
        {
            IList pair = new Pair<int, string>(1, "Hello");
            object o = pair[2];
        }

        [TestMethod]
        public void Pair_Indexer3()
        {
            Pair<int, string> pair = new Pair<int, string>();
            pair[0] = 1;
            Assert.AreEqual(1, pair.First);
            pair[1] = "Hello";
            Assert.AreEqual("Hello", pair.Second);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Pair_Indexer4()
        {
            Pair<int, string> pair = new Pair<int, string>();
            pair[0] = "Hello";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Pair_Indexer5()
        {
            IList pair = new Pair<int, string>(1, "Hello");
            pair[2] = null;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Pair_Indexer6()
        {
            IList pair = new Pair<int, string>(1, "Hello", true);
            pair[0] = 2;
        }

        [TestMethod]
        public void Pair_IsReadOnly()
        {
            IList pair = new Pair<int, string>();
            Assert.AreEqual(false, pair.IsReadOnly);

            pair = new Pair<int, string>(1, "Hello", true);
            Assert.AreEqual(true, pair.IsReadOnly);
        }

        [TestMethod]
        public void Pair_IsFixedSize()
        {
            IList pair = new Pair<int, string>();
            Assert.AreEqual(true, pair.IsFixedSize);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Pair_CopyTo()
        {
            IList pair = new Pair<int, string>();
            object[] array = new object[2];
            pair.CopyTo(array, 0);
        }

        [TestMethod]
        public void Pair_Count()
        {
            IList pair = new Pair<int, string>();
            Assert.AreEqual(2, pair.Count);
        }

        [TestMethod]
        public void Pair_SyncRoot()
        {
            IList pair = new Pair<int, string>();
            Assert.IsNotNull(pair.SyncRoot);
        }

        [TestMethod]
        public void Pair_IsSynchronized()
        {
            IList pair = new Pair<int, string>();
            Assert.AreEqual(false, pair.IsSynchronized);
        }

        [TestMethod]
        public void Pair_GetEnumerator()
        {
            IList pair = new Pair<int, string>(1, "Hello");
            object[] array = new object[2];
            IEnumerator enumerator = pair.GetEnumerator();
            enumerator.MoveNext();
            array[0] = enumerator.Current;
            enumerator.MoveNext();
            array[1] = enumerator.Current;
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual("Hello", array[1]);
        }

        [TestMethod]
        public void Pair_Clone()
        {
            Pair<int, string> first = new Pair<int, string>(1, "Hello");
            Pair<int, string> second = (Pair<int, string>)first.Clone();
            Assert.AreEqual(first.First, second.First);
            Assert.AreEqual(first.Second, second.Second);
        }

        [TestMethod]
        public void Pair_ReadOnly()
        {
            Pair<int, string> pair = new Pair<int, string>();
            Assert.AreEqual(false, pair.ReadOnly);

            pair = new Pair<int, string>(1, "Hello", true);
            Assert.AreEqual(true, pair.ReadOnly);
        }

        [TestMethod]
        public void Pair_AsReadOnly()
        {
            Pair<int, string> first = new Pair<int, string>(1, "Hello");
            Assert.AreEqual(false, first.ReadOnly);

            Pair<int, string> second = first.AsReadOnly();
            Assert.AreEqual(first.First, second.First);
            Assert.AreEqual(first.Second, second.Second);
            Assert.AreEqual(true, second.ReadOnly);
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void Pair_ThrowIfReadOnly()
        {
            Pair<int, string> pair = new Pair<int, string>(1, "Hello", true);
            pair.ThrowIfReadOnly();
        }

        [TestMethod]
        public void Pair_Equals1()
        {
            Pair<int, string> first = new Pair<int, string>(1, "Hello");
            Pair<int, string> second = new Pair<int, string>(1, "Hello");
            Assert.AreEqual(true, first.Equals(second));

            second = new Pair<int, string>(2, "World");
            Assert.AreEqual(false, first.Equals(second));
        }

        [TestMethod]
        public void Pair_Equals2()
        {
            Pair<int, string> first = new Pair<int, string>(1, "Hello");
            object second = new Pair<int, string>(1, "Hello");
            Assert.AreEqual(true, first.Equals(second));

            second = new Pair<int, string>(2, "World");
            Assert.AreEqual(false, first.Equals(second));

            Assert.AreEqual(false, first.Equals(null));
            Assert.AreEqual(true, first.Equals((object)first));
            Assert.AreEqual(false, first.Equals("Hello"));
        }

        [TestMethod]
        public void Pair_GetHashCode()
        {
            Pair<int, string> first = new Pair<int, string>(1, "Hello");
            Pair<int, string> second = new Pair<int, string>(2, "World");
            Assert.AreNotEqual
                (
                    first.GetHashCode(),
                    second.GetHashCode()
                );
        }

        [TestMethod]
        public void Pair_ToString()
        {
            Pair<int, string> pair = new Pair<int, string>(1, "Hello");
            Assert.AreEqual("1;Hello", pair.ToString());
        }

        [TestMethod]
        public void Pair_SetReadOnly()
        {
            Pair<int,string> pair = new Pair<int, string>(1, "Hello");
            Assert.AreEqual(false, pair.ReadOnly);
            pair.SetReadOnly();
            Assert.AreEqual(true, pair.ReadOnly);
        }
    }
}
