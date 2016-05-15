using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class BinaryWriterTest
    {
        [TestMethod]
        public void TestWriteString()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            string[] text1 = {"Hello", "World"};
            writer.WriteArray(text1);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            string[] text2 = reader.ReadStringArray();

            Assert.AreEqual(text1.Length, text2.Length);
            Assert.AreEqual(text1[0], text2[0]);
            Assert.AreEqual(text1[1], text2[1]);
        }
    }
}
