using System;

using ManagedIrbis.Pft;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft
{
    [TestClass]
    public class PftExceptionTest
    {
        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void PftException_Construction_1()
        {
            throw new PftException();
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void PftException_Construction_2()
        {
            throw new PftException("File not found");
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void PftException_Construction_3()
        {
            Exception innerException = new Exception();
            throw new PftException("Delegate invocation error", innerException);
        }
    }
}