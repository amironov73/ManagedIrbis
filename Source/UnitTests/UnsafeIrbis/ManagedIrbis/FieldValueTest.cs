using Microsoft.VisualStudio.TestTools.UnitTesting;

using UnsafeIrbis;

namespace UnitTests.UnsafeIrbis
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
    }
}
