using System;
using System.Collections.Generic;
using AM;
using AM.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM
{
    [TestClass]
    public class UtilityTest
    {
        [TestMethod]
        public void Utility_DumpBytes_1()
        {
            Assert.AreEqual
                (
                    string.Empty,
                    Utility.DumpBytes(new byte[0])
                        .DosToUnix()
                );

            Assert.AreEqual
                (
                    "000000: 01\n",
                    Utility.DumpBytes(new byte[]{0x01})
                        .DosToUnix()
                );

            Assert.AreEqual
                (
                    "000000: 01 02 03 04 05 06 07 08 09 0A 0B 0B 0C 0D 0E 0F\n000010: 10 11 12\n",
                    Utility.DumpBytes(new byte[] { 0x01, 0x02, 0x03,
                        0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B,
                        0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12
                        })
                        .DosToUnix()
                );
        }

        [TestMethod]
        public void Utility_EnumerableEquals_1()
        {
            byte[] first = {1, 2, 3};
            byte[] second = {1, 2, 3};
            byte[] third = {1, 2};

            Assert.IsTrue(Utility.EnumerableEquals(first, second));
            Assert.IsTrue(Utility.EnumerableEquals(first, first));
            Assert.IsFalse(Utility.EnumerableEquals(first, third));
            Assert.IsFalse(Utility.EnumerableEquals(first, null));
            Assert.IsFalse(Utility.EnumerableEquals(null, null));
        }

        [TestMethod]
        public void Utility_GetItem_1()
        {
            byte[] array = {1, 2, 3};

            Assert.AreEqual(1, array.GetItem(0));
            Assert.AreEqual(2, array.GetItem(1));
            Assert.AreEqual(3, array.GetItem(2));
            Assert.AreEqual(0, array.GetItem(3));
            Assert.AreEqual(3, array.GetItem(-1));
        }

        [TestMethod]
        public void Utility_GetItem_2()
        {
            List<byte> list = new List<byte>{1,2,3};

            Assert.AreEqual(1, list.GetItem(0));
            Assert.AreEqual(2, list.GetItem(1));
            Assert.AreEqual(3, list.GetItem(2));
            Assert.AreEqual(0, list.GetItem(3));
            Assert.AreEqual(3, list.GetItem(-1));
        }

        [TestMethod]
        public void Utility_IsOneOf_1()
        {
            Assert.IsTrue(1.OneOf(1,2,3));
            Assert.IsTrue(2.OneOf(1,2,3));
            Assert.IsTrue(3.OneOf(1,2,3));
            Assert.IsFalse(0.OneOf(1,2,3));
        }
    }
}
