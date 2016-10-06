using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Threading;

namespace UnitTests.AM.Threading
{
    //[TestClass]
    //[Ignore]
    public class StateGuardTest
    {
        [TestMethod]
        public void TestStateGuard()
        {
            const int expected = 1;
            const int nonExpected = 2;

            StateHolder<int> holder = expected;

            using (new StateGuard<int>(holder))
            {
                holder.Value = nonExpected;
            }

            Assert.AreEqual(expected, holder.Value);
        }
    }
}
