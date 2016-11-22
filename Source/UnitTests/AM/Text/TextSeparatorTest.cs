using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Pft;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class TextSeparatorTest
    {
        private void _TestSeparator
            (
                string source,
                string expected
            )
        {
            PftTextSeparator separator = new PftTextSeparator();
            bool endState = separator.SeparateText(source);
            Assert.IsFalse(endState);
            string actual = separator.Accumulator;
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void TextSeparator_SeparateText1()
        {
            _TestSeparator("", "");
            _TestSeparator("Hello", "<<<Hello>>>");
            _TestSeparator
                (
                    "Hello, <%v200^a%> World!",
                    "<<<Hello, >>>v200^a<<< World!>>>"
                );
            _TestSeparator
                (
                    "Hello, <%%> World!",
                    "<<<Hello, >>><<< World!>>>"
                );
            _TestSeparator
                (
                    "<%v200^a, |:|v200^e%>",
                    "v200^a, |:|v200^e"
                );
        }

        [TestMethod]
        public void TextSeparator_SeparateText2()
        {
            _TestSeparator("", "");
            _TestSeparator("<Hello>!", "<<<<Hello>!>>>");
            _TestSeparator
                (
                    "1% < 2%",
                    "<<<1% < 2%>>>"
                );
        }
    }
}
