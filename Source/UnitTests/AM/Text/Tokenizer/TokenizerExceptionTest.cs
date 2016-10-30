using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Tokenizer;

namespace UnitTests.AM.Text.Tokenizer
{
    [TestClass]
    public class TokenizerExceptionTest
    {
        [TestMethod]
        public void TokenizerException_Construction1()
        {
            TokenizerException exception = new TokenizerException();
            Assert.IsNotNull(exception.Message);
        }

        [TestMethod]
        public void TokenizerException_Construction2()
        {
            const string expected = "Key";
            TokenizerException exception = new TokenizerException(expected);
            Assert.AreEqual(expected, exception.Message);
        }

        [TestMethod]
        public void TokenizerException_Construction3()
        {
            Exception innerException = new Exception("Message");
            const string expected = "Key";
            TokenizerException exception = new TokenizerException
                (
                    expected,
                    innerException
                );
            Assert.AreEqual(expected, exception.Message);
            Assert.AreEqual(innerException, exception.InnerException);
        }
    }
}
