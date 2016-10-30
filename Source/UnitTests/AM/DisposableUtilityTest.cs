using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class DisposableUtilityTest
    {
        class MyDisposable
            : IDisposable
        {
            public bool Disposed;

            public void Dispose()
            {
                Disposed = true;
            }
        }

        [TestMethod]
        public void DisposableUtility_SafeDispose()
        {
            MyDisposable myObject = new MyDisposable();
            myObject.SafeDispose();
            Assert.IsTrue(myObject.Disposed);
        }
    }
}
