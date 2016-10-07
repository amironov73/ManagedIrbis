using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Threading;

namespace UnitTests.AM.Threading
{
    [TestClass]
    public class StateHolderTest
    {
        [TestMethod]
        public void TestStateHolderEvent()
        {
            StateHolder<int> holder = 10;
            bool flag = false;
            holder.ValueChanged += (sender, args) => { flag = true; };
            holder.Value = 100;
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void TestStateHolderWaitHandle()
        {
            StateHolder<int> holder = 10;
            bool flag = false;
            Task task = Task.Factory.StartNew
                (
                    () =>
                    {
                        holder.WaitHandle.WaitOne();
                        flag = true;
                    }
                );
            holder.Value = 100;
            task.Wait();
            Assert.IsTrue(flag);
        }
    }
}
