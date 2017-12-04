using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class TextWriterUtilityTest
    {
        [TestMethod]
        public void TextWriterUtility_Append_1()
        {
            Encoding encoding = Encoding.ASCII;

            string fileName = Path.GetTempFileName();
            File.WriteAllText(fileName, "Hello, ", encoding);
            using (TextWriter writer = TextWriterUtility.Append(fileName, encoding))
            {
                writer.Write("world!");
            }
            string actual = File.ReadAllText(fileName, encoding);
            Assert.AreEqual("Hello, world!", actual);
        }

        [TestMethod]
        public void TextWriterUtility_Create_1()
        {
            string expected = "Hello, world!";
            Encoding encoding = Encoding.ASCII;
            string fileName = Path.GetTempFileName();
            File.Delete(fileName);
            using (TextWriter writer = TextWriterUtility.Create(fileName, encoding))
            {
                writer.Write(expected);
            }
            string actual = File.ReadAllText(fileName, encoding);
            Assert.AreEqual(expected, actual);
        }
    }
}
