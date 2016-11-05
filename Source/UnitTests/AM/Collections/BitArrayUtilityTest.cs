using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class BitArrayUtilityTest
    {
        [TestMethod]
        public void BitArrayUtility_AreEqual()
        {
            BitArray left = new BitArray(10);
            left[1] = true;

            BitArray right = new BitArray(10);
            right[1] = true;

            Assert.IsTrue
                (
                    BitArrayUtility.AreEqual
                    (
                        left,
                        right
                    )
                );

            right[2] = true;

            Assert.IsFalse
                (
                    BitArrayUtility.AreEqual
                    (
                        left,
                        right
                    )
                );
        }
    }
}
