using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class NonNullValueTest
    {
        [TestMethod]
        public void TestNonNullValueConstructor()
        {
            const string expected = "abc";

            NonNullValue<string> value
                = new NonNullValue<string>(expected);
            Assert.AreEqual(expected, value.Value);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void TestNonNullValueException()
        {
            NonNullValue<string> value = new NonNullValue<string>("a");
            value.SetValue(null);
            Assert.IsNotNull(value.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNonNullValueAssignment1()
        {
            NonNullValue<string> value = new NonNullValue<string>("a");
            string text = null;
            value = text;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNonNullValueAssignment2()
        {
            NonNullValue<string> value = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNonNullValueDefault()
        {
            NonNullValue<string> value = new NonNullValue<string>();
            Assert.IsNotNull(value.Value);
        }
    }
}
