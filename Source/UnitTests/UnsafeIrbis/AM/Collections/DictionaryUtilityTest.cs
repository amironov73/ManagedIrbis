using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UnsafeAM.Collections;

// ReSharper disable ExpressionIsAlwaysNull

namespace UnitTests.UnsafeAM.Collections
{
    [TestClass]
    public class DictionaryUtilityTest
    {
        [TestMethod]
        public void DictionaryUtility_GetValueOrDefault_1()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>
            {
                {"one", 1},
                {"two", 2},
                {"three", 3}
            };

            Assert.AreEqual(1, dictionary.GetValueOrDefault("one"));
            Assert.AreEqual(2, dictionary.GetValueOrDefault("two"));
            Assert.AreEqual(3, dictionary.GetValueOrDefault("three"));
            Assert.AreEqual(0, dictionary.GetValueOrDefault("four"));
        }

        [TestMethod]
        public void DictionaryUtility_GetValueOrDefault_2()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>
            {
                {"one", 1},
                {"two", 2},
                {"three", 3}
            };

            Assert.AreEqual(1, dictionary.GetValueOrDefault("one", 100));
            Assert.AreEqual(2, dictionary.GetValueOrDefault("two", 100));
            Assert.AreEqual(3, dictionary.GetValueOrDefault("three", 100));
            Assert.AreEqual(100, dictionary.GetValueOrDefault("four", 100));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DictionaryUtility_MergeWithConflicts_1()
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

           DictionaryUtility.MergeWithConflicts
                    (
                        first,
                        second
                    );
        }

        [TestMethod]
        public void DictionaryUtility_MergeWithConflicts_2()
        {
            Dictionary<string, int> first = new Dictionary<string, int>
            {
                {"one", 1},
                {"two", 2},
                {"three", 3}
            };

            Dictionary<string, int> second = new Dictionary<string, int>
            {
                {"four", 4},
                {"five", 5},
                {"six", 6}
            };

            Dictionary<string, int> result = DictionaryUtility.MergeWithConflicts
                    (
                        first,
                        second
                    );

            Assert.AreEqual(6, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryUtility_MergeWithConflicts_3()
        {
            Dictionary<string, int> first = new Dictionary<string, int>();
            Dictionary<string, int> second = null;
            DictionaryUtility.MergeWithConflicts(first, second);
        }

        [TestMethod]
        public void DictionaryUtility_MergeFirstValues_1()
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

            Dictionary<string, int> result = DictionaryUtility.MergeFirstValues
                    (
                        first,
                        second
                    );

            Assert.AreEqual(5, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryUtility_MergeFirstValues_2()
        {
            Dictionary<string, int> first = new Dictionary<string, int>();
            Dictionary<string, int> second = null;
            DictionaryUtility.MergeFirstValues(first, second);
        }

        [TestMethod]
        public void DictionaryUtility_MergeLastValues_1()
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

            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(4, result["three"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryUtility_MergeLastValues_2()
        {
            Dictionary<string, int> first = new Dictionary<string, int>();
            Dictionary<string, int> second = null;
            DictionaryUtility.MergeLastValues(first, second);
        }

    }
}
