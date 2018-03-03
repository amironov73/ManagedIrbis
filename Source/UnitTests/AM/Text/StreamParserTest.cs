using System;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class StreamParserTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void StreamParser_Construction_1()
        {
            string text = "Hello, world!";
            StringReader reader = new StringReader(text);
            StreamParser parser = new StreamParser(reader);
            Assert.IsNotNull(parser);
            Assert.IsFalse(parser.EndOfStream);
        }

        [TestMethod]
        public void StreamParser_FromFile()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "record.txt"
                );

            using (StreamParser parser
                = StreamParser.FromFile(fileName, Encoding.UTF8))
            {
                Assert.IsNotNull(parser);
                Assert.IsFalse(parser.EndOfStream);
            }
        }

        [TestMethod]
        public void StreamParser_ReadInt16_1()
        {
            const string text = "  \t1234 ogo";
            StreamParser parser = StreamParser.FromString(text);
            short? int16 = parser.ReadInt16();
            Assert.IsTrue(int16.HasValue);
            Assert.AreEqual(1234, int16.Value);

            parser = StreamParser.FromString(" -1234 ");
            int16 = parser.ReadInt16();
            Assert.IsTrue(int16.HasValue);
            Assert.AreEqual(-1234, int16.Value);

            parser = StreamParser.FromString("  ");
            int16 = parser.ReadInt16();
            Assert.IsFalse(int16.HasValue);
        }

        [TestMethod]
        public void StreamParser_ReadUInt16_1()
        {
            const string text = "  \t1234 ogo";
            StreamParser parser = StreamParser.FromString(text);
            ushort? uint16 = parser.ReadUInt16();
            Assert.IsTrue(uint16.HasValue);
            Assert.AreEqual(1234u, uint16.Value);


            parser = StreamParser.FromString("  ");
            uint16 = parser.ReadUInt16();
            Assert.IsFalse(uint16.HasValue);
        }

        [TestMethod]
        public void StreamParser_ReadInt32_1()
        {
            const string text = "  \t1234 ogo";
            StreamParser parser = StreamParser.FromString(text);
            int? int32 = parser.ReadInt32();
            Assert.IsTrue(int32.HasValue);
            Assert.AreEqual(1234, int32.Value);

            parser = StreamParser.FromString(" -1234 ");
            int32 = parser.ReadInt32();
            Assert.IsTrue(int32.HasValue);
            Assert.AreEqual(-1234, int32.Value);

            parser = StreamParser.FromString("  ");
            int32 = parser.ReadInt32();
            Assert.IsFalse(int32.HasValue);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void StreamParser_ReadInt32_2()
        {
            const string text = "  ogo";
            StreamParser parser = StreamParser.FromString(text);
            parser.ReadInt32();
        }

        [TestMethod]
        public void StreamParser_ReadUInt32_1()
        {
            const string text = "  \t1234 ogo";
            StreamParser parser = StreamParser.FromString(text);
            uint? uint32 = parser.ReadUInt32();
            Assert.IsTrue(uint32.HasValue);
            Assert.AreEqual(1234u, uint32.Value);

            parser = StreamParser.FromString("  ");
            uint32 = parser.ReadUInt32();
            Assert.IsFalse(uint32.HasValue);
        }

        [TestMethod]
        public void StreamParser_ReadInt64_1()
        {
            const string text = "  \t1234 ogo";
            StreamParser parser = StreamParser.FromString(text);
            long? int64 = parser.ReadInt64();
            Assert.IsTrue(int64.HasValue);
            Assert.AreEqual(1234, int64.Value);

            parser = StreamParser.FromString(" -1234 ");
            int64 = parser.ReadInt64();
            Assert.IsTrue(int64.HasValue);
            Assert.AreEqual(-1234, int64.Value);

            parser = StreamParser.FromString("  ");
            int64 = parser.ReadInt64();
            Assert.IsFalse(int64.HasValue);
        }

        [TestMethod]
        public void StreamParser_ReadUInt64_1()
        {
            const string text = "  \t1234 ogo";
            StreamParser parser = StreamParser.FromString(text);
            ulong? uint64 = parser.ReadUInt64();
            Assert.IsTrue(uint64.HasValue);
            Assert.AreEqual(1234u, uint64.Value);


            parser = StreamParser.FromString("  ");
            uint64 = parser.ReadUInt64();
            Assert.IsFalse(uint64.HasValue);
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

        [TestMethod]
        public void StreamParser_ReadDouble_1()
        {
            _TestDouble("1", 1.0);
            _TestDouble("1.", 1.0);
            _TestDouble("1.0", 1.0);
            _TestDouble("+1", 1.0);
            _TestDouble("-1", -1.0);
            _TestDouble("1e2",100.0);
            _TestDouble("1e-2",0.01);

            StreamParser parser = StreamParser.FromString("  ");
            double? number = parser.ReadDouble();
            Assert.IsFalse(number.HasValue);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void StreamParser_ReadDouble_2()
        {
            const string text = "  ogo";
            StreamParser parser = StreamParser.FromString(text);
            parser.ReadDouble();
        }

        private void _TestSingle
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

        [TestMethod]
        public void StreamParser_ReadSingle_1()
        {
            _TestSingle("1", 1.0F);
            _TestSingle("1.", 1.0F);
            _TestSingle("1.0", 1.0F);
            _TestSingle("+1", 1.0F);
            _TestSingle("-1", -1.0F);
            _TestSingle("1e2", 100.0F);
            _TestSingle("1e-2", 0.01F);

            StreamParser parser = StreamParser.FromString("  ");
            float? number = parser.ReadSingle();
            Assert.IsFalse(number.HasValue);
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
        public void StreamParser_ReadDecimal_1()
        {
            _TestDecimal("1", 1.0m);
            _TestDecimal("1.", 1.0m);
            _TestDecimal("1.0", 1.0m);
            _TestDecimal("+1", 1.0m);
            _TestDecimal("-1", -1.0m);

            StreamParser parser = StreamParser.FromString("  ");
            decimal? number = parser.ReadDecimal();
            Assert.IsFalse(number.HasValue);
        }


        [TestMethod]
        public void StreamParser_ReadSingle_2()
        {
            StreamParser parser = StreamParser.FromString(string.Empty);
            double? number = parser.ReadDouble();
            Assert.IsFalse(number.HasValue);
        }

        [TestMethod]
        public void StreamParser_IsControl_1()
        {
            StreamParser parser = StreamParser.FromString("1\nhello");
            Assert.IsFalse(parser.IsControl());
            parser.ReadChar();
            Assert.IsTrue(parser.IsControl());
            parser.ReadChar();
            Assert.IsFalse(parser.IsControl());
        }

        [TestMethod]
        public void StreamParser_IsLetter_1()
        {
            StreamParser parser = StreamParser.FromString("1h!ello");
            Assert.IsFalse(parser.IsLetter());
            parser.ReadChar();
            Assert.IsTrue(parser.IsLetter());
            parser.ReadChar();
            Assert.IsFalse(parser.IsLetter());
        }

        [TestMethod]
        public void StreamParser_IsLetterOrDigit_1()
        {
            StreamParser parser = StreamParser.FromString("1h!ello");
            Assert.IsTrue(parser.IsLetterOrDigit());
            parser.ReadChar();
            Assert.IsTrue(parser.IsLetterOrDigit());
            parser.ReadChar();
            Assert.IsFalse(parser.IsLetterOrDigit());
        }

        [TestMethod]
        public void StreamParser_IsNumber_1()
        {
            StreamParser parser = StreamParser.FromString("1½hello");
            Assert.IsTrue(parser.IsNumber());
            parser.ReadChar();
            Assert.IsTrue(parser.IsNumber());
            parser.ReadChar();
            Assert.IsFalse(parser.IsNumber());
        }

        [TestMethod]
        public void StreamParser_IsPunctuation_1()
        {
            StreamParser parser = StreamParser.FromString("1!hello");
            Assert.IsFalse(parser.IsPunctuation());
            parser.ReadChar();
            Assert.IsTrue(parser.IsPunctuation());
            parser.ReadChar();
            Assert.IsFalse(parser.IsPunctuation());
        }

        [TestMethod]
        public void StreamParser_IsSeparator_1()
        {
            StreamParser parser = StreamParser.FromString("1 hello");
            Assert.IsFalse(parser.IsSeparator());
            parser.ReadChar();
            Assert.IsTrue(parser.IsSeparator());
            parser.ReadChar();
            Assert.IsFalse(parser.IsSeparator());
        }

        [TestMethod]
        public void StreamParser_IsSurrogate_1()
        {
            StreamParser parser = StreamParser.FromString("1\xd801hello");
            Assert.IsFalse(parser.IsSurrogate());
            parser.ReadChar();
            Assert.IsTrue(parser.IsSurrogate());
            parser.ReadChar();
            Assert.IsFalse(parser.IsSurrogate());
        }

        [TestMethod]
        public void StreamParser_IsSymbol_1()
        {
            StreamParser parser = StreamParser.FromString("1№hello");
            Assert.IsFalse(parser.IsSymbol());
            parser.ReadChar();
            Assert.IsTrue(parser.IsSymbol());
            parser.ReadChar();
            Assert.IsFalse(parser.IsSymbol());
        }

        [TestMethod]
        public void StreamParser_SkipControl_1()
        {
            StreamParser parser = StreamParser.FromString("\r\nhello");
            parser.SkipControl();
            Assert.AreEqual('h', parser.ReadChar());
        }

        [TestMethod]
        public void StreamParser_SkipControl_2()
        {
            StreamParser parser = StreamParser.FromString("\r\n");
            Assert.IsFalse(parser.SkipControl());
        }

        [TestMethod]
        public void StreamParser_SkipPunctuation_1()
        {
            StreamParser parser = StreamParser.FromString(".,hello");
            parser.SkipPunctuation();
            Assert.AreEqual('h', parser.ReadChar());
        }

        [TestMethod]
        public void StreamParser_SkipPunctuation_2()
        {
            StreamParser parser = StreamParser.FromString(".,");
            Assert.IsFalse(parser.SkipPunctuation());
        }
    }
}
