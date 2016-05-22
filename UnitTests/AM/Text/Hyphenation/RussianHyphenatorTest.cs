using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Hyphenation;

namespace UnitTests.AM.Text.Hyphenation
{
    [TestClass]
    public class RussianHyphenatorTest
        : HyphenatorTest
    {

        [TestMethod]
        public void TestRussianHyphenator()
        {
            _TestHyphenate<RussianHyphenator>("молоко", "мо-ло-ко");
            _TestHyphenate<RussianHyphenator>("окно", "ок-но");
            _TestHyphenate<RussianHyphenator>("два", "два");
            _TestHyphenate<RussianHyphenator>("июнь", "июнь");
        }
    }
}
