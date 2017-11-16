using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

// ReSharper disable CollectionNeverUpdated
// ReSharper disable InlineOutVariableDeclaration

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class BidirectionalDictionaryTest
    {
        class MyInt32Comparer
            : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return x == y;
            }

            public int GetHashCode(int obj)
            {
                return obj;
            }
        }

        private BidirectionalDictionary<string, int> GetDictionary1()
        {
            return new BidirectionalDictionary<string, int>
            {
                {"one", 1 },
                {"two", 2 },
                {"three", 3 },
                {"four", 4 },
            };
        }

        private BidirectionalDictionary<string, int> GetDictionary2()
        {
            return new BidirectionalDictionary<string, int>
                (
                    StringComparer.OrdinalIgnoreCase,
                    new MyInt32Comparer()
                )
            {
                {"one", 1 },
                {"two", 2 },
                {"three", 3 },
                {"four", 4 },
            };
        }

        [TestMethod]
        public void BidirectionalDictionary_Construction_1()
        {
            BidirectionalDictionary<string, int> dictionary
                = new BidirectionalDictionary<string, int>();
            Assert.AreEqual(0, dictionary.Count);
            Assert.IsFalse(dictionary.IsReadOnly);
        }

        [TestMethod]
        public void BidirectionalDictionary_Construction_2()
        {
            BidirectionalDictionary<string, int> dictionary
                = new BidirectionalDictionary<string, int>
                    (
                        StringComparer.OrdinalIgnoreCase,
                        new MyInt32Comparer()
                    );
            Assert.AreEqual(0, dictionary.Count);
            Assert.IsFalse(dictionary.IsReadOnly);
        }

        [TestMethod]
        public void BidirectionalDictionary_Indexer_1()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();

            Assert.AreEqual(4, dictionary.Count);
            Assert.AreEqual(1, dictionary["one"]);
        }

        [TestMethod]
        public void BidirectionalDictionary_Indexer_2()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary2();

            Assert.AreEqual(4, dictionary.Count);
            Assert.AreEqual(1, dictionary["ONE"]);
            Assert.AreEqual(1, dictionary["one"]);
        }

        [TestMethod]
        public void BidirectionalDictionary_Indexer_3()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();

            Assert.AreEqual(4, dictionary.Count);
            Assert.AreEqual(1, dictionary["one"]);
            dictionary["one"] = 11;
            Assert.AreEqual(4, dictionary.Count);
            Assert.AreEqual(11, dictionary["one"]);
        }

        [TestMethod]
        public void BidirectionalDictionary_Indexer_4()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary2();

            Assert.AreEqual(4, dictionary.Count);
            Assert.AreEqual(1, dictionary["ONE"]);
            Assert.AreEqual(1, dictionary["one"]);
            dictionary["ONE"] = 11;
            Assert.AreEqual(4, dictionary.Count);
            Assert.AreEqual(11, dictionary["ONE"]);
            Assert.AreEqual(11, dictionary["one"]);
        }

        [TestMethod]
        public void BidirectionaDictionary_Add_1()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();
            Assert.AreEqual(4, dictionary.Count);
            dictionary.Add("five", 5);
            Assert.AreEqual(5, dictionary.Count);
            Assert.AreEqual(5, dictionary["five"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BidirectionaDictionary_Add_1a()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();
            Assert.AreEqual(4, dictionary.Count);
            dictionary.Add("four", 4);
        }

        [TestMethod]
        public void BidirectionaDictionary_Add_2()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary2();
            Assert.AreEqual(4, dictionary.Count);
            dictionary.Add("five", 5);
            Assert.AreEqual(5, dictionary.Count);
            Assert.AreEqual(5, dictionary["FIVE"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BidirectionaDictionary_Add_3a()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary2();
            Assert.AreEqual(4, dictionary.Count);
            dictionary.Add("FOUR", 4);
        }

        [TestMethod]
        public void BidirectionalDictionary_Clear_1()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();
            Assert.AreEqual(4, dictionary.Count);
            dictionary.Clear();
            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void BidirectionalDictionary_Clear_2()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary2();
            Assert.AreEqual(4, dictionary.Count);
            dictionary.Clear();
            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void BidirectionaDictionary_AsReadOnly_1()
        {
            BidirectionalDictionary<string, int> dictionary
                = GetDictionary1().AsReadOnly();
            Assert.IsTrue(dictionary.IsReadOnly);
            Assert.AreEqual(4, dictionary.Count);
            dictionary["four"] = 44;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void BidirectionaDictionary_AsReadOnly_2()
        {
            BidirectionalDictionary<string, int> dictionary
                = GetDictionary1().AsReadOnly();
            Assert.IsTrue(dictionary.IsReadOnly);
            Assert.AreEqual(4, dictionary.Count);
            dictionary.Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void BidirectionaDictionary_AsReadOnly_3()
        {
            BidirectionalDictionary<string, int> dictionary
                = GetDictionary1().AsReadOnly();
            Assert.IsTrue(dictionary.IsReadOnly);
            Assert.AreEqual(4, dictionary.Count);
            dictionary.Add("five", 5);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void BidirectionaDictionary_AsReadOnly_4()
        {
            BidirectionalDictionary<string, int> dictionary
                = GetDictionary1().AsReadOnly();
            Assert.IsTrue(dictionary.IsReadOnly);
            Assert.AreEqual(4, dictionary.Count);
            dictionary.Remove("two");
        }

        [TestMethod]
        public void BidirectionalDictionary_Clone_1()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();
            BidirectionalDictionary<string, int> clone
                = (BidirectionalDictionary<string, int>) dictionary.Clone();
            Assert.AreEqual(dictionary.Count, clone.Count);
            Assert.AreEqual(dictionary["one"], clone["one"]);
        }

        [TestMethod]
        public void BidirectionaDictionary_ContainsKey_1()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();
            Assert.IsTrue(dictionary.ContainsKey("one"));
            Assert.IsTrue(dictionary.ContainsKey("two"));
            Assert.IsTrue(dictionary.ContainsKey("three"));
            Assert.IsTrue(dictionary.ContainsKey("four"));
            Assert.IsFalse(dictionary.ContainsKey("five"));
        }

        [TestMethod]
        public void BidirectionaDictionary_ContainsKey_2()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary2();
            Assert.IsTrue(dictionary.ContainsKey("ONE"));
            Assert.IsTrue(dictionary.ContainsKey("TWO"));
            Assert.IsTrue(dictionary.ContainsKey("THREE"));
            Assert.IsTrue(dictionary.ContainsKey("FOUR"));
            Assert.IsFalse(dictionary.ContainsKey("FIVE"));
        }

        [TestMethod]
        public void BidirectionaDictionary_ContainsValue_1()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();
            Assert.IsTrue(dictionary.ContainsValue(1));
            Assert.IsTrue(dictionary.ContainsValue(2));
            Assert.IsTrue(dictionary.ContainsValue(3));
            Assert.IsTrue(dictionary.ContainsValue(4));
            Assert.IsFalse(dictionary.ContainsValue(5));
        }

        [TestMethod]
        public void BidirectionaDictionary_ContainsValue_2()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary2();
            Assert.IsTrue(dictionary.ContainsValue(1));
            Assert.IsTrue(dictionary.ContainsValue(2));
            Assert.IsTrue(dictionary.ContainsValue(3));
            Assert.IsTrue(dictionary.ContainsValue(4));
            Assert.IsFalse(dictionary.ContainsValue(5));
        }

        [TestMethod]
        public void BidirectionalDictionary_Keys_1()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();
            string[] keys = dictionary.Keys.ToArray();
            Array.Sort(keys);
            Assert.AreEqual(4, keys.Length);
            Assert.AreEqual("four", keys[0]);
            Assert.AreEqual("one", keys[1]);
            Assert.AreEqual("three", keys[2]);
            Assert.AreEqual("two", keys[3]);
        }

        [TestMethod]
        public void BidirectionalDictionary_Values_1()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();
            int[] values = dictionary.Values.ToArray();
            Array.Sort(values);
            Assert.AreEqual(4, values.Length);
            Assert.AreEqual(1, values[0]);
            Assert.AreEqual(2, values[1]);
            Assert.AreEqual(3, values[2]);
            Assert.AreEqual(4, values[3]);
        }

        [TestMethod]
        public void BidirectionalDictionary_GetEnumerator_1()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();
            IEnumerator<KeyValuePair<string, int>> enumerator = dictionary.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(1, enumerator.Current.Value);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(2, enumerator.Current.Value);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(3, enumerator.Current.Value);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(4, enumerator.Current.Value);
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();
        }

        [TestMethod]
        public void BidirectionalDictionary_GetEnumerator_2()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();
            KeyValuePair<string, int>[] array
                = dictionary.OrderBy(pair => pair.Value).ToArray();
            Assert.AreEqual(4, array.Length);
            Assert.AreEqual("one", array[0].Key);
            Assert.AreEqual("two", array[1].Key);
            Assert.AreEqual("three", array[2].Key);
            Assert.AreEqual("four", array[3].Key);
        }

        [TestMethod]
        public void BidirectionalDictionary_GetKey_1()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();
            Assert.AreEqual("one", dictionary.GetKey(1));
            Assert.AreEqual("two", dictionary.GetKey(2));
            Assert.AreEqual("three", dictionary.GetKey(3));
            Assert.AreEqual("four", dictionary.GetKey(4));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void BidirectionalDictionary_GetKey_1a()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();
            Assert.AreEqual("five", dictionary.GetKey(5));
        }

        [TestMethod]
        public void BidirectionalDictionary_TryGetKey_1()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();
            string key;
            Assert.IsTrue(dictionary.TryGetKey(1, out key));
            Assert.AreEqual("one", key);
            Assert.IsFalse(dictionary.TryGetKey(5, out key));
        }

        [TestMethod]
        public void BidirectionalDictionary_TryGetKey_2()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary2();
            string key;
            Assert.IsTrue(dictionary.TryGetKey(1, out key));
            Assert.AreEqual("one", key);
            Assert.IsFalse(dictionary.TryGetKey(5, out key));
        }

        [TestMethod]
        public void BidirectionalDictionary_TryGetValue_1()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();
            int value;
            Assert.IsTrue(dictionary.TryGetValue("one", out value));
            Assert.AreEqual(1, value);
            Assert.IsFalse(dictionary.TryGetValue("five", out value));
        }

        [TestMethod]
        public void BidirectionalDictionary_TryGetValue_2()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary2();
            int value;
            Assert.IsTrue(dictionary.TryGetValue("one", out value));
            Assert.AreEqual(1, value);
            Assert.IsTrue(dictionary.TryGetValue("ONE", out value));
            Assert.AreEqual(1, value);
            Assert.IsFalse(dictionary.TryGetValue("five", out value));
        }

        [TestMethod]
        public void BidirectionalDictionary_Remove_1()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();
            Assert.IsTrue(dictionary.ContainsKey("two"));
            dictionary.Remove("two");
            Assert.IsFalse(dictionary.ContainsKey("two"));
        }

        [TestMethod]
        public void BidirectionalDictionary_Remove_2()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary2();
            Assert.IsTrue(dictionary.ContainsKey("two"));
            Assert.IsTrue(dictionary.ContainsKey("TWO"));
            dictionary.Remove("TWO");
            Assert.IsFalse(dictionary.ContainsKey("two"));
            Assert.IsFalse(dictionary.ContainsKey("TWO"));
        }

        [TestMethod]
        public void BidirectionalDictionary_Remove_3()
        {
            BidirectionalDictionary<string, int> dictionary = GetDictionary1();
            Assert.IsFalse(dictionary.ContainsKey("five"));
            dictionary.Remove("five");
            Assert.IsFalse(dictionary.ContainsKey("five"));
        }
    }
}
