using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class OptionalTest
    {
        [TestMethod]
        public void TestOptional_DefaultConstructor()
        {
            Optional<object> optional = new Optional<object>();

            Assert.IsFalse(optional.HasValue);
        }

        [TestMethod]
        public void TestOptional_Constructor()
        {
            Optional<string> optional = new Optional<string>("Hello");

            Assert.IsTrue(optional.HasValue);
            Assert.AreEqual("Hello", optional.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestOptional_Exception()
        {
            Optional<object> optional = new Optional<object>();
            object value = optional.Value;
        }

        [TestMethod]
        public void TestOptional_Operators()
        {
            Optional<string> optional = "Hello";
            Assert.IsTrue(optional.HasValue);
            Assert.AreEqual("Hello", optional.Value);

            string hello = (string) optional;
            Assert.AreEqual("Hello", hello);
        }

        [TestMethod]
        public void TestOptional_Equals1()
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
        public void TestOptional_Equals2()
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
        public void TestOptional_GetHashCode()
        {
            Optional<int> first = new Optional<int>();
            Assert.AreEqual(0, first.GetHashCode());

            Optional<string> second = new Optional<string>(null);
            Assert.AreEqual(-1, second.GetHashCode());

            Optional<int> third = 1;
            Assert.AreEqual(1, third.GetHashCode());
        }
    }
}
