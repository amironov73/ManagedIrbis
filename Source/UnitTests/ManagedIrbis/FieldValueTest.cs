using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class FieldValueTest
    {
        [TestMethod]
        public void TestFieldValue_IsValidValue()
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
        public void TestFieldValue_Verify()
        {
            Assert.IsTrue(FieldValue.Verify(null));
            Assert.IsTrue(FieldValue.Verify(string.Empty));
            Assert.IsTrue(FieldValue.Verify("A"));
            Assert.IsTrue(FieldValue.Verify("Я"));
            Assert.IsTrue(FieldValue.Verify("У попа была собака"));
            Assert.IsFalse(FieldValue.Verify("У попа ^была собака"));
            Assert.IsFalse(FieldValue.Verify("^"));
        }
    }
}
