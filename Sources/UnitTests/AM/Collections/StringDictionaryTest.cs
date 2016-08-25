using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;
using AM.Runtime;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class StringDictionaryTest
    {
        [TestMethod]
        public void TestStringDictionary_Construction()
        {
            StringDictionary dictionary = new StringDictionary
            {
                {"one", "first"},
                {"two", "second"},
                {"three", "third"}
            };

            Assert.AreEqual(3, dictionary.Count);
            Assert.AreEqual("first", dictionary["one"]);
        }

        private void _TestSerialization
            (
                StringDictionary first
            )
        {
            byte[] bytes = first.SaveToMemory();

            StringDictionary second = bytes
                .RestoreObjectFromMemory<StringDictionary>();

            Assert.AreEqual(first.Count, second.Count);
            foreach (var pair in first)
            {
                string key = pair.Key;
                string expected = pair.Value;
                string actual = second[key];

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void TestStringDictionary_Serialization()
        {
            StringDictionary dictionary = new StringDictionary();
            _TestSerialization(dictionary);

            dictionary = new StringDictionary
            {
                {"one", "first"},
                {"two", "second"},
                {"three", "third"}
            };
            _TestSerialization(dictionary);
        }
    }
}
