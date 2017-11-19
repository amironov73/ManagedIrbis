using System;

using ManagedIrbis.Pft;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft
{
    [TestClass]
    public class PftSemanticExceptionTest
    {
        [TestMethod]
        public void PftSemanticException_Construction_1()
        {
            PftSemanticException exception = new PftSemanticException();
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void PftSemanticException_Construction_2()
        {
            const string message = "Message";
            PftSemanticException exception
                = new PftSemanticException(message);
            Assert.AreEqual(message, exception.Message);
        }

        [TestMethod]
        public void PftSemanticException_Construction_3()
        {
            const string message = "Message";
            Exception innerException = new Exception();
            PftSemanticException exception
                = new PftSemanticException(message, innerException);
            Assert.AreEqual(message, exception.Message);
            Assert.AreSame(innerException, exception.InnerException);
        }
    }
}
