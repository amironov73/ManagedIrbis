using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class DictionaryUtilityTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDictionaryUtilityMergeWithConflicts()
        {
            Dictionary<string, int> first = new Dictionary<string, int>
            {
                {"one", 1},
                {"two", 2},
                {"three", 3}
            };

            Dictionary<string, int> second = new Dictionary<string, int>
            {
                {"three", 3},
                {"four", 4},
                {"five", 5}
            };

            Dictionary<string, int> result
                = DictionaryUtility.MergeWithConflicts
                    (
                        first,
                        second
                    );

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestDictionaryUtilityMergeWithoutConflicts()
        {
            Dictionary<string, int> first = new Dictionary<string, int>
            {
                {"one", 1},
                {"two", 2},
                {"three", 3}
            };

            Dictionary<string, int> second = new Dictionary<string, int>
            {
                {"three", 4},
                {"four", 5},
                {"five", 6}
            };

            Dictionary<string, int> result
                = DictionaryUtility.MergeWithoutConflicts
                    (
                        first,
                        second
                    );

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count);
        }

        [TestMethod]
        public void TestDictionaryUtilityMergeLastValues()
        {
            Dictionary<string, int> first = new Dictionary<string, int>
            {
                {"one", 1},
                {"two", 2},
                {"three", 3}
            };

            Dictionary<string, int> second = new Dictionary<string, int>
            {
                {"three", 4},
                {"four", 5},
                {"five", 6}
            };

            Dictionary<string, int> result
                = DictionaryUtility.MergeLastValues
                    (
                        first,
                        second
                    );

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(4, result["three"]);
        }
    }
}
