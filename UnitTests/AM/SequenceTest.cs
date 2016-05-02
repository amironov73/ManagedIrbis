using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests
{
    [TestClass]
    public class SequenceTest
    {
        [TestMethod]
        public void TestSequence()
        {
            int[] array = Sequence.Repeat(5, 3).ToArray();
            Assert.AreEqual(3,array.Length);

            int[] array2 = Sequence.Repeat
                (
                    array.AsEnumerable(),
                    3
                ).ToArray();
            Assert.AreEqual(9, array2.Length);
        }
    }
}
