using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.AOT.Stemming;

namespace UnitTests.AM.AOT.Stemming
{
    [TestClass]
    public class EnglishStemmerTest
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
        public void EnglishStemmer_Stem_1()
        {
            IStemmer stemmer = new EnglishStemmer();
            _TestStemmer(stemmer, "jUmp", "jump");
            _TestStemmer(stemmer, "jumping", "jump");
            _TestStemmer(stemmer, "Jumps", "jump");
            _TestStemmer(stemmer, "jumps", "jump");
        }
    }
}
