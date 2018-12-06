using Microsoft.VisualStudio.TestTools.UnitTesting;

using UnsafeIrbis;

namespace UnitTests.UnsafeIrbis
{
    [TestClass]
    public class FieldTagTest
    {
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
            _TestNormalize("10", "10");
            _TestNormalize("01", "1");
            _TestNormalize("A", "A");
            _TestNormalize("0A", "A");
            _TestNormalize("Ф", "Ф");
            _TestNormalize("0Ф", "Ф");
        }
    }
}
