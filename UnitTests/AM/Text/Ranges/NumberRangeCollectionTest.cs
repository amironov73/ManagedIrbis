using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Ranges;

namespace UnitTests.AM.Text.Ranges
{
    [TestClass]
    public class NumberRangeCollectionTest
    {
        [TestMethod]
        public void TestNumberRangeCollection_Parse1()
        {
            NumberRangeCollection collection
                = NumberRangeCollection.Parse("10-15");
            Assert.AreEqual(1, collection.Count);

            collection = NumberRangeCollection.Parse("10;15");
            Assert.AreEqual(2, collection.Count);

            collection = NumberRangeCollection.Parse("10;15-20;30");
            Assert.AreEqual(3, collection.Count);

            collection = NumberRangeCollection.Parse("10; ;15");
            Assert.AreEqual(2, collection.Count);

            collection = NumberRangeCollection.Parse("10   15-20,;30");
            Assert.AreEqual(3, collection.Count);
        }

        [TestMethod]
        public void TestNumberRangeCollection_Parse2()
        {
            NumberRangeCollection collection
                = NumberRangeCollection.Parse("10 - 15");
            Assert.AreEqual(1, collection.Count);

            collection = NumberRangeCollection.Parse("10 ;15");
            Assert.AreEqual(2, collection.Count);

            collection = NumberRangeCollection.Parse("10;15 - 20;30");
            Assert.AreEqual(3, collection.Count);

            collection = NumberRangeCollection.Parse("10; ;15");
            Assert.AreEqual(2, collection.Count);

            collection = NumberRangeCollection.Parse("10   15 - 20,;30");
            Assert.AreEqual(3, collection.Count);
        }

        [TestMethod]
        public void TestNumberRangeCollection_Parse3()
        {
            NumberRangeCollection collection
                = NumberRangeCollection.Parse("10 15");
            Assert.AreEqual(2, collection.Count);

            collection = NumberRangeCollection.Parse("10 - 15 20");
            Assert.AreEqual(2, collection.Count);

            collection = NumberRangeCollection.Parse("10 15 - 20 30");
            Assert.AreEqual(3, collection.Count);

            collection = NumberRangeCollection.Parse("10 15 30");
            Assert.AreEqual(3, collection.Count);

            collection = NumberRangeCollection.Parse("10   15 - 20 30-40");
            Assert.AreEqual(3, collection.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestNumberRangeCollection_Parse_Exception1()
        {
            NumberRangeCollection.Parse("-10");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestNumberRangeCollection_Parse_Exception2()
        {
            NumberRangeCollection.Parse("10-15;-");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestNumberRangeCollection_Parse_Exception3()
        {
            NumberRangeCollection.Parse(";;-,");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNumberRangeCollection_Parse_Exception4()
        {
            NumberRangeCollection.Parse(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNumberRangeCollection_Parse_Exception5()
        {
            NumberRangeCollection.Parse(null);
        }
    }
}
