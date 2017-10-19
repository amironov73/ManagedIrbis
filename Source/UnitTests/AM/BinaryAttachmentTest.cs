using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class BinaryAttachmentTest
    {
        [TestMethod]
        public void BinaryAttachment_Construction_1()
        {
            string name = "Name";
            byte[] content = {1, 2, 3};
            BinaryAttachment attachment = new BinaryAttachment(name, content);
            Assert.AreSame(name, attachment.Name);
            Assert.AreSame(content, attachment.Content);
        }

        [TestMethod]
        public void BinaryAttachment_ToString_1()
        {
            string name = "Name";
            byte[] content = { 1, 2, 3 };
            BinaryAttachment attachment = new BinaryAttachment(name, content);
            Assert.AreEqual("Name: 3 bytes", attachment.ToString());
        }
    }
}
