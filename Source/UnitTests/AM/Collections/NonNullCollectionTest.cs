using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class NonNullCollectionTest
    {
        [TestMethod]
        public void NonNullCollection_Construction_1()
        {
            NonNullCollection<object> collection = new NonNullCollection<object>();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void NonNullCollection_Add_1()
        {
            NonNullCollection<object> collection = new NonNullCollection<object>
                {
                    new object()
                };
            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NonNullCollection_Add_1a()
        {
            NonNullCollection<object> collection = new NonNullCollection<object>
            {
                null
            };
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void NonNullCollection_AddRange_1()
        {
            NonNullCollection<object> collection
                = new NonNullCollection<object>();
            collection.AddRange
                (
                    new[]
                    {
                        new object(),
                        new object(),
                        new object()
                    }
                );
            Assert.AreEqual(3, collection.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NonNullCollection_AddRange_2()
        {
            NonNullCollection<object> collection
                = new NonNullCollection<object>();
            collection.AddRange
                (
                    new[]
                    {
                        new object(),
                        null,
                        new object()
                    }
                );
        }

        [TestMethod]
        public void NonNullCollection_Clear_1()
        {
            NonNullCollection<object> collection
                = new NonNullCollection<object>
                {
                    new object()
                };

            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void NonNullCollection_Indexer_1()
        {
            NonNullCollection<object> collection = new NonNullCollection<object>
                {
                    new object()
                };
            collection[0] = 1;
            Assert.AreEqual(1, collection[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NonNullCollection_Indexer_1a()
        {
            NonNullCollection<object> collection = new NonNullCollection<object>
                {
                    new object()
                };
            collection[0] = null;
        }

        [TestMethod]
        public void NonNullCollection_EnsureCapacity_1()
        {
            NonNullCollection<object> collection = new NonNullCollection<object>
            {
                new object()
            };
            collection.EnsureCapacity(100);
            Assert.IsTrue(collection.Capacity >= 100);
        }
    }
}
