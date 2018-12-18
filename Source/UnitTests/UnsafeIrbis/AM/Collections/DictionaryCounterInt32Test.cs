using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UnsafeAM.Collections;

// ReSharper disable CollectionNeverUpdated.Local

namespace UnitTests.UnsafeAM.Collections
{
    [TestClass]
    public class DictionaryCounterInt32Test
    {
        private DictionaryCounterInt32<string> _GetDictionary1()
        {
            DictionaryCounterInt32<string> result
                = new DictionaryCounterInt32<string>
                {
                    {"first", 10},
                    {"second", 20},
                    {"third", 30}
                };

            return result;
        }

        private DictionaryCounterInt32<string> _GetDictionary2()
        {
            DictionaryCounterInt32<string> result
                = new DictionaryCounterInt32<string>(_GetComparer())
                {
                    {"first", 10},
                    {"second", 20},
                    {"third", 30}
                };

            return result;
        }

        private IEqualityComparer<string> _GetComparer()
        {
            return StringComparer.OrdinalIgnoreCase;
        }

        [TestMethod]
        public void DictionaryCounterInt32_Construction_1()
        {
            DictionaryCounterInt32<string> dictionary
                = new DictionaryCounterInt32<string>();

            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryCounterInt32_Construction_2()
        {
            DictionaryCounterInt32<string> dictionary
                = new DictionaryCounterInt32<string>(100);

            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryCounterInt32_Construction_3()
        {
            DictionaryCounterInt32<string> first = _GetDictionary1();
            DictionaryCounterInt32<string> second
                = new DictionaryCounterInt32<string>(first);

            Assert.AreEqual(first.Count, second.Count);
        }

        [TestMethod]
        public void DictionaryCounterInt32_Construction_4()
        {
            DictionaryCounterInt32<string> dictionary
                = new DictionaryCounterInt32<string>(_GetComparer());
            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryCounterInt32_Augment_1()
        {
            DictionaryCounterInt32<string> dictionary
                = new DictionaryCounterInt32<string>();

            dictionary.Augment("first", 2);
            Assert.AreEqual(2, dictionary.GetValue("first"));

            dictionary.Augment("first", 2);
            Assert.AreEqual(4, dictionary.GetValue("first"));
        }

        [TestMethod]
        public void DictionaryCounterInt32_Augment_2()
        {
            DictionaryCounterInt32<string> dictionary
                = new DictionaryCounterInt32<string>(_GetComparer());

            dictionary.Augment("first", 2);
            Assert.AreEqual(2, dictionary.GetValue("first"));

            dictionary.Augment("FIRST", 2);
            Assert.AreEqual(4, dictionary.GetValue("first"));
        }

        [TestMethod]
        public void DictionaryCounterInt32_Clear_1()
        {
            DictionaryCounterInt32<string> dictionary = _GetDictionary1();
            dictionary.Clear();

            Assert.AreEqual(0, dictionary.GetValue("first"));
            Assert.AreEqual(0, dictionary.GetValue("second"));
            Assert.AreEqual(0, dictionary.GetValue("third"));
            Assert.AreEqual(0, dictionary.GetValue("fourth"));
        }

        [TestMethod]
        public void DictionaryCounterInt32_Clear_2()
        {
            DictionaryCounterInt32<string> dictionary = _GetDictionary2();
            dictionary.Clear();

            Assert.AreEqual(0, dictionary.GetValue("first"));
            Assert.AreEqual(0, dictionary.GetValue("second"));
            Assert.AreEqual(0, dictionary.GetValue("third"));
            Assert.AreEqual(0, dictionary.GetValue("fourth"));
        }

        [TestMethod]
        public void DictionaryCounterInt32_GetValue_1()
        {
            DictionaryCounterInt32<string> dictionary = _GetDictionary1();

            Assert.AreEqual(10, dictionary.GetValue("first"));
            Assert.AreEqual(0, dictionary.GetValue("FIRST"));
            Assert.AreEqual(20, dictionary.GetValue("second"));
            Assert.AreEqual(0, dictionary.GetValue("SECOND"));
            Assert.AreEqual(30, dictionary.GetValue("third"));
            Assert.AreEqual(0, dictionary.GetValue("THIRD"));
            Assert.AreEqual(0, dictionary.GetValue("fourth"));
            Assert.AreEqual(0, dictionary.GetValue("FOURTH"));
        }

        [TestMethod]
        public void DictionaryCounterInt32_GetValue_2()
        {
            DictionaryCounterInt32<string> dictionary = _GetDictionary2();

            Assert.AreEqual(10, dictionary.GetValue("first"));
            Assert.AreEqual(10, dictionary.GetValue("FIRST"));
            Assert.AreEqual(20, dictionary.GetValue("second"));
            Assert.AreEqual(20, dictionary.GetValue("SECOND"));
            Assert.AreEqual(30, dictionary.GetValue("third"));
            Assert.AreEqual(30, dictionary.GetValue("THIRD"));
            Assert.AreEqual(0, dictionary.GetValue("fourth"));
            Assert.AreEqual(0, dictionary.GetValue("FOURTH"));
        }

        [TestMethod]
        public void DictionaryCounterInt32_Increment_1()
        {
            DictionaryCounterInt32<string> dictionary = _GetDictionary1();

            dictionary.Increment("second");
            dictionary.Increment("third");
            dictionary.Increment("first");

            Assert.AreEqual(11, dictionary.GetValue("first"));
            Assert.AreEqual(0, dictionary.GetValue("FIRST"));
            Assert.AreEqual(21, dictionary.GetValue("second"));
            Assert.AreEqual(0, dictionary.GetValue("SECOND"));
            Assert.AreEqual(31, dictionary.GetValue("third"));
            Assert.AreEqual(0, dictionary.GetValue("THIRD"));
            Assert.AreEqual(0, dictionary.GetValue("fourth"));
            Assert.AreEqual(0, dictionary.GetValue("FOURTH"));
        }

        [TestMethod]
        public void DictionaryCounterInt32_Increment_2()
        {
            DictionaryCounterInt32<string> dictionary = _GetDictionary2();

            dictionary.Increment("SECOND");
            dictionary.Increment("THIRD");
            dictionary.Increment("FIRST");

            Assert.AreEqual(11, dictionary.GetValue("first"));
            Assert.AreEqual(11, dictionary.GetValue("FIRST"));
            Assert.AreEqual(21, dictionary.GetValue("second"));
            Assert.AreEqual(21, dictionary.GetValue("SECOND"));
            Assert.AreEqual(31, dictionary.GetValue("third"));
            Assert.AreEqual(31, dictionary.GetValue("THIRD"));
            Assert.AreEqual(0, dictionary.GetValue("fourth"));
            Assert.AreEqual(0, dictionary.GetValue("FOURTH"));
        }

        [TestMethod]
        public void DictionaryCounterInt32_Total_1()
        {
            DictionaryCounterInt32<string> dictionary = _GetDictionary1();

            Assert.AreEqual(60, dictionary.Total);
        }
    }
}
