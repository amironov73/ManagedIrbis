using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class VerificationExceptionTest
    {
        [TestMethod]
        public void VerificationException_Construction1()
        {
            VerificationException exception = new VerificationException();
            Assert.IsNotNull(exception.Message);
        }

        [TestMethod]
        public void VerificationException_Construction2()
        {
            const string expected = "Key";
            VerificationException exception = new VerificationException(expected);
            Assert.AreEqual(expected, exception.Message);
        }

        [TestMethod]
        public void VerificationException_Construction3()
        {
            Exception innerException = new Exception("Message");
            const string expected = "Key";
            VerificationException exception = new VerificationException
                (
                    expected,
                    innerException
                );
            Assert.AreEqual(expected, exception.Message);
            Assert.AreEqual(innerException, exception.InnerException);
        }
    }
}
