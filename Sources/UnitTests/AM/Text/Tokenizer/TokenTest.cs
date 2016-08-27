using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;
using AM.Runtime;
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
        public void TestToken_Serialization()
        {
            Token token = new Token();
            _TestSerialization(token);

            token = new Token(TokenKind.Number, "123", 1, 2);
            _TestSerialization(token);
        }

        [TestMethod]
        public void TestToken_operators()
        {
            Token token = "Hello";
            Assert.AreEqual("Hello", token.Value);

            string hello = token;
            Assert.AreEqual("Hello", hello);
        }

        [TestMethod]
        public void TestToken_ToString()
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
        public void TestToken_IsEOF()
        {
            Token token = new Token(TokenKind.Number, "123", 1, 2);
            Assert.IsFalse(token.IsEOF);

            token = new Token(TokenKind.EOF, null, 0, 0);
            Assert.IsTrue(token.IsEOF);
        }

        [TestMethod]
        public void TestToken_Convert()
        {
            string[] words = {"Hello", "Irbis", "Word"};
            Token[] tokens = Token.Convert(words);
            Assert.AreEqual(words.Length, tokens.Length);
            for (int i = 0; i < words.Length; i++)
            {
                Assert.AreEqual(words[i], tokens[i].Value);
            }
        }
    }
}
