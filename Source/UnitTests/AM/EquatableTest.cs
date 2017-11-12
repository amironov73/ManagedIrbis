using System;

using AM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ExpressionIsAlwaysNull

namespace UnitTests.AM
{
    [TestClass]
    public class EquatableTest
    {
        class MyClass
            : Equatable<MyClass>
        {
            public MyClass(int value)
            {
                Value = value;
            }

            public readonly int Value;

            public override int GetHashCode()
            {
                return Value;
            }
        }

        [TestMethod]
        public void Equatable_Equals_1()
        {
            MyClass first = new MyClass(1);
            MyClass second = new MyClass(1);
            Assert.IsTrue(first.Equals(second));

            second = new MyClass(2);
            Assert.IsFalse(first.Equals(second));

            second = null;
            Assert.IsFalse(first.Equals(second));

            second = first;
            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void Equatable_Equals_2()
        {
            MyClass first = new MyClass(1);
            object second = new MyClass(1);
            Assert.IsTrue(first.Equals(second));

            second = new MyClass(2);
            Assert.IsFalse(first.Equals(second));

            second = null;
            Assert.IsFalse(first.Equals(second));

            second = first;
            Assert.IsTrue(first.Equals(second));

            second = "Some text";
            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void Equatable_Equals_3()
        {
            MyClass first = new MyClass(1);
            MyClass second = new MyClass(1);
            Assert.IsTrue(first == second);

            second = new MyClass(2);
            Assert.IsFalse(first == second);

            second = null;
            Assert.IsFalse(first == second);

            second = first;
            Assert.IsTrue(first == second);
        }

        [TestMethod]
        public void Equatable_Equals_4()
        {
            MyClass first = new MyClass(1);
            MyClass second = new MyClass(1);
            Assert.IsFalse(first != second);

            second = new MyClass(2);
            Assert.IsTrue(first != second);

            second = null;
            Assert.IsTrue(first != second);

            second = first;
            Assert.IsFalse(first != second);
        }
    }
}
