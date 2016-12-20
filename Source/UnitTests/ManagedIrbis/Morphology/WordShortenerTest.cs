using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Morphology;

namespace UnitTests.ManagedIrbis.Morphology
{
    [TestClass]
    public class WordShortenerTest
    {
        private void _TestShortenByStandardFullEnding
            (
                string word,
                string expected
            )
        {
            string actual = WordShortener.ShortenByStandardFullEnding(word);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordShortener_ShortenByStandardFullEnding()
        {
            _TestShortenByStandardFullEnding("периодический", "период.");
            _TestShortenByStandardFullEnding("социальный", "соц.");
            _TestShortenByStandardFullEnding("военный", "воен.");
            _TestShortenByStandardFullEnding("кавказский", "кавк.");
        }

        private void _TestShortenByGost
            (
                string word,
                string expected
            )
        {
            string actual = WordShortener.ShortenByGost(word);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordShortener_ShortenByGost()
        {
            _TestShortenByGost("периодический", "периодический");
            _TestShortenByGost("хозяйство", "хоз-во");
            _TestShortenByGost("военный", "военный");
            _TestShortenByGost("страница", "с.");
        }
    }
}
