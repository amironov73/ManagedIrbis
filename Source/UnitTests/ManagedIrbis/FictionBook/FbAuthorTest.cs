using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.FictionBook;

namespace UnitTests.ManagedIrbis.FictionBook
{
    [TestClass]
    public class FbAuthorTest
    {
        [TestMethod]
        public void FbAuthor_ToString_1()
        {
            FbAuthor author = new FbAuthor();
            Assert.AreEqual("(empty)", author.ToString());
        }

        [TestMethod]
        public void FbAuthor_ToString_2()
        {
            FbAuthor author = new FbAuthor
            {
                FirstName = "Лев",
                MiddleName = "Николаевич",
                LastName = "Толстой"
            };
            Assert.AreEqual("Лев Николаевич Толстой", author.ToString());
        }
    }
}
