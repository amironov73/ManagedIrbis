using System;
using System.Text;

using AM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM
{
    [TestClass]
    public class FastNumberTest
    {
        [TestMethod]
        public void FastNumber_Int32ToString_1()
        {
            Assert.AreEqual("0", FastNumber.Int32ToString(0));
            Assert.AreEqual("1", FastNumber.Int32ToString(1));
            Assert.AreEqual("12", FastNumber.Int32ToString(12));
            Assert.AreEqual("123", FastNumber.Int32ToString(123));
            Assert.AreEqual("1234", FastNumber.Int32ToString(1234));
            Assert.AreEqual("12345", FastNumber.Int32ToString(12345));
            Assert.AreEqual("123456", FastNumber.Int32ToString(123456));
            Assert.AreEqual("1234567", FastNumber.Int32ToString(1234567));
            Assert.AreEqual("12345678", FastNumber.Int32ToString(12345678));
            Assert.AreEqual("123456789", FastNumber.Int32ToString(123456789));
        }

        [TestMethod]
        public void FastNumber_Int64ToString_1()
        {
            Assert.AreEqual("0", FastNumber.Int64ToString(0));
            Assert.AreEqual("1", FastNumber.Int64ToString(1));
            Assert.AreEqual("12", FastNumber.Int64ToString(12));
            Assert.AreEqual("123", FastNumber.Int64ToString(123));
            Assert.AreEqual("1234", FastNumber.Int64ToString(1234));
            Assert.AreEqual("12345", FastNumber.Int64ToString(12345));
            Assert.AreEqual("123456", FastNumber.Int64ToString(123456));
            Assert.AreEqual("1234567", FastNumber.Int64ToString(1234567));
            Assert.AreEqual("12345678", FastNumber.Int64ToString(12345678));
            Assert.AreEqual("123456789", FastNumber.Int64ToString(123456789));
            Assert.AreEqual("1234567890", FastNumber.Int64ToString(1234567890));
            Assert.AreEqual("12345678901", FastNumber.Int64ToString(12345678901));
            Assert.AreEqual("123456789012", FastNumber.Int64ToString(123456789012));
            Assert.AreEqual("1234567890123", FastNumber.Int64ToString(1234567890123));
            Assert.AreEqual("12345678901234", FastNumber.Int64ToString(12345678901234));
            Assert.AreEqual("123456789012345", FastNumber.Int64ToString(123456789012345));
            Assert.AreEqual("1234567890123456", FastNumber.Int64ToString(1234567890123456));
            Assert.AreEqual("12345678901234567", FastNumber.Int64ToString(12345678901234567));
            Assert.AreEqual("123456789012345678", FastNumber.Int64ToString(123456789012345678));
            Assert.AreEqual("1234567890123456789", FastNumber.Int64ToString(1234567890123456789));
        }

        [TestMethod]
        public void FastNumber_ParseInt32_1()
        {
            Assert.AreEqual(0, FastNumber.ParseInt32("0"));
            Assert.AreEqual(1, FastNumber.ParseInt32("1"));
            Assert.AreEqual(12, FastNumber.ParseInt32("12"));
            Assert.AreEqual(123, FastNumber.ParseInt32("123"));
            Assert.AreEqual(1234, FastNumber.ParseInt32("1234"));
            Assert.AreEqual(12345, FastNumber.ParseInt32("12345"));
            Assert.AreEqual(123456, FastNumber.ParseInt32("123456"));
            Assert.AreEqual(1234567, FastNumber.ParseInt32("1234567"));
            Assert.AreEqual(12345678, FastNumber.ParseInt32("12345678"));
            Assert.AreEqual(123456789, FastNumber.ParseInt32("123456789"));
        }

        [TestMethod]
        public void FastNumber_ParseInt32_3()
        {
            string text = "123456789";
            Assert.AreEqual(0, FastNumber.ParseInt32("0", 0, 1));
            Assert.AreEqual(1, FastNumber.ParseInt32(text, 0, 1));
            Assert.AreEqual(12, FastNumber.ParseInt32(text, 0, 2));
            Assert.AreEqual(123, FastNumber.ParseInt32(text, 0, 3));
            Assert.AreEqual(1234, FastNumber.ParseInt32(text, 0, 4));
            Assert.AreEqual(12345, FastNumber.ParseInt32(text, 0, 5));
            Assert.AreEqual(123456, FastNumber.ParseInt32(text, 0, 6));
            Assert.AreEqual(1234567, FastNumber.ParseInt32(text, 0, 7));
            Assert.AreEqual(12345678, FastNumber.ParseInt32(text, 0, 8));
            Assert.AreEqual(123456789, FastNumber.ParseInt32(text, 0, 9));
        }

        [TestMethod]
        public void FastNumber_ParseInt32_4()
        {
            char[] text = "123456789".ToCharArray();
            Assert.AreEqual(0, FastNumber.ParseInt32("0".ToCharArray(), 0, 1));
            Assert.AreEqual(1, FastNumber.ParseInt32(text, 0, 1));
            Assert.AreEqual(12, FastNumber.ParseInt32(text, 0, 2));
            Assert.AreEqual(123, FastNumber.ParseInt32(text, 0, 3));
            Assert.AreEqual(1234, FastNumber.ParseInt32(text, 0, 4));
            Assert.AreEqual(12345, FastNumber.ParseInt32(text, 0, 5));
            Assert.AreEqual(123456, FastNumber.ParseInt32(text, 0, 6));
            Assert.AreEqual(1234567, FastNumber.ParseInt32(text, 0, 7));
            Assert.AreEqual(12345678, FastNumber.ParseInt32(text, 0, 8));
            Assert.AreEqual(123456789, FastNumber.ParseInt32(text, 0, 9));
        }

        [TestMethod]
        public void FastNumber_ParseInt32_5()
        {
            byte[] text = Encoding.ASCII.GetBytes("123456789");
            Assert.AreEqual(0, FastNumber.ParseInt32(new byte[] { 0x30 }, 0, 1));
            Assert.AreEqual(1, FastNumber.ParseInt32(text, 0, 1));
            Assert.AreEqual(12, FastNumber.ParseInt32(text, 0, 2));
            Assert.AreEqual(123, FastNumber.ParseInt32(text, 0, 3));
            Assert.AreEqual(1234, FastNumber.ParseInt32(text, 0, 4));
            Assert.AreEqual(12345, FastNumber.ParseInt32(text, 0, 5));
            Assert.AreEqual(123456, FastNumber.ParseInt32(text, 0, 6));
            Assert.AreEqual(1234567, FastNumber.ParseInt32(text, 0, 7));
            Assert.AreEqual(12345678, FastNumber.ParseInt32(text, 0, 8));
            Assert.AreEqual(123456789, FastNumber.ParseInt32(text, 0, 9));
        }

        [TestMethod]
        public void FastNumber_ParseInt64_1()
        {
            Assert.AreEqual(0L, FastNumber.ParseInt64("0"));
            Assert.AreEqual(1L, FastNumber.ParseInt64("1"));
            Assert.AreEqual(12L, FastNumber.ParseInt64("12"));
            Assert.AreEqual(123L, FastNumber.ParseInt64("123"));
            Assert.AreEqual(1234L, FastNumber.ParseInt64("1234"));
            Assert.AreEqual(12345L, FastNumber.ParseInt64("12345"));
            Assert.AreEqual(123456L, FastNumber.ParseInt64("123456"));
            Assert.AreEqual(1234567L, FastNumber.ParseInt64("1234567"));
            Assert.AreEqual(12345678L, FastNumber.ParseInt64("12345678"));
            Assert.AreEqual(123456789L, FastNumber.ParseInt64("123456789"));
            Assert.AreEqual(1234567890L, FastNumber.ParseInt64("1234567890"));
            Assert.AreEqual(12345678901L, FastNumber.ParseInt64("12345678901"));
            Assert.AreEqual(123456789012L, FastNumber.ParseInt64("123456789012"));
            Assert.AreEqual(1234567890123L, FastNumber.ParseInt64("1234567890123"));
            Assert.AreEqual(12345678901234L, FastNumber.ParseInt64("12345678901234"));
            Assert.AreEqual(123456789012345L, FastNumber.ParseInt64("123456789012345"));
            Assert.AreEqual(1234567890123456L, FastNumber.ParseInt64("1234567890123456"));
            Assert.AreEqual(12345678901234567L, FastNumber.ParseInt64("12345678901234567"));
            Assert.AreEqual(123456789012345678L, FastNumber.ParseInt64("123456789012345678"));
            Assert.AreEqual(1234567890123456789L, FastNumber.ParseInt64("1234567890123456789"));
        }

        [TestMethod]
        public void FastNumber_ParseInt64_3()
        {
            string text = "1234567890123456789";
            Assert.AreEqual(0L, FastNumber.ParseInt64("0", 0, 1));
            Assert.AreEqual(1L, FastNumber.ParseInt64(text, 0, 1));
            Assert.AreEqual(12L, FastNumber.ParseInt64(text, 0, 2));
            Assert.AreEqual(123L, FastNumber.ParseInt64(text, 0, 3));
            Assert.AreEqual(1234L, FastNumber.ParseInt64(text, 0, 4));
            Assert.AreEqual(12345L, FastNumber.ParseInt64(text, 0, 5));
            Assert.AreEqual(123456L, FastNumber.ParseInt64(text, 0, 6));
            Assert.AreEqual(1234567L, FastNumber.ParseInt64(text, 0, 7));
            Assert.AreEqual(12345678L, FastNumber.ParseInt64(text, 0, 8));
            Assert.AreEqual(123456789L, FastNumber.ParseInt64(text, 0, 9));
            Assert.AreEqual(1234567890L, FastNumber.ParseInt64(text, 0, 10));
            Assert.AreEqual(12345678901L, FastNumber.ParseInt64(text, 0, 11));
            Assert.AreEqual(123456789012L, FastNumber.ParseInt64(text, 0, 12));
            Assert.AreEqual(1234567890123L, FastNumber.ParseInt64(text, 0, 13));
            Assert.AreEqual(12345678901234L, FastNumber.ParseInt64(text, 0, 14));
            Assert.AreEqual(123456789012345L, FastNumber.ParseInt64(text, 0, 15));
            Assert.AreEqual(1234567890123456L, FastNumber.ParseInt64(text, 0, 16));
            Assert.AreEqual(12345678901234567L, FastNumber.ParseInt64(text, 0, 17));
            Assert.AreEqual(123456789012345678L, FastNumber.ParseInt64(text, 0, 18));
            Assert.AreEqual(1234567890123456789L, FastNumber.ParseInt64(text, 0, 19));
        }

        [TestMethod]
        public void FastNumber_ParseInt64_4()
        {
            char[] text = "1234567890123456789".ToCharArray();
            Assert.AreEqual(0L, FastNumber.ParseInt64("0".ToCharArray(), 0, 1));
            Assert.AreEqual(1L, FastNumber.ParseInt64(text, 0, 1));
            Assert.AreEqual(12L, FastNumber.ParseInt64(text, 0, 2));
            Assert.AreEqual(123L, FastNumber.ParseInt64(text, 0, 3));
            Assert.AreEqual(1234L, FastNumber.ParseInt64(text, 0, 4));
            Assert.AreEqual(12345L, FastNumber.ParseInt64(text, 0, 5));
            Assert.AreEqual(123456L, FastNumber.ParseInt64(text, 0, 6));
            Assert.AreEqual(1234567L, FastNumber.ParseInt64(text, 0, 7));
            Assert.AreEqual(12345678L, FastNumber.ParseInt64(text, 0, 8));
            Assert.AreEqual(123456789L, FastNumber.ParseInt64(text, 0, 9));
            Assert.AreEqual(1234567890L, FastNumber.ParseInt64(text, 0, 10));
            Assert.AreEqual(12345678901L, FastNumber.ParseInt64(text, 0, 11));
            Assert.AreEqual(123456789012L, FastNumber.ParseInt64(text, 0, 12));
            Assert.AreEqual(1234567890123L, FastNumber.ParseInt64(text, 0, 13));
            Assert.AreEqual(12345678901234L, FastNumber.ParseInt64(text, 0, 14));
            Assert.AreEqual(123456789012345L, FastNumber.ParseInt64(text, 0, 15));
            Assert.AreEqual(1234567890123456L, FastNumber.ParseInt64(text, 0, 16));
            Assert.AreEqual(12345678901234567L, FastNumber.ParseInt64(text, 0, 17));
            Assert.AreEqual(123456789012345678L, FastNumber.ParseInt64(text, 0, 18));
            Assert.AreEqual(1234567890123456789L, FastNumber.ParseInt64(text, 0, 19));
        }

        [TestMethod]
        public void FastNumber_ParseInt64_5()
        {
            byte[] text = Encoding.ASCII.GetBytes("1234567890123456789");
            Assert.AreEqual(0L, FastNumber.ParseInt64(new byte[] { 0x30 }, 0, 1));
            Assert.AreEqual(1L, FastNumber.ParseInt64(text, 0, 1));
            Assert.AreEqual(12L, FastNumber.ParseInt64(text, 0, 2));
            Assert.AreEqual(123L, FastNumber.ParseInt64(text, 0, 3));
            Assert.AreEqual(1234L, FastNumber.ParseInt64(text, 0, 4));
            Assert.AreEqual(12345L, FastNumber.ParseInt64(text, 0, 5));
            Assert.AreEqual(123456L, FastNumber.ParseInt64(text, 0, 6));
            Assert.AreEqual(1234567L, FastNumber.ParseInt64(text, 0, 7));
            Assert.AreEqual(12345678L, FastNumber.ParseInt64(text, 0, 8));
            Assert.AreEqual(123456789L, FastNumber.ParseInt64(text, 0, 9));
            Assert.AreEqual(1234567890L, FastNumber.ParseInt64(text, 0, 10));
            Assert.AreEqual(12345678901L, FastNumber.ParseInt64(text, 0, 11));
            Assert.AreEqual(123456789012L, FastNumber.ParseInt64(text, 0, 12));
            Assert.AreEqual(1234567890123L, FastNumber.ParseInt64(text, 0, 13));
            Assert.AreEqual(12345678901234L, FastNumber.ParseInt64(text, 0, 14));
            Assert.AreEqual(123456789012345L, FastNumber.ParseInt64(text, 0, 15));
            Assert.AreEqual(1234567890123456L, FastNumber.ParseInt64(text, 0, 16));
            Assert.AreEqual(12345678901234567L, FastNumber.ParseInt64(text, 0, 17));
            Assert.AreEqual(123456789012345678L, FastNumber.ParseInt64(text, 0, 18));
            Assert.AreEqual(1234567890123456789L, FastNumber.ParseInt64(text, 0, 19));
        }
    }
}
