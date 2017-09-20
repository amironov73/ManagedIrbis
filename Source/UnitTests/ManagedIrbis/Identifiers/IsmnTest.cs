using System;
using ManagedIrbis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Identifiers;

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public class IsmnTest
    {
        [TestMethod]
        public void Ismn_CheckControlDigit_1()
        {
            Assert.IsTrue
                (
                    Ismn.CheckControlDigit("M-060-11561-5")
                );
            Assert.IsFalse
                (
                    Ismn.CheckControlDigit("M-060-11561-4")
                );
        }

        [TestMethod]
        public void Ismn_CheckControlDigit_2()
        {
            Assert.IsTrue
                (
                    Ismn.CheckControlDigit("979-0-060-11561-5")
                );
            Assert.IsFalse
                (
                    Ismn.CheckControlDigit("979-0-060-11561-8")
                );
        }

        [TestMethod]
        public void Ismn_CheckControlDigit_3()
        {
            Assert.IsTrue
                (
                    Ismn.CheckControlDigit("979-0-9016791-7-7")
                );
            Assert.IsFalse
                (
                    Ismn.CheckControlDigit("979-0-9016791-7-8")
                );
        }

        [TestMethod]
        public void Ismn_CheckControlDigit_4()
        {
            Assert.IsFalse(Ismn.CheckControlDigit(null));
            Assert.IsFalse(Ismn.CheckControlDigit(string.Empty));
        }

        [TestMethod]
        public void Ismn_CheckControlDigit_5()
        {
            Assert.IsFalse
                (
                    Ismn.CheckControlDigit("979-0-9016791-A-7")
                );
            Assert.IsFalse
                (
                    Ismn.CheckControlDigit("N-060-11561-5")
                );
            Assert.IsFalse
                (
                    Ismn.CheckControlDigit("0-M60-11561-5")
                );
        }

        [TestMethod]
        public void Ismn_CheckControlDigit_6()
        {
            Assert.IsFalse
                (
                    Ismn.CheckControlDigit("979-0-9016791-77-7")
                );
        }

        [TestMethod]
        public void Ismn_CheckHyphens_1()
        {
            Assert.IsTrue
                (
                    Ismn.CheckHyphens("M-060-11561-5")
                );
            Assert.IsFalse
                (
                    Ismn.CheckHyphens("M060-11561-5")
                );
            Assert.IsFalse
                (
                    Ismn.CheckHyphens("M 060-11561-5")
                );
            Assert.IsFalse
                (
                    Ismn.CheckHyphens("M-060-1156-15")
                );
        }

        [TestMethod]
        public void Ismn_CheckHyphens_2()
        {
            Assert.IsTrue
                (
                    Ismn.CheckHyphens("979-0-060-11561-5")
                );
            Assert.IsFalse
                (
                    Ismn.CheckHyphens("97-90-060-11561-5")
                );
            Assert.IsFalse
                (
                    Ismn.CheckHyphens("979-0-0-60-11561-5")
                );
            Assert.IsFalse
                (
                    Ismn.CheckHyphens("979-0-060-1156-15")
                );
        }

        [TestMethod]
        public void Ismn_CheckHyphens_3()
        {
            Assert.IsFalse
                (
                    Ismn.CheckHyphens("979-0--60-11561-5")
                );
            Assert.IsFalse
                (
                    Ismn.CheckHyphens("M-0601156--15")
                );
            Assert.IsFalse
                (
                    Ismn.CheckHyphens("-M0601156--15")
                );
            Assert.IsFalse
                (
                    Ismn.CheckHyphens("M--0601156-15")
                );
        }

        [TestMethod]
        public void Ismn_Verify_1()
        {
            Assert.IsTrue
                (
                    Ismn.Verify("M-060-11561-5", false)
                );
            Assert.IsFalse
                (
                    Ismn.Verify("M-060-11561-4", false)
                );
            Assert.IsTrue
                (
                    Ismn.Verify("979-0-060-11561-5", false)
                );
            Assert.IsFalse
                (
                    Ismn.Verify("979-0-060-11561-8", false)
                );
            Assert.IsTrue
                (
                    Ismn.Verify("979-0-9016791-7-7", false)
                );
            Assert.IsFalse
                (
                    Ismn.Verify("979-0-9016791-7-8", false)
                );
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void Ismn_Verify_2()
        {
            Ismn.Verify("M-060-11561-4", true);
        }
    }
}
