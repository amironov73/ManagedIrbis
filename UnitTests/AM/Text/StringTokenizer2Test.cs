using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Tokenizer;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class StringTokenizer2Test
    {
        [TestMethod]
        public void TestStringTokenizer2()
        {
            StringTokenizer2 tokenizer = new StringTokenizer2("Hello from ArsMagna");
            Token[] tokens = tokenizer.GetAllTokens();
            Assert.AreEqual(4, tokens.Length);

        }
    }
}
