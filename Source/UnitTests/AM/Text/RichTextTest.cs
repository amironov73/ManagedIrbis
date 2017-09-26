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
            Assert.AreEqual(null, RichText.Encode(null));
            Assert.AreEqual("", RichText.Encode(""));
            Assert.AreEqual("Hello", RichText.Encode("Hello"));
            Assert.AreEqual("\\{\\}", RichText.Encode("{}"));
            Assert.AreEqual("Hel\\\\lo", RichText.Encode("Hel\\lo"));
        }
    }
}
