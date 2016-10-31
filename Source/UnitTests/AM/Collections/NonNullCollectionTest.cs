using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class NonNullCollectionTest
    {
        [TestMethod]
        public void NonNullCollection_Construction()
        {
            NonNullCollection<object> collection
                = new NonNullCollection<object>();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void NonNullCollection_Add()
        {
            NonNullCollection<object> collection 
                = new NonNullCollection<object>
                {
                    new object()
                };
            Assert.AreEqual(1, collection.Count);

            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void NonNullCollection_AddRange()
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
        public void NonNullCollection_AddRange_Exception()
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
        public void NonNullCollection_Clear()
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void NonNullCollection_Add_Exception()
        {
            NonNullCollection<object> collection
                = new NonNullCollection<object>
                {
                    null
                };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NonNullCollection_Indexer()
        {
            NonNullCollection<object> collection
                = new NonNullCollection<object>
                {
                    new object()
                };
            collection[0] = null;
        }
    }
}
