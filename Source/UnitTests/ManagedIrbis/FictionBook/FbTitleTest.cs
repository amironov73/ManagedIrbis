using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.FictionBook;

namespace UnitTests.ManagedIrbis.FictionBook
{
    [TestClass]
    public class FbTitleTest
    {
        [TestMethod]
        public void FbTitle_ToString_1()
        {
            FbTitle title = new FbTitle();
            Assert.AreEqual("(null)", title.ToString());
        }

        [TestMethod]
        public void FbTitle_ToString_2()
        {
            FbTitle title = new FbTitle
            {
                Title = "Война и мир"
            };
            Assert.AreEqual("Война и мир", title.ToString());
        }
    }
}
