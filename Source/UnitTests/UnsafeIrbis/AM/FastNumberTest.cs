using System;
using System.Text;

using UnsafeAM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.UnsafeAM
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

        private void _TestMemory
            (
                int expected,
                string text,
                int offset,
                int length
            )
        {
            char[] chars = text.ToCharArray();
            ReadOnlyMemory<char> memory1 = new ReadOnlyMemory<char>(chars, offset, length);
            int actual = FastNumber.ParseInt32(memory1);
            Assert.AreEqual(expected, actual);

            byte[] bytes = Encoding.Default.GetBytes(text);
            ReadOnlyMemory<byte> memory2 = new ReadOnlyMemory<byte>(bytes, offset, length);
            actual = FastNumber.ParseInt32(memory2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FastNumber_ParseInt32_2()
        {
            string text = "123456789";
            _TestMemory(0, "0", 0, 1);
            _TestMemory(1, text, 0, 1);
            _TestMemory(12, text, 0, 2);
            _TestMemory(123, text, 0, 3);
            _TestMemory(1234, text, 0, 4);
            _TestMemory(12345, text, 0, 5);
            _TestMemory(123456, text, 0, 6);
            _TestMemory(1234567, text, 0, 7);
            _TestMemory(12345678, text, 0, 8);
            _TestMemory(123456789, text, 0, 9);
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

        private void _TestMemory
            (
                long expected,
                string text,
                int offset,
                int length
            )
        {
            char[] chars = text.ToCharArray();
            ReadOnlyMemory<char> memory1 = new ReadOnlyMemory<char>(chars, offset, length);
            long actual = FastNumber.ParseInt64(memory1);
            Assert.AreEqual(expected, actual);

            byte[] bytes = Encoding.Default.GetBytes(text);
            ReadOnlyMemory<byte> memory2 = new ReadOnlyMemory<byte>(bytes, offset, length);
            actual = FastNumber.ParseInt64(memory2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FastNumber_ParseInt64_2()
        {
            string text = "1234567890123456789";
            _TestMemory(0L, "0", 0, 1);
            _TestMemory(1L, text, 0, 1);
            _TestMemory(12L, text, 0, 2);
            _TestMemory(123L, text, 0, 3);
            _TestMemory(1234L, text, 0, 4);
            _TestMemory(12345L, text, 0, 5);
            _TestMemory(123456L, text, 0, 6);
            _TestMemory(1234567L, text, 0, 7);
            _TestMemory(12345678L, text, 0, 8);
            _TestMemory(123456789L, text, 0, 9);
            _TestMemory(1234567890L, text, 0, 10);
            _TestMemory(12345678901L, text, 0, 11);
            _TestMemory(123456789012L, text, 0, 12);
            _TestMemory(1234567890123L, text, 0, 13);
            _TestMemory(12345678901234L, text, 0, 14);
            _TestMemory(123456789012345L, text, 0, 15);
            _TestMemory(1234567890123456L, text, 0, 16);
            _TestMemory(12345678901234567L, text, 0, 17);
            _TestMemory(123456789012345678L, text, 0, 18);
            _TestMemory(1234567890123456789L, text, 0, 19);
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
