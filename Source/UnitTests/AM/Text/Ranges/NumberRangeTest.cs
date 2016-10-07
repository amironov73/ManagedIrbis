using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;
using AM.Text;
using AM.Text.Ranges;

namespace UnitTests.AM.Text.Ranges
{
    [TestClass]
    public class NumberRangeTest
    {
        [TestMethod]
        public void TestNumberRange_Constructor()
        {
            NumberRange range = new NumberRange("10", "15");
            Assert.AreEqual("10-15", range.ToString());
            Assert.AreEqual(true, range.Contains("12"));
            Assert.AreEqual(false, range.Contains("18"));
        }

        [TestMethod]
        public void TestNumberRange_Parse1()
        {
            NumberRange range = NumberRange.Parse("10-15");
            Assert.AreEqual(range.Start, new NumberText("10"));
            Assert.AreEqual(range.Stop, new NumberText("15"));
            Assert.AreEqual(true, range.Contains("12"));
            Assert.AreEqual(false, range.Contains("18"));
        }

        [TestMethod]
        public void TestNumberRange_Parse2()
        {
            NumberRange range = NumberRange.Parse(" 10-15");
            Assert.AreEqual(range.Start, new NumberText("10"));
            Assert.AreEqual(range.Stop, new NumberText("15"));
            Assert.AreEqual(true, range.Contains("12"));
            Assert.AreEqual(false, range.Contains("18"));
        }

        [TestMethod]
        public void TestNumberRange_Parse3()
        {
            NumberRange range = NumberRange.Parse("10-15 ");
            Assert.AreEqual(range.Start, new NumberText("10"));
            Assert.AreEqual(range.Stop, new NumberText("15"));
            Assert.AreEqual(true, range.Contains("12"));
            Assert.AreEqual(false, range.Contains("18"));
        }

        [TestMethod]
        public void TestNumberRange_Parse4()
        {
            NumberRange range = NumberRange.Parse("10 - 15");
            Assert.AreEqual(range.Start, new NumberText("10"));
            Assert.AreEqual(range.Stop, new NumberText("15"));
            Assert.AreEqual(true, range.Contains("12"));
            Assert.AreEqual(false, range.Contains("18"));
        }

        [TestMethod]
        public void TestNumberRange_Parse5()
        {
            NumberRange range = NumberRange.Parse(" 10 - 15 ");
            Assert.AreEqual(range.Start, new NumberText("10"));
            Assert.AreEqual(range.Stop, new NumberText("15"));
            Assert.AreEqual(true, range.Contains("12"));
            Assert.AreEqual(false, range.Contains("18"));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestNumberRange_Parse_Exception1()
        {
            NumberRange range = NumberRange.Parse("10-15-");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestNumberRange_Parse_Exception2()
        {
            NumberRange range = NumberRange.Parse("-10-15");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestNumberRange_Parse_Exception3()
        {
            NumberRange range = NumberRange.Parse("10--15");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestNumberRange_Parse_Exception4()
        {
            NumberRange range = NumberRange.Parse("-");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestNumberRange_Parse_Exception5()
        {
            NumberRange range = NumberRange.Parse(";");
        }


        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestNumberRange_Parse_Exception6()
        {
            NumberRange range = NumberRange.Parse(";-");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNumberRange_Parse_Exception7()
        {
            NumberRange range = NumberRange.Parse(string.Empty);
        }

        [TestMethod]
        public void TestNumberRange_Equals()
        {
            NumberRange left = new NumberRange("10", "20");
            NumberRange right = new NumberRange("10", "20");

            Assert.IsTrue(left.Equals(right));

            right.Stop = "200";

            Assert.IsFalse(left.Equals(right));
        }

        private void _TestSerialization
            (
                string text
            )
        {
            NumberRange first = NumberRange.Parse(text);
            byte[] bytes = first.SaveToMemory();
            NumberRange second = bytes
                .RestoreObjectFromMemory<NumberRange>();

            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void TestNumberRange_Serialization()
        {
            _TestSerialization("1");
            _TestSerialization("a1");
            _TestSerialization("a1b");
            _TestSerialization("a1b2");
        }

        [TestMethod]
        public void TestNumberRange_Verify()
        {
            NumberRange range = new NumberRange();
            Assert.IsFalse(range.Verify(false));

            range.Start = "10";
            Assert.IsFalse(range.Verify(false));

            range.Stop = "20";
            Assert.IsTrue(range.Verify(false));

            range.Stop = "2";
            Assert.IsFalse(range.Verify(false));
        }
    }
}
