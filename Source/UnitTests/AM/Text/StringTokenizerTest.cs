using System;
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
            StringTokenizer tokenizer = new StringTokenizer("Hello 2 from 3.14E-2 ArsMagna 11. Hello .3");
            Token[] tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(9, tokens.Length);

        }
    }
}
