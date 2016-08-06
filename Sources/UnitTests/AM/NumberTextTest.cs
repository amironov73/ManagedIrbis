using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;
using AM.Text;

namespace UnitTests.AM
{
    [TestClass]
    public class NumberTextTest
    {
        [TestMethod]
        public void TestNumberText_Construction()
        {
            NumberText number = new NumberText();
            Assert.AreEqual(true,number.Empty);
            Assert.AreEqual(-1,number.LastIndex);
            Assert.AreEqual(0,number.Length);
            Assert.AreEqual(string.Empty,number.ToString());

            number=new NumberText("hello2");
            Assert.AreEqual(false,number.Empty);
            Assert.AreEqual(0,number.LastIndex);
            Assert.AreEqual(1,number.Length);
            Assert.AreEqual("hello2",number.ToString());

            number = number.Increment();
            Assert.AreEqual("hello3",number.ToString());
        }

        private void _TestSerialization
            (
                NumberText first
            )
        {
            byte[] bytes = first.SaveToMemory();

            NumberText second = bytes
                .RestoreObjectFromMemory<NumberText>();

            Assert.AreEqual(first, second);
        }

        [TestMethod]
        public void TestNumberText_Serialization()
        {
            _TestSerialization("");
            _TestSerialization("1");
            _TestSerialization("a1");
            _TestSerialization("a1b2");
        }

        [TestMethod]
        public void TestNumberText_Arithmetics()
        {
            NumberText number = "a1";
            number = number + 2;

            Assert.AreEqual("a3", number.ToString());
        }

        private void _TestVerify
            (
                NumberText number,
                bool expected
            )
        {
            bool actual = number.Verify(false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestNumberText_Verify()
        {
            _TestVerify("1", true);
            _TestVerify("a1", true);
            _TestVerify("a1b", true);
            _TestVerify("a1b2", true);
        }
    }
}
