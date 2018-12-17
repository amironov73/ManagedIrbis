using System;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UnsafeAM.Text;

// ReSharper disable ObjectCreationAsStatement

namespace UnitTests.UnsafeAM.Text
{
    [TestClass]
    public class TextNavigatorTest
        : Common.CommonUnitTest
    {
        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void TextNavigator_Construction_1()
        //{
        //    new TextNavigator("\uD801", true);
        //}

        [TestMethod]
        public void TextNavigator_Column_1()
        {
            const string text = "ABC";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual(1, navigator.Column);
            navigator.ReadChar();
            Assert.AreEqual(2, navigator.Column);
        }

        [TestMethod]
        public void TextNavigator_IsEOF_1()
        {
            const string text = "ABC";
            TextNavigator navigator = new TextNavigator(text);
            Assert.IsFalse(navigator.IsEOF);
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsEOF);
            navigator.SkipChar(2);
            Assert.IsTrue(navigator.IsEOF);
        }

        [TestMethod]
        public void TextNavigator_Length_1()
        {
            const string text = "ABC";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual(text.Length, navigator.Length);
        }

        [TestMethod]
        public void TextNavigator_Line_1()
        {
            const string text = "ABC\nDEF";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual(1, navigator.Line);
            navigator.ReadChar();
            Assert.AreEqual(1, navigator.Line);
            navigator.SkipChar(3);
            Assert.AreEqual(2, navigator.Line);
        }

        [TestMethod]
        public void TextNavigator_Position_1()
        {
            const string text = "ABC";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual(0, navigator.Position);
            navigator.ReadChar();
            Assert.AreEqual(1, navigator.Position);
            navigator.ReadChar();
            Assert.AreEqual(2, navigator.Position);
            navigator.ReadChar();
            Assert.AreEqual(3, navigator.Position);
            navigator.ReadChar();
            Assert.AreEqual(3, navigator.Position);
        }

        [TestMethod]
        public void TextNavigator_Text_1()
        {
            string text = "ABC";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreSame(text, navigator.Text);
        }

        [TestMethod]
        public void TextNavigator_Clone_1()
        {
            const string text = "ABC";
            TextNavigator first = new TextNavigator(text);
            first.ReadChar();
            TextNavigator second = first.Clone();
            Assert.AreSame(first.Text, second.Text);
            Assert.AreEqual(first.Column, second.Column);
            Assert.AreEqual(first.Line, second.Line);
            Assert.AreEqual(first.Position, second.Position);
        }

        [TestMethod]
        public void TextNavigator_FromFile_1()
        {
            string fileName = Path.Combine(TestDataPath, "record.txt");
            TextNavigator navigator = TextNavigator.FromFile(fileName);
            Assert.AreEqual('#', navigator.ReadChar());
        }

        [TestMethod]
        public void TextNavigator_FromFile_2()
        {
            string fileName = Path.Combine(TestDataPath, "record.txt");
            TextNavigator navigator = TextNavigator.FromFile(fileName, Encoding.UTF8);
            Assert.AreEqual('#', navigator.ReadChar());
        }

        [TestMethod]
        public void TextNavigator_GetRemainingText_1()
        {
            const string text = "ABC";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual(text, navigator.GetRemainingText());
            navigator.ReadChar();
            Assert.AreEqual("BC", navigator.GetRemainingText());
            navigator.ReadChar();
            Assert.AreEqual("C", navigator.GetRemainingText());
            navigator.ReadChar();
            Assert.IsNull(navigator.GetRemainingText());
        }

        [TestMethod]
        public void TextNavigator_IsControl_1()
        {
            const string text = "A\tBC";
            TextNavigator navigator = new TextNavigator(text);
            Assert.IsFalse(navigator.IsControl());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsControl());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsControl());
        }

        [TestMethod]
        public void TextNavigator_IsDigit_1()
        {
            const string text = "A1BC";
            TextNavigator navigator = new TextNavigator(text);
            Assert.IsFalse(navigator.IsDigit());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsDigit());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsDigit());
        }

        [TestMethod]
        public void TextNavigator_IsLetter_1()
        {
            const string text = "A1BC";
            TextNavigator navigator = new TextNavigator(text);
            Assert.IsTrue(navigator.IsLetter());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsLetter());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsLetter());
        }

        [TestMethod]
        public void TextNavigator_IsLetterOrDigit_1()
        {
            const string text = "A_1";
            TextNavigator navigator = new TextNavigator(text);
            Assert.IsTrue(navigator.IsLetterOrDigit());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsLetterOrDigit());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsLetterOrDigit());
        }

        [TestMethod]
        public void TextNavigator_IsNumber_1()
        {
            const string text = "1+²";
            TextNavigator navigator = new TextNavigator(text);
            Assert.IsTrue(navigator.IsNumber());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsNumber());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsNumber());
        }

        [TestMethod]
        public void TextNavigator_IsPunctuation_1()
        {
            const string text = ".A,";
            TextNavigator navigator = new TextNavigator(text);
            Assert.IsTrue(navigator.IsPunctuation());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsPunctuation());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsPunctuation());
        }

        [TestMethod]
        public void TextNavigator_IsSeparator_1()
        {
            const string text = "\u2028A ";
            TextNavigator navigator = new TextNavigator(text);
            Assert.IsTrue(navigator.IsSeparator());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsSeparator());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsSeparator());
        }

        [TestMethod]
        public void TextNavigator_IsSymbol_1()
        {
            const string text = "$A+";
            TextNavigator navigator = new TextNavigator(text);
            Assert.IsTrue(navigator.IsSymbol());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsSymbol());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsSymbol());
        }

        [TestMethod]
        public void TextNavigator_IsWhiteSpace_1()
        {
            const string text = " A\t";
            TextNavigator navigator = new TextNavigator(text);
            Assert.IsTrue(navigator.IsWhiteSpace());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsWhiteSpace());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsWhiteSpace());
        }

        [TestMethod]
        public void TextNavigator_LookAhead_1()
        {
            const string text = "ABC";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual('B', navigator.LookAhead());
            navigator.ReadChar();
            Assert.AreEqual('C', navigator.LookAhead());
            navigator.ReadChar();
            Assert.AreEqual(TextNavigator.EOF, navigator.LookAhead());
            navigator.ReadChar();
            Assert.AreEqual(TextNavigator.EOF, navigator.LookAhead());
        }

        [TestMethod]
        public void TextNavigator_LookAhead_2()
        {
            const string text = "ABC";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual('C', navigator.LookAhead(2));
            navigator.ReadChar();
            Assert.AreEqual(TextNavigator.EOF, navigator.LookAhead(2));
            navigator.ReadChar();
            Assert.AreEqual(TextNavigator.EOF, navigator.LookAhead(2));
            navigator.ReadChar();
            Assert.AreEqual(TextNavigator.EOF, navigator.LookAhead(2));
        }

        [TestMethod]
        public void TextNavigator_LookBehind_1()
        {
            const string text = "ABC";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual(TextNavigator.EOF, navigator.LookBehind());
            navigator.ReadChar();
            Assert.AreEqual('A', navigator.LookBehind());
            navigator.ReadChar();
            Assert.AreEqual('B', navigator.LookBehind());
            navigator.ReadChar();
            Assert.AreEqual('C', navigator.LookBehind());
            navigator.ReadChar();
            Assert.AreEqual('C', navigator.LookBehind());
            navigator.ReadChar();
        }

        [TestMethod]
        public void TextNavigator_LookBehind_2()
        {
            const string text = "ABC";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual(TextNavigator.EOF, navigator.LookBehind(2));
            navigator.ReadChar();
            Assert.AreEqual(TextNavigator.EOF, navigator.LookBehind(2));
            navigator.ReadChar();
            Assert.AreEqual('A', navigator.LookBehind(2));
            navigator.ReadChar();
            Assert.AreEqual('B', navigator.LookBehind(2));
            navigator.ReadChar();
            Assert.AreEqual('B', navigator.LookBehind(2));
            navigator.ReadChar();
        }

        [TestMethod]
        public void TextNavigator_Move_1()
        {
            const string text = "ABC";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreSame(navigator, navigator.Move(2));
            Assert.AreEqual(2, navigator.Position);
            Assert.AreSame(navigator, navigator.Move(-2));
            Assert.AreEqual(0, navigator.Position);
        }

        [TestMethod]
        public void TextNavigator_PeekChar_1()
        {
            const string text = "ABC";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual('A', navigator.PeekChar());
            navigator.ReadChar();
            Assert.AreEqual('B', navigator.PeekChar());
            navigator.ReadChar();
            Assert.AreEqual('C', navigator.PeekChar());
            navigator.ReadChar();
            Assert.AreEqual(TextNavigator.EOF, navigator.PeekChar());
        }

        [TestMethod]
        public void TextNavigator_PeekString_1()
        {
            const string text = "ABC";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual("AB", navigator.PeekString(2));
            navigator.ReadChar();
            Assert.AreEqual("BC", navigator.PeekString(2));
            navigator.ReadChar();
            Assert.AreEqual("C", navigator.PeekString(2));
            navigator.ReadChar();
            Assert.IsNull(navigator.PeekString(2));
        }

        [TestMethod]
        public void TextNavigator_PeekTo_1()
        {
            const string text = "ABC]DEF";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual("ABC]", navigator.PeekTo(']'));
            navigator.ReadChar();
            Assert.AreEqual("BC]", navigator.PeekTo(']'));
            navigator.ReadChar();
            navigator.ReadChar();
            Assert.AreEqual("]", navigator.PeekTo(']'));
            navigator.ReadChar();
            Assert.AreEqual("DEF", navigator.PeekTo(']'));
            navigator.Move(3);
            Assert.IsNull(navigator.PeekTo(']'));
        }

        [TestMethod]
        public void TextNavigator_PeekTo_2()
        {
            const string text = "ABC]DE+F";
            char[] stop = { ']', '+' };
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual("ABC]", navigator.PeekTo(stop));
            navigator.ReadChar();
            Assert.AreEqual("BC]", navigator.PeekTo(stop));
            navigator.ReadChar();
            navigator.ReadChar();
            Assert.AreEqual("]", navigator.PeekTo(stop));
            navigator.ReadChar();
            Assert.AreEqual("DE+", navigator.PeekTo(stop));
            navigator.Move(3);
            Assert.AreEqual("F", navigator.PeekTo(stop));
            navigator.ReadChar();
            Assert.IsNull(navigator.PeekTo(stop));
        }

        [TestMethod]
        public void TextNavigator_PeekUntil_1()
        {
            const string text = "ABC]DEF";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual("ABC", navigator.PeekUntil(']'));
            navigator.ReadChar();
            Assert.AreEqual("BC", navigator.PeekUntil(']'));
            navigator.ReadChar();
            navigator.ReadChar();
            Assert.AreEqual(string.Empty, navigator.PeekUntil(']'));
            navigator.ReadChar();
            Assert.AreEqual("DEF", navigator.PeekUntil(']'));
            navigator.Move(3);
            Assert.IsNull(navigator.PeekUntil(']'));
        }

        [TestMethod]
        public void TextNavigator_PeekUntil_2()
        {
            const string text = "ABC]DE+F";
            char[] stop = { ']', '+' };
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual("ABC", navigator.PeekUntil(stop));
            navigator.ReadChar();
            Assert.AreEqual("BC", navigator.PeekUntil(stop));
            navigator.ReadChar();
            navigator.ReadChar();
            Assert.AreEqual(string.Empty, navigator.PeekUntil(stop));
            navigator.ReadChar();
            Assert.AreEqual("DE", navigator.PeekUntil(stop));
            navigator.Move(3);
            Assert.AreEqual("F", navigator.PeekUntil(stop));
            navigator.ReadChar();
            Assert.IsNull(navigator.PeekUntil(stop));
        }

        [TestMethod]
        public void TextNavigator_ReadChar_1()
        {
            const string text = "ABC";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual('A', navigator.ReadChar());
            Assert.AreEqual('B', navigator.ReadChar());
            Assert.AreEqual('C', navigator.ReadChar());
            Assert.AreEqual(TextNavigator.EOF, navigator.ReadChar());
            Assert.AreEqual(TextNavigator.EOF, navigator.ReadChar());
        }

        [TestMethod]
        public void TextNavigator_ReadEscapedUntil_1()
        {
            const string text = "AB[tC]D";
            TextNavigator navigator = new TextNavigator(text);
            string expected = "ABtC";
            string actual = navigator.ReadEscapedUntil('[', ']');
            Assert.AreEqual(expected, actual);
            Assert.AreEqual('D', navigator.ReadChar());
            Assert.IsNull(navigator.ReadEscapedUntil('[', ']'));
        }

        [TestMethod]
        public void TextNavigator_ReadEscapedUntil_2()
        {
            const string text = "AB[tC";
            TextNavigator navigator = new TextNavigator(text);
            string expected = "ABtC";
            string actual = navigator.ReadEscapedUntil('[', ']');
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(TextNavigator.EOF, navigator.ReadChar());
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TextNavigator_ReadEscapedUntil_3()
        {
            const string text = "AB[";
            TextNavigator navigator = new TextNavigator(text);
            navigator.ReadEscapedUntil('[', ']');
        }

        [TestMethod]
        public void TextNavigator_ReadFrom_1()
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

            navigator = new TextNavigator(string.Empty);
            actual = navigator.ReadFrom('\'', '\'');
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TextNavigator_ReadFrom_2()
        {
            const string text1 = "[ABC>DEF";
            char[] open = { '[', '<' }, close = { '>', '>' };
            TextNavigator navigator = new TextNavigator(text1);
            string actual = navigator.ReadFrom(open, close);
            Assert.AreEqual("[ABC>", actual);

            const string text2 = "[ABCDEF";
            navigator = new TextNavigator(text2);
            actual = navigator.ReadFrom(open, close);
            Assert.AreEqual(string.Empty, actual);

            const string text3 = "ABC[DEF";
            navigator = new TextNavigator(text3);
            actual = navigator.ReadFrom(open, close);
            Assert.AreEqual(string.Empty, actual);

            navigator = new TextNavigator(string.Empty);
            actual = navigator.ReadFrom(open, close);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TextNavigator_ReadInteger_1()
        {
            const string text1 = "314abc";
            TextNavigator navigator = new TextNavigator(text1);
            string actual = navigator.ReadInteger();
            Assert.AreEqual("314", actual);

            actual = navigator.ReadInteger();
            Assert.AreEqual(string.Empty, actual);

            navigator = new TextNavigator(string.Empty);
            actual = navigator.ReadInteger();
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TextNavigator_ReadLine_1()
        {
            const string text = "ABC\r\nDEF";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual("ABC", navigator.ReadLine());
            Assert.AreEqual("DEF", navigator.ReadLine());
            Assert.IsNull(navigator.ReadLine());
        }

        [TestMethod]
        public void TextNavigator_ReadString_1()
        {
            const string text = "ABCDEF";
            TextNavigator navigator = new TextNavigator(text);
            Assert.AreEqual("ABC", navigator.ReadString(3));
            Assert.AreEqual("DEF", navigator.ReadString(4));
            Assert.IsNull(navigator.ReadString(3));
        }

        [TestMethod]
        public void TextNavigator_ReadTo_1()
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

            navigator = new TextNavigator(string.Empty);
            Assert.IsNull(navigator.ReadTo(open));
        }

        [TestMethod]
        public void TextNavigator_ReadTo_2()
        {
            char[] stop = { ']', '>' };
            TextNavigator navigator = new TextNavigator("ABC]>DEF");
            Assert.AreEqual("ABC]", navigator.ReadTo(stop));
            Assert.AreEqual(">", navigator.ReadTo(stop));
            Assert.AreEqual("DEF", navigator.ReadTo(stop));
            Assert.IsNull(navigator.ReadTo(stop));
        }

        [TestMethod]
        public void TextNavigator_ReadTo_3()
        {
            TextNavigator navigator = new TextNavigator("314abc>>>hello");
            string actual = navigator.ReadTo(">>>");
            Assert.AreEqual("314abc", actual);
            Assert.AreEqual("hello", navigator.GetRemainingText());

            navigator = new TextNavigator("314abc>>hello");
            actual = navigator.ReadTo(">>>");
            Assert.IsNull(actual);
            Assert.AreEqual("314abc>>hello", navigator.GetRemainingText());

            navigator = new TextNavigator(string.Empty);
            Assert.IsNull(navigator.ReadTo(">>>"));
        }

        [TestMethod]
        public void TextNavigator_ReadUntil_1()
        {
            const string text = "'ABC'DEF";
            TextNavigator navigator = new TextNavigator(text);
            char open = navigator.ReadChar();
            string actual = navigator.ReadUntil(open);
            Assert.AreEqual("ABC", actual);

            navigator = new TextNavigator(string.Empty);
            Assert.IsNull(navigator.ReadUntil(open));
        }

        [TestMethod]
        public void TextNavigator_ReadUntil_2()
        {
            char[] openChars = { '(' };
            char[] closeChars = { ')' };
            char[] stopChars = { ')' };
            char[] stopChars2 = { ']' };

            TextNavigator navigator = new TextNavigator("12345)");
            string actual = navigator.ReadUntil(openChars, closeChars, stopChars);
            Assert.AreEqual("12345", actual);

            navigator = new TextNavigator("12(3)(4)5)");
            actual = navigator.ReadUntil(openChars, closeChars, stopChars);
            Assert.AreEqual("12(3)(4)5", actual);

            navigator = new TextNavigator("12(3(4))5)");
            actual = navigator.ReadUntil(openChars, closeChars, stopChars);
            Assert.AreEqual("12(3(4))5", actual);

            navigator = new TextNavigator("12(3(4))5");
            actual = navigator.ReadUntil(openChars, closeChars, stopChars);
            Assert.IsNull(actual);

            navigator = new TextNavigator("12(3(4)5)");
            actual = navigator.ReadUntil(openChars, closeChars, stopChars);
            Assert.IsNull(actual);

            navigator = new TextNavigator("1234]5)");
            actual = navigator.ReadUntil(openChars, closeChars, stopChars2);
            Assert.AreEqual("1234", actual);

            navigator = new TextNavigator("123(4])]5)");
            actual = navigator.ReadUntil(openChars, closeChars, stopChars2);
            Assert.AreEqual("123(4])", actual);

            navigator = new TextNavigator(string.Empty);
            Assert.IsNull(navigator.ReadUntil(openChars, closeChars, stopChars));
        }

        [TestMethod]
        public void TextNavigator_ReadUntil_3()
        {
            TextNavigator navigator = new TextNavigator("12345<.>");
            string actual = navigator.ReadUntil("<.>");
            Assert.AreEqual("12345", actual);
            Assert.AreEqual("<.>", navigator.PeekString(3));
            Assert.AreEqual("<.>", navigator.GetRemainingText());

            navigator = new TextNavigator("12345");
            actual = navigator.ReadUntil("<.>");
            Assert.IsNull(actual);

            navigator = new TextNavigator("12345<");
            actual = navigator.ReadUntil("<.>");
            Assert.IsNull(actual);

            navigator = new TextNavigator("12345<.");
            actual = navigator.ReadUntil("<.>");
            Assert.IsNull(actual);

            navigator = new TextNavigator("12345<.6>");
            actual = navigator.ReadUntil("<.>");
            Assert.IsNull(actual);

            navigator = new TextNavigator("12345<.>67890");
            actual = navigator.ReadUntil("<.>");
            Assert.AreEqual("12345", actual);
            Assert.AreEqual("<.>", navigator.PeekString(3));
            Assert.AreEqual("<.>67890", navigator.GetRemainingText());

            navigator = new TextNavigator(string.Empty);
            Assert.IsNull(navigator.ReadUntil("<.>"));
        }

        [TestMethod]
        public void TextNavigator_ReadUntil_4()
        {
            char[] stop = { ']', '>' };
            TextNavigator navigator = new TextNavigator("ABC>]DEF");
            Assert.AreEqual("ABC", navigator.ReadUntil(stop));
            navigator.ReadChar();
            Assert.AreEqual(string.Empty, navigator.ReadUntil(stop));
            navigator.ReadChar();
            Assert.AreEqual("DEF", navigator.ReadUntil(stop));
            navigator.ReadChar();
            Assert.IsNull(navigator.ReadUntil(stop));
        }

        [TestMethod]
        public void TextNavigator_ReadUntilWhiteSpace_1()
        {
            TextNavigator navigator = new TextNavigator("arg1 arg2 arg3");
            Assert.AreEqual("arg1", navigator.ReadUntilWhiteSpace());
            Assert.IsTrue(navigator.SkipWhitespace());
            Assert.AreEqual("arg2", navigator.ReadUntilWhiteSpace());
            Assert.IsTrue(navigator.SkipWhitespace());
            Assert.AreEqual("arg3", navigator.ReadUntilWhiteSpace());
            Assert.IsTrue(navigator.IsEOF);
            Assert.IsNull(navigator.ReadUntilWhiteSpace());
        }

        [TestMethod]
        public void TextNavigator_ReadWhile_1()
        {
            TextNavigator navigator = new TextNavigator("111234");
            Assert.AreEqual("111", navigator.ReadWhile('1'));
            Assert.AreEqual(string.Empty, navigator.ReadWhile('1'));
            navigator.Move(3);
            Assert.IsNull(navigator.ReadWhile('1'));
        }

        [TestMethod]
        public void TextNavigator_ReadWhile_2()
        {
            const string text1 = "314abc";
            char[] good = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            TextNavigator navigator = new TextNavigator(text1);
            Assert.AreEqual("314", navigator.ReadWhile(good));
            Assert.AreEqual(string.Empty, navigator.ReadWhile(good));
            navigator.Move(3);
            Assert.IsNull(navigator.ReadWhile(good));
        }

        [TestMethod]
        public void TextNavigator_ReadWord_1()
        {
            TextNavigator navigator = new TextNavigator("Hello, world!");
            Assert.AreEqual("Hello", navigator.ReadWord());
            Assert.AreEqual(string.Empty, navigator.ReadWord());
            navigator.Move(2);
            Assert.AreEqual("world", navigator.ReadWord());
            Assert.AreEqual(string.Empty, navigator.ReadWord());
            navigator.Move(2);
            Assert.IsNull(navigator.ReadWord());
        }

        [TestMethod]
        public void TextNavigator_ReadWord_2()
        {
            char[] additional = { '<', '>' };
            TextNavigator navigator = new TextNavigator("<Hello>, world!");
            Assert.AreEqual("<Hello>", navigator.ReadWord(additional));
            Assert.AreEqual(string.Empty, navigator.ReadWord(additional));
            navigator.Move(2);
            Assert.AreEqual("world", navigator.ReadWord(additional));
            Assert.AreEqual(string.Empty, navigator.ReadWord(additional));
            navigator.Move(2);
            Assert.IsNull(navigator.ReadWord(additional));
        }

        [TestMethod]
        public void TextNavigator_RecentText_1()
        {
            TextNavigator navigator = new TextNavigator("Hello, world!");
            Assert.AreEqual(string.Empty, navigator.RecentText(4));
            navigator.Move(4);
            Assert.AreEqual(string.Empty, navigator.RecentText(-1));
            Assert.AreEqual("Hell", navigator.RecentText(4));
            navigator.Move(9);
            Assert.AreEqual("rld!", navigator.RecentText(4));
            Assert.AreEqual("Hello, world!", navigator.RecentText(20));
            navigator.Move(9);
            Assert.AreEqual(string.Empty, navigator.RecentText(1));
        }

        [TestMethod]
        public void TextNavigator_RestorePosition_1()
        {
            TextNavigator navigator = new TextNavigator("Hello, world!");
            TextPosition saved = navigator.SavePosition();
            navigator.ReadChar();
            navigator.RestorePosition(saved);
            Assert.AreEqual(saved.Position, navigator.Position);
            Assert.AreEqual(saved.Line, navigator.Line);
            Assert.AreEqual(saved.Column, navigator.Column);
            Assert.AreEqual('H', navigator.ReadChar());
        }

        [TestMethod]
        public void TextNavigator_SavePosition_1()
        {
            TextNavigator navigator = new TextNavigator("Hello, world!");
            TextPosition saved = navigator.SavePosition();
            Assert.AreEqual(navigator.Position, saved.Position);
            Assert.AreEqual(navigator.Line, saved.Line);
            Assert.AreEqual(navigator.Column, saved.Column);
        }

        [TestMethod]
        public void TextNavigator_SkipChar_1()
        {
            TextNavigator navigator = new TextNavigator("111234");
            Assert.IsTrue(navigator.SkipChar('1'));
            Assert.IsTrue(navigator.SkipChar('1'));
            Assert.IsTrue(navigator.SkipChar('1'));
            Assert.AreEqual('2', navigator.ReadChar());
            Assert.IsFalse(navigator.SkipChar('1'));
        }

        [TestMethod]
        public void TextNavigator_SkipChar_2()
        {
            TextNavigator navigator = new TextNavigator("123456");
            Assert.IsTrue(navigator.SkipChar(3));
            Assert.AreEqual('4', navigator.ReadChar());
            Assert.IsFalse(navigator.SkipChar(3));
            Assert.IsTrue(navigator.IsEOF);
        }

        [TestMethod]
        public void TextNavigator_SkipChar_3()
        {
            char[] allowed = { '1', '2' };
            TextNavigator navigator = new TextNavigator("123456");
            Assert.IsTrue(navigator.SkipChar(allowed));
            Assert.IsTrue(navigator.SkipChar(allowed));
            Assert.IsFalse(navigator.SkipChar(allowed));
            Assert.AreEqual('3', navigator.ReadChar());
        }

        [TestMethod]
        public void TextNavigator_SkipControl_1()
        {
            TextNavigator navigator = new TextNavigator("\t\tABC");
            Assert.IsTrue(navigator.SkipControl());
            Assert.AreEqual('A', navigator.ReadChar());
            navigator.Move(2);
            Assert.IsFalse(navigator.SkipControl());
        }

        [TestMethod]
        public void TextNavigator_SkipPunctuation_1()
        {
            TextNavigator navigator = new TextNavigator(".,ABC");
            Assert.IsTrue(navigator.SkipPunctuation());
            Assert.AreEqual('A', navigator.ReadChar());
            navigator.Move(2);
            Assert.IsFalse(navigator.SkipPunctuation());
        }

        [TestMethod]
        public void TextNavigator_SkipNonWord_1()
        {
            TextNavigator navigator = new TextNavigator(". (ABC");
            Assert.IsTrue(navigator.SkipNonWord());
            Assert.AreEqual('A', navigator.ReadChar());
        }

        [TestMethod]
        public void TextNavigator_SkipNonWord_2()
        {
            TextNavigator navigator = new TextNavigator(". (<ABC");
            Assert.IsTrue(navigator.SkipNonWord('<', '>'));
            Assert.AreEqual('<', navigator.ReadChar());
        }

        [TestMethod]
        public void TextNavigator_SkipRange_1()
        {
            TextNavigator navigator = new TextNavigator("123ABC");
            Assert.IsTrue(navigator.SkipRange('0', '9'));
            Assert.AreEqual('A', navigator.ReadChar());
            navigator.Move(2);
            Assert.IsFalse(navigator.SkipRange('0', '9'));
        }

        [TestMethod]
        public void TextNavigator_SkipTo_1()
        {
            TextNavigator navigator = new TextNavigator("123ABC");
            Assert.IsTrue(navigator.SkipTo('A'));
            Assert.AreEqual('A', navigator.ReadChar());
            Assert.IsFalse(navigator.SkipTo('A'));
        }

        [TestMethod]
        public void TextNavigator_SkipWhileNot_1()
        {
            char[] good = { 'A', 'B' };
            TextNavigator navigator = new TextNavigator("123ABC");
            Assert.IsTrue(navigator.SkipWhileNot(good));
            Assert.AreEqual('A', navigator.ReadChar());
            Assert.IsTrue(navigator.SkipWhileNot(good));
            Assert.AreEqual('B', navigator.ReadChar());
            Assert.IsFalse(navigator.SkipWhileNot(good));
            Assert.AreEqual(TextNavigator.EOF, navigator.ReadChar());
        }

        [TestMethod]
        public void TextNavigator_SkipWhile_1()
        {
            TextNavigator navigator = new TextNavigator("111ABC");
            Assert.IsTrue(navigator.SkipWhile('1'));
            Assert.AreEqual('A', navigator.ReadChar());
            navigator.Move(2);
            Assert.IsFalse(navigator.SkipWhile('1'));
        }

        [TestMethod]
        public void TextNavigator_SkipWhile_2()
        {
            char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            TextNavigator navigator = new TextNavigator("314ABC");
            Assert.IsTrue(navigator.SkipWhile(digits));
            Assert.AreEqual('A', navigator.ReadChar());
            navigator.Move(2);
            Assert.IsFalse(navigator.SkipWhile(digits));
        }

        [TestMethod]
        public void TextNavigator_SkipWhitespace_1()
        {
            TextNavigator navigator = new TextNavigator(" \t\r\nABC ");
            Assert.IsTrue(navigator.SkipWhitespace());
            Assert.AreEqual('A', navigator.ReadChar());
            navigator.ReadChar();
            navigator.ReadChar();
            Assert.IsFalse(navigator.SkipWhitespace());
            Assert.IsTrue(navigator.IsEOF);
        }

        [TestMethod]
        public void TextNavigator_SkipWhitespaceAndPunctuation_1()
        {
            TextNavigator navigator = new TextNavigator(" \t,\r\nABC. ");
            Assert.IsTrue(navigator.SkipWhitespaceAndPunctuation());
            Assert.AreEqual('A', navigator.ReadChar());
            navigator.ReadChar();
            navigator.ReadChar();
            Assert.IsFalse(navigator.SkipWhitespaceAndPunctuation());
            Assert.IsTrue(navigator.IsEOF);
        }

        [TestMethod]
        public void TextNavigator_SplitByGoodCharacters_1()
        {
            char[] good = { 'A', 'B', 'C', 'a', 'b', 'c' };
            TextNavigator navigator = new TextNavigator("HELLOaworldBc!");
            string[] result = navigator.SplitByGoodCharacters(good);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("a", result[0]);
            Assert.AreEqual("Bc", result[1]);
        }

        [TestMethod]
        public void TextNavigator_SplitToWords_1()
        {
            TextNavigator navigator = new TextNavigator("Hello, world!");
            string[] result = navigator.SplitToWords();
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("Hello", result[0]);
            Assert.AreEqual("world", result[1]);
        }

        [TestMethod]
        public void TextNavigator_SplitToWords_2()
        {
            char[] additional = { '<', '>' };
            TextNavigator navigator = new TextNavigator("<Hello>, world!");
            string[] result = navigator.SplitToWords(additional);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("<Hello>", result[0]);
            Assert.AreEqual("world", result[1]);
        }

        [TestMethod]
        public void TextNavigator_Substring_1()
        {
            TextNavigator navigator = new TextNavigator("Hello, world!");
            Assert.AreEqual("world", navigator.Substring(7, 5));
        }

        [TestMethod]
        public void TextNavigator_ToString_1()
        {
            TextNavigator navigator = new TextNavigator("Hello, world!");
            Assert.AreEqual("Line=1, Column=1", navigator.ToString());
        }
    }
}
