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

        class FieldsOnlyClass
            : ComparableObject
        {
            // There are no properties, fields only

            public int Id;
        }

        class EmptyClass
            : ComparableObject
        {
            // No public properties, no public fields
        }

        [TestMethod]
        public void TestComparableObject_Equals1()
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
        public void TestComparableObject_Equals2()
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
        public void TestComparableObject_Equals3()
        {
            MyClass first = new MyClass
            {
                Id = 1,
                Title = null
            };
            MyClass second = null;
            Assert.IsFalse(first.Equals(second));

            Assert.IsFalse(first.Equals("Hello"));
        }

        [TestMethod]
        public void TestComparableObject_Equals4()
        {
            FieldsOnlyClass first = new FieldsOnlyClass
            {
                Id = 1
            };
            FieldsOnlyClass second = new FieldsOnlyClass
            {
                Id = 1
            };
            Assert.IsTrue(first.Equals(second));

            second.Id = 2;
            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void TestComparableObject_Equals5()
        {
            EmptyClass first = new EmptyClass();
            EmptyClass second = new EmptyClass();
            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void TestComparableObject_GetHashCode1()
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
        public void TestComparableObject_GetHashCode2()
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

        [TestMethod]
        public void TestComparableObject_GetHashCode3()
        {
            FieldsOnlyClass first = new FieldsOnlyClass
            {
                Id = 1
            };
            FieldsOnlyClass second = new FieldsOnlyClass
            {
                Id = 2
            };
            Assert.AreNotEqual(first.GetHashCode(), second.GetHashCode());

            second.Id = 1;
            Assert.AreEqual(first.GetHashCode(), second.GetHashCode());
        }

        [TestMethod]
        public void TestComparableObject_GetHashCode4()
        {
            EmptyClass first = new EmptyClass();
            EmptyClass second = new EmptyClass();

            Assert.AreEqual(first.GetHashCode(), second.GetHashCode());
        }
    }
}
