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
        public void EnglishHyphenator_Hyphenate1()
        {
            _TestHyphenate<EnglishHyphenator>("here", "he-re");
            _TestHyphenate<EnglishHyphenator>("there", "the-re");
            _TestHyphenate<EnglishHyphenator>("anywhere", "any-w-he-re");
        }

        [TestMethod]
        public void EnglishHyphenator_Hyphenate2()
        {
            _TestHyphenate<EnglishHyphenator>("HERE", "HERE");
        }

        [TestMethod]
        public void EnglishHyphenator_Hyphenate3()
        {
            _TestHyphenate<EnglishHyphenator>("The", "The");
        }

        [TestMethod]
        public void EnglishHyphenator_LanguageName()
        {
            Hyphenator hyphenator = new EnglishHyphenator();
            Assert.AreEqual("English", hyphenator.LanguageName);
        }
    }
}
