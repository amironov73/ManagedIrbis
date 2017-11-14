using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class ExceptionUtilityTest
    {
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ExceptionUtility_Throw_1()
        {
            ExceptionUtility.Throw("Some message: {0}", 123);
        }

        [TestMethod]
        public void ExceptionUtility_Unwrap_1()
        {
            Exception simpleException = new Exception();
            Exception unwrapped = ExceptionUtility.Unwrap(simpleException);
            Assert.AreSame(simpleException, unwrapped);
        }

        [TestMethod]
        public void ExceptionUtility_Unwrap_2()
        {
            Exception innerException = new Exception();
            AggregateException aggregate = new AggregateException(innerException);
            Exception unwrapped = ExceptionUtility.Unwrap(aggregate);
            Assert.AreSame(innerException, unwrapped);
        }
    }
}
