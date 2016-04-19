using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests
{
    [TestClass]
    public class ReferenceTest
    {
        [TestMethod]
        public void TestReference()
        {
            Reference<int> reference = 10;
            int newValue = 0;
            reference.TargetChanged += (sender, args) =>
            {
                newValue = (Reference<int>) sender;
            };
            reference.Target = 11;
            Assert.AreEqual(reference.Target,newValue);
        }
    }
}
