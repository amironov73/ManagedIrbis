using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class DictionaryCounterTest
    {
        [TestMethod]
        public void TestDictionaryCounter1()
        {
            DictionaryCounterDouble<string> dictionary
                = new DictionaryCounterDouble<string>();

            dictionary.Augment("first", 2.0);
            dictionary.Increment("second");
            dictionary.Increment("third");
            dictionary.Increment("first");

            Assert.AreEqual(3.0, dictionary.GetValue("first"), 0.01);
            Assert.AreEqual(1.0, dictionary.GetValue("second"), 0.01);
            Assert.AreEqual(1.0, dictionary.GetValue("third"), 0.01);
            Assert.AreEqual(0.0, dictionary.GetValue("fourth"), 0.01);
        }

        [TestMethod]
        public void TestDictionaryCounter2()
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
    }
}
