using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class OptionalTest
    {
        [TestMethod]
        public void Optional_Construction_1()
        {
            Optional<object> optional = new Optional<object>();

            Assert.IsFalse(optional.HasValue);
        }

        [TestMethod]
        public void Optional_Construction_2()
        {
            Optional<string> optional = new Optional<string>("Hello");

            Assert.IsTrue(optional.HasValue);
            Assert.AreEqual("Hello", optional.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Optional_Exception_1()
        {
            Optional<object> optional = new Optional<object>();
            object value = optional.Value;
        }

        [TestMethod]
        public void Optional_Operators_1()
        {
            Optional<string> optional = "Hello";
            Assert.IsTrue(optional.HasValue);
            Assert.AreEqual("Hello", optional.Value);

            string hello = (string) optional;
            Assert.AreEqual("Hello", hello);
        }

        [TestMethod]
        public void Optional_Equals_1()
        {
            Optional<int> first = 1;
            Optional<int> second = 1;
            Assert.IsTrue(first.Equals(second));

            Optional<int> third = 2;
            Assert.IsFalse(first.Equals(third));

            Optional<int> fourth = new Optional<int>();
            Assert.IsFalse(first.Equals(fourth));
        }

        [TestMethod]
        public void Optional_Equals_2()
        {
            Optional<int> first = 1;
            object second = new Optional<int>(1);
            Assert.IsTrue(first.Equals(second));

            object third = new Optional<int>(2);
            Assert.IsFalse(first.Equals(third));

            object fourth = new Optional<int>();
            Assert.IsFalse(first.Equals(fourth));

            object fifth = new Optional<string>("Hello");
            Assert.IsFalse(first.Equals(fifth));
        }

        [TestMethod]
        public void Optional_GetHashCode_1()
        {
            Optional<int> first = new Optional<int>();
            Assert.AreEqual(0, first.GetHashCode());

            Optional<string> second = new Optional<string>(null);
            Assert.AreEqual(-1, second.GetHashCode());

            Optional<int> third = 1;
            Assert.AreEqual(1, third.GetHashCode());
        }

        [TestMethod]
        public void Optional_ToString_1()
        {
            Optional<int> first = 1;
            Assert.AreEqual("1", first.ToString());

            Optional<int> second = new Optional<int>();
            Assert.AreEqual("(not set)", second.ToString());

            Optional<string> third = new Optional<string>(null);
            Assert.AreEqual("(null)", third.ToString());
        }
    }
}
