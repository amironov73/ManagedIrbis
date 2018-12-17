using System;

using UnsafeAM.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.UnsafeAM.Text
{
    [TestClass]
    public class HtmlTextTest
    {
        [TestMethod]
        public void HtmlText_Encode_1()
        {
            Assert.AreEqual(string.Empty, HtmlText.Encode(string.Empty));
            Assert.AreEqual("&quot;Hello&quot;", HtmlText.Encode("\"Hello\""));
            Assert.AreEqual("&num;5", HtmlText.Encode("#5"));
            Assert.AreEqual("&amp;5", HtmlText.Encode("&5"));
            Assert.AreEqual("&apos;Hello&apos;", HtmlText.Encode("'Hello'"));
            Assert.AreEqual("&lt;10", HtmlText.Encode("<10"));
            Assert.AreEqual("&gt;10", HtmlText.Encode(">10"));
            Assert.AreEqual("Hello&nbsp;World", HtmlText.Encode("Hello\x00A0World"));
            Assert.AreEqual("&cent;10", HtmlText.Encode("\x00A210"));
            Assert.AreEqual("&pound;10", HtmlText.Encode("\x00A310"));
            Assert.AreEqual("&yen;10", HtmlText.Encode("\x00A510"));
            Assert.AreEqual("&sect;10", HtmlText.Encode("\x00A710"));
            Assert.AreEqual("&copy; by me", HtmlText.Encode("\x00A9 by me"));
            Assert.AreEqual("Hel&shy;lo", HtmlText.Encode("Hel\x00ADlo"));
            Assert.AreEqual("&reg; 2017", HtmlText.Encode("\x00AE 2017"));
            Assert.AreEqual("&euro;10", HtmlText.Encode("\x20AC10"));
        }

        [TestMethod]
        public void HtmlText_HtmlToPlainText_1()
        {
            string html = string.Empty;
            Assert.AreEqual
                (
                    string.Empty,
                    HtmlText.ToPlainText(html)
                );

            html = "<p>У попа была</p> <b>собака</b>";
            Assert.AreEqual
                (
                    "У попа была собака",
                    HtmlText.ToPlainText(html)
                );
        }
    }
}
