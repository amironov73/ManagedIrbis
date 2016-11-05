using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class DictionaryCounterDoubleTest
    {
        [TestMethod]
        public void DictionaryCounterDouble_Construction1()
        {
            DictionaryCounterDouble<string> dictionary
                = new DictionaryCounterDouble<string>();

            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryCounterDouble_Construction2()
        {
            DictionaryCounterDouble<string> dictionary
                = new DictionaryCounterDouble<string>(100);

            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryCounterDouble_Construction3()
        {
            DictionaryCounterDouble<string> first
                = new DictionaryCounterDouble<string>
                {
                    {"first", 10.0},
                    {"second", 20.0},
                    {"third", 30.0}
                };
            DictionaryCounterDouble<string> second
                = new DictionaryCounterDouble<string>(first);

            Assert.AreEqual(first.Count, second.Count);
        }

        [TestMethod]
        public void DictionaryCounterDouble_Augment()
        {
            DictionaryCounterDouble<string> dictionary
                = new DictionaryCounterDouble<string>();

            dictionary.Augment("first", 2.0);
            Assert.AreEqual(2.0, dictionary.GetValue("first"), 0.01);

            dictionary.Augment("first", 2.0);
            Assert.AreEqual(4.0, dictionary.GetValue("first"), 0.01);
        }

        [TestMethod]
        public void DictionaryCounterDouble_Clear()
        {
            DictionaryCounterDouble<string> dictionary
                = new DictionaryCounterDouble<string>();

            dictionary.Augment("first", 2.0);
            dictionary.Increment("second");
            dictionary.Increment("third");
            dictionary.Increment("first");

            dictionary.Clear();

            Assert.AreEqual(0.0, dictionary.GetValue("first"), 0.01);
            Assert.AreEqual(0.0, dictionary.GetValue("second"), 0.01);
            Assert.AreEqual(0.0, dictionary.GetValue("third"), 0.01);
            Assert.AreEqual(0.0, dictionary.GetValue("fourth"), 0.01);
        }

        [TestMethod]
        public void DictionaryCounterDouble_GetValue()
        {
            DictionaryCounterDouble<string> dictionary
                = new DictionaryCounterDouble<string>
                {
                    {"first", 10.0},
                    {"second", 20.0},
                    {"third", 30.0}
                };

            Assert.AreEqual(10.0, dictionary.GetValue("first"), 0.01);
            Assert.AreEqual(20.0, dictionary.GetValue("second"), 0.01);
            Assert.AreEqual(30.0, dictionary.GetValue("third"), 0.01);
            Assert.AreEqual(0.0, dictionary.GetValue("fourth"), 0.01);
        }

        [TestMethod]
        public void DictionaryCounterDouble_Increment()
        {
            DictionaryCounterDouble<string> dictionary
                = new DictionaryCounterDouble<string>();

            dictionary.Increment("second");
            dictionary.Increment("third");
            dictionary.Increment("first");

            Assert.AreEqual(1.0, dictionary.GetValue("first"), 0.01);
            Assert.AreEqual(1.0, dictionary.GetValue("second"), 0.01);
            Assert.AreEqual(1.0, dictionary.GetValue("third"), 0.01);
            Assert.AreEqual(0.0, dictionary.GetValue("fourth"), 0.01);
        }

        [TestMethod]
        public void DictionaryCounterDouble_Total()
        {
            DictionaryCounterDouble<string> dictionary
                = new DictionaryCounterDouble<string>
                {
                    {"first", 10.0},
                    {"second", 20.0},
                    {"third", 30.0}
                };

            Assert.AreEqual(60.0, dictionary.Total, 0.01);
        }
    }
}
