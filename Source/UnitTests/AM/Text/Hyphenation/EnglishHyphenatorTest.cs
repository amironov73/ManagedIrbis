
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Hyphenation;

namespace UnitTests.AM.Text.Hyphenation
{
    [TestClass]
    public class EnglishHyphenatorTest
        : HyphenatorTest
    {
        [TestMethod]
        public void EnglishHyphenator_Hyphenate_1()
        {
            _TestHyphenate<EnglishHyphenator>("here", "he-re");
            _TestHyphenate<EnglishHyphenator>("there", "the-re");
            _TestHyphenate<EnglishHyphenator>("anywhere", "any-w-he-re");
        }

        [TestMethod]
        public void EnglishHyphenator_Hyphenate_2()
        {
            _TestHyphenate<EnglishHyphenator>("HERE", "HERE");
        }

        [TestMethod]
        public void EnglishHyphenator_Hyphenate_3()
        {
            _TestHyphenate<EnglishHyphenator>("The", "The");
        }

        [TestMethod]
        public void EnglishHyphenator_Hyphenate_4()
        {
            _TestHyphenate<EnglishHyphenator>("graffiti", "graf-fiti");

            // Должно быть con-ver-ter
            _TestHyphenate<EnglishHyphenator>("converter", "co-n-ve-r-ter");
        }

        [TestMethod]
        public void EnglishHyphenator_Hyphenate_5()
        {
            _TestHyphenate<EnglishHyphenator>("dislike", "dis-li-ke");

            // Должно быть treat-ment
            _TestHyphenate<EnglishHyphenator>("treatment", "tre-a-t-ment");

            // Должно быть snow-man
            _TestHyphenate<EnglishHyphenator>("snowman", "sno-w-man");
        }

        [TestMethod]
        public void EnglishHyphenator_Hyphenate_6()
        {
            // Должно быть poly-chro-me
            _TestHyphenate<EnglishHyphenator>("polychrome", "po-lyc-hro-me");
        }

        [TestMethod]
        public void EnglishHyphenator_LanguageName_1()
        {
            Hyphenator hyphenator = new EnglishHyphenator();
            Assert.AreEqual("English", hyphenator.LanguageName);
        }
    }
}
