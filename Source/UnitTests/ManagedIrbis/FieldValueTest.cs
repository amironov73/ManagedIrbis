using System;
using AM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class FieldValueTest
    {
        [TestMethod]
        public void FieldValue_IsValidValue_1()
        {
            Assert.IsTrue(FieldValue.IsValidValue(null));
            Assert.IsTrue(FieldValue.IsValidValue(string.Empty));
            Assert.IsTrue(FieldValue.IsValidValue("A"));
            Assert.IsTrue(FieldValue.IsValidValue("Я"));
            Assert.IsTrue(FieldValue.IsValidValue("У попа была собака"));
            Assert.IsFalse(FieldValue.IsValidValue("У попа ^была собака"));
            Assert.IsFalse(FieldValue.IsValidValue("^"));
        }

        [TestMethod]
        public void FieldValue_Normalize_1()
        {
            Assert.AreEqual(string.Empty, FieldValue.Normalize(string.Empty));
            Assert.AreEqual("Test", FieldValue.Normalize(" Test "));
        }

        [TestMethod]
        public void FieldValue_Verify_1()
        {
            Assert.IsTrue(FieldValue.Verify(null));
            Assert.IsTrue(FieldValue.Verify(string.Empty));
            Assert.IsTrue(FieldValue.Verify("A"));
            Assert.IsTrue(FieldValue.Verify("Я"));
            Assert.IsTrue(FieldValue.Verify("У попа была собака"));
            Assert.IsFalse(FieldValue.Verify("У попа ^была собака"));
            Assert.IsFalse(FieldValue.Verify("^"));
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void FieldValue_Verify_Exception_1()
        {
            FieldValue.Verify("У попа ^была собака", true);
        }
    }
}
