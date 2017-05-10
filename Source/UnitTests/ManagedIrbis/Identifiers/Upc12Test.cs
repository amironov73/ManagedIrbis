using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Identifiers;

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public class Upc12Test
    {
        [TestMethod]
        public void Upc12_ComputeCheckDigit_1()
        {
            Assert.AreEqual
                (
                    '4',
                    Upc12.ComputeCheckDigit
                        (
                            "041689300494".ToCharArray()
                        )
                );
        }

        [TestMethod]
        public void Upc12_CheckControlDigit_1()
        {
            Assert.IsTrue
                (
                    Upc12.CheckControlDigit
                        (
                            "041689300494".ToCharArray()
                        )
                );

            Assert.IsFalse
                (
                    Upc12.CheckControlDigit
                        (
                            "041689300493".ToCharArray()
                        )
                );
        }
    }
}
