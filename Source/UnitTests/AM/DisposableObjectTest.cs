using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class DisposableObjectTest
    {
        public class MyObject
            : DisposableObject
        {
            [AutoDispose]
            public DisposableObject Inner1 { get; set; }

            [AutoDispose]
            public DisposableObject Inner2;

            [AutoDispose]
            public DisposableObject Inner3;

            public string SomeText;

            public int Number;

            public MyObject(bool disposeByReflection)
                : base(disposeByReflection)
            {
                Inner1 = new DisposableObject(disposeByReflection);
            }

            public void DoSomething()
            {
                CheckDisposed();
            }
        }

        [TestMethod]
        public void DisposableObject_Dispose_1()
        {
            DisposableObject obj = new DisposableObject();
            Assert.IsFalse(obj.Disposed);

            obj.Dispose();
            Assert.IsTrue(obj.Disposed);
        }

        [TestMethod]
        public void DisposableObject_Dispose_2()
        {
            DisposableObject obj = new DisposableObject(false);
            Assert.IsFalse(obj.Disposed);

            obj.Dispose();
            Assert.IsTrue(obj.Disposed);
        }

        [TestMethod]
        public void DisposableObject_Dispose_3()
        {
            DisposableObject obj = new DisposableObject(true);
            Assert.IsFalse(obj.Disposed);

            obj.Dispose();
            Assert.IsTrue(obj.Disposed);
        }

        [TestMethod]
        public void DisposableObject_Dispose_4()
        {
            MyObject obj = new MyObject(false);
            Assert.IsFalse(obj.Disposed);

            obj.Dispose();
            Assert.IsTrue(obj.Disposed);
        }

        [TestMethod]
        public void DisposableObject_Dispose_5()
        {
            MyObject obj = new MyObject(true);
            Assert.IsFalse(obj.Disposed);

            obj.Dispose();
            Assert.IsTrue(obj.Disposed);
        }

        [TestMethod]
        public void DisposableObject_CheckDisposed_1()
        {
            MyObject obj = new MyObject(true);
            obj.DoSomething();
            obj.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void DisposableObject_CheckDisposed_2()
        {
            MyObject obj = new MyObject(true);
            obj.Dispose();
            obj.DoSomething();
        }
    }
}
