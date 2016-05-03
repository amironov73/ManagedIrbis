using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Tokenizer;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class StringTokenizerTest
    {
        [TestMethod]
        public void TestStringTokenizer()
        {
            StringTokenizer tokenizer = new StringTokenizer("Hello from ArsMagna")
            {
                IgnoreWhiteSpace = true
            };
            List<Token> tokens = new List<Token>();
            while (true)
            {
                Token token = tokenizer.Next();
                tokens.Add(token);
                if (token.Kind == TokenKind.EOF)
                {
                    break;
                }
            }
            Assert.AreEqual(4,tokens.Count);
        }
    }
}
