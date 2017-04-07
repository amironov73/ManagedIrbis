using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.AOT.Stemming;

namespace UnitTests.AM.AOT.Stemming
{
    [TestClass]
    public class RussianStemmerTest
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
        public void RussianStemmer_Stem_1()
        {
            IStemmer stemmer = new RussianStemmer();
            _TestStemmer(stemmer, "красОта", "красот");
            _TestStemmer(stemmer, "красоту", "красот");
            _TestStemmer(stemmer, "красоте", "красот");
            _TestStemmer(stemmer, "КрАсОтОй", "красот");
            _TestStemmer(stemmer, "красот", "красот");
        }
    }
}
