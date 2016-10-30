using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Tokenizer;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class StringTokenizerTest
    {
        [TestMethod]
        public void StringTokenizer_GetAllTokens1()
        {
            StringTokenizer tokenizer = new StringTokenizer
                (
                    "Hello 2 from 3.14E-2 ArsMagna 11. Hello .3"
                );
            Token[] tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(9, tokens.Length);
            Assert.AreEqual("Hello", tokens[0].Value);
            Assert.AreEqual("2", tokens[1].Value);
            Assert.AreEqual("from", tokens[2].Value);
            Assert.AreEqual("3.14E-2", tokens[3].Value);
            Assert.AreEqual("ArsMagna", tokens[4].Value);
            Assert.AreEqual("11.", tokens[5].Value);
            Assert.AreEqual("Hello", tokens[6].Value);
            Assert.AreEqual(".3", tokens[7].Value);
            Assert.AreEqual(TokenKind.EOF, tokens[8].Kind);
        }

        [TestMethod]
        public void StringTokenizer_GetAllTokens2()
        {
            StringTokenizer tokenizer = new StringTokenizer
                (
                    "Hello \"This is a string\" World"
                );
            Token[] tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(4, tokens.Length);
            Assert.AreEqual("Hello", tokens[0].Value);
            Assert.AreEqual("\"This is a string\"", tokens[1].Value);
            Assert.AreEqual("World", tokens[2].Value);
            Assert.AreEqual(TokenKind.EOF, tokens[3].Kind);
        }

        [TestMethod]
        public void StringTokenizer_GetAllTokens3()
        {
            StringTokenizer tokenizer = new StringTokenizer
            (
                "Hello \"This is a string\" World"
            )
            {
                Settings = {TrimQuotes = true}
                
            };
            Token[] tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(4, tokens.Length);
            Assert.AreEqual("Hello", tokens[0].Value);
            Assert.AreEqual("This is a string", tokens[1].Value);
            Assert.AreEqual("World", tokens[2].Value);
            Assert.AreEqual(TokenKind.EOF, tokens[3].Kind);
        }

        [TestMethod]
        public void StringTokenizer_GetAllTokens4()
        {
            StringTokenizer tokenizer = new StringTokenizer
            (
                "Hello + \r\n 123.456 №"
            )
            {
                Settings =
                {
                    IgnoreNewLine = false,
                    TrimQuotes = true
                }
            };
            Token[] tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(6, tokens.Length);
            Assert.AreEqual("Hello", tokens[0].Value);
            Assert.AreEqual("+", tokens[1].Value);
            Assert.AreEqual(TokenKind.EOL, tokens[2].Kind);
            Assert.AreEqual("123.456", tokens[3].Value);
            Assert.AreEqual(TokenKind.Unknown, tokens[4].Kind);
            Assert.AreEqual(TokenKind.EOF, tokens[5].Kind);
        }

        [TestMethod]
        public void StringTokenizer_ReadString1()
        {
            StringTokenizer tokenizer = new StringTokenizer
                (
                    "Hello \"The\\nstring\" World"
                )
            {
                Settings =
                    {
                        TrimQuotes = true
                    }
            };
            Token[] tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(4, tokens.Length);
            Assert.AreEqual("The\\nstring", tokens[1].Value);
        }

        [TestMethod]
        public void StringTokenizer_ReadString2()
        {
            StringTokenizer tokenizer = new StringTokenizer
                (
                    "Hello \"The\"\"string\" World"
                )
            {
                Settings =
                    {
                        TrimQuotes = true
                    }
            };
            Token[] tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(4, tokens.Length);
            Assert.AreEqual("The\"string", tokens[1].Value);
        }

        [TestMethod]
        public void StringTokenizer_Enumeration()
        {
            StringTokenizer tokenizer = new StringTokenizer
                (
                    "Hello 2 from 3.14E-2 ArsMagna 11. Hello .3"
                );
            Token[] tokens = tokenizer.ToArray();
            Assert.AreEqual(9, tokens.Length);
            Assert.AreEqual("Hello", tokens[0].Value);
            Assert.AreEqual("2", tokens[1].Value);
            Assert.AreEqual("from", tokens[2].Value);
            Assert.AreEqual("3.14E-2", tokens[3].Value);
            Assert.AreEqual("ArsMagna", tokens[4].Value);
            Assert.AreEqual("11.", tokens[5].Value);
            Assert.AreEqual("Hello", tokens[6].Value);
            Assert.AreEqual(".3", tokens[7].Value);
            Assert.AreEqual(TokenKind.EOF, tokens[8].Kind);
        }
    }
}
