using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class StreamParserTest
    {
        [TestMethod]
        public void TestStreamParser()
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
        public void TestStreamParserFail()
        {
            const string text = "  ogo";
            StreamParser parser = StreamParser.FromString(text);
            int? number = parser.ReadInt32();
            Assert.IsFalse(number.HasValue);
        }
    }
}
