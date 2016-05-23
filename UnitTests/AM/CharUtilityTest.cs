using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class CharUtilityTest
    {
        [TestMethod]
        public void TestCharUtilityIsArabicDigit()
        {
            Assert.IsTrue('0'.IsArabicDigit());
            Assert.IsTrue('9'.IsArabicDigit());
            Assert.IsFalse('A'.IsArabicDigit());
            Assert.IsFalse('\x0661'.IsArabicDigit());
        }

        [TestMethod]
        public void TestCharUtilityIsLatinLetter()
        {
            Assert.IsTrue('A'.IsLatinLetter());
            Assert.IsTrue('z'.IsLatinLetter());
            Assert.IsFalse('А'.IsLatinLetter());
            Assert.IsFalse('Я'.IsLatinLetter());
        }

        [TestMethod]
        public void TestCharUtilityIsLatinLetterOrArabicDigit()
        {
            Assert.IsTrue('0'.IsLatinLetterOrArabicDigit());
            Assert.IsTrue('9'.IsLatinLetterOrArabicDigit());
            Assert.IsTrue('A'.IsLatinLetterOrArabicDigit());
            Assert.IsFalse('\x0661'.IsLatinLetterOrArabicDigit());
            Assert.IsTrue('A'.IsLatinLetterOrArabicDigit());
            Assert.IsTrue('z'.IsLatinLetterOrArabicDigit());
            Assert.IsFalse('А'.IsLatinLetterOrArabicDigit());
            Assert.IsFalse('Я'.IsLatinLetterOrArabicDigit());
        }

        [TestMethod]
        public void TestCharUtilityIsRussianLetter()
        {
            Assert.IsTrue('А'.IsRussianLetter());
            Assert.IsTrue('Я'.IsRussianLetter());
            Assert.IsFalse('A'.IsRussianLetter());
            Assert.IsFalse('0'.IsRussianLetter());
        }

    }
}
