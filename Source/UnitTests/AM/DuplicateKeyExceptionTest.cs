using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class DuplicateKeyExceptionTest
    {
        [TestMethod]
        public void DuplicateKeyException_Construction1()
        {
            DuplicateKeyException exception = new DuplicateKeyException();
            Assert.IsNotNull(exception.Message);
        }

        [TestMethod]
        public void DuplicateKeyException_Construction2()
        {
            const string expected = "Key";
            DuplicateKeyException exception = new DuplicateKeyException(expected);
            Assert.AreEqual(expected, exception.Message);
        }

        [TestMethod]
        public void DuplicateKeyException_Construction3()
        {
            Exception innerException = new Exception("Message");
            const string expected = "Key";
            DuplicateKeyException exception = new DuplicateKeyException
                (
                    expected,
                    innerException
                );
            Assert.AreEqual(expected, exception.Message);
            Assert.AreEqual(innerException, exception.InnerException);
        }
    }
}
