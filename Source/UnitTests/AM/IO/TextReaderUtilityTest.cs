using System;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.IO;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class TextReaderUtilityTest
    {
        private readonly string NL = Environment.NewLine;

        [TestMethod]
        public void TextReaderUtility_OpenRead_1()
        {
            string expected = "Hello, world!";
            Encoding encoding = Encoding.ASCII;
            string fileName = Path.GetTempFileName();
            File.WriteAllText(fileName, expected, encoding);

            using (TextReader reader = TextReaderUtility.OpenRead(fileName, encoding))
            {
                string actual = reader.ReadToEnd();
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void TextReaderUtility_RequireLine_1()
        {
            string testText = "first line"
                              + NL
                              + "second line";
            int counter = 0;
            StringReader reader = new StringReader(testText);
            reader.RequireLine();
            counter++;
            reader.RequireLine();
            counter++;
            try
            {
                reader.RequireLine();
            }
            catch (ArsMagnaException)
            {
                counter++;
            }

            Assert.AreEqual(3, counter);
        }
    }
}
