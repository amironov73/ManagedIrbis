using AM;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class FieldTagTest
    {
        [TestMethod]
        public void FieldTag_IsValidTag_1()
        {
            Assert.IsFalse(FieldTag.IsValidTag(null));
            Assert.IsFalse(FieldTag.IsValidTag(string.Empty));
            Assert.IsFalse(FieldTag.IsValidTag("0"));
            Assert.IsFalse(FieldTag.IsValidTag("00"));
            Assert.IsFalse(FieldTag.IsValidTag("000"));
            Assert.IsTrue(FieldTag.IsValidTag("1"));
            Assert.IsTrue(FieldTag.IsValidTag("01"));
            Assert.IsFalse(FieldTag.IsValidTag("A"));
            Assert.IsFalse(FieldTag.IsValidTag("0A"));
            Assert.IsFalse(FieldTag.IsValidTag("Ф"));
            Assert.IsFalse(FieldTag.IsValidTag("0Ф"));
        }

        private void _TestNormalize
            (
                string source,
                string expected
            )
        {
            string actual = FieldTag.Normalize(source);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FieldTag_Normalize_1()
        {
            _TestNormalize(null, null);
            _TestNormalize(string.Empty, string.Empty);
            _TestNormalize("0", "0");
            _TestNormalize("00", "0");
            _TestNormalize("000", "0");
            _TestNormalize("1", "1");
            _TestNormalize("01", "1");
            _TestNormalize("A", "A");
            _TestNormalize("0A", "A");
            _TestNormalize("Ф", "Ф");
            _TestNormalize("0Ф", "Ф");
        }

        [TestMethod]
        public void FieldTag_Verify_1()
        {
            Assert.IsFalse(FieldTag.Verify(null));
            Assert.IsFalse(FieldTag.Verify(string.Empty));
            Assert.IsFalse(FieldTag.Verify("0"));
            Assert.IsFalse(FieldTag.Verify("00"));
            Assert.IsFalse(FieldTag.Verify("000"));
            Assert.IsTrue(FieldTag.Verify("1"));
            Assert.IsTrue(FieldTag.Verify("01"));
            Assert.IsFalse(FieldTag.Verify("A"));
            Assert.IsFalse(FieldTag.Verify("0A"));
            Assert.IsFalse(FieldTag.Verify("Ф"));
            Assert.IsFalse(FieldTag.Verify("0Ф"));
        }

        [TestMethod]
        public void FieldTag_Verify_2()
        {
            Assert.IsTrue(FieldTag.Verify(100, false));
            Assert.IsFalse(FieldTag.Verify(0, false));
            Assert.IsFalse(FieldTag.Verify(-100, false));
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void FieldTag_Verify_Exception_1()
        {
            FieldTag.Verify("Ф", true);
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void FieldTag_Verify_Exception_2()
        {
            FieldTag.Verify(-100, true);
        }
    }
}
