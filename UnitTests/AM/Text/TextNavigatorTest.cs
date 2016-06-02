using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class TextNavigatorTest
    {
        [TestMethod]
        public void TestTextNavigatorSkipWhitespace()
        {
            const string text = " \tABC ";
            TextNavigator navigator = new TextNavigator(text);
            navigator.SkipWhitespace();
            char c = navigator.ReadChar();
            Assert.AreEqual('A', c);
        }

        [TestMethod]
        public void TestTextNavigatorReadFrom()
        {
            const string text1 = "'ABC'DEF";
            TextNavigator navigator = new TextNavigator(text1);
            string actual = navigator.ReadFrom('\'', '\'');
            Assert.AreEqual("'ABC'", actual);

            const string text2 = "'ABCDEF";
            navigator = new TextNavigator(text2);
            actual = navigator.ReadFrom('\'', '\'');
            Assert.AreEqual(string.Empty, actual);

            const string text3 = "ABC'DEF";
            navigator = new TextNavigator(text3);
            actual = navigator.ReadFrom('\'', '\'');
            Assert.AreEqual(string.Empty, actual);
        }

        [TestMethod]
        public void TestTextNavigatorReadTo()
        {
            const string text1 = "'ABC'DEF";
            TextNavigator navigator = new TextNavigator(text1);
            char open = navigator.ReadChar();
            string actual = navigator.ReadTo(open);
            Assert.AreEqual("ABC'", actual);

            const string text2 = "'ABC";
            navigator = new TextNavigator(text2);
            open = navigator.ReadChar();
            actual = navigator.ReadTo(open);
            Assert.AreEqual("ABC", actual);
        }

        [TestMethod]
        public void TestTextNavigatorReadUntil()
        {
            const string text = "'ABC'DEF";
            TextNavigator navigator = new TextNavigator(text);
            char open = navigator.ReadChar();
            string actual = navigator.ReadUntil(open);
            Assert.AreEqual("ABC", actual);
        }

        [TestMethod]
        public void TestTextNavigatorReadWhile()
        {
            const string text1 = "314abc";
            TextNavigator navigator = new TextNavigator(text1);
            string actual = navigator.ReadWhile('0', '1', '2', '3',
                '4', '5', '6', '7', '8', '9');
            Assert.AreEqual("314", actual);
        }

        [TestMethod]
        public void TestTexNavigatorSkipWhile()
        {
            const string text1 = "314abc";
            TextNavigator navigator = new TextNavigator(text1);
            navigator.SkipWhile('0', '1', '2', '3',
                '4', '5', '6', '7', '8', '9');
            string actual = navigator.GetRemainingText();
            Assert.AreEqual("abc", actual);
        }
    }
}
