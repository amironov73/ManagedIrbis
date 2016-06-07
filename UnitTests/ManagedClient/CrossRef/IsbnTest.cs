using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient.Identifiers;

namespace UnitTests.ManagedClient
{
    [TestClass]
    public class IsbnTest
    {
        [TestMethod]
        public void TestIsbnCheckHyphens()
        {
            Assert.IsTrue(Isbn.CheckHyphens("5-02-003157-7"));
            Assert.IsTrue(Isbn.CheckHyphens("5-02-003228-X"));
            Assert.IsFalse(Isbn.CheckHyphens("502003228X"));
        }

        [TestMethod]
        public void TestIsbnCheckControlDigit()
        {
            Assert.IsTrue(Isbn.CheckControlDigit("5-02-003206-9"));
            Assert.IsFalse(Isbn.CheckControlDigit("5-02-0032239-5"));
            Assert.IsFalse(Isbn.CheckControlDigit("5-85-202-063-X"));
        }
    }
}
