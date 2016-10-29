using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Tokenizer;

namespace UnitTests.AM.Text.Tokenizer
{
    [TestClass]
    public class StringTokenizerTest
    {
        [TestMethod]
        public void StringTokenizer_Construction()
        {
            const string text = "Hello, world!";
            StringTokenizer tokenizer = new StringTokenizer(text);
            Assert.IsNotNull(tokenizer.Settings);
        }

        [TestMethod]
        public void StringTokenizer_GetAllTokens()
        {
            const string text = "Hello, world!";
            StringTokenizer tokenizer = new StringTokenizer(text);
            Token[] tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(5, tokens.Length);
            Assert.AreEqual("Hello", tokens[0].Value);
            Assert.AreEqual(",", tokens[1].Value);
            Assert.AreEqual("world", tokens[2].Value);
            Assert.AreEqual("!", tokens[3].Value);
            Assert.AreEqual(TokenKind.EOF, tokens[4].Kind);
        }

        [TestMethod]
        [ExpectedException(typeof(TokenizerException))]
        public void StringTokenizer_ReadNumber_Exception()
        {
            const string text = "Hello, 123EWorld!";
            StringTokenizer tokenizer = new StringTokenizer(text);
            tokenizer.GetAllTokens();
        }

        [TestMethod]
        public void StringTokenizer_NextToken1()
        {
            const string text = "Hello\r\nWorld!";
            StringTokenizer tokenizer = new StringTokenizer(text);
            Token[] tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(4, tokens.Length);
        }

        [TestMethod]
        public void StringTokenizer_NextToken2()
        {
            const string text = "Hello World!";
            StringTokenizer tokenizer = new StringTokenizer(text);
            tokenizer.Settings.IgnoreWhitespace = false;
            Token[] tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(5, tokens.Length);
        }

        [TestMethod]
        public void StringTokenizer_ReadWhitespace()
        {
            const string text = "Hello  World!";
            StringTokenizer tokenizer = new StringTokenizer(text);
            tokenizer.Settings.IgnoreWhitespace = false;
            Token[] tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(5, tokens.Length);
        }

        [TestMethod]
        public void StringTokenizer_ReadString()
        {
            const string text = "Hello\"\\x123\"World!";
            StringTokenizer tokenizer = new StringTokenizer(text);
            Token[] tokens = tokenizer.GetAllTokens();

            // TODO: fix this!

            Assert.AreEqual(3, tokens.Length);
        }

        [TestMethod]
        public void StringTokenizer_ReadChar()
        {
            StringTokenizer tokenizer = new StringTokenizer("");
            tokenizer.ReadChar();
            Assert.AreEqual('\0', tokenizer.ReadChar());
        }

        [TestMethod]
        public void StringTokenizer_GetEnumerator()
        {
            const string text = "Hello, World!";
            IEnumerable tokenizer = new StringTokenizer(text);
            int count = 0;
            foreach (object o in tokenizer)
            {
                count++;
            }
            Assert.AreEqual(5, count);
        }

        [TestMethod]
        public void StringTokenizer_IgnoreEOF()
        {
            const string text = "Hello, world!";
            StringTokenizer tokenizer = new StringTokenizer(text);
            tokenizer.Settings.IgnoreEOF = true;
            Token[] tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(4, tokens.Length);
            Assert.AreEqual("Hello", tokens[0].Value);
            Assert.AreEqual(",", tokens[1].Value);
            Assert.AreEqual("world", tokens[2].Value);
            Assert.AreEqual("!", tokens[3].Value);
        }
    }
}
