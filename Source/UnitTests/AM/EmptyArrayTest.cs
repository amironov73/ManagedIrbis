using AM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM
{
    [TestClass]
    public class EmptyArrayTest
    {
        [TestMethod]
        public void EmptyArray_Value_1()
        {
            int[] empty = EmptyArray<int>.Value;
            Assert.IsNotNull(empty);
            Assert.AreEqual(0, empty.Length);
        }

        [TestMethod]
        public void EmptyArray_Value_2()
        {
            string[] empty = EmptyArray<string>.Value;
            Assert.IsNotNull(empty);
            Assert.AreEqual(0, empty.Length);
        }
    }
}
