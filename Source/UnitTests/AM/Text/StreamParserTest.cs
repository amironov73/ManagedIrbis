using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class StreamParserTest
    {
        [TestMethod]
        public void TestStreamParserInt()
        {
            const string text = "  \t1234 ogo";
            StreamParser parser = StreamParser.FromString(text);
            int? int32 = parser.ReadInt32();
            Assert.IsTrue(int32.HasValue);
            Assert.AreEqual(1234, int32.Value);

            parser = StreamParser.FromString(text);
            long? int64 = parser.ReadInt64();
            Assert.IsTrue(int64.HasValue);
            Assert.AreEqual(1234, int64.Value);

            parser = StreamParser.FromString(text);
            uint? uint32 = parser.ReadUInt32();
            Assert.IsTrue(uint32.HasValue);
            Assert.AreEqual(1234u, uint32.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestStreamParserIntFail()
        {
            const string text = "  ogo";
            StreamParser parser = StreamParser.FromString(text);
            int? number = parser.ReadInt32();
            Assert.IsFalse(number.HasValue);
        }

        private void _TestDouble
            (
                string text,
                double expected
            )
        {
            StreamParser parser = StreamParser.FromString(text);
            double? number = parser.ReadDouble();
            Assert.IsTrue(number.HasValue);
            Assert.AreEqual(expected, number.Value);
        }

        private void _TestFloat
            (
                string text,
                float expected
            )
        {
            StreamParser parser = StreamParser.FromString(text);
            float? number = parser.ReadSingle();
            Assert.IsTrue(number.HasValue);
            Assert.AreEqual(expected, number.Value);
        }

        private void _TestDecimal
            (
                string text,
                decimal expected
            )
        {
            StreamParser parser = StreamParser.FromString(text);
            decimal? number = parser.ReadDecimal();
            Assert.IsTrue(number.HasValue);
            Assert.AreEqual(expected, number.Value);
        }

        [TestMethod]
        public void TestStreamParserDouble()
        {
            _TestDouble("1", 1.0);
            _TestDouble("1.", 1.0);
            _TestDouble("1.0", 1.0);
            _TestDouble("+1", 1.0);
            _TestDouble("-1", -1.0);
            _TestDouble("1e2",100.0);
            _TestDouble("1e-2",0.01);
        }

        [TestMethod]
        public void TestStreamParserFloat()
        {
            _TestFloat("1", 1.0F);
            _TestFloat("1.", 1.0F);
            _TestFloat("1.0", 1.0F);
            _TestFloat("+1", 1.0F);
            _TestFloat("-1", -1.0F);
            _TestFloat("1e2", 100.0F);
            _TestFloat("1e-2", 0.01F);
        }

        [TestMethod]
        public void TestStreamParserDecimal()
        {
            _TestDecimal("1", 1.0m);
            _TestDecimal("1.", 1.0m);
            _TestDecimal("1.0", 1.0m);
            _TestDecimal("+1", 1.0m);
            _TestDecimal("-1", -1.0m);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestStreamParserDoubleFail()
        {
            const string text = "  ogo";
            StreamParser parser = StreamParser.FromString(text);
            double? number = parser.ReadDouble();
            Assert.IsFalse(number.HasValue);
        }

        [TestMethod]
        public void TestStreamParserFloatEof()
        {
            StreamParser parser = StreamParser.FromString(string.Empty);
            double? number = parser.ReadDouble();
            Assert.IsFalse(number.HasValue);
        }
    }
}
