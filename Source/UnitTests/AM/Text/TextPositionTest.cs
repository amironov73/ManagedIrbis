using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class TextPositionTest
    {
        [TestMethod]
        public void TextPosition_Construction()
        {
            string text = "Hello";
            TextNavigator navigator = new TextNavigator(text);
            navigator.ReadChar();
            TextPosition position = new TextPosition(navigator);
            Assert.AreEqual(navigator.Column, position.Column);
            Assert.AreEqual(navigator.Line, position.Line);
            Assert.AreEqual(navigator.Position, position.Position);
        }
        [TestMethod]
        public void TextPosition_ToString()
        {
            string text = "Hello";
            TextNavigator navigator = new TextNavigator(text);
            navigator.ReadChar();
            TextPosition position = new TextPosition(navigator);
            Assert.AreEqual("Line=1, Column=2", position.ToString());
        }

    }
}
