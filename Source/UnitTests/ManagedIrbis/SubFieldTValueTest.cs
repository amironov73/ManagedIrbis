using AM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class SubFieldTValueTest
    {
        [TestMethod]
        public void SubFieldValue_IsValidValue_1()
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
        public void SubFieldValue_Normalize_1()
        {
            _TestNormalize(null);
            _TestNormalize(string.Empty);
            _TestNormalize("A");
            _TestNormalize(" ");
        }

        [TestMethod]
        public void SubFieldValue_Verify_1()
        {
            Assert.IsTrue(SubFieldValue.Verify(null));
            Assert.IsTrue(SubFieldValue.Verify(string.Empty));
            Assert.IsTrue(SubFieldValue.Verify("A"));
            Assert.IsTrue(SubFieldValue.Verify(" "));

            Assert.IsFalse(SubFieldValue.Verify("^"));
            Assert.IsFalse(SubFieldValue.Verify("A^B"));
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void SubFieldValue_ThrowOnVerify_1()
        {
            bool save = SubFieldValue.ThrowOnVerify;
            SubFieldValue.ThrowOnVerify = true;
            try
            {
                SubFieldValue.Verify("^");
            }
            finally
            {
                SubFieldValue.ThrowOnVerify = save;
            }
        }
    }
}
