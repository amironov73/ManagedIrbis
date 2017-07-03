using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Pft.Infrastructure;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftMaskTest
    {
        private void _TestMatch
            (
                string maskText,
                string value,
                bool expected
            )
        {
            PftMask mask = new PftMask(maskText);
            bool actual = mask.Match(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftMask_Match_1()
        {
            _TestMatch("", "", true);
            _TestMatch("?", "", false);
            _TestMatch("*", "", true);
            _TestMatch("", "1", false);
            _TestMatch("Hello", "Hello", true);
            _TestMatch("Hello", "Hell", false);
            _TestMatch("Hello*", "Hello", true);
            _TestMatch("Hello?", "Hello", false);
            _TestMatch("Hello", "hello", false);
            _TestMatch("Hello|hello", "Hello", true);
            _TestMatch("Hello|hello", "hello", true);
            _TestMatch("Hello|hello", "zello", false);
            _TestMatch("Hel?o", "Hello", true);
            _TestMatch("Hel?o", "Hel_o", true);
            _TestMatch("Hel?o", "hello", false);
            _TestMatch("*123", "", true);
            _TestMatch("*123", "Hello", true);
            _TestMatch("?123", "Hello", false);
        }
    }
}
