using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Hyphenation;

namespace UnitTests.AM.Text.Hyphenation
{
    [TestClass]
    public class RussianHyphenatorTest
        : HyphenatorTest
    {

        [TestMethod]
        public void RussianHyphenator_Hyphenate_1()
        {
            _TestHyphenate<RussianHyphenator>("молоко", "мо-ло-ко");
            _TestHyphenate<RussianHyphenator>("окно", "ок-но");
            _TestHyphenate<RussianHyphenator>("два", "два");
            _TestHyphenate<RussianHyphenator>("июнь", "июнь");
        }

        [TestMethod]
        public void RussianHyphenator_Hyphenate_2()
        {
            _TestHyphenate<RussianHyphenator>("ОКПО", "ОКПО");
        }

        [TestMethod]
        public void RussianHyphenator_Hyphenate_3()
        {
            _TestHyphenate<RussianHyphenator>("ага", "ага");
        }

        [TestMethod]
        public void RussianHyphenator_Hyphenate_4()
        {
            _TestHyphenate<RussianHyphenator>("классный", "клас-сный");
            _TestHyphenate<RussianHyphenator>("скользкий", "ско-льз-кий");
        }

        [TestMethod]
        public void RussianHyphenator_Hyphenate_5()
        {
            _TestHyphenate<RussianHyphenator>("майор", "май-ор");
            _TestHyphenate<RussianHyphenator>("безопасность", "без-о-па-с-но-сть");
            _TestHyphenate<RussianHyphenator>("разочарованный", "раз-о-ча-ро-ван-ный");
        }

        [TestMethod]
        public void RussianHyphenator_Hyphenate_6()
        {
            _TestHyphenate<RussianHyphenator>("разыскать", "разы-с-кать");
            _TestHyphenate<RussianHyphenator>("пятиграммовый", "пя-ти-г-рам-мовый");
        }

        [TestMethod]
        public void RussianHyphenator_LanguageName_1()
        {
            Hyphenator hyphenator = new RussianHyphenator();
            Assert.AreEqual("Русский", hyphenator.LanguageName);
        }
    }
}
