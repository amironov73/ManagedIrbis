using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class RussianFormatTest
    {
        [TestMethod]
        public void TestRussianFormat()
        {
            Assert.AreEqual
                (
                    "10000",
                    RussianFormat.Format(10000)
                );

            Assert.AreEqual
                (
                    "10000",
                    RussianFormat.Format(10000.0)
                );
        }
    }
}
