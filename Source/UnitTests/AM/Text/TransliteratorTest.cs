using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

using JetBrains.Annotations;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class TransliteratorTest
    {
        private void _TestTransliterate
        (
            [NotNull] string word,
            [NotNull] string expected
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
