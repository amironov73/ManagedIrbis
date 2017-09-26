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
        public void BuiltinCultures_AmericanEnglish_1()
        {
            CultureInfo culture = BuiltinCultures.AmericanEnglish;
            Assert.IsNotNull(culture);
            Assert.AreEqual("en-US", culture.Name);
        }

        [TestMethod]
        public void BuiltinCultures_Russian_1()
        {
            CultureInfo culture = BuiltinCultures.Russian;
            Assert.IsNotNull(culture);
            Assert.AreEqual("ru-RU", culture.Name);
        }
    }
}
