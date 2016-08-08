using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

namespace UnitTests.AM.Runtime
{
    [TestClass]
    public class NumberUnionTest
    {
        [TestMethod]
        public void TestNumberUnion()
        {
            NumberUnion number = new NumberUnion(1);
            Assert.AreEqual(1, number.SignedInt16);
            Assert.AreEqual(1, number.SignedInt64);
        }
    }
}
