using System;
using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class XRangeTest
    {
        [TestMethod]
        public void XRange_Construction_1()
        {
            const int Length = 10;
            XRange range = new XRange(Length);
            Assert.AreEqual(0, range.Start);
            Assert.AreEqual(Length, range.Length);
        }

        [TestMethod]
        public void XRange_Construction_2()
        {
            const int Start = 1;
            const int Length = 10;
            XRange range = new XRange(Start, Length);
            Assert.AreEqual(Start, range.Start);
            Assert.AreEqual(Length, range.Length);
        }

        [TestMethod]
        public void XRange_Enumeration_1()
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

        [TestMethod]
        public void XRange_Enumeration_2()
        {
            const int Length = 10;
            XRange range = new XRange(Length);

            IEnumerator enumerator = range.GetEnumerator();
            Assert.IsNotNull(enumerator);
            for (int i = 0; i < Length; i++)
            {
                Assert.IsTrue(enumerator.MoveNext());
                int item = (int) enumerator.Current;
                Assert.AreEqual(i, item);
            }
            Assert.IsFalse(enumerator.MoveNext());
        }
    }
}
