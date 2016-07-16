using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis.Requests;

namespace UnitTests.ManagedClient.Requests
{
    [TestClass]
    public class BookRequestTest
    {
        private void _TestSerialization
            (
                BookRequest first
            )
        {
            byte[] bytes = first.SaveToMemory();

            BookRequest second = bytes
                .RestoreObjectFromMemory<BookRequest>();

            Assert.AreEqual(first.BookCode, second.BookCode);
            Assert.AreEqual(first.BookDescription, second.BookDescription);
            Assert.AreEqual(first.Mfn, second.Mfn);
        }

        [TestMethod]
        public void TestBookRequestSerialization()
        {
            BookRequest bookRequest = new BookRequest();
            _TestSerialization(bookRequest);
        }
    }
}
