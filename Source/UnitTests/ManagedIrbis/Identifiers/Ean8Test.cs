using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Identifiers;

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public class Ean8Test
    {
        [TestMethod]
        public void Ean8_ComputeCheckDigit_1()
        {
            Assert.AreEqual
                (
                    '3',
                    Ean8.ComputeCheckDigit
                        (
                            "46009333".ToCharArray()
                        )
                );
        }

        [TestMethod]
        public void Ean8_CheckControlDigit_1()
        {
            Assert.IsTrue
                (
                    Ean8.CheckControlDigit
                        (
                            "46009333".ToCharArray()
                        )
                );

            Assert.IsFalse
                (
                    Ean8.CheckControlDigit
                        (
                            "46009332".ToCharArray()
                        )
                );
        }
    }
}
