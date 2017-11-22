using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Search;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class SearchSyntaxExceptionTest
    {
        [TestMethod]
        public void SearchSyntaxException_Construction_1()
        {
            SearchSyntaxException exception = new SearchSyntaxException();
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void SearchSyntaxException_Construction_2()
        {
            string message = "Message";
            SearchSyntaxException exception = new SearchSyntaxException(message);
            Assert.AreSame(message, exception.Message);
        }

        [TestMethod]
        public void SearchSyntaxException_Construction_3()
        {
            string message = "Message";
            Exception innerException = new Exception();
            SearchSyntaxException exception = new SearchSyntaxException(message, innerException);
            Assert.AreSame(message, exception.Message);
            Assert.AreSame(innerException, exception.InnerException);
        }
    }
}
