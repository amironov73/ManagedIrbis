using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Threading;

namespace UnitTests.AM.Threading
{
    [TestClass]
    public class BusyStateTest
    {
        [TestMethod]
        public void TestBusyStateEvent()
        {
            BusyState state = true;
            bool flag = false;
            state.StateChanged += (sender, args) => { flag = true; };
            state.SetState(false);
            Assert.IsTrue(flag);
            Assert.IsFalse(state);
        }

        [TestMethod]
        public void TestBusyStateWaitHandle()
        {
            BusyState state = true;
            bool flag = false;
            Task task = Task.Factory.StartNew
                (
                    () =>
                    {
                        state.WaitFreeState();
                        flag = true;
                    }
                );
            state.SetState(false);
            task.Wait();
            Assert.IsTrue(flag);
            Assert.IsFalse(state);
        }
    }
}
