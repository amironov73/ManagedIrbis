using AM.Data;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Data
{
    [TestClass]
    public class ColumnHeaderAttributeTest
    {
        [TestMethod]
        public void ColumnHeaderAttribute_Construction_1()
        {
            ColumnHeaderAttribute attribute
                = new ColumnHeaderAttribute("Hello");
            Assert.AreEqual("Hello", attribute.Header);
        }
    }
}