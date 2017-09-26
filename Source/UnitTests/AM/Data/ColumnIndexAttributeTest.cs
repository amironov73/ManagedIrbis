using AM.Data;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Data
{
    [TestClass]
    public class ColumnIndexAttributeTest
    {
        [TestMethod]
        public void ColumnIndexAttribute_Construction_1()
        {
            ColumnIndexAttribute attribute
                = new ColumnIndexAttribute(123);
            Assert.AreEqual(123, attribute.Index);
        }
    }
}