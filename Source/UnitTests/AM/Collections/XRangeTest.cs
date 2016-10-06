using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class XRangeTest
    {
        [TestMethod]
        public void TestXRangeConstruction()
        {
            const int Length = 10;
            XRange range = new XRange(Length);

            int[] array = range.ToArray();
            
            Assert.AreEqual(Length, array.Length);
            for (int i = 0; i < Length; i++)
            {
                Assert.AreEqual(i, array[i]);
            }
        }
    }
}
