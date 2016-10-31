using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;
using AM.Runtime;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class StringDictionaryTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void StringDictionary_Construction()
        {
            StringDictionary dictionary = new StringDictionary();
            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void StringDictionary_Add()
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
        public void StringDictionary_Serialization()
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

        [TestMethod]
        public void StringDictionary_Load_Save1()
        {
            StringDictionary first = new StringDictionary
            {
                {"one", "first"},
                {"two", "second"},
                {"three", "third"}
            };
            StringWriter writer = new StringWriter();
            first.Save(writer);
            string text = writer.ToString();
            StringReader reader = new StringReader(text);
            StringDictionary second = StringDictionary.Load(reader);
            Assert.AreEqual(first.Count, second.Count);
        }

        [TestMethod]
        public void StringDictionary_Load_Save2()
        {
            string fileName = Path.GetTempFileName();

            StringDictionary first = new StringDictionary
            {
                {"one", "first"},
                {"two", "second"},
                {"three", "third"}
            };
            Encoding encoding = Encoding.UTF8;
            first.Save(fileName, encoding);
            StringDictionary second = StringDictionary.Load
                (
                    fileName,
                    encoding
                );
            Assert.AreEqual(first.Count, second.Count);
        }
    }
}
