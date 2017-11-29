using AM.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class RichTextTest
    {
        [TestMethod]
        public void RichText_Encode_1()
        {
            Assert.AreEqual(null, RichText.Encode(null, null));
            Assert.AreEqual("", RichText.Encode("", null));
            Assert.AreEqual("Hello", RichText.Encode("Hello", null));
            Assert.AreEqual("\\{\\}", RichText.Encode("{}", null));
            Assert.AreEqual("Hel\\\\lo", RichText.Encode("Hel\\lo", null));
        }

        [TestMethod]
        public void RichText_Encode_2()
        {
            Assert.AreEqual("Hello,\\'0aWorld!", RichText.Encode("Hello,\nWorld!", null));
            Assert.AreEqual("Copyright \\'a9 2017", RichText.Encode("Copyright © 2017", null));
        }

        [TestMethod]
        public void RichText_Encode_3()
        {
            UnicodeRange range = UnicodeRange.Cyrillic;
            Assert.AreEqual("Hello", RichText.Encode("Hello", range));
            Assert.AreEqual("Привет", RichText.Encode("Привет", range));
        }

        [TestMethod]
        public void RichText_Encode_4()
        {
            UnicodeRange range = UnicodeRange.LatinExtended;
            Assert.AreEqual("\\u1055?\\u1088?\\u1080?\\u1074?\\u1077?\\u1090?", RichText.Encode("Привет", range));
        }
    }
}
