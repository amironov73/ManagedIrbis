using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class TransliteratorTest
    {
        private void _TestTransliterate
            (
                string word,
                string expected
            )
        {
            string actual = Transliterator.Transliterate(word);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Transliterator_Transliterate()
        {
            _TestTransliterate("", "");
            _TestTransliterate("Hello", "Hello");
            _TestTransliterate("Ого", "Ogo");
            _TestTransliterate("Миронов", "Mironov");
        }
    }
}
