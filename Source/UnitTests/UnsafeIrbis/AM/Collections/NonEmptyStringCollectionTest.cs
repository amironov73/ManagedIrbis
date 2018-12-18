using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UnsafeAM.Collections;

namespace UnitTests.UnsafeAM.Collections
{
    [TestClass]
    public class NonEmptyStringCollectionTest
    {
        [TestMethod]
        public void NonEmptyStringCollection_Insert_1()
        {
            NonEmptyStringCollection collection = new NonEmptyStringCollection
            {
                "one",
                "two",
                "three"
            };
            Assert.AreEqual(3, collection.Count);
            Assert.AreEqual("one", collection[0]);
            Assert.AreEqual("two", collection[1]);
            Assert.AreEqual("three", collection[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NonEmptyStringCollection_Insert_2()
        {
            NonEmptyStringCollection collection = new NonEmptyStringCollection
            {
                null
            };
            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NonEmptyStringCollection_Insert_3()
        {
            NonEmptyStringCollection collection = new NonEmptyStringCollection
            {
                string.Empty
            };
            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        public void NonEmptyStringCollection_Set_1()
        {
            NonEmptyStringCollection collection = new NonEmptyStringCollection
            {
                "one"
            };
            Assert.AreEqual(1, collection.Count);
            collection[0] = "two";
            Assert.AreEqual("two", collection[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NonEmptyStringCollection_Set_2()
        {
            NonEmptyStringCollection collection = new NonEmptyStringCollection
            {
                "one"
            };
            Assert.AreEqual(1, collection.Count);
            collection[0] = null;
            Assert.AreEqual("two", collection[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NonEmptyStringCollection_Set_3()
        {
            NonEmptyStringCollection collection = new NonEmptyStringCollection
            {
                "one"
            };
            Assert.AreEqual(1, collection.Count);
            collection[0] = string.Empty;
            Assert.AreEqual("two", collection[0]);
        }
    }
}
