using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class StringDictionaryTest
    {
        [TestMethod]
        public void TestStringDictionaryConstruction()
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
    }
}
