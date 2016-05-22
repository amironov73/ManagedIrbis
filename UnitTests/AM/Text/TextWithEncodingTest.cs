using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class TextWithEncodingTest
    {
        [TestMethod]
        public void TestTextWithEncoding()
        {
            TextWithEncoding text = new TextWithEncoding("Hello", true);
            byte[] bytes = text.ToBytes();
            Assert.AreEqual(5, bytes.Length);
        }
    }
}
