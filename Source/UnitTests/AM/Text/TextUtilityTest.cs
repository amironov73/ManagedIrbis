using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class TextUtilityTest
    {
        [TestMethod]
        public void TextUtility_DetermineTextKind_1()
        {
            Assert.AreEqual(TextKind.PlainText, TextUtility.DetermineTextKind(null));
            Assert.AreEqual(TextKind.PlainText, TextUtility.DetermineTextKind(""));

            Assert.AreEqual(TextKind.RichText, TextUtility.DetermineTextKind(@"{\rtf1 Hello}"));
            Assert.AreEqual(TextKind.RichText, TextUtility.DetermineTextKind(@"Hello, {\b World}!"));

            Assert.AreEqual(TextKind.Html, TextUtility.DetermineTextKind("<html><body>Hello</body></html>"));
            Assert.AreEqual(TextKind.Html, TextUtility.DetermineTextKind("Hello, <b>World</b>!"));

            Assert.AreEqual(TextKind.PlainText, TextUtility.DetermineTextKind("Hello, world!"));
            Assert.AreEqual(TextKind.PlainText, TextUtility.DetermineTextKind("Hello, <world}!"));
        }
    }
}
