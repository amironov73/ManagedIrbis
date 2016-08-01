using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class SubFieldTValueTest
    {
        [TestMethod]
        public void TestSubFieldValue_IsValidValue()
        {
            Assert.IsTrue(SubFieldValue.IsValidValue(null));
            Assert.IsTrue(SubFieldValue.IsValidValue(string.Empty));
            Assert.IsTrue(SubFieldValue.IsValidValue("A"));
            Assert.IsTrue(SubFieldValue.IsValidValue(" "));

            Assert.IsFalse(SubFieldValue.IsValidValue("^"));
            Assert.IsFalse(SubFieldValue.IsValidValue("A^B"));
        }

        private void _TestNormalize
            (
                string expected
            )
        {
            string actual = SubFieldValue.Normalize(expected);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSubFieldValue_Normalize()
        {
            _TestNormalize(null);
            _TestNormalize(string.Empty);
            _TestNormalize("A");
            _TestNormalize(" ");
        }

        [TestMethod]
        public void TestSubFieldValue_Verify()
        {
            Assert.IsTrue(SubFieldValue.Verify(null));
            Assert.IsTrue(SubFieldValue.Verify(string.Empty));
            Assert.IsTrue(SubFieldValue.Verify("A"));
            Assert.IsTrue(SubFieldValue.Verify(" "));

            Assert.IsFalse(SubFieldValue.Verify("^"));
            Assert.IsFalse(SubFieldValue.Verify("A^B"));
        }
    }
}
