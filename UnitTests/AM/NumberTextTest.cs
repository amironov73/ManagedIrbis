using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM
{
    [TestClass]
    public class NumberTextTest
    {
        [TestMethod]
        public void TestNumberText()
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
    }
}
