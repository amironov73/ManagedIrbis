using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class UniversalComparerTest
    {
        [TestMethod]
        public void UniversalComparer_Compare()
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

        [TestMethod]
        public void UniversalComparer_Function()
        {
            UniversalComparer<int> comparer
                = new UniversalComparer<int>
                    (
                        (left, right) => left - right
                    );
            Func<int, int, int> func = comparer.Function;

            Assert.IsTrue
                (
                    0 ==
                    func(1, 1)
                );
            Assert.IsTrue
                (
                    0 <
                    func(2, 1)
                );
            Assert.IsTrue
                (
                    0 >
                    func(1, 2)
                );
        }
    }
}
