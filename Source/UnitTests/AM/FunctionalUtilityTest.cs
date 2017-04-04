using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class FunctionalUtilityTest
    {
        [TestMethod]
        public void FunctionalUtility_PipeTo_1()
        {
            const string expected = "100";
            string actual = 10
                .PipeTo(x => x * x)
                .PipeTo(x => x.ToInvariantString());
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FunctionalUtility_Either_1()
        {
            const string expected = "Positive 10";
            string actual = 10
                .Either
                (
                    x => x > 0,
                    x => "Positive " + x,
                    x => "Negative " + x
                );
            Assert.AreEqual(expected, actual);
        }
    }
}
