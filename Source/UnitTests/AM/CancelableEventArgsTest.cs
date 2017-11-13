using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class CancelableEventArgsTest
    {
        class MyClass
        {
            public bool Flag { get; set; }

            public void Handler1(object sender, CancelableEventArgs args)
            {
                Flag = true;
            }

            public void Handler2(object sender, CancelableEventArgs args)
            {
                args.Cancel = true;
            }
        }

        [TestMethod]
        public void CancelableEventArgs_Construction()
        {
            CancelableEventArgs args = new CancelableEventArgs();
            Assert.IsNotNull(args);
        }

        [TestMethod]
        public void CancelableEventArgs_Cancel()
        {
            CancelableEventArgs args = new CancelableEventArgs();
            Assert.IsFalse(args.Cancel);
            args.Cancel = true;
            Assert.IsTrue(args.Cancel);
        }

        [TestMethod]
        public void CancelableEventArgs_Handle1()
        {
            MyClass myClass = new MyClass();
            CancelableEventArgs args = new CancelableEventArgs();
            args.Handle(myClass, myClass.Handler1);
            Assert.IsTrue(myClass.Flag);
            Assert.IsFalse(args.Cancel);
        }

        [TestMethod]
        public void CancelableEventArgs_Handle2()
        {
            MyClass myClass = new MyClass();
            CancelableEventArgs args = new CancelableEventArgs();
            args.Handle(myClass, myClass.Handler2);
            Assert.IsFalse(myClass.Flag);
            Assert.IsTrue(args.Cancel);
        }

        [TestMethod]
        public void CancelableEventArgs_Handle3()
        {
            MyClass myClass = new MyClass();
            CancelableEventArgs args = new CancelableEventArgs();
            args.Handle(myClass, null);
            Assert.IsFalse(myClass.Flag);
            Assert.IsFalse(args.Cancel);
        }
    }
}
