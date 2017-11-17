using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

// ReSharper disable CollectionNeverUpdated.Local

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class DictionaryCounterDoubleTest
    {
        private DictionaryCounterDouble<string> _GetDictionary1()
        {
            DictionaryCounterDouble<string> result
                = new DictionaryCounterDouble<string>
                {
                    {"first", 10.0},
                    {"second", 20.0},
                    {"third", 30.0}
                };

            return result;
        }

        private DictionaryCounterDouble<string> _GetDictionary2()
        {
            DictionaryCounterDouble<string> result
                = new DictionaryCounterDouble<string>(_GetComparer())
                {
                    {"first", 10.0},
                    {"second", 20.0},
                    {"third", 30.0}
                };

            return result;
        }

        private IEqualityComparer<string> _GetComparer()
        {
            return StringComparer.OrdinalIgnoreCase;
        }

        [TestMethod]
        public void DictionaryCounterDouble_Construction_1()
        {
            DictionaryCounterDouble<string> dictionary
                = new DictionaryCounterDouble<string>();

            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryCounterDouble_Construction_2()
        {
            DictionaryCounterDouble<string> dictionary
                = new DictionaryCounterDouble<string>(100);

            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryCounterDouble_Construction_3()
        {
            DictionaryCounterDouble<string> first = _GetDictionary1();
            DictionaryCounterDouble<string> second
                = new DictionaryCounterDouble<string>(first);

            Assert.AreEqual(first.Count, second.Count);
        }

        [TestMethod]
        public void DictionaryCounterDouble_Construction_4()
        {
            DictionaryCounterDouble<string> dictionary
                = new DictionaryCounterDouble<string>(_GetComparer());
            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryCounterDouble_Augment_1()
        {
            DictionaryCounterDouble<string> dictionary
                = new DictionaryCounterDouble<string>();

            dictionary.Augment("first", 2.0);
            Assert.AreEqual(2.0, dictionary.GetValue("first"));

            dictionary.Augment("first", 2.0);
            Assert.AreEqual(4.0, dictionary.GetValue("first"));
        }

        [TestMethod]
        public void DictionaryCounterDouble_Augment_2()
        {
            DictionaryCounterDouble<string> dictionary
                = new DictionaryCounterDouble<string>(_GetComparer());

            dictionary.Augment("first", 2.0);
            Assert.AreEqual(2.0, dictionary.GetValue("first"));

            dictionary.Augment("FIRST", 2.0);
            Assert.AreEqual(4.0, dictionary.GetValue("first"));
        }

        [TestMethod]
        public void DictionaryCounterDouble_Clear_1()
        {
            DictionaryCounterDouble<string> dictionary = _GetDictionary1();
            dictionary.Clear();

            Assert.AreEqual(0.0, dictionary.GetValue("first"));
            Assert.AreEqual(0.0, dictionary.GetValue("second"));
            Assert.AreEqual(0.0, dictionary.GetValue("third"));
            Assert.AreEqual(0.0, dictionary.GetValue("fourth"));
        }

        [TestMethod]
        public void DictionaryCounterDouble_Clear_2()
        {
            DictionaryCounterDouble<string> dictionary = _GetDictionary2();
            dictionary.Clear();

            Assert.AreEqual(0.0, dictionary.GetValue("first"));
            Assert.AreEqual(0.0, dictionary.GetValue("second"));
            Assert.AreEqual(0.0, dictionary.GetValue("third"));
            Assert.AreEqual(0.0, dictionary.GetValue("fourth"));
        }

        [TestMethod]
        public void DictionaryCounterDouble_GetValue_1()
        {
            DictionaryCounterDouble<string> dictionary = _GetDictionary1();

            Assert.AreEqual(10.0, dictionary.GetValue("first"));
            Assert.AreEqual(0.0, dictionary.GetValue("FIRST"));
            Assert.AreEqual(20.0, dictionary.GetValue("second"));
            Assert.AreEqual(0.0, dictionary.GetValue("SECOND"));
            Assert.AreEqual(30.0, dictionary.GetValue("third"));
            Assert.AreEqual(0.0, dictionary.GetValue("THIRD"));
            Assert.AreEqual(0.0, dictionary.GetValue("fourth"));
            Assert.AreEqual(0.0, dictionary.GetValue("FOURTH"));
        }

        [TestMethod]
        public void DictionaryCounterDouble_GetValue_2()
        {
            DictionaryCounterDouble<string> dictionary = _GetDictionary2();

            Assert.AreEqual(10.0, dictionary.GetValue("first"));
            Assert.AreEqual(10.0, dictionary.GetValue("FIRST"));
            Assert.AreEqual(20.0, dictionary.GetValue("second"));
            Assert.AreEqual(20.0, dictionary.GetValue("SECOND"));
            Assert.AreEqual(30.0, dictionary.GetValue("third"));
            Assert.AreEqual(30.0, dictionary.GetValue("THIRD"));
            Assert.AreEqual(0.0, dictionary.GetValue("fourth"));
            Assert.AreEqual(0.0, dictionary.GetValue("FOURTH"));
        }

        [TestMethod]
        public void DictionaryCounterDouble_Increment_1()
        {
            DictionaryCounterDouble<string> dictionary = _GetDictionary1();

            dictionary.Increment("second");
            dictionary.Increment("third");
            dictionary.Increment("first");

            Assert.AreEqual(11.0, dictionary.GetValue("first"));
            Assert.AreEqual(0.0, dictionary.GetValue("FIRST"));
            Assert.AreEqual(21.0, dictionary.GetValue("second"));
            Assert.AreEqual(0.0, dictionary.GetValue("SECOND"));
            Assert.AreEqual(31.0, dictionary.GetValue("third"));
            Assert.AreEqual(0.0, dictionary.GetValue("THIRD"));
            Assert.AreEqual(0.0, dictionary.GetValue("fourth"));
            Assert.AreEqual(0.0, dictionary.GetValue("FOURTH"));
        }

        [TestMethod]
        public void DictionaryCounterDouble_Increment_2()
        {
            DictionaryCounterDouble<string> dictionary = _GetDictionary2();

            dictionary.Increment("SECOND");
            dictionary.Increment("THIRD");
            dictionary.Increment("FIRST");

            Assert.AreEqual(11.0, dictionary.GetValue("first"));
            Assert.AreEqual(11.0, dictionary.GetValue("FIRST"));
            Assert.AreEqual(21.0, dictionary.GetValue("second"));
            Assert.AreEqual(21.0, dictionary.GetValue("SECOND"));
            Assert.AreEqual(31.0, dictionary.GetValue("third"));
            Assert.AreEqual(31.0, dictionary.GetValue("THIRD"));
            Assert.AreEqual(0.0, dictionary.GetValue("fourth"));
            Assert.AreEqual(0.0, dictionary.GetValue("FOURTH"));
        }

        [TestMethod]
        public void DictionaryCounterDouble_Total_1()
        {
            DictionaryCounterDouble<string> dictionary = _GetDictionary1();

            Assert.AreEqual(60.0, dictionary.Total);
        }
    }
}
