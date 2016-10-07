using System;
using System.IO;
using System.IO.Compression;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.System.IO.Compression
{
    [TestClass]
    public class DeflateStreamTest
    {
        private void _TestCompression
            (
                byte[] first
            )
        {
            int length = first.Length;
            byte[] second;
            byte[] third;

            using (MemoryStream memory = new MemoryStream())
            using (DeflateStream deflate = new DeflateStream
                (
                memory,
                CompressionMode.Compress
                ))
            {
                using (BinaryWriter writer = new BinaryWriter(deflate))
                {
                    writer.Write(first);
                }
                second = memory.ToArray();
            }

            using (MemoryStream memory = new MemoryStream(second))
            using (DeflateStream deflate = new DeflateStream
                (
                    memory,
                    CompressionMode.Decompress
                ))
            using (BinaryReader reader = new BinaryReader(deflate))
            {
                third = new byte[length];
                reader.Read(third, 0, length);
            }

            for (int i = 0; i < length; i++)
            {
                Assert.AreEqual(first[i], third[i]);
            }
        }

        [TestMethod]
        public void TestDeflateStreamCompression()
        {
            _TestCompression(new byte[0]);
            _TestCompression(new byte[10]);
            _TestCompression(new byte[1000]);
        }
    }
}
