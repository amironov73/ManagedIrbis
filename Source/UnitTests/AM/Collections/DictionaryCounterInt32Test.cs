using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class DictionaryCounterInt32Test
    {
        [TestMethod]
        public void DictionaryCounterInt32_Construction1()
        {
            DictionaryCounterInt32<string> dictionary
                = new DictionaryCounterInt32<string>();

            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryCounterInt32_Construction2()
        {
            DictionaryCounterInt32<string> dictionary
                = new DictionaryCounterInt32<string>(100);

            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryCounterInt32_Construction3()
        {
            DictionaryCounterInt32<string> first
                = new DictionaryCounterInt32<string>
                {
                    {"first", 10},
                    {"second", 20},
                    {"third", 30}
                };
            DictionaryCounterInt32<string> second
                = new DictionaryCounterInt32<string>(first);

            Assert.AreEqual(first.Count, second.Count);
        }

        [TestMethod]
        public void DictionaryCounterInt32_Augment()
        {
            DictionaryCounterInt32<string> dictionary
                = new DictionaryCounterInt32<string>();

            dictionary.Augment("first", 2);
            Assert.AreEqual(2, dictionary.GetValue("first"));

            dictionary.Augment("first", 2);
            Assert.AreEqual(4, dictionary.GetValue("first"));
        }

        [TestMethod]
        public void DictionaryCounterInt32_Clear()
        {
            DictionaryCounterInt32<string> dictionary
                = new DictionaryCounterInt32<string>();

            dictionary.Augment("first", 2);
            dictionary.Increment("second");
            dictionary.Increment("third");
            dictionary.Increment("first");

            dictionary.Clear();

            Assert.AreEqual(0, dictionary.GetValue("first"));
            Assert.AreEqual(0, dictionary.GetValue("second"));
            Assert.AreEqual(0, dictionary.GetValue("third"));
            Assert.AreEqual(0, dictionary.GetValue("fourth"));
        }

        [TestMethod]
        public void DictionaryCounterInt32_GetValue()
        {
            DictionaryCounterInt32<string> dictionary
                = new DictionaryCounterInt32<string>
                {
                    {"first", 10},
                    {"second", 20},
                    {"third", 30}
                };

            Assert.AreEqual(10, dictionary.GetValue("first"));
            Assert.AreEqual(20, dictionary.GetValue("second"));
            Assert.AreEqual(30, dictionary.GetValue("third"));
            Assert.AreEqual(0, dictionary.GetValue("fourth"));
        }

        [TestMethod]
        public void DictionaryCounterInt32_Increment()
        {
            DictionaryCounterInt32<string> dictionary
                = new DictionaryCounterInt32<string>();

            dictionary.Increment("second");
            dictionary.Increment("third");
            dictionary.Increment("first");

            Assert.AreEqual(1, dictionary.GetValue("first"));
            Assert.AreEqual(1, dictionary.GetValue("second"));
            Assert.AreEqual(1, dictionary.GetValue("third"));
            Assert.AreEqual(0, dictionary.GetValue("fourth"));
        }

        [TestMethod]
        public void DictionaryCounterInt32_Total()
        {
            DictionaryCounterInt32<string> dictionary
                = new DictionaryCounterInt32<string>
                {
                    {"first", 10},
                    {"second", 20},
                    {"third", 30}
                };

            Assert.AreEqual(60, dictionary.Total);
        }
    }
}
