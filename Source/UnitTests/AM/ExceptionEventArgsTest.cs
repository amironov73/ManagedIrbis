using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class ExceptionEventArgsTest
    {
        [TestMethod]
        public void ExceptionEventArgs_Construction_1()
        {
            Exception exception = new Exception("Hello");
            ExceptionEventArgs<Exception> args
                = new ExceptionEventArgs<Exception>(exception);
            Assert.AreSame(exception, args.Exception);
            Assert.IsFalse(args.Handled);
        }

        [TestMethod]
        public void ExceptionEventArgs_Construction_2()
        {
            Exception exception = new Exception("Hello");
            ExceptionEventArgs args = new ExceptionEventArgs(exception);
            Assert.AreSame(exception, args.Exception);
        }

        [TestMethod]
        public void ExceptionEventArgs_Handled_1()
        {
            Exception exception = new Exception("Hello");
            ExceptionEventArgs args = new ExceptionEventArgs(exception)
                {
                    Handled = true
                };
            Assert.IsTrue(args.Handled);
        }

        [TestMethod]
        public void ExceptionEventArgs_Handled_2()
        {
            Exception exception = new Exception("Hello");
            ExceptionEventArgs<Exception> args
                = new ExceptionEventArgs<Exception>(exception)
                {
                    Handled = true
                };
            Assert.IsTrue(args.Handled);
        }
    }
}
