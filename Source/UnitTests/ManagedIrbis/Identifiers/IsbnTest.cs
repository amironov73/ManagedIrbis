using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Identifiers;

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public class IsbnTest
    {
        [TestMethod]
        public void Isbn_CheckHyphens_1()
        {
            Assert.IsTrue(Isbn.CheckHyphens("5-02-003157-7"));
            Assert.IsTrue(Isbn.CheckHyphens("5-02-003228-X"));
            Assert.IsFalse(Isbn.CheckHyphens("502003228X"));
        }

        [TestMethod]
        public void Isbn_CheckHyphens_2()
        {
            Assert.IsFalse(Isbn.CheckHyphens("5-02--03157-7"));
            Assert.IsFalse(Isbn.CheckHyphens("5-02--0031577"));
        }

        [TestMethod]
        public void Isbn_CheckControlDigit_1()
        {
            Assert.IsTrue(Isbn.CheckControlDigit("5-02-003206-9"));
            Assert.IsFalse(Isbn.CheckControlDigit("5-02-0032239-5"));
            Assert.IsFalse(Isbn.CheckControlDigit("5-85-202-063-X"));
        }

        [TestMethod]
        public void Isbn_CheckControlDigit_2()
        {
            Assert.IsTrue(Isbn.CheckControlDigit("5-01-001033-X"));
            Assert.IsFalse(Isbn.CheckControlDigit("5-01-00103X-3"));
        }

        [TestMethod]
        public void Isbn_CheckControlDigit_3()
        {
            Assert.IsFalse(Isbn.CheckControlDigit("5-01-00A033-X"));
        }

        [TestMethod]
        public void Isbn_ToEan13_1()
        {
            Assert.AreEqual
                (
                    "9785020032064",
                    Isbn.ToEan13("5-02-003206-9")
                );
        }

        [TestMethod]
        public void Isbn_ToEan13_2()
        {
            Assert.IsNull(Isbn.ToEan13(null));
            Assert.IsNull(Isbn.ToEan13(string.Empty));
            Assert.IsNull(Isbn.ToEan13("123"));
        }

        [TestMethod]
        public void Isbn_FromEan13_1()
        {
            Assert.AreEqual
                (
                    "5-020-03206-9",
                    Isbn.FromEan13("9785020032064")
                );
        }

        [TestMethod]
        public void Isbn_FromEan13_2()
        {
            Assert.IsNull(Isbn.FromEan13(null));
            Assert.IsNull(Isbn.FromEan13(string.Empty));
            Assert.IsNull(Isbn.FromEan13("123"));
        }

        [TestMethod]
        public void Isbn_Validate_1()
        {
            Assert.IsTrue(Isbn.Validate("5-02-003206-9", false));
            Assert.IsFalse(Isbn.Validate("5-02-0032239-5", false));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Isbn_Validate_2()
        {
            Isbn.Validate("5-02-0032239-5", true);
        }
    }
}
