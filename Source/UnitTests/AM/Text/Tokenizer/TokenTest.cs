using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;
using AM.Runtime;
using AM.Text;
using AM.Text.Tokenizer;

namespace UnitTests.AM.Text.Tokenizer
{
    [TestClass]
    public class TokenTest
    {
        private void _TestSerialization
            (
                Token first
            )
        {
            byte[] bytes = first.SaveToMemory();

            Token second = bytes.RestoreObjectFromMemory<Token>();

            Assert.AreEqual(first.Value, second.Value);
            Assert.AreEqual(first.Column, second.Column);
            Assert.AreEqual(first.Line, second.Line);
            Assert.AreEqual(first.Kind, second.Kind);
        }

        [TestMethod]
        public void Token_Serialization()
        {
            Token token = new Token();
            _TestSerialization(token);

            token = new Token(TokenKind.Number, "123", 1, 2);
            _TestSerialization(token);
        }

        [TestMethod]
        public void Token_Implicit()
        {
            Token token = "Hello";
            Assert.AreEqual("Hello", token.Value);

            string hello = token;
            Assert.AreEqual("Hello", hello);
        }

        [TestMethod]
        public void Token_ToString()
        {
            Token token = new Token(TokenKind.Number, "123", 1, 2);
            string actual = token.ToString();
            Assert.AreEqual
                (
                    "Kind: Number, Column: 2, Line: 1, Value: 123",
                    actual
                );
        }

        [TestMethod]
        public void Token_IsEOF()
        {
            Token token = new Token(TokenKind.Number, "123", 1, 2);
            Assert.IsFalse(token.IsEOF);

            token = new Token(TokenKind.EOF, null, 0, 0);
            Assert.IsTrue(token.IsEOF);
        }

        [TestMethod]
        public void Token_Convert()
        {
            string[] words = {"Hello", "Irbis", "Word"};
            Token[] tokens = Token.Convert(words);
            Assert.AreEqual(words.Length, tokens.Length);
            for (int i = 0; i < words.Length; i++)
            {
                Assert.AreEqual(words[i], tokens[i].Value);
            }
        }

        [TestMethod]
        public void Token_FromNavigator1()
        {
            const string text = "Hello, world!";
            TextNavigator navigator = new TextNavigator(text);
            navigator.ReadChar();
            navigator.ReadChar();
            navigator.ReadChar();
            navigator.ReadChar();
            navigator.ReadChar();
            navigator.SkipWhitespaceAndPunctuation();
            Token token = Token.FromNavigator
                (
                    navigator,
                    TokenKind.Word,
                    navigator.GetRemainingText()
                );
            Assert.IsFalse(token.IsEOF);
            Assert.AreEqual(1, token.Line);
            Assert.AreEqual(8, token.Column);
            Assert.AreEqual(TokenKind.Word, token.Kind);
            Assert.AreEqual("world!", token.Value);
        }

        [TestMethod]
        public void Token_FromNavigator2()
        {
            const string text = "Hello, world!";
            TextNavigator navigator = new TextNavigator(text);
            navigator.ReadChar();
            navigator.ReadChar();
            navigator.ReadChar();
            navigator.ReadChar();
            navigator.ReadChar();
            navigator.SkipWhitespaceAndPunctuation();
            Token token = Token.FromNavigator
                (
                    navigator,
                    navigator.GetRemainingText()
                );
            Assert.IsFalse(token.IsEOF);
            Assert.AreEqual(1, token.Line);
            Assert.AreEqual(8, token.Column);
            Assert.AreEqual(TokenKind.Unknown, token.Kind);
            Assert.AreEqual("world!", token.Value);
        }
    }
}
