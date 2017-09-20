using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Identifiers;

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public class IssnTest
    {
        [TestMethod]
        public void Issn_ComputeCheckDigit_1()
        {
            Assert.AreEqual
                (
                    'X',
                    Issn.ComputeCheckDigit
                    (
                        "0033765X".ToCharArray()
                    )
                );
        }

        [TestMethod]
        public void Issn_CheckControlDigit_1()
        {
            Assert.IsTrue
                (
                    Issn.CheckControlDigit
                        (
                            "0033765X".ToCharArray()
                        )
                );

            Assert.IsFalse
                (
                    Issn.CheckControlDigit
                        (
                            "00337651".ToCharArray()
                        )
                );
        }

        [TestMethod]
        public void Issn_CheckControlDigit_2()
        {
            Assert.IsFalse(Issn.CheckControlDigit("003376X".ToCharArray()));
        }

        [TestMethod]
        public void Issn_CheckControlDigit_3()
        {
            Assert.IsTrue(Issn.CheckControlDigit("0033765X"));
            Assert.IsFalse(Issn.CheckControlDigit("00337651"));
        }
    }
}
