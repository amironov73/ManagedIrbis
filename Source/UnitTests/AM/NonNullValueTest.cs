using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class NonNullValueTest
    {
        [TestMethod]
        public void NonNullValue_Construction()
        {
            const string expected = "abc";

            NonNullValue<string> value
                = new NonNullValue<string>(expected);
            Assert.AreEqual(expected, value.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NonNullValue_Construction_Exception()
        {
            NonNullValue<string> value = new NonNullValue<string>("a");
            value.SetValue(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NonNullValue_Assignment()
        {
            NonNullValue<string> value = new NonNullValue<string>("a");
            string text = null;
            value = text;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NonNullValue_Assignment_Exception()
        {
            NonNullValue<string> value = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NonNullValue_Default_Exception()
        {
            NonNullValue<string> value = new NonNullValue<string>();
            Assert.IsNotNull(value.Value);
        }

        [TestMethod]
        public void NonNullValue_SetValue()
        {
            const string expected = "World";
            NonNullValue<string> value = "Hello";
            value.SetValue(expected);
            Assert.AreEqual(expected, value.Value);
        }

        [TestMethod]
        public void NonNullValue_ToString()
        {
            string expected = "Hello";
            NonNullValue<string> value = expected;
            Assert.AreEqual(expected, value.ToString());
        }

        [TestMethod]
        public void NonNullValue_Value()
        {
            const string expected = "World";
            NonNullValue<string> value = "Hello";
            value.Value = expected;
            Assert.AreEqual(expected, value.Value);
        }
    }
}
