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

        [TestMethod]
        public void TestWritePackedInt32()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            int[] values = {0, 1, 100, 127, 256, 1000, 
                1000000, 20000030, 2012345678,
                -1, -2, -100, -127, -128, -256, -1000,
                -1000000, -20000030, -2012345678 };
            foreach (int value in values)
            {
                writer.WritePackedInt32(value);
            }

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            for (int i = 0; i < values.Length; i++)
            {
                int value = reader.ReadPackedInt32();
                Assert.AreEqual(values[i], value);
            }

        }

        [TestMethod]
        public void TestWritePackedInt64()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            long[] values = {0, 1, 100, 127, 256, 1000, 
                1000000, 20000030, 2012345678, 2012345678901,
                2012345678901234 };
            foreach (long value in values)
            {
                writer.WritePackedInt64(value);
            }

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            for (int i = 0; i < values.Length; i++)
            {
                long value = reader.ReadPackedInt64();
                Assert.AreEqual(values[i], value);
            }

        }
    }
}
