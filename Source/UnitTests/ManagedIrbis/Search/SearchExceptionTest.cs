using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Search;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class SearchExceptionTest
    {
        [TestMethod]
        public void SearchException_Construction_1()
        {
            SearchException exception = new SearchException();
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void SearchException_Construction_2()
        {
            string message = "Message";
            SearchException exception = new SearchException(message);
            Assert.AreSame(message, exception.Message);
        }

        [TestMethod]
        public void SearchException_Construction_3()
        {
            string message = "Message";
            Exception innerException = new Exception();
            SearchException exception = new SearchException(message, innerException);
            Assert.AreSame(message, exception.Message);
            Assert.AreSame(innerException, exception.InnerException);
        }
    }
}
