using System;
using System.IO;

using AM.IO;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class StreamPackerTest
    {
        static void _CheckUInt32(uint first)
        {
            using (Stream stream = new MemoryStream())
            {
                StreamPacker.PackUInt32(stream, first);
                stream.Position = 0;
                uint second = StreamPacker.UnpackUInt32(stream);
                Assert.AreEqual(first, second);
            }
        }

        static void _CheckUInt64(ulong first)
        {
            using (Stream stream = new MemoryStream())
            {
                StreamPacker.PackUInt64(stream, first);
                stream.Position = 0;
                ulong second = StreamPacker.UnpackUInt64(stream);
                Assert.AreEqual(first, second);
            }
        }

        [TestMethod]
        public void StreamPacker_PackUInt32_1()
        {
            _CheckUInt32(0);
            _CheckUInt32(1);
            _CheckUInt32(11);
            _CheckUInt32(111);
            _CheckUInt32(1111);
            _CheckUInt32(11111);
            _CheckUInt32(111111);
            _CheckUInt32(1111111);
            _CheckUInt32(11111111);
        }

        [TestMethod]
        public void StreamPacker_PackUInt64_1()
        {
            _CheckUInt64(0);
            _CheckUInt64(1);
            _CheckUInt64(11);
            _CheckUInt64(111);
            _CheckUInt64(1111);
            _CheckUInt64(11111);
            _CheckUInt64(111111);
            _CheckUInt64(1111111);
            _CheckUInt64(11111111);
            _CheckUInt64(111111111);
            _CheckUInt64(1111111111);
            _CheckUInt64(11111111111);
            _CheckUInt64(111111111111);
            _CheckUInt64(1111111111111);
            _CheckUInt64(11111111111111);
            _CheckUInt64(111111111111111);
            _CheckUInt64(1111111111111111);
        }

        private void _CheckPackBytes
            (
                params byte[] first
            )
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamPacker.PackBytes(stream, first);
                stream.Position = 0;
                byte[] second = StreamPacker.UnpackBytes(stream);
                CollectionAssert.AreEqual(first, second);
            }
        }

        [TestMethod]
        public void StreamPacker_PackBytes_1()
        {
            _CheckPackBytes();
            _CheckPackBytes(1);
            _CheckPackBytes(1, 2);
            _CheckPackBytes(1, 2, 3, 4, 5, 6, 7, 8);
        }

        private void _CheckPackString
            (
                [NotNull] string first
            )
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamPacker.PackString(stream, first);
                stream.Position = 0;
                string second = StreamPacker.UnpackString(stream);
                Assert.AreEqual(first, second);
            }
        }

        [TestMethod]
        public void StreamPacker_PackString_1()
        {
            _CheckPackString("");
            _CheckPackString("Hello");
            _CheckPackString("Привет");
        }
    }
}
