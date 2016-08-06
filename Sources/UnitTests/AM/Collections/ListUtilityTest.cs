using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class ListUtilityTest
    {
        [TestMethod]
        public void TestListUtilityIsNullOrEmpty()
        {
            List<int> list = null;
            Assert.IsTrue(list.IsNullOrEmpty());

            list = new List<int>();
            Assert.IsTrue(list.IsNullOrEmpty());

            list.Add(1);
            Assert.IsFalse(list.IsNullOrEmpty());
        }

        [TestMethod]
        public void TestListUtilityContainsValue()
        {
            List<int> list = new List<int> { 1, 2, 3 };

            Assert.IsTrue(list.ContainsValue
                (
                    1,
                   EqualityComparer<int>.Default
                ));

            Assert.IsFalse(list.ContainsValue
                (
                    4,
                   EqualityComparer<int>.Default
                ));
        }

        [TestMethod]
        public void TestListUtilityAddDistinct()
        {
            List<int> list = new List<int>{1,2,3};

            Assert.IsTrue(list.AddDistinct(4));
            Assert.IsFalse(list.AddDistinct(4));

            list = new List<int> { 1, 2, 3 };
            IEqualityComparer<int> comparer
                = EqualityComparer<int>.Default;

            Assert.IsTrue(list.AddDistinct(4, comparer));
            Assert.IsFalse(list.AddDistinct(4, comparer));
        }

        [TestMethod]
        public void TestListUtilityAddRangeDistinct()
        {
            List<int> list = new List<int> { 1, 2, 3 };

            IEqualityComparer<int> comparer
                = EqualityComparer<int>.Default;

            Assert.IsTrue(list.AddRangeDistinct(new[]{4,5,6}, comparer));
            Assert.IsFalse(list.AddRangeDistinct(new[]{6,7,8}, comparer));
        }

        [TestMethod]
        public void TestListUtilityIndexOf()
        {
            List<int> list = new List<int> { 1, 2, 3 };

            Func<int, bool> predicate = i => i == 2;
            Assert.AreEqual(1, list.IndexOf(predicate));

            predicate = i => i == 4;
            Assert.IsTrue(list.IndexOf(predicate) < 0);
        }
    }
}
