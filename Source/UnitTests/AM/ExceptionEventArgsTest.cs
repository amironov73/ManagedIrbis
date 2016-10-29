using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class ExceptionEventArgsTest
    {
        [TestMethod]
        public void ExceptionEventArgs_Construction()
        {
            Exception exception = new Exception("Hello");
            ExceptionEventArgs<Exception> args
                = new ExceptionEventArgs<Exception>(exception);
            Assert.AreEqual(exception, args.Exception);
            Assert.IsFalse(args.Handled);
        }

        [TestMethod]
        public void ExceptionEventArgs_Handled()
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
