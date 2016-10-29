using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Hyphenation;

namespace UnitTests.AM.Text.Hyphenation
{
    [TestClass]
    public class RussianHyphenatorTest
        : HyphenatorTest
    {

        [TestMethod]
        public void RussianHyphenator_Hyphenate1()
        {
            _TestHyphenate<RussianHyphenator>("молоко", "мо-ло-ко");
            _TestHyphenate<RussianHyphenator>("окно", "ок-но");
            _TestHyphenate<RussianHyphenator>("два", "два");
            _TestHyphenate<RussianHyphenator>("июнь", "июнь");
        }

        [TestMethod]
        public void RussianHyphenator_Hyphenate2()
        {
            _TestHyphenate<RussianHyphenator>("ОКПО", "ОКПО");
        }

        [TestMethod]
        public void RussianHyphenator_Hyphenate3()
        {
            _TestHyphenate<RussianHyphenator>("ага", "ага");
        }

        [TestMethod]
        public void RussianHyphenator_LanguageName()
        {
            Hyphenator hyphenator = new RussianHyphenator();
            Assert.AreEqual("Русский", hyphenator.LanguageName);
        }
    }
}
