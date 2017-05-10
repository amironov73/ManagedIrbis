using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Identifiers;

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public class Ean13Test
    {
        [TestMethod]
        public void Ean13_ComputeCheckDigit_1()
        {
            Assert.AreEqual
                (
                    '7',
                    Ean13.ComputeCheckDigit
                        (
                            "4600051000057".ToCharArray()
                        )
                );
        }

        [TestMethod]
        public void Ean13_CheckControlDigit_1()
        {
            Assert.IsTrue
                (
                    Ean13.CheckControlDigit
                        (
                            "4600051000057".ToCharArray()
                        )
                );

            Assert.IsFalse
                (
                    Ean13.CheckControlDigit
                        (
                            "4600051000056".ToCharArray()
                        )
                );
        }
    }
}
