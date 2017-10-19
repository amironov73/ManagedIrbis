using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class BooleanUtilityTest
    {
        [TestMethod]
        public void BooleanUtility_TryParse_1()
        {
            bool value;
            Assert.IsTrue(BooleanUtility.TryParse("true", out value));
            Assert.IsTrue(value);

            Assert.IsTrue(BooleanUtility.TryParse("false", out value));
            Assert.IsFalse(value);

            Assert.IsFalse(BooleanUtility.TryParse("unknown", out value));
        }
    }
}
