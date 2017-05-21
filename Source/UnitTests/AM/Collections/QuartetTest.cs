using System;
using System.Collections;
using AM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class QuartetTest
    {
        [TestMethod]
        public void Quartet_Construction_1()
        {
            Quartet<int, string, bool, double> quartet
                = new Quartet<int, string, bool, double>();
            Assert.AreEqual(0, quartet.First);
            Assert.AreEqual(null, quartet.Second);
            Assert.AreEqual(false, quartet.Third);
            Assert.AreEqual(0.0, quartet.Fourth);
        }

        [TestMethod]
        public void Quartet_Construction_2()
        {
            Quartet<int, string, bool, double> firstQuartet
                = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14);
            Quartet<int, string, bool, double> secondQuartet
                = new Quartet<int, string, bool, double>(firstQuartet);
            Assert.AreEqual(firstQuartet.First, secondQuartet.First);
            Assert.AreEqual(firstQuartet.Second, secondQuartet.Second);
            Assert.AreEqual(firstQuartet.Third, secondQuartet.Third);
            Assert.AreEqual(firstQuartet.Fourth, secondQuartet.Fourth);
        }

        [TestMethod]
        public void Quartet_Construction_3()
        {
            Quartet<int, string, bool, double> quartet
                = new Quartet<int, string, bool, double>(1);
            Assert.AreEqual(1, quartet.First);
            Assert.AreEqual(null, quartet.Second);
            Assert.AreEqual(false, quartet.Third);
            Assert.AreEqual(0.0, quartet.Fourth);
        }

        [TestMethod]
        public void Quartet_Construction_4()
        {
            Quartet<int, string, bool, double> quartet
                = new Quartet<int, string, bool, double>(1, "Hello");
            Assert.AreEqual(1, quartet.First);
            Assert.AreEqual("Hello", quartet.Second);
            Assert.AreEqual(false, quartet.Third);
            Assert.AreEqual(0.0, quartet.Fourth);
        }

        [TestMethod]
        public void Quartet_Construction_5()
        {
            Quartet<int, string, bool, double> quartet
                = new Quartet<int, string, bool, double>(1, "Hello", true);
            Assert.AreEqual(1, quartet.First);
            Assert.AreEqual("Hello", quartet.Second);
            Assert.AreEqual(true, quartet.Third);
            Assert.AreEqual(0.0, quartet.Fourth);
        }

        [TestMethod]
        public void Quartet_Construction_6()
        {
            Quartet<int, string, bool, double> quartet
                = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14);
            Assert.AreEqual(1, quartet.First);
            Assert.AreEqual("Hello", quartet.Second);
            Assert.AreEqual(true, quartet.Third);
            Assert.AreEqual(3.14, quartet.Fourth);
        }

        [TestMethod]
        public void Quartet_Construction_7()
        {
            Quartet<int, string, bool, double> quartet
                = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14, true);
            Assert.AreEqual(1, quartet.First);
            Assert.AreEqual("Hello", quartet.Second);
            Assert.AreEqual(true, quartet.Third);
            Assert.AreEqual(3.14, quartet.Fourth);
            Assert.AreEqual(true, quartet.ReadOnly);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Quartet_Add_1()
        {
            IList quartet = new Quartet<int, string, bool, double>();
            quartet.Add("hello");
        }

        [TestMethod]
        public void Quartet_Contains_1()
        {
            IList quartet = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14);
            Assert.AreEqual(true, quartet.Contains(1));
            Assert.AreEqual(true, quartet.Contains("Hello"));
            Assert.AreEqual(true, quartet.Contains(true));
            Assert.AreEqual(true, quartet.Contains(3.14));
            Assert.AreEqual(false, quartet.Contains("World"));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Quartet_Clear_1()
        {
            IList quartet = new Quartet<int, string, bool, double>();
            quartet.Clear();
        }

        [TestMethod]
        public void Quartet_IndexOf_1()
        {
            IList quartet = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14);
            Assert.AreEqual(0, quartet.IndexOf(1));
            Assert.AreEqual(1, quartet.IndexOf("Hello"));
            Assert.AreEqual(2, quartet.IndexOf(true));
            Assert.AreEqual(3, quartet.IndexOf(3.14));
            Assert.AreEqual(-1, quartet.IndexOf("World"));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Quartet_Insert_1()
        {
            IList quartet = new Quartet<int, string, bool, double>();
            quartet.Insert(0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Quartet_Remove_1()
        {
            IList quartet = new Quartet<int, string, bool, double>();
            quartet.Remove(1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Quartet_RemoveAt_1()
        {
            IList quartet = new Quartet<int, string, bool, double>();
            quartet.RemoveAt(1);
        }

        [TestMethod]
        public void Quartet_Indexer_1()
        {
            IList quartet = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14);
            Assert.AreEqual(1, quartet[0]);
            Assert.AreEqual("Hello", quartet[1]);
            Assert.AreEqual(true, quartet[2]);
            Assert.AreEqual(3.14, quartet[3]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Quartet_Indexer_2()
        {
            IList quartet = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14);
            object o = quartet[4];
        }

        [TestMethod]
        public void Quartet_Indexer_3()
        {
            Quartet<int, string, bool, double> quartet
                = new Quartet<int, string, bool, double>();
            quartet[0] = 1;
            Assert.AreEqual(1, quartet.First);
            quartet[1] = "Hello";
            Assert.AreEqual("Hello", quartet.Second);
            quartet[2] = true;
            Assert.AreEqual(true, quartet.Third);
            quartet[3] = 3.14;
            Assert.AreEqual(3.14, quartet.Fourth);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Quartet_Indexer_4()
        {
            Quartet<int, string, bool, double> quartet
                = new Quartet<int, string, bool, double>();
            quartet[0] = "Hello";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Quartet_Indexer_5()
        {
            IList quartet = new Quartet<int, string, bool, double>(1, "Hello");
            quartet[4] = null;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Quartet_Indexer_6()
        {
            IList quartet = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14, true);
            quartet[0] = 2;
        }

        [TestMethod]
        public void Quartet_IsReadOnly_1()
        {
            IList quartet = new Quartet<int, string, bool, double>();
            Assert.AreEqual(false, quartet.IsReadOnly);

            quartet = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14, true);
            Assert.AreEqual(true, quartet.IsReadOnly);
        }

        [TestMethod]
        public void Quartet_IsFixedSize_1()
        {
            IList quartet = new Quartet<int, string, bool, double>();
            Assert.AreEqual(true, quartet.IsFixedSize);
        }

        [TestMethod]
        public void Quartet_CopyTo_1()
        {
            IList quartet = new Quartet<int, string, bool, double>();
            object[] array = new object[4];
            quartet.CopyTo(array, 0);
        }

        [TestMethod]
        public void Quartet_Count_1()
        {
            IList quartet = new Quartet<int, string, bool, double>();
            Assert.AreEqual(4, quartet.Count);
        }

        [TestMethod]
        public void Quartet_SyncRoot_1()
        {
            IList quartet = new Quartet<int, string, bool, double>();
            Assert.IsNotNull(quartet.SyncRoot);
        }

        [TestMethod]
        public void Quartet_IsSynchronized()
        {
            IList quartet = new Quartet<int, string, bool, double>();
            Assert.AreEqual(false, quartet.IsSynchronized);
        }

        [TestMethod]
        public void Quartet_GetEnumerator_1()
        {
            IList quartet = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14);
            object[] array = new object[4];
            IEnumerator enumerator = quartet.GetEnumerator();
            enumerator.MoveNext();
            array[0] = enumerator.Current;
            enumerator.MoveNext();
            array[1] = enumerator.Current;
            enumerator.MoveNext();
            array[2] = enumerator.Current;
            enumerator.MoveNext();
            array[3] = enumerator.Current;
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual("Hello", array[1]);
            Assert.AreEqual(true, array[2]);
            Assert.AreEqual(3.14, array[3]);
        }

        [TestMethod]
        public void Quartet_Clone_1()
        {
            Quartet<int, string, bool, double> first
                = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14);
            Quartet<int, string, bool, double> second 
                = (Quartet<int, string, bool, double>)first.Clone();
            Assert.AreEqual(first.First, second.First);
            Assert.AreEqual(first.Second, second.Second);
            Assert.AreEqual(first.Third, second.Third);
            Assert.AreEqual(first.Fourth, second.Fourth);
        }

        [TestMethod]
        public void Quartet_ReadOnly_1()
        {
            Quartet<int, string, bool, double> quartet
                = new Quartet<int, string, bool, double>();
            Assert.AreEqual(false, quartet.ReadOnly);

            quartet = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14, true);
            Assert.AreEqual(true, quartet.ReadOnly);
        }

        [TestMethod]
        public void Quartet_AsReadOnly_1()
        {
            Quartet<int, string, bool, double> first
                = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14);
            Assert.AreEqual(false, first.ReadOnly);

            Quartet<int, string, bool, double> second = first.AsReadOnly();
            Assert.AreEqual(first.First, second.First);
            Assert.AreEqual(first.Second, second.Second);
            Assert.AreEqual(first.Third, second.Third);
            Assert.AreEqual(first.Fourth, second.Fourth);
            Assert.AreEqual(true, second.ReadOnly);
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void Quartet_ThrowIfReadOnly_1()
        {
            Quartet<int, string, bool, double> quartet
                = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14, true);
            quartet.ThrowIfReadOnly();
        }

        [TestMethod]
        public void Quartet_Equals_1()
        {
            Quartet<int, string, bool, double> first
                = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14);
            Quartet<int, string, bool, double> second
                = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14);
            Assert.AreEqual(true, first.Equals(second));

            second = new Quartet<int, string, bool, double>(2, "World", false, 3.15);
            Assert.AreEqual(false, first.Equals(second));
        }

        [TestMethod]
        public void Quartet_Equals_2()
        {
            Quartet<int, string, bool, double> first
                = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14);
            object second = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14);
            Assert.AreEqual(true, first.Equals(second));

            second = new Quartet<int, string, bool, double>(2, "World", false, 3.15);
            Assert.AreEqual(false, first.Equals(second));

            Assert.AreEqual(false, first.Equals(null));
            Assert.AreEqual(true, first.Equals((object)first));
            Assert.AreEqual(false, first.Equals("Hello"));
        }

        [TestMethod]
        public void Quartet_GetHashCode_1()
        {
            Quartet<int, string, bool, double> first
                = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14);
            Quartet<int, string, bool, double> second
                = new Quartet<int, string, bool, double>(2, "World", false, 3.15);
            Assert.AreNotEqual
                (
                    first.GetHashCode(),
                    second.GetHashCode()
                );
        }

        [TestMethod]
        public void Quartet_ToString_1()
        {
            Quartet<int, string, bool, double> quartet
                = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14);
            string expected = "1;Hello;True;" + 3.14;
            Assert.AreEqual(expected, quartet.ToString());
        }

        [TestMethod]
        public void Quartet_SetReadOnly_1()
        {
            Quartet<int, string, bool, double> Quartet
                = new Quartet<int, string, bool, double>(1, "Hello", true, 3.14);
            Assert.AreEqual(false, Quartet.ReadOnly);
            Quartet.SetReadOnly();
            Assert.AreEqual(true, Quartet.ReadOnly);
        }
    }
}
