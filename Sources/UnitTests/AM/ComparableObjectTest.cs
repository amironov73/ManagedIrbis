using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class ComparableObjectTest
    {
        class MyClass
            : ComparableObject
        {
            public int Id { get; set; }

            public string Title { get; set; }
        }

        [TestMethod]
        public void TestComparableObject_Equals()
        {
            MyClass first = new MyClass
            {
                Id = 1,
                Title = "one"
            };
            MyClass second = new MyClass
            {
                Id = 1,
                Title = "one"
            };
            Assert.IsTrue(first.Equals(second));

            MyClass third = new MyClass
            {
                Id = 1,
                Title = "two"
            };
            Assert.IsFalse(first.Equals(third));

            MyClass fourth = new MyClass
            {
                Id = 2,
                Title = "two"
            };
            Assert.IsFalse(third.Equals(fourth));

            third.Id = 2;
            Assert.IsTrue(third.Equals(fourth));
        }

        [TestMethod]
        public void TestComparableObject_Equals_WithNull()
        {
            MyClass first = new MyClass
            {
                Id = 1,
                Title = null
            };
            MyClass second = new MyClass
            {
                Id = 1,
                Title = null
            };
            Assert.IsTrue(first.Equals(second));

            MyClass third = new MyClass
            {
                Id = 1,
                Title = "two"
            };
            Assert.IsFalse(first.Equals(third));
        }

        [TestMethod]
        public void TestComparableObject_GetHashCode()
        {
            MyClass first = new MyClass
            {
                Id = 1,
                Title = "one"
            };
            MyClass second = new MyClass
            {
                Id = 1,
                Title = "one"
            };
            Assert.AreEqual(first.GetHashCode(), second.GetHashCode());

            MyClass third = new MyClass
            {
                Id = 1,
                Title = "two"
            };
            Assert.AreNotEqual(first.GetHashCode(), third.GetHashCode());
        }

        [TestMethod]
        [Ignore]
        public void TestComparableObject_GetHashCode_WithNull()
        {
            MyClass first = new MyClass
            {
                Id = 1,
                Title = null
            };
            MyClass second = new MyClass
            {
                Id = 1,
                Title = null
            };
            Assert.AreEqual(first.GetHashCode(), second.GetHashCode());

            MyClass third = new MyClass
            {
                Id = 1,
                Title = "two"
            };
            Assert.AreNotEqual(first.GetHashCode(), third.GetHashCode());
        }
    }
}
