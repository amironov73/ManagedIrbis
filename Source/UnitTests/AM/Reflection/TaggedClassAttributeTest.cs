using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Reflection;

namespace UnitTests.AM.Reflection
{
    [TestClass]
    public class TaggedClassAttributeTest
    {
        [TestMethod]
        public void TaggedClassAttribute_Construction_1()
        {
            const string tag = "tag";
            TaggedClassAttribute attribute = new TaggedClassAttribute(tag);
            Assert.AreEqual(tag, attribute.Tag);
        }
    }
}
