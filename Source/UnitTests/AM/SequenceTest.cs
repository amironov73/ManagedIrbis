using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class SequenceTest
    {
        [TestMethod]
        public void Sequence_FirstOr()
        {
            int[] array = {1, 2, 3};
            int result = array.FirstOr(100);
            Assert.AreEqual(1, result);

            array=new int[0];
            result = array.FirstOr(100);
            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void Sequence_FromItems()
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
        public void Sequence_NonEmptyLines()
        {
            string[] array = { "Hello", "", "World", null, "!" };
            string[] result = array.NonEmptyLines().ToArray();
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual("Hello", result[0]);
            Assert.AreEqual("World", result[1]);
            Assert.AreEqual("!", result[2]);
        }

        [TestMethod]
        public void Sequence_NonNullItems()
        {
            string[] array = {"Hello", "", "World", null, "!"};
            string[] result = array.NonNullItems().ToArray();
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual("Hello", result[0]);
            Assert.AreEqual("", result[1]);
            Assert.AreEqual("World", result[2]);
            Assert.AreEqual("!", result[3]);
        }

        [TestMethod]
        public void Sequence_Repeat()
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
        public void Sequence_Replace1()
        {
            int[] array = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            int[] result = array.Replace(3, 33).ToArray();
            Assert.AreEqual(10, result.Length);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(33, result[2]);
            Assert.AreEqual(4, result[3]);
            Assert.AreEqual(5, result[4]);
            Assert.AreEqual(6, result[5]);
            Assert.AreEqual(7, result[6]);
            Assert.AreEqual(8, result[7]);
            Assert.AreEqual(9, result[8]);
            Assert.AreEqual(10, result[9]);
        }

        [TestMethod]
        public void Sequence_Replace2()
        {
            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] result = array.Replace(33, 333).ToArray();
            Assert.AreEqual(10, result.Length);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(3, result[2]);
            Assert.AreEqual(4, result[3]);
            Assert.AreEqual(5, result[4]);
            Assert.AreEqual(6, result[5]);
            Assert.AreEqual(7, result[6]);
            Assert.AreEqual(8, result[7]);
            Assert.AreEqual(9, result[8]);
            Assert.AreEqual(10, result[9]);
        }

        [TestMethod]
        public void Sequence_Replace3()
        {
            int[] array = new int[0];
            int[] result = array.Replace(33, 333).ToArray();
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void Sequence_Segment1()
        {
            int[] array = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            int[] segment = array.Segment(3, 3).ToArray();
            Assert.AreEqual(3, segment.Length);
            Assert.AreEqual(4, segment[0]);
            Assert.AreEqual(5, segment[1]);
            Assert.AreEqual(6, segment[2]);
        }

        [TestMethod]
        public void Sequence_Segment2()
        {
            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] segment = array.Segment(3, 0).ToArray();
            Assert.AreEqual(0, segment.Length);
        }

        [TestMethod]
        public void Sequence_Segment3()
        {
            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] segment = array.Segment(11, 3).ToArray();
            Assert.AreEqual(0, segment.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Sequence_Segment_Exception1()
        {
            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            array.Segment(-1, 3).ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Sequence_Segment_Exception2()
        {
            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            array.Segment(3, -1).ToArray();
        }

        [TestMethod]
        public void Sequence_Separate()
        {
            int[] array = {1, 2, 3};
            int[] separated = array.Separate(0)
                .Cast<int>()
                .ToArray();

            Assert.AreEqual(5, separated.Length);
            Assert.AreEqual(1, separated[0]);
            Assert.AreEqual(0, separated[1]);
            Assert.AreEqual(2, separated[2]);
            Assert.AreEqual(0, separated[3]);
            Assert.AreEqual(3, separated[4]);
        }

        [TestMethod]
        public void Sequence_Slice()
        {
            int[] array = { 1, 2, 3 };
            int[][] sliced = array.Slice(2).ToArray();
            Assert.AreEqual(2, sliced.Length);

            array = new int[0];
            sliced = array.Slice(2).ToArray();
            Assert.AreEqual(0, sliced.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Sequence_Slice_Exception()
        {
            int[] array = { 1, 2, 3 };
            array.Slice(-2).ToArray();
        }

        [TestMethod]
        public void Sequence_Tee1()
        {
            int counter = 0;
            int[] array = {1, 2, 3};
            int[] result = array.Tee(item => counter++).ToArray();
            Assert.AreEqual(array.Length, result.Length);
            Assert.AreEqual(array.Length, counter);
            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(array[i], result[i]);
            }
        }

        [TestMethod]
        public void Sequence_Tee2()
        {
            int[] array = { 1, 2, 3 };
            int[] buffer=new int[array.Length];
            int[] result = array.Tee((index, item) => buffer[index]=item).ToArray();
            Assert.AreEqual(array.Length, result.Length);
            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(array[i], result[i]);
                Assert.AreEqual(array[i], buffer[i]);
            }
        }
    }
}
