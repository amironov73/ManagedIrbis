using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class KeyedCollectionTest
    {
        private KeyedCollection<string, string> _GetCollection1()
        {
            KeyedCollection<string, string> result
                = new KeyedCollection<string, string>()
                {
                    { "one", "two" },
                    { "one", "three" },
                    { "two", "three" },
                    { "four", "five" }
                };

            return result;
        }

        private KeyedCollection<string, string> _GetCollection2()
        {
            KeyedCollection<string, string> result
                = new KeyedCollection<string, string>
                (
                    StringComparer.OrdinalIgnoreCase
                )
                {
                    { "one", "two" },
                    { "one", "three" },
                    { "two", "three" },
                    { "four", "five" }
                };

            return result;
        }

        [TestMethod]
        public void TestKeyedCollection_Construction()
        {
            KeyedCollection<string, string> collection
                = _GetCollection1();

            Assert.AreEqual(4, collection.Count);
            Assert.AreEqual("two", collection[0]);
            Assert.AreEqual("three", collection[1]);
            Assert.AreEqual("three", collection[2]);
            Assert.AreEqual("five", collection[3]);
        }

        [TestMethod]
        public void TestKeyedCollection_SetElement()
        {
            KeyedCollection<string, string> collection
                = _GetCollection1();

            Assert.AreEqual("three", collection[2]);
            collection[2] = "four";
            Assert.AreEqual("four", collection[2]);
        }

        [TestMethod]
        public void TestKeyedCollection_Comparer1()
        {
            KeyedCollection<string, string> collection
                = _GetCollection1();

            Assert.AreEqual("two", collection["one"]);
            Assert.AreEqual("three", collection["two"]);
        }

        [TestMethod]
        public void TestKeyedCollection_Comparer2()
        {
            KeyedCollection<string, string> collection
                = _GetCollection2();

            Assert.AreEqual("two", collection["One"]);
            Assert.AreEqual("three", collection["tWo"]);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TestKeyedCollection_NonExistingKey1()
        {
            KeyedCollection<string, string> collection
                = _GetCollection1();

            Assert.IsNull(collection["KeyNotExist"]);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TestKeyedCollection_NonExistingKey2()
        {
            KeyedCollection<string, string> collection
                = _GetCollection2();

            Assert.IsNull(collection["KeyNotExist"]);
        }

        [TestMethod]
        public void TestKeyedCollection_Clear()
        {
            KeyedCollection<string, string> collection
                = _GetCollection2();

            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void TestKeyedCollection_Comparer()
        {
            KeyedCollection<string, string> collection
                = _GetCollection2();

            Assert.IsTrue
                (
                    collection.Comparer.Equals
                    (
                        "one",
                        "One"
                    )
                );
        }

        [TestMethod]
        public void TestKeyedCollection_Contains()
        {
            KeyedCollection<string, string> collection
                = _GetCollection2();

            Assert.IsTrue(collection.Contains("one"));
            Assert.IsTrue(collection.Contains("One"));
            Assert.IsFalse(collection.Contains("NoSuchKey"));
        }

        [TestMethod]
        public void TestKeyedCollection_Remove()
        {
            KeyedCollection<string, string> collection
                = _GetCollection1();

            collection.Remove("one");
            Assert.AreEqual(2, collection.Count);
        }

        [TestMethod]
        public void TestKeyedCollection_GetValues()
        {
            KeyedCollection<string, string> collection
                = _GetCollection1();

            string[] values = collection.GetValues("one");
            Assert.AreEqual(2, values.Length);
            Assert.AreEqual("two", values[0]);
            Assert.AreEqual("three", values[1]);
        }

        [TestMethod]
        public void TestKeyedCollection_GetEnumerator()
        {
            KeyedCollection<string, string> collection
                = _GetCollection1();

            KeyedCollection<string, string>.Element[] elements
                = collection.ToArray();
            Assert.AreEqual(4, elements.Length);

            IEnumerator enumerator
                = ((IEnumerable)collection).GetEnumerator();
            Assert.IsNotNull(enumerator);
            enumerator.MoveNext();
            KeyedCollection<string, string>.Element element
                = (KeyedCollection<string, string>.Element)enumerator.Current;
            Assert.AreEqual("one", element.Key);
        }

        [TestMethod]
        public void TestKeyedCollection_Element()
        {
            KeyedCollection<string, string>.Element element
                = new KeyedCollection<string, string>.Element
                (
                    "one",
                    "two"
                );

            string actual = element.ToString();
            Assert.AreEqual("one=two", actual);
        }

        [TestMethod]
        public void TestKeyedCollection_AddOrReplace()
        {
            KeyedCollection<string, string> collection
                = _GetCollection1();

            collection.AddOrReplace("one", "ten");
            Assert.AreEqual(4, collection.Count);
            Assert.AreEqual("ten", collection["one"]);

            collection.AddOrReplace("ten", "twelve");
            Assert.AreEqual(5, collection.Count);
            Assert.AreEqual("twelve", collection["ten"]);
        }
    }
}
