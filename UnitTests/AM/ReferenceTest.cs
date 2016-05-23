using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class ReferenceTest
    {
        [TestMethod]
        public void TestReferenceConstructor()
        {
            const string expected1 = "abc", expected2 = "cba";
            Reference<string> reference = new Reference<string>(expected1);
            int count = 0;
            reference.TargetChanged += (sender, args) => { count++; };
            Assert.AreEqual(expected1, reference.Target);
            reference.Target = expected2;
            Assert.AreEqual(expected2, reference.Target);
            Assert.AreEqual(1, count);
        }
    }
}
