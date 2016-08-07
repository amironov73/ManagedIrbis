using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests
{
    [TestClass]
    public class SequenceTest
    {
        [TestMethod]
        public void TestSequence_FromItems()
        {
            int[] array = Sequence.FromItem(1).ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(1,array[0]);

            array = Sequence.FromItems(1, 2).ToArray();
            Assert.AreEqual(2, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);

            array = Sequence.FromItems(1, 2, 3).ToArray();
            Assert.AreEqual(3, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);

            array = Sequence.FromItems(1, 2, 3, 4).ToArray();
            Assert.AreEqual(4, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
            Assert.AreEqual(4, array[3]);
        }

        [TestMethod]
        public void TestSequence_Repeat()
        {
            int[] array = Sequence.Repeat(5, 3).ToArray();
            Assert.AreEqual(3,array.Length);

            array = Sequence.Repeat
                (
                    array.AsEnumerable(),
                    3
                ).ToArray();
            Assert.AreEqual(9, array.Length);
        }

        [TestMethod]
        public void TestSequence_Slice()
        {
            int[] array = { 1, 2, 3 };
            int[][] sliced = array.Slice(2).ToArray();

            Assert.AreEqual(2, sliced.Length);
        }
    }
}
