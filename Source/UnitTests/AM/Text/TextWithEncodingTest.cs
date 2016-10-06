using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class TextWithEncodingTest
    {
        [TestMethod]
        public void TestTextWithEncodingConstruction()
        {
            TextWithEncoding text = new TextWithEncoding("Hello", true);
            byte[] bytes = text.ToBytes();
            Assert.AreEqual(5, bytes.Length);
        }

        [TestMethod]
        public void TestTextWithEncodingComparison()
        {
            TextWithEncoding first = new TextWithEncoding("Hello", true);
            TextWithEncoding second = new TextWithEncoding("Hello", false);

            Assert.AreEqual
                (
                    true,
                    first == second
                );
        }
    }
}
