using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Hyphenation;

namespace UnitTests.AM.Text.Hyphenation
{
    [TestClass]
    public class HyphenatorTest
    {
        protected void _TestHyphenate<T>
            (
                string word,
                string expected
            )
            where T: Hyphenator, new()
        {
            Hyphenator hyphenator = new T();

            int[] positions = hyphenator.Hyphenate(word);
            string actual = Hyphenator.ShowHyphenated
                (
                    word,
                    positions
                );

            Assert.AreEqual(expected, actual);
        }
    }
}
