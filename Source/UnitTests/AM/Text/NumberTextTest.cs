using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class NumberTextTest
    {
        [TestMethod]
        public void NumberText_Construction1()
        {
            NumberText number = new NumberText();
            Assert.IsTrue(number.Empty);
        }

        [TestMethod]
        public void NumberText_Construction2()
        {
            NumberText number = new NumberText("hello1");
            Assert.IsFalse(number.Empty);
        }

        [TestMethod]
        public void NumberText_Enumeration1()
        {
            NumberText number = new NumberText();
            string[] array = number.ToArray();
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        public void NumberText_Enumeration2()
        {
            NumberText number = new NumberText("hello1");
            string[] array = number.ToArray();
            Assert.AreEqual(1, array.Length);
        }

        [TestMethod]
        public void NumberText_Enumeration3()
        {
            NumberText number = new NumberText("hello1goodbye2");
            string[] array = number.ToArray();
            Assert.AreEqual(2, array.Length);
        }

        [TestMethod]
        public void NumberText_Comparison1()
        {
            NumberText first = "hello2";
            NumberText second = "hello10";
            Assert.IsTrue(first.CompareTo(second) < 0);
        }

        [TestMethod]
        public void NumberText_Comparison2()
        {
            NumberText first = "hello2";
            NumberText second = "hello010";
            Assert.IsTrue(first.CompareTo(second) < 0);
        }

        [TestMethod]
        public void NumberText_Comparison3()
        {
            NumberText first = "20";
            NumberText second = "21";
            Assert.IsTrue(first.CompareTo(second) < 0);
        }

        [TestMethod]
        public void NumberText_Comparison4()
        {
            NumberText first = "20";
            NumberText second = "21";
            Assert.IsFalse(second.CompareTo(first) <= 0);
        }

        [TestMethod]
        public void NumberText_Increment1()
        {
            NumberText number = "hello2";
            number.Increment();
            Assert.AreEqual("hello3", number.ToString());
        }

        [TestMethod]
        public void NumberText_Increment2()
        {
            NumberText number = "hello2goodbye1";
            number.Increment();
            Assert.AreEqual("hello2goodbye2", number.ToString());
        }

        [TestMethod]
        public void NumberText_Increment3()
        {
            NumberText number = "hello2goodbye1";
            number.Increment(0, 1);
            Assert.AreEqual("hello3goodbye1", number.ToString());
        }

        [TestMethod]
        public void NumberText_Max1()
        {
            NumberText first = "hello2";
            NumberText second = "hello010";
            NumberText max = NumberText.Max(first, second);
            Assert.AreEqual("hello010", max.ToString());
        }

        [TestMethod]
        public void NumberText_Min1()
        {
            NumberText first = "hello2";
            NumberText second = "hello010";
            NumberText min = NumberText.Min(first, second);
            Assert.AreEqual("hello2", min.ToString());
        }

        [TestMethod]
        public void NumberText_EqualityOperator()
        {
            const string text = "hello2";
            NumberText number = text;
            Assert.IsTrue(number == text);
        }
    }
}
