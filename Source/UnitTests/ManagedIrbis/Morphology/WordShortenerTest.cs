using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Morphology;

namespace UnitTests.ManagedIrbis.Morphology
{
    [TestClass]
    public class WordShortenerTest
    {
        private void _TestShorten1
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
            _TestShorten1("периодический", "период.");
            _TestShorten1("социальный", "соц.");
            _TestShorten1("военный", "воен.");
            _TestShorten1("кавказский", "кавк.");
        }
    }
}
