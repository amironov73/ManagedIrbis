using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class UniversalComparerTest
    {
        [TestMethod]
        public void TestUniversalComparer()
        {
            UniversalComparer<int> comparer
                = new UniversalComparer<int>
                    (
                        (left, right) => left - right
                    );

            Assert.IsTrue
                (
                    0 ==
                    comparer.Compare(1, 1)
                );
            Assert.IsTrue
                (
                    0 <
                    comparer.Compare(2,1)
                );
            Assert.IsTrue
                (
                    0 > 
                    comparer.Compare(1,2)
                );
        }
    }
}
