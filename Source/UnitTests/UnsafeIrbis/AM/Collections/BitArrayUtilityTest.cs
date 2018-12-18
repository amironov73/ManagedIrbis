using System.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UnsafeAM.Collections;

namespace UnitTests.UnsafeAM.Collections
{
    [TestClass]
    public class BitArrayUtilityTest
    {
        [TestMethod]
        public void BitArrayUtility_AreEqual_1()
        {
            BitArray left = new BitArray(10);
            left[1] = true;

            BitArray right = new BitArray(10);
            right[1] = true;

            Assert.IsTrue(BitArrayUtility.AreEqual(left, right));

            right[2] = true;
            Assert.IsFalse(BitArrayUtility.AreEqual(left, right));

            right = new BitArray(11);
            right[1] = true;
            Assert.IsFalse(BitArrayUtility.AreEqual(left, right));
        }
    }
}
