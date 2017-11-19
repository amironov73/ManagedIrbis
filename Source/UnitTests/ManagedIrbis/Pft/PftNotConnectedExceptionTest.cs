using System;

using ManagedIrbis.Pft;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft
{
    [TestClass]
    public class PftNotConnectedExceptionTest
    {
        [TestMethod]
        public void PftNotConnectedException_Construction_1()
        {
            PftNotConnectedException exception = new PftNotConnectedException();
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void PftNotConnectedException_Construction_2()
        {
            const string message = "Message";
            PftNotConnectedException exception
                = new PftNotConnectedException(message);
            Assert.AreEqual(message, exception.Message);
        }

        [TestMethod]
        public void PftNotConnectedException_Construction_3()
        {
            const string message = "Message";
            Exception innerException = new Exception();
            PftNotConnectedException exception
                = new PftNotConnectedException(message, innerException);
            Assert.AreEqual(message, exception.Message);
            Assert.AreSame(innerException, exception.InnerException);
        }
    }
}
