using AM.Data;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Data
{
    [TestClass]
    public class ColumnWidthAttributeTest
    {
        [TestMethod]
        public void ColumnWidthAttribute_Construction_1()
        {
            ColumnWidthAttribute attribute
                = new ColumnWidthAttribute(1000);
            Assert.AreEqual(1000, attribute.Width);
        }
    }
}