using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.AOT.Stemming;

namespace UnitTests.AM.AOT.Stemming
{
    [TestClass]
    public class GermanStemmerTest
    {
        private void _TestStemmer
            (
                IStemmer stemmer,
                string word,
                string expected
            )
        {
            string actual = stemmer.Stem(word);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GermanStemmer_Stem_1()
        {
            IStemmer stemmer = new GermanStemmer();
            _TestStemmer(stemmer, "mochte", "mocht");
            _TestStemmer(stemmer, "mochtest", "mocht");
            _TestStemmer(stemmer, "mochten", "mocht");
            _TestStemmer(stemmer, "mochtet", "mochtet");
        }
    }
}
