using AM.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class ScopedLockFactoryTest
    {
        [TestMethod]
        public void ScopedLockFactory_Construction_1()
        {
            using (ScopedLockFactory factory = new ScopedLockFactory())
            {
                using (var lock1 = factory.CreateLock())
                {
                    Assert.IsNotNull(lock1);
                }

                using (var lock2 = factory.CreateLock())
                {
                    Assert.IsNotNull(lock2);
                }
            }
        }
    }
}
