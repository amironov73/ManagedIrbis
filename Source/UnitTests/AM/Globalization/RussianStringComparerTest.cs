using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Globalization;

namespace UnitTests.AM.Globalization
{
    [TestClass]
    public class RussianStringComparerTest
    {
        private void _TestCompare1
            (
                int expected,
                string str1,
                string str2
            )
        {
            RussianStringComparer comparer
                = new RussianStringComparer(true, false);

            int actual = comparer.Compare(str1, str2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RussianStringComparer_Compare1()
        {
            _TestCompare1(0, "ежик", "ёжик");
            _TestCompare1(-1, "ежик", "ножик");
        }

        private void _TestCompare2
            (
                int expected,
                string str1,
                string str2
            )
        {
            RussianStringComparer comparer
                = new RussianStringComparer(true, true);

            int actual = comparer.Compare(str1, str2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RussianStringComparer_Compare2()
        {
            _TestCompare2(0, "ежик", "ЁЖИК");
            _TestCompare2(-1, "ежик", "НОЖИК");
        }

        private void _TestEquals1
            (
                bool expected,
                string str1,
                string str2
            )
        {
            RussianStringComparer comparer
                = new RussianStringComparer(true, false);

            bool actual = comparer.Equals(str1, str2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RussianStringComparer_Equals1()
        {
            _TestEquals1(true, "ежик", "ёжик");
            _TestEquals1(false, "ежик", "ножик");
        }

        private void _TestEquals2
            (
                bool expected,
                string str1,
                string str2
            )
        {
            RussianStringComparer comparer
                = new RussianStringComparer(true, true);

            bool actual = comparer.Equals(str1, str2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RussianStringComparer_Equals2()
        {
            _TestEquals2(true, "ежик", "ЁЖИК");
            _TestEquals2(false, "ежик", "НОЖИК");
        }

        [TestMethod]
        public void RussianStringComparer_GetHashCode1()
        {
            RussianStringComparer comparer
                = new RussianStringComparer(false, false);

            Assert.AreEqual(0, comparer.GetHashCode(null));

            Assert.AreNotEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ёжик")
                );

            Assert.AreNotEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ЕЖИК")
                );

            Assert.AreNotEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ножик")
                );
        }

        [TestMethod]
        public void RussianStringComparer_GetHashCode2()
        {
            RussianStringComparer comparer
                = new RussianStringComparer(false, true);

            Assert.AreEqual(0, comparer.GetHashCode(null));

            Assert.AreNotEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ёжик")
                );

            Assert.AreEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ЕЖИК")
                );

            Assert.AreNotEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ножик")
                );
        }

        [TestMethod]
        public void RussianStringComparer_GetHashCode3()
        {
            RussianStringComparer comparer
                = new RussianStringComparer(true, false);

            Assert.AreEqual(0, comparer.GetHashCode(null));

            Assert.AreEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ёжик")
                );

            Assert.AreNotEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ЕЖИК")
                );

            Assert.AreNotEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ножик")
                );
        }

        [TestMethod]
        public void RussianStringComparer_GetHashCode4()
        {
            RussianStringComparer comparer
                = new RussianStringComparer(true, true);

            Assert.AreEqual(0, comparer.GetHashCode(null));

            Assert.AreEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ёжик")
                );

            Assert.AreEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ЕЖИК")
                );

            Assert.AreNotEqual
                (
                    comparer.GetHashCode("ежик"),
                    comparer.GetHashCode("ножик")
                );
        }
    }
}
