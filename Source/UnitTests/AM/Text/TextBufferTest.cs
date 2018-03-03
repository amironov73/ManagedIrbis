using System;

using AM;
using AM.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class TextBufferTest
    {
        [TestMethod]
        public void TextBuffer_Construction_1()
        {
            TextBuffer buffer = new TextBuffer();
            Assert.AreEqual(1, buffer.Column);
            Assert.AreEqual(1, buffer.Line);
            Assert.AreEqual(0, buffer.Length);
        }

        [TestMethod]
        public void TextBuffer_Backspace_1()
        {
            TextBuffer buffer = new TextBuffer();
            Assert.IsFalse(buffer.Backspace());

            buffer.Write("Hello");
            Assert.IsTrue(buffer.Backspace());
            Assert.AreEqual("Hell", buffer.ToString());
            Assert.AreEqual(5, buffer.Column);
            Assert.AreEqual(1, buffer.Line);
        }

        [TestMethod]
        public void TextBuffer_Backspace_2()
        {
            TextBuffer buffer = new TextBuffer();
            Assert.IsFalse(buffer.Backspace());

            buffer.Write("Hello\n");
            Assert.IsTrue(buffer.Backspace());
            Assert.AreEqual("Hello", buffer.ToString());
            Assert.AreEqual(6, buffer.Column);
            Assert.AreEqual(1, buffer.Line);
        }

        [TestMethod]
        public void TextBuffer_Backspace_3()
        {
            TextBuffer buffer = new TextBuffer();
            Assert.IsFalse(buffer.Backspace());

            buffer.Write("Hello\nworld\n");
            Assert.IsTrue(buffer.Backspace());
            Assert.AreEqual("Hello\nworld", buffer.ToString());
            Assert.AreEqual(6, buffer.Column);
            Assert.AreEqual(2, buffer.Line);
        }

        [TestMethod]
        public void TextBuffer_Clear_1()
        {
            TextBuffer buffer = new TextBuffer();
            buffer.Write("Hello");

            Assert.AreSame(buffer, buffer.Clear());
            Assert.AreEqual(1, buffer.Line);
            Assert.AreEqual(1, buffer.Column);
            Assert.AreEqual(0, buffer.Length);
        }

        [TestMethod]
        public void TextBuffer_GetLastChar_1()
        {
            TextBuffer buffer = new TextBuffer();
            Assert.AreEqual('\0', buffer.GetLastChar());

            buffer.Write("Hello");
            Assert.AreEqual('o', buffer.GetLastChar());
        }

        [TestMethod]
        public void TextBuffer_PrecededByNewLine_1()
        {
            TextBuffer buffer = new TextBuffer();
            Assert.IsFalse(buffer.PrecededByNewLine());

            buffer.Write("Hello");
            Assert.IsFalse(buffer.PrecededByNewLine());

            buffer.WriteLine();
            Assert.IsTrue(buffer.PrecededByNewLine());

            buffer.Write("World");
            Assert.IsFalse(buffer.PrecededByNewLine());
        }

        [TestMethod]
        public void TextBuffer_RemoveEmptyLines_1()
        {
            TextBuffer buffer = new TextBuffer();
            Assert.AreSame(buffer, buffer.RemoveEmptyLines());

            buffer.Write("Hello");
            buffer.WriteLine();
            buffer.RemoveEmptyLines();
            Assert.AreEqual("Hello", buffer.ToString());

            buffer.WriteLine();
            buffer.WriteLine();
            buffer.RemoveEmptyLines();
            Assert.AreEqual("Hello", buffer.ToString());
        }

        [TestMethod]
        public void TextBuffer_Write_1()
        {
            TextBuffer buffer = new TextBuffer();
            int length = 10 * 1024;
            char c = 'A';
            string expected = new string(c, length);
            for (int i = 0; i < length; i++)
            {
                Assert.AreSame(buffer, buffer.Write(c));
            }
            Assert.AreEqual(length, buffer.Length);
            Assert.AreEqual(length + 1, buffer.Column);
            Assert.AreEqual(1, buffer.Line);
            Assert.AreEqual(expected, buffer.ToString());
        }

        [TestMethod]
        public void TextBuffer_Write_2()
        {
            TextBuffer buffer = new TextBuffer();
            int length = 10 * 1024;
            char c = '\n';
            string expected = new string(c, length);
            for (int i = 0; i < length; i++)
            {
                Assert.AreSame(buffer, buffer.Write(c));
            }
            Assert.AreEqual(length, buffer.Length);
            Assert.AreEqual(1, buffer.Column);
            Assert.AreEqual(length + 1, buffer.Line);
            Assert.AreEqual(expected, buffer.ToString());
        }

        [TestMethod]
        public void TextBuffer_Write_3()
        {
            TextBuffer buffer = new TextBuffer();
            string one = "one";
            int length = 1000;
            int total = length * one.Length;
            for (int i = 0; i < length; i++)
            {
                Assert.AreSame(buffer, buffer.Write(one));
            }
            Assert.AreEqual(total, buffer.Length);
            Assert.AreEqual(total + 1, buffer.Column);
            Assert.AreEqual(1, buffer.Line);
            string expected = StringUtility.Replicate(one, length);
            Assert.AreEqual(expected, buffer.ToString());
        }

        [TestMethod]
        public void TextBuffer_Write_4()
        {
            TextBuffer buffer = new TextBuffer();
            string one = "o\ne";
            int length = 1000;
            int total = length * one.Length;
            for (int i = 0; i < length; i++)
            {
                Assert.AreSame(buffer, buffer.Write(one));
            }
            Assert.AreEqual(total, buffer.Length);
            Assert.AreEqual(2, buffer.Column);
            Assert.AreEqual(length + 1, buffer.Line);
            string expected = StringUtility.Replicate(one, length);
            Assert.AreEqual(expected, buffer.ToString());
        }

        [TestMethod]
        public void TextBuffer_Write_5()
        {
            TextBuffer buffer = new TextBuffer();
            Assert.AreSame(buffer, buffer.Write(null));
            Assert.AreEqual(0, buffer.Length);
            Assert.AreEqual(1, buffer.Line);
            Assert.AreEqual(1, buffer.Column);

            Assert.AreSame(buffer, buffer.Write(string.Empty));
            Assert.AreEqual(0, buffer.Length);
            Assert.AreEqual(1, buffer.Line);
            Assert.AreEqual(1, buffer.Column);
        }

        [TestMethod]
        public void TextBuffer_Write_6()
        {
            TextBuffer buffer = new TextBuffer();
            Assert.AreSame(buffer, buffer.Write("{0}, {1}!", "Hello", "world"));
            Assert.AreEqual("Hello, world!", buffer.ToString());
        }

        [TestMethod]
        public void TextBuffer_WriteLine_1()
        {
            TextBuffer buffer = new TextBuffer();
            Assert.AreSame(buffer, buffer.WriteLine());
            Assert.AreEqual(Environment.NewLine, buffer.ToString());
        }

        [TestMethod]
        public void TextBuffer_WriteLine_2()
        {
            TextBuffer buffer = new TextBuffer();
            string text = "Hello";
            Assert.AreSame(buffer, buffer.WriteLine(text));
            Assert.AreEqual(text + Environment.NewLine, buffer.ToString());
        }

        [TestMethod]
        public void TextBuffer_WriteLine_3()
        {
            TextBuffer buffer = new TextBuffer();
            Assert.AreSame(buffer, buffer.WriteLine("{0}, {1}!", "Hello", "world"));
            Assert.AreEqual("Hello, world!" + Environment.NewLine, buffer.ToString());
        }

        [TestMethod]
        public void TextBuffer_ToString_1()
        {
            TextBuffer buffer = new TextBuffer();
            Assert.AreEqual(string.Empty, buffer.ToString());

            string text = "Hello, world";
            buffer.Write(text);
            Assert.AreEqual(text, buffer.ToString());
        }
    }
}
