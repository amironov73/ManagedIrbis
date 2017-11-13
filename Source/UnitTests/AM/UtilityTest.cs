using System;
using System.Collections.Generic;

using AM;
using AM.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable InvokeAsExtensionMethod

namespace UnitTests.AM
{
    [TestClass]
    public class UtilityTest
    {
        class MyClass1
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        class MyClass2
        {
            public string X { get; set; }
            public string Y { get; set; }
        }

        class MyClass3
        {
            public int X, Y;
        }

        class MyClass4
        {
            public string X, Y;
        }

        class MyClass5
        {
            public int X;

            bool Equals(MyClass5 other)
            {
                return X == other.X;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((MyClass5)obj);
            }

            public override int GetHashCode()
            {
                return X;
            }
        }

        class MyClass6
        {
            public MyClass5 X;
        }

        class MyClass7
        {
            public int[] X;
        }

        class MyClass8
        {
            public MyClass7 X;
        }

        class MyClass9
        {
            public MyClass7 X { get; set; }
            public MyClass7 Y { get; set; }
        }

        [TestMethod]
        public void Utility_DumpBytes_1()
        {
            Assert.AreEqual
                (
                    string.Empty,
                    Utility.DumpBytes(new byte[0])
                        .DosToUnix()
                );

            Assert.AreEqual
                (
                    "000000: 01\n",
                    Utility.DumpBytes(new byte[] { 0x01 }).DosToUnix()
                );

            Assert.AreEqual
                (
                    "000000: 01 02 03 04 05 06 07 08 09 0A 0B 0B 0C 0D 0E 0F\n000010: 10 11 12\n",
                    Utility.DumpBytes(new byte[] { 0x01, 0x02, 0x03,
                        0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B,
                        0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12
                        })
                        .DosToUnix()
                );
        }

        [TestMethod]
        public void Utility_EnumerableEquals_1()
        {
            byte[] first = { 1, 2, 3 };
            byte[] second = { 1, 2, 3 };
            byte[] third = { 1, 2 };

            Assert.IsTrue(Utility.EnumerableEquals(first, second));
            Assert.IsTrue(Utility.EnumerableEquals(first, first));
            Assert.IsFalse(Utility.EnumerableEquals(first, third));
            Assert.IsFalse(Utility.EnumerableEquals(first, null));
            Assert.IsFalse(Utility.EnumerableEquals(null, null));
        }

        [TestMethod]
        public void Utility_GetItem_1()
        {
            byte[] array = { 1, 2, 3 };

            Assert.AreEqual(1, array.GetItem(0));
            Assert.AreEqual(2, array.GetItem(1));
            Assert.AreEqual(3, array.GetItem(2));
            Assert.AreEqual(0, array.GetItem(3));
            Assert.AreEqual(3, array.GetItem(-1));
        }

        [TestMethod]
        public void Utility_GetItem_2()
        {
            List<byte> list = new List<byte> { 1, 2, 3 };

            Assert.AreEqual(1, list.GetItem(0));
            Assert.AreEqual(2, list.GetItem(1));
            Assert.AreEqual(3, list.GetItem(2));
            Assert.AreEqual(0, list.GetItem(3));
            Assert.AreEqual(3, list.GetItem(-1));
        }

        [TestMethod]
        public void Utility_IsOneOf_1()
        {
            Assert.IsTrue(Utility.IsOneOf(1, 1, 2, 3));
            Assert.IsTrue(Utility.IsOneOf(2, 1, 2, 3));
            Assert.IsTrue(Utility.IsOneOf(3, 1, 2, 3));
            Assert.IsFalse(Utility.IsOneOf(0, 1, 2, 3));
        }

        //[TestMethod]
        //public void Utility_NotDefault_1()
        //{
        //    Assert.IsFalse(1.NotDefault());
        //    Assert.IsTrue(0.NotDefault());
        //}

        [TestMethod]
        public void Utility_NullableToString_1()
        {
            Assert.AreEqual("System.Object", new object().NullableToString());
            Assert.AreEqual(null, ((object)null).NullableToString());
        }

        [TestMethod]
        public void Utility_ThrowIfNull_1()
        {
            string text = "Hello";
            Assert.AreSame(text, Utility.ThrowIfNull(text));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Utility_ThrowIfNull_2()
        {
            string text = null;
            Assert.AreSame(text, Utility.ThrowIfNull(text));
        }

        [TestMethod]
        public void Utility_ThrowIfNull_3()
        {
            string text = "Hello";
            Assert.AreSame(text, Utility.ThrowIfNull<string, ArgumentNullException>(text));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Utility_ThrowIfNull_4()
        {
            string text = null;
            Assert.AreSame(text, Utility.ThrowIfNull<string, ArgumentNullException>(text));
        }

        [TestMethod]
        public void Utility_ThrowIfNull_5()
        {
            string text = "Hello";
            Assert.AreSame(text, Utility.ThrowIfNull(text, "text=null"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Utility_ThrowIfNull_6()
        {
            string text = null;
            Assert.AreSame(text, Utility.ThrowIfNull(text, "text=null"));
        }


        [TestMethod]
        public void Utility_ToVisibleString_1()
        {
            string text = "Hello";
            Assert.AreEqual("Hello", text.ToVisibleString());

            text = null;
            Assert.AreEqual("(null)", Utility.ToVisibleString(text));
        }

        [TestMethod]
        public void Utility_PropertyEquals_1()
        {
            object first = new MyClass1 { X = 1, Y = 2 };
            object second = new MyClass1 { X = 1, Y = 2 };
            Assert.IsTrue(Utility.PropertyEquals(first, second));
            Assert.IsTrue(Utility.PropertyEquals(second, first));
            Assert.IsTrue(Utility.PropertyEquals(first, first));
            Assert.IsTrue(Utility.PropertyEquals(second, second));

            second = new MyClass1 { X = 2, Y = 1 };
            Assert.IsFalse(Utility.PropertyEquals(first, second));
            Assert.IsFalse(Utility.PropertyEquals(second, first));

            second = null;
            Assert.IsFalse(Utility.PropertyEquals(first, second));
            Assert.IsFalse(Utility.PropertyEquals(second, first));

            second = "Hello";
            Assert.IsFalse(Utility.PropertyEquals(first, second));
            Assert.IsFalse(Utility.PropertyEquals(second, first));
        }

        [TestMethod]
        public void Utility_PropertyEquals_2()
        {
            object first = new MyClass2 { X = "X", Y = "Y" };
            object second = new MyClass2 { X = "X", Y = "Y" };
            Assert.IsTrue(Utility.PropertyEquals(first, second));
            Assert.IsTrue(Utility.PropertyEquals(second, first));
            Assert.IsTrue(Utility.PropertyEquals(first, first));
            Assert.IsTrue(Utility.PropertyEquals(second, second));

            second = new MyClass1 { X = 1, Y = 2 };
            Assert.IsFalse(Utility.PropertyEquals(first, second));
            Assert.IsFalse(Utility.PropertyEquals(second, first));

            second = new MyClass2();
            Assert.IsFalse(Utility.PropertyEquals(first, second));
            Assert.IsFalse(Utility.PropertyEquals(second, first));
        }

        [TestMethod]
        public void Utility_MemberwiseEquals_1()
        {
            object first = new MyClass3 { X = 1, Y = 2 };
            object second = new MyClass3 { X = 1, Y = 2 };
            Assert.IsTrue(Utility.MemberwiseEquals(first, second));
            Assert.IsTrue(Utility.MemberwiseEquals(second, first));
            Assert.IsTrue(Utility.MemberwiseEquals(first, first));
            Assert.IsTrue(Utility.MemberwiseEquals(second, second));

            second = new MyClass3 { X = 2, Y = 1 };
            Assert.IsFalse(Utility.MemberwiseEquals(first, second));
            Assert.IsFalse(Utility.MemberwiseEquals(second, first));

            second = null;
            Assert.IsFalse(Utility.MemberwiseEquals(first, second));
            Assert.IsFalse(Utility.MemberwiseEquals(second, first));

            second = "Hello";
            Assert.IsFalse(Utility.MemberwiseEquals(first, second));
            Assert.IsFalse(Utility.MemberwiseEquals(second, first));
        }

        [TestMethod]
        public void Utility_MemberwiseEquals_2()
        {
            object first = new MyClass4 { X = "X", Y = "Y" };
            object second = new MyClass4 { X = "X", Y = "Y" };
            Assert.IsTrue(Utility.MemberwiseEquals(first, second));
            Assert.IsTrue(Utility.MemberwiseEquals(second, first));
            Assert.IsTrue(Utility.MemberwiseEquals(first, first));
            Assert.IsTrue(Utility.MemberwiseEquals(second, second));

            second = new MyClass3 { X = 1, Y = 2 };
            Assert.IsFalse(Utility.MemberwiseEquals(first, second));
            Assert.IsFalse(Utility.MemberwiseEquals(second, first));

            second = new MyClass4();
            Assert.IsFalse(Utility.MemberwiseEquals(first, second));
            Assert.IsFalse(Utility.MemberwiseEquals(second, first));
        }

        [TestMethod]
        public void Utility_MemberwiseEquals_3()
        {
            object first = new MyClass6 { X = new MyClass5 { X = 1 } };
            object second = new MyClass6 { X = new MyClass5 { X = 1 } };
            Assert.IsTrue(Utility.MemberwiseEquals(first, second));
            Assert.IsTrue(Utility.MemberwiseEquals(second, first));
            Assert.IsTrue(Utility.MemberwiseEquals(first, first));
            Assert.IsTrue(Utility.MemberwiseEquals(second, second));

            second = new MyClass6 { X = new MyClass5 { X = 2 } };
            Assert.IsFalse(Utility.MemberwiseEquals(first, second));
            Assert.IsFalse(Utility.MemberwiseEquals(second, first));
        }

        [TestMethod]
        public void Utility_MemberwiseEquals_4()
        {
            object first = new MyClass8 { X = new MyClass7 { X = new[] { 1, 2 } } };
            object second = new MyClass8 { X = new MyClass7 { X = new[] { 1, 2 } } };
            Assert.IsTrue(Utility.MemberwiseEquals(first, second));
            Assert.IsTrue(Utility.MemberwiseEquals(second, first));
            Assert.IsTrue(Utility.MemberwiseEquals(first, first));
            Assert.IsTrue(Utility.MemberwiseEquals(second, second));

            second = new MyClass8 { X = new MyClass7 { X = new[] { 3, 4 } } };
            Assert.IsFalse(Utility.MemberwiseEquals(first, second));
            Assert.IsFalse(Utility.MemberwiseEquals(second, first));
        }

        [TestMethod]
        public void Utility_MemberwiseEquals_5()
        {
            object first = new MyClass9
            {
                X = new MyClass7 { X = new[] { 1, 2 } },
                Y = new MyClass7 { X = new[] { 3, 4 } }
            };
            object second = new MyClass9
            {
                X = new MyClass7 { X = new[] { 1, 2 } },
                Y = new MyClass7 { X = new[] { 3, 4 } }
            };
            Assert.IsTrue(Utility.MemberwiseEquals(first, second));
            Assert.IsTrue(Utility.MemberwiseEquals(second, first));
            Assert.IsTrue(Utility.MemberwiseEquals(first, first));
            Assert.IsTrue(Utility.MemberwiseEquals(second, second));

            second = new MyClass9
            {
                X = new MyClass7 { X = new[] { 3, 4 } },
                Y = new MyClass7 { X = new[] { 5, 6 } }
            };
            Assert.IsFalse(Utility.MemberwiseEquals(first, second));
            Assert.IsFalse(Utility.MemberwiseEquals(second, first));
        }

        [TestMethod]
        public void Utility_GetHashCodeAggregate_1()
        {
            int[] array = {1, 2, 3};
            Assert.AreEqual(4678213, array.GetHashCodeAggregate(157));
        }

        [TestMethod]
        public void Utility_GetHashCodeAggregate_2()
        {
            string[] array = {"Happy", "New", "Year"};
            Assert.AreEqual(1841205845, array.GetHashCodeAggregate(157));
        }

        [TestMethod]
        public void Utility_GetHashCodeAggregate_3()
        {
            string[] array = {"Happy", null, "Year"};
            Assert.AreEqual(687480778, array.GetHashCodeAggregate(157));
        }

        [TestMethod]
        public void Utility_GetHashCodeAggregate_4()
        {
            int[] array = {1, 2, 3};
            Assert.AreEqual(507473, array.GetHashCodeAggregate());
        }

        [TestMethod]
        public void Utility_GetHashCodeAggregate_5()
        {
            string[] array = {"Happy", "New", "Year"};
            Assert.AreEqual(1837035105, array.GetHashCodeAggregate());
        }

        [TestMethod]
        public void Utility_GetHashCodeAggregate_6()
        {
            string[] array = {"Happy", null, "Year"};
            Assert.AreEqual(687346238, array.GetHashCodeAggregate());
        }
    }
}
