using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

// ReSharper disable InlineOutVariableDeclaration
// ReSharper disable InvokeAsExtensionMethod

namespace UnitTests.AM
{
    [TestClass]
    public class NumericUtilityTest
    {
        [TestMethod]
        public void NumericUtility_IsPositiveInteger_1()
        {
            Assert.IsTrue("1".IsPositiveInteger());
            Assert.IsFalse("-1".IsPositiveInteger());
            Assert.IsFalse("0".IsPositiveInteger());
            Assert.IsFalse("hello".IsPositiveInteger());
        }

        [TestMethod]
        public void NumericUtility_CompressRange_1()
        {
            int[] array = {1, 2, 3, 5, 6, 7};
            Assert.AreEqual("1-3, 5-7", NumericUtility.CompressRange(array));

            Assert.AreEqual(string.Empty, NumericUtility.CompressRange(null));
        }

        [TestMethod]
        public void NumericUtility_CompressRange_2()
        {
            int[] array = new int[0];
            Assert.AreEqual(string.Empty, NumericUtility.CompressRange(array));
        }

        [TestMethod]
        public void NumericUtility_FormatRange_1()
        {
            Assert.AreEqual("1-5", NumericUtility.FormatRange(1, 5));
            Assert.AreEqual("1, 2", NumericUtility.FormatRange(1, 2));
            Assert.AreEqual("1", NumericUtility.FormatRange(1, 1));
        }

        [TestMethod]
        public void NumericUtlity_OneOf_1()
        {
            IEnumerable<int> array = new List<int>{1, 2, 3};
            Assert.IsTrue(1.OneOf(array));
            Assert.IsFalse((-1).OneOf(array));
        }

        [TestMethod]
        public void NumericUtlity_OneOf_2()
        {
            int[] array = {1, 2, 3};
            Assert.IsTrue(1.OneOf(array));
            Assert.IsFalse((-1).OneOf(array));
        }

        [TestMethod]
        public void NumericUtility_ParseDecimal_1()
        {
            Assert.AreEqual(1.23m, NumericUtility.ParseDecimal("1.23"));
            Assert.AreEqual(1.23m, NumericUtility.ParseDecimal("1,23"));
            Assert.AreEqual(123.45m, NumericUtility.ParseDecimal("1,23.45"));
        }

        [TestMethod]
        public void NumericUtility_ParseDouble_1()
        {
            Assert.AreEqual(1.23, NumericUtility.ParseDouble("1.23"));
            Assert.AreEqual(1.23, NumericUtility.ParseDouble("1,23"));
            Assert.AreEqual(123.45, NumericUtility.ParseDouble("1,23.45"));
        }

        [TestMethod]
        public void NumericUtility_ParseInt16_1()
        {
            Assert.AreEqual(123, NumericUtility.ParseInt16("123"));
        }

        [TestMethod]
        public void NumericUtility_ParseInt32_1()
        {
            Assert.AreEqual(123, NumericUtility.ParseInt32("123"));
            Assert.AreEqual(123, NumericUtility.ParseInt32("12,3"));
            Assert.AreEqual(123, NumericUtility.ParseInt32("123,"));
        }

        [TestMethod]
        public void NumericUtility_ParseInt64_1()
        {
            Assert.AreEqual(123, NumericUtility.ParseInt64("123"));
        }

        [TestMethod]
        public void NumericUtility_SafeToDecimal_1()
        {
            Assert.AreEqual(1.23m, NumericUtility.SafeToDecimal("hello", 1.23m));
            Assert.AreEqual(1.23m, NumericUtility.SafeToDecimal(null, 1.23m));
            Assert.AreEqual(1.23m, NumericUtility.SafeToDecimal("hello", 1.23m));
            Assert.AreEqual(1.32m, NumericUtility.SafeToDecimal("1.32", 1.23m));
        }

        [TestMethod]
        public void NumericUtility_SafeToDouble_1()
        {
            Assert.AreEqual(1.23, NumericUtility.SafeToDouble("hello", 1.23));
            Assert.AreEqual(1.23, NumericUtility.SafeToDouble(null, 1.23));
            Assert.AreEqual(1.23, NumericUtility.SafeToDouble("hello", 1.23));
            Assert.AreEqual(1.32, NumericUtility.SafeToDouble("1.32", 1.23));
        }

        [TestMethod]
        public void NumericUtility_SafeToInt32_1()
        {
            Assert.AreEqual(123, NumericUtility.SafeToInt32("hello", 123, 100, 200));
            Assert.AreEqual(123, NumericUtility.SafeToInt32(null, 123, 100, 200));
            Assert.AreEqual(132, NumericUtility.SafeToInt32("132", 123, 100, 200));
            Assert.AreEqual(123, NumericUtility.SafeToInt32("32", 123, 100, 200));
        }

        [TestMethod]
        public void NumericUtility_SafeToInt32_2()
        {
            Assert.AreEqual(123, NumericUtility.SafeToInt32("hello", 123));
            Assert.AreEqual(123, NumericUtility.SafeToInt32(null, 123));
            Assert.AreEqual(132, NumericUtility.SafeToInt32("132", 123));
            Assert.AreEqual(32, NumericUtility.SafeToInt32("32", 123));
        }

        [TestMethod]
        public void NumericUtility_SafeToInt32_3()
        {
            Assert.AreEqual(0, NumericUtility.SafeToInt32("hello"));
            Assert.AreEqual(0, NumericUtility.SafeToInt32(null));
            Assert.AreEqual(132, NumericUtility.SafeToInt32("132"));
        }

        [TestMethod]
        public void NumericUtility_SafeToInt64_1()
        {
            Assert.AreEqual(0L, NumericUtility.SafeToInt64("hello"));
            Assert.AreEqual(0L, NumericUtility.SafeToInt64(null));
            Assert.AreEqual(132L, NumericUtility.SafeToInt64("132"));
        }

        [TestMethod]
        public void NumericUtility_ToInvariantString_1()
        {
            Assert.AreEqual("123", ((short)123).ToInvariantString());
        }

        [TestMethod]
        public void NumericUtility_ToInvariantString_2()
        {
            Assert.AreEqual("123", 123.ToInvariantString());
        }

        [TestMethod]
        public void NumericUtility_ToInvariantString_3()
        {
            Assert.AreEqual("123", ((long)123).ToInvariantString());
        }

        [TestMethod]
        public void NumericUtility_ToInvariantString_4()
        {
            Assert.AreEqual("1.23", 1.23.ToInvariantString());
        }

        [TestMethod]
        public void NumericUtility_ToInvariantString_5()
        {
            Assert.AreEqual("1.2", 1.23.ToInvariantString("0.0"));
        }

        [TestMethod]
        public void NumericUtility_ToInvariantString_6()
        {
            Assert.AreEqual("1.23", 1.23m.ToInvariantString());
        }

        [TestMethod]
        public void NumericUtility_ToInvariantString_7()
        {
            Assert.AreEqual("1.2", 1.23m.ToInvariantString("0.0"));
        }

        [TestMethod]
        public void NumericUtility_ToInvariantString_8()
        {
            Assert.AreEqual("a", 'a'.ToInvariantString());
        }

        [TestMethod]
        public void NumericUtility_TryParseDecimal_1()
        {
            decimal value;
            Assert.IsTrue(NumericUtility.TryParseDecimal("1.23", out value));
            Assert.AreEqual(1.23m, value);

            Assert.IsFalse(NumericUtility.TryParseDecimal("hello", out value));
            Assert.IsFalse(NumericUtility.TryParseDecimal(string.Empty, out value));
            Assert.IsFalse(NumericUtility.TryParseDecimal(null, out value));
        }

        [TestMethod]
        public void NumericUtility_TryParseDouble_1()
        {
            double value;
            Assert.IsTrue(NumericUtility.TryParseDouble("1.23", out value));
            Assert.AreEqual(1.23, value);

            Assert.IsTrue(NumericUtility.TryParseDouble("1,23", out value));
            Assert.AreEqual(1.23, value);

            Assert.IsFalse(NumericUtility.TryParseDouble("hello", out value));
            Assert.IsFalse(NumericUtility.TryParseDouble(string.Empty, out value));
            Assert.IsFalse(NumericUtility.TryParseDouble(null, out value));
        }

        [TestMethod]
        public void NumericUtility_TryParseFloat_1()
        {
            float value;
            Assert.IsTrue(NumericUtility.TryParseFloat("1.23", out value));
            Assert.AreEqual(1.23f, value);

            Assert.IsTrue(NumericUtility.TryParseFloat("1,23", out value));
            Assert.AreEqual(1.23f, value);

            Assert.IsFalse(NumericUtility.TryParseFloat("hello", out value));
            Assert.IsFalse(NumericUtility.TryParseFloat(string.Empty, out value));
            Assert.IsFalse(NumericUtility.TryParseFloat(null, out value));
        }

        [TestMethod]
        public void NumericUtility_TryParseInt16_1()
        {
            short value;
            Assert.IsTrue(NumericUtility.TryParseInt16("123", out value));
            Assert.AreEqual(123, value);

            Assert.IsFalse(NumericUtility.TryParseInt16("hello", out value));
            Assert.IsFalse(NumericUtility.TryParseInt16(string.Empty, out value));
            Assert.IsFalse(NumericUtility.TryParseInt16(null, out value));
        }
        [TestMethod]
        public void NumericUtility_TryParseInt32_1()
        {
            int value;
            Assert.IsTrue(NumericUtility.TryParseInt32("123", out value));
            Assert.AreEqual(123, value);

            Assert.IsFalse(NumericUtility.TryParseInt32("hello", out value));
            Assert.IsFalse(NumericUtility.TryParseInt32(string.Empty, out value));
            Assert.IsFalse(NumericUtility.TryParseInt32(null, out value));
        }

        [TestMethod]
        public void NumericUtility_TryParseInt64_1()
        {
            long value;
            Assert.IsTrue(NumericUtility.TryParseInt64("123", out value));
            Assert.AreEqual(123, value);

            Assert.IsFalse(NumericUtility.TryParseInt64("hello", out value));
            Assert.IsFalse(NumericUtility.TryParseInt64(string.Empty, out value));
            Assert.IsFalse(NumericUtility.TryParseInt64(null, out value));
        }

        [TestMethod]
        public void NumericUtility_TryParseUInt16_1()
        {
            ushort value;
            Assert.IsTrue(NumericUtility.TryParseUInt16("123", out value));
            Assert.AreEqual(123, value);

            Assert.IsFalse(NumericUtility.TryParseUInt16("hello", out value));
            Assert.IsFalse(NumericUtility.TryParseUInt16(string.Empty, out value));
            Assert.IsFalse(NumericUtility.TryParseUInt16(null, out value));
        }
        [TestMethod]
        public void NumericUtility_TryParseUInt32_1()
        {
            uint value;
            Assert.IsTrue(NumericUtility.TryParseUInt32("123", out value));
            Assert.AreEqual(123u, value);

            Assert.IsFalse(NumericUtility.TryParseUInt32("hello", out value));
            Assert.IsFalse(NumericUtility.TryParseUInt32(string.Empty, out value));
            Assert.IsFalse(NumericUtility.TryParseUInt32(null, out value));
        }

        [TestMethod]
        public void NumericUtility_TryParseUInt64_1()
        {
            ulong value;
            Assert.IsTrue(NumericUtility.TryParseUInt64("123", out value));
            Assert.AreEqual(123u, value);

            Assert.IsFalse(NumericUtility.TryParseUInt64("hello", out value));
            Assert.IsFalse(NumericUtility.TryParseUInt64(string.Empty, out value));
            Assert.IsFalse(NumericUtility.TryParseUInt64(null, out value));
        }
    }
}
