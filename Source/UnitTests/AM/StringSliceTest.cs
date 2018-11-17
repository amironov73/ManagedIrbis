using System;

using AM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM
{
    [TestClass]
    public class StringSliceTest
    {
        [TestMethod]
        public void StringSlice_Constructor_1()
        {
            StringSlice slice = new StringSlice();
            Assert.IsNull(slice.Text);
            // ReSharper disable HeuristicUnreachableCode
            Assert.AreEqual(0, slice.Offset);
            Assert.AreEqual(0, slice.Length);
            Assert.IsTrue(slice.IsEmpty);
            // ReSharper restore HeuristicUnreachableCode
        }

        [TestMethod]
        public void StringSlice_Constructor_2()
        {
            string original = "One two three";
            int offset = 4, length = 3;
            StringSlice slice = new StringSlice(original, offset, length);
            Assert.AreSame(original, slice.Text);
            Assert.AreEqual(offset, slice.Offset);
            Assert.AreEqual(length, slice.Length);
            Assert.IsFalse(slice.IsEmpty);
            Assert.AreEqual("two", slice.ToString());
        }

        [TestMethod]
        public void StringSlice_Constructor_3()
        {
            string original = "One two three";
            int offset = 4, length = 0;
            StringSlice slice = new StringSlice(original, offset, length);
            Assert.AreSame(original, slice.Text);
            Assert.AreEqual(offset, slice.Offset);
            Assert.AreEqual(length, slice.Length);
            Assert.IsTrue(slice.IsEmpty);
            Assert.AreEqual(string.Empty, slice.ToString());
        }

        [TestMethod]
        public void StringSlice_OperatorEquals_1()
        {
            Assert.IsTrue(new StringSlice("One two three", 4, 3) == "two");
            Assert.IsFalse(new StringSlice("One two three", 4, 3) == "One");
            Assert.IsFalse(new StringSlice("One two three", 4, 3) == null);
        }

        [TestMethod]
        public void StringSlice_OperatorEquals_2()
        {
            Assert.IsTrue(new StringSlice("One two three", 4, 0) == string.Empty);
            Assert.IsFalse(new StringSlice("One two three", 4, 0) == "two");
            Assert.IsFalse(new StringSlice("One two three", 4, 0) == null);
        }

        [TestMethod]
        public void StringSlice_OperatorNotEquals_1()
        {
            Assert.IsFalse(new StringSlice("One two three", 4, 3) != "two");
            Assert.IsTrue(new StringSlice("One two three", 4, 3) != "One");
            Assert.IsTrue(new StringSlice("One two three", 4, 3) != null);
        }

        [TestMethod]
        public void StringSlice_OperatorNotEquals_2()
        {
            Assert.IsFalse(new StringSlice("One two three", 4, 0) != string.Empty);
            Assert.IsTrue(new StringSlice("One two three", 4, 0) != "two");
            Assert.IsTrue(new StringSlice("One two three", 4, 0) != null);
        }

        [TestMethod]
        public void StringSlice_Equals_1()
        {
            Assert.IsFalse(new StringSlice("One two three", 4, 3).Equals(null));
            StringSlice left = new StringSlice("One two three", 4, 3);
            object right = new StringSlice("two", 0, 3);
            Assert.IsTrue(left.Equals(right));
            right = new StringSlice("two", 0, 2);
            Assert.IsFalse(left.Equals(right));
            right = new StringSlice("tow", 0, 3);
            Assert.IsFalse(left.Equals(right));
        }

        [TestMethod]
        public void StringSlice_GetHashCode_1()
        {
            Assert.AreEqual(18250392, new StringSlice("One two three", 4, 3).GetHashCode());
        }

        [TestMethod]
        public void StringSlice_ToString_1()
        {
            Assert.AreEqual("two", new StringSlice("One two three", 4, 3).ToString());
        }

        [TestMethod]
        public void StringUtility_Slice_1()
        {
            Assert.AreEqual("two", "One two three".Slice(4, 3).ToString());
        }

        [TestMethod]
        public void StringUtility_Slice_3()
        {
            Assert.AreEqual("two", "One two".Slice(4).ToString());
        }
    }
}
