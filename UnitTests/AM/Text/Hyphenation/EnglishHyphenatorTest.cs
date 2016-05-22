using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Hyphenation;

namespace UnitTests.AM.Text.Hyphenation
{
    [TestClass]
    public class EnglishHyphenatorTest
        : HyphenatorTest
    {
        [TestMethod]
        public void TestEnglishHyphenator()
        {
            _TestHyphenate<EnglishHyphenator>("here", "he-re");
            _TestHyphenate<EnglishHyphenator>("there", "the-re");
            _TestHyphenate<EnglishHyphenator>("anywhere", "any-w-he-re");
        }
    }
}
