using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class InvariantFormatTest
    {
        [TestMethod]
        public void TestInvariantFormat()
        {
            Assert.AreEqual
                (
                    "10000",
                    InvariantFormat.Format(10000)
                );

            Assert.AreEqual
                (
                    "10000",
                    InvariantFormat.Format(10000.0)
                );
        }
    }
}
