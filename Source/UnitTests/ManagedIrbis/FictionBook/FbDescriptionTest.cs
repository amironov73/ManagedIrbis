using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.FictionBook;

namespace UnitTests.ManagedIrbis.FictionBook
{
    [TestClass]
    public class FbDescriptionTest
    {
        [TestMethod]
        public void FbDescription_ToString_1()
        {
            FbDescription description = new FbDescription();
            Assert.AreEqual("(null)", description.ToString());
        }

        [TestMethod]
        public void FbDescription_ToString_2()
        {
            FbDescription description = new FbDescription
            {
                Title = new FbTitle()
            };
            Assert.AreEqual("(null)", description.ToString());
        }

        [TestMethod]
        public void FbDescription_ToString_3()
        {
            FbDescription description = new FbDescription
            {
                Title = new FbTitle
                {
                    Title = "Война и мир"
                }
            };
            Assert.AreEqual("Война и мир", description.ToString());
        }
    }
}
