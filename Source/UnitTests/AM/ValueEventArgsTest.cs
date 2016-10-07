using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class ValueEventArgsTest
    {
        [TestMethod]
        public void ValueEventArgs1_Construction ()
        {
            ValueEventArgs<int> eventArgs
                = new ValueEventArgs<int>(1);

            Assert.AreEqual(1, eventArgs.Value);
        }

        [TestMethod]
        public void ValueEventArgs2_Construction()
        {
            ValueEventArgs<int, int> eventArgs
                = new ValueEventArgs<int, int>(1, 2);

            Assert.AreEqual(1, eventArgs.Value1);
            Assert.AreEqual(2, eventArgs.Value2);
        }

        [TestMethod]
        public void ValueEventArgs1_Empty()
        {
            ValueEventArgs<int> empty
                = ValueEventArgs<int>.Empty;

            Assert.IsNotNull(empty);
            Assert.AreEqual(0, empty.Value);
        }

        [TestMethod]
        public void ValueEventArgs2_Empty()
        {
            ValueEventArgs<int, int> empty
                = ValueEventArgs<int, int>.Empty;

            Assert.IsNotNull(empty);
            Assert.AreEqual(0, empty.Value1);
            Assert.AreEqual(0, empty.Value2);
        }

        [TestMethod]
        public void ValueEventArgs1_ToString()
        {
            ValueEventArgs<string> eventArgs
                = new ValueEventArgs<string>("1");
            Assert.AreEqual("1", eventArgs.ToString());

            eventArgs = ValueEventArgs<string>.Empty;
            Assert.AreEqual("(null)", eventArgs.ToString());
        }

        [TestMethod]
        public void ValueEventArgs2_ToString()
        {
            ValueEventArgs<string, string> eventArgs
                = new ValueEventArgs<string, string>("1", "2");
            Assert.AreEqual("1 2", eventArgs.ToString());

            eventArgs = ValueEventArgs<string, string>.Empty;
            Assert.AreEqual("(null) (null)", eventArgs.ToString());
        }
    }
}
