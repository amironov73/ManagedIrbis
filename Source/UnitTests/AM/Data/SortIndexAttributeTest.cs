using AM.Data;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Data
{
    [TestClass]
    public class SortIndexAttributeTest
    {
        [TestMethod]
        public void SortIndexAttribute_Construction_1()
        {
            SortIndexAttribute attribute
                = new SortIndexAttribute(123);
            Assert.AreEqual(123, attribute.Index);
        }
    }
}