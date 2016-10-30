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
        public void StreamParser_Construction1()
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
        public void StreamParser_ReadInt16()
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
        public void StreamParser_ReadUInt16()
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
        public void StreamParser_ReadInt32()
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
        public void StreamParser_ReadInt32_Exception()
        {
            const string text = "  ogo";
            StreamParser parser = StreamParser.FromString(text);
            int? number = parser.ReadInt32();
        }

        [TestMethod]
        public void StreamParser_ReadUInt32()
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
        public void StreamParser_ReadInt64()
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
        public void StreamParser_ReadUInt64()
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
        public void StreamParser_ReadDouble()
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
        public void StreamParser_ReadDouble_Exception()
        {
            const string text = "  ogo";
            StreamParser parser = StreamParser.FromString(text);
            double? number = parser.ReadDouble();
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
        public void StreamParser_ReadSingle()
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
        public void StreamParser_ReadDecimal()
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
        public void StreamParser_ReadSingle_Eof()
        {
            StreamParser parser = StreamParser.FromString(string.Empty);
            double? number = parser.ReadDouble();
            Assert.IsFalse(number.HasValue);
        }

        [TestMethod]
        public void StreamParser_IsControl()
        {
            StreamParser parser = StreamParser.FromString("1\nhello");
            Assert.IsFalse(parser.IsControl());
            parser.ReadChar();
            Assert.IsTrue(parser.IsControl());
            parser.ReadChar();
            Assert.IsFalse(parser.IsControl());
        }

        [TestMethod]
        public void StreamParser_IsLetter()
        {
            StreamParser parser = StreamParser.FromString("1h!ello");
            Assert.IsFalse(parser.IsLetter());
            parser.ReadChar();
            Assert.IsTrue(parser.IsLetter());
            parser.ReadChar();
            Assert.IsFalse(parser.IsLetter());
        }

        [TestMethod]
        public void StreamParser_IsLetterOrDigit()
        {
            StreamParser parser = StreamParser.FromString("1h!ello");
            Assert.IsTrue(parser.IsLetterOrDigit());
            parser.ReadChar();
            Assert.IsTrue(parser.IsLetterOrDigit());
            parser.ReadChar();
            Assert.IsFalse(parser.IsLetterOrDigit());
        }

        [TestMethod]
        public void StreamParser_IsNumber()
        {
            StreamParser parser = StreamParser.FromString("1½hello");
            Assert.IsTrue(parser.IsNumber());
            parser.ReadChar();
            Assert.IsTrue(parser.IsNumber());
            parser.ReadChar();
            Assert.IsFalse(parser.IsNumber());
        }

        [TestMethod]
        public void StreamParser_IsPunctuation()
        {
            StreamParser parser = StreamParser.FromString("1!hello");
            Assert.IsFalse(parser.IsPunctuation());
            parser.ReadChar();
            Assert.IsTrue(parser.IsPunctuation());
            parser.ReadChar();
            Assert.IsFalse(parser.IsPunctuation());
        }

        [TestMethod]
        public void StreamParser_IsSeparator()
        {
            StreamParser parser = StreamParser.FromString("1 hello");
            Assert.IsFalse(parser.IsSeparator());
            parser.ReadChar();
            Assert.IsTrue(parser.IsSeparator());
            parser.ReadChar();
            Assert.IsFalse(parser.IsSeparator());
        }

        [TestMethod]
        public void StreamParser_IsSurrogate()
        {
            StreamParser parser = StreamParser.FromString("1\xd801hello");
            Assert.IsFalse(parser.IsSurrogate());
            parser.ReadChar();
            Assert.IsTrue(parser.IsSurrogate());
            parser.ReadChar();
            Assert.IsFalse(parser.IsSurrogate());
        }

        [TestMethod]
        public void StreamParser_IsSymbol()
        {
            StreamParser parser = StreamParser.FromString("1№hello");
            Assert.IsFalse(parser.IsSymbol());
            parser.ReadChar();
            Assert.IsTrue(parser.IsSymbol());
            parser.ReadChar();
            Assert.IsFalse(parser.IsSymbol());
        }

        [TestMethod]
        public void StreamParser_SkipControl1()
        {
            StreamParser parser = StreamParser.FromString("\r\nhello");
            parser.SkipControl();
            Assert.AreEqual('h', parser.ReadChar());
        }

        [TestMethod]
        public void StreamParser_SkipControl2()
        {
            StreamParser parser = StreamParser.FromString("\r\n");
            Assert.IsFalse(parser.SkipControl());
        }

        [TestMethod]
        public void StreamParser_SkipPunctuation1()
        {
            StreamParser parser = StreamParser.FromString(".,hello");
            parser.SkipPunctuation();
            Assert.AreEqual('h', parser.ReadChar());
        }

        [TestMethod]
        public void StreamParser_SkipPunctuation2()
        {
            StreamParser parser = StreamParser.FromString(".,");
            Assert.IsFalse(parser.SkipPunctuation());
        }
    }
}
