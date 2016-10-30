using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class TextWithEncodingTest
    {
        [TestMethod]
        public void TextWithEncoding_Construction1()
        {
            TextWithEncoding text = new TextWithEncoding();
            byte[] bytes = text.ToBytes();
            Assert.AreEqual(0, bytes.Length);
        }

        [TestMethod]
        public void TextWithEncoding_Construction2()
        {
            TextWithEncoding text = new TextWithEncoding("Привет");
            byte[] bytes = text.ToBytes();
            Assert.AreEqual(12, bytes.Length);
        }

        [TestMethod]
        public void TextWithEncoding_Construction3()
        {
            TextWithEncoding text = new TextWithEncoding("Hello", true);
            byte[] bytes = text.ToBytes();
            Assert.AreEqual(5, bytes.Length);
        }

        [TestMethod]
        public void TextWithEncoding_Construction4()
        {
            TextWithEncoding text = new TextWithEncoding("Привет", Encoding.GetEncoding(1251));
            byte[] bytes = text.ToBytes();
            Assert.AreEqual(6, bytes.Length);
        }

        [TestMethod]
        public void TextWithEncoding_ImplicitOperator()
        {
            TextWithEncoding text = "Hello";
            byte[] bytes = text.ToBytes();
            Assert.AreEqual(5, bytes.Length);
        }

        [TestMethod]
        public void TextWithEncoding_CompareTo1()
        {
            TextWithEncoding first = "Hello";
            TextWithEncoding second = "World";
            Assert.IsTrue(first.CompareTo(second) < 0);
        }

        [TestMethod]
        public void TextWithEncoding_CompareTo2()
        {
            TextWithEncoding first = "Hello";
            TextWithEncoding second = null;
            Assert.IsTrue(first.CompareTo(second) > 0);
        }

        [TestMethod]
        public void TextWithEncoding_Comparison1()
        {
            TextWithEncoding first = new TextWithEncoding("Hello", true);
            TextWithEncoding second = new TextWithEncoding("Hello", false);

            Assert.AreEqual
                (
                    true,
                    first == second
                );
        }

        [TestMethod]
        public void TextWithEncoding_Comparison2()
        {
            TextWithEncoding first = new TextWithEncoding("Hello", true);
            TextWithEncoding second = null;

            Assert.AreEqual
                (
                    false,
                    first == second
                );
        }

        [TestMethod]
        public void TextWithEncoding_Comparison3()
        {
            TextWithEncoding first = null;
            TextWithEncoding second = new TextWithEncoding("Hello", true);

            Assert.AreEqual
                (
                    false,
                    first == second
                );
        }

        [TestMethod]
        public void TextWithEncoding_Comparison4()
        {
            TextWithEncoding first = new TextWithEncoding("Hello", true);
            TextWithEncoding second = new TextWithEncoding("World", false);

            Assert.AreEqual
                (
                    true,
                    first != second
                );
        }

        [TestMethod]
        public void TextWithEncoding_Comparison5()
        {
            TextWithEncoding first = new TextWithEncoding("Hello", true);
            TextWithEncoding second = null;

            Assert.AreEqual
                (
                    true,
                    first != second
                );
        }

        [TestMethod]
        public void TextWithEncoding_Comparison6()
        {
            TextWithEncoding first = null;
            TextWithEncoding second = new TextWithEncoding("Hello", true);

            Assert.AreEqual
                (
                    true,
                    first != second
                );
        }

        [TestMethod]
        public void TextWithEncoding_Equals1()
        {
            TextWithEncoding first = new TextWithEncoding("Hello", true);
            TextWithEncoding second = new TextWithEncoding("World", false);

            Assert.IsFalse(first.Equals(second));
            Assert.IsTrue(first.Equals(first));
            Assert.IsFalse(first.Equals(null));
        }

        [TestMethod]
        public void TextWithEncoding_Equals2()
        {
            TextWithEncoding first = new TextWithEncoding("Hello", true);
            TextWithEncoding second = new TextWithEncoding("World", false);

            Assert.IsFalse(first.Equals((object)second));
            Assert.IsTrue(first.Equals((object)first));
            Assert.IsFalse(first.Equals((object)null));
        }

        [TestMethod]
        public void TextWithEncoding_GetHashCode()
        {
            TextWithEncoding text = new TextWithEncoding();
            Assert.AreEqual(0, text.GetHashCode());

            text = "Hello";
            Assert.AreEqual("Hello".GetHashCode(), text.GetHashCode());
        }

        [TestMethod]
        public void TextWithEncoding_ToString()
        {
            TextWithEncoding text = new TextWithEncoding();
            Assert.AreEqual(string.Empty, text.ToString());

            text = "Hello";
            Assert.AreEqual("Hello", text.ToString());
        }
    }
}
