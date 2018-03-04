using AM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Runtime
{
    [TestClass]
    public class SingletonTest
    {
        [TestMethod]
        public void Singleton_Roundtrip_1()
        {
            Singleton.Clear();
            Assert.IsFalse(Singleton.HaveInstance<CanaryClass>());

            CanaryClass canary1 = Singleton.Instance<CanaryClass>();
            Assert.IsNotNull(canary1);
            Assert.IsTrue(Singleton.HaveInstance<CanaryClass>());

            CanaryClass canary2 = Singleton.Instance<CanaryClass>();
            Assert.IsNotNull(canary2);
            Assert.AreSame(canary1, canary2);

            Assert.IsTrue(Singleton.RemoveInstance<CanaryClass>());
            Assert.IsFalse(Singleton.HaveInstance<CanaryClass>());
        }
    }
}
