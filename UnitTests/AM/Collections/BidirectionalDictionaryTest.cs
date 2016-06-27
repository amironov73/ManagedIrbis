using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class BidirectionalDictionaryTest
    {
        [TestMethod]
        public void TestBidirectionalDictionaryConstruction()
        {
            BidirectionalDictionary<string, int> dictionary
                = new BidirectionalDictionary<string, int>
                {
                    {"one", 1},
                    {"two", 2},
                    {"three", 3},
                    {"four", 4}
                };

            Assert.AreEqual(4, dictionary.Count);
            Assert.AreEqual(1, dictionary["one"]);
        }
    }
}
