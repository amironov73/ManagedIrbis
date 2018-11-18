using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class ReadOnlyExceptionTest
    {
        [TestMethod]
        public void ReadOnlyException_Construction1()
        {
            ReadOnlyException exception = new ReadOnlyException();
            Assert.IsNotNull(exception.Message);
        }

        [TestMethod]
        public void ReadOnlyException_Construction2()
        {
            const string expected = "Key";
            ReadOnlyException exception = new ReadOnlyException(expected);
            Assert.AreEqual(expected, exception.Message);
        }

        [TestMethod]
        public void ReadOnlyException_Construction3()
        {
            Exception innerException = new Exception("Message");
            const string expected = "Key";
            ReadOnlyException exception = new ReadOnlyException
                (
                    expected,
                    innerException
                );
            Assert.AreEqual(expected, exception.Message);
            Assert.AreEqual(innerException, exception.InnerException);
        }
    }
}
