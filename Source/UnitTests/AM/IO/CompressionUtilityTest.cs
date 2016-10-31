using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class CompressionUtilityTest
    {
        private void _TestCompression
            (
                byte[] first
            )
        {
            byte[] zipped = CompressionUtility.Compress(first);

            byte[] second = CompressionUtility.Decompress(zipped);

            Assert.AreEqual(first.Length, second.Length);

            for (int i = 0; i < first.Length; i++)
            {
                Assert.AreEqual(first[i], second[i]);
            }
        }

        [TestMethod]
        public void CompressionUtility_Compress_Decompress()
        {
            byte[] bytes = new byte[0];
            _TestCompression(bytes);

            bytes = new byte[] { 0 };
            _TestCompression(bytes);

            bytes = new byte[] { 0, 1, 2, 3 };
            _TestCompression(bytes);
        }
    }
}
