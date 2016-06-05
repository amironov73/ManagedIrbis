using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Globalization;

namespace UnitTests.AM.Globalization
{
    [TestClass]
    public class BuiltinCulturesTest
    {
        [TestMethod]
        public void TestBuiltinCulturesAmericanEnglish()
        {
            CultureInfo culture = BuiltinCultures.AmericanEnglish;
            Assert.IsNotNull(culture);
            Assert.AreEqual("en-US", culture.Name);
        }

        [TestMethod]
        public void TestBuiltintCulturesRussian()
        {
            CultureInfo culture = BuiltinCultures.Russian;
            Assert.IsNotNull(culture);
            Assert.AreEqual("ru-RU", culture.Name);
        }
    }
}
