using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Threading;

namespace UnitTests.AM.Threading
{
    [TestClass]
    public class BusyGuardTest
    {
        //[TestMethod]
        //[Ignore]
        public void TestBusyGuard()
        {
            bool done = false;
            BusyState busy = new BusyState(true);

            Task task = Task.Factory.StartNew
                (
                    () =>
                    {
                        using (new BusyGuard(busy))
                        {
                            done = true;
                        }
                    }
                );

            busy.SetState(false);

            task.Wait();

            Assert.IsTrue(done);
        }
    }
}
