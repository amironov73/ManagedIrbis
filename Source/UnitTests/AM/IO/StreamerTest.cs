using System;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

using JetBrains.Annotations;

using Moq;
using Moq.Protected;

// ReSharper disable ObjectCreationAsStatement

namespace UnitTests.AM.IO
{
    [TestClass]
    public class StreamerTest
    {
        [NotNull]
        private MemoryStream _GetStream
            (
                string text = "Hello,\r\nWorld!"
            )
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            MemoryStream result = new MemoryStream(bytes);

            return result;
        }

        [TestMethod]
        public void Streamer_Construction_1()
        {
            MemoryStream memory = _GetStream();
            Streamer streamer = new Streamer(memory, 4);
            Assert.AreSame(memory, streamer.InnerStream);
            Assert.IsFalse(streamer.Eof);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Streamer_Construction_2()
        {
            Mock<Stream> mock = new Mock<Stream>();
            mock.SetupGet(s => s.CanRead).Returns(false);
            Stream stream = mock.Object;
            new Streamer(stream, 4);
        }

        [TestMethod]
        public void Streamer_PeekByte_1()
        {
            MemoryStream memory = new MemoryStream(new byte[] { 1, 2, 3 });
            Streamer streamer = new Streamer(memory, 2);
            Assert.AreEqual(1, streamer.PeekByte());
            Assert.AreEqual(1, streamer.PeekByte());
            Assert.AreEqual(1, streamer.ReadByte());
            Assert.AreEqual(2, streamer.PeekByte());
            Assert.AreEqual(2, streamer.PeekByte());
            Assert.AreEqual(2, streamer.ReadByte());
            Assert.AreEqual(3, streamer.PeekByte());
            Assert.AreEqual(3, streamer.PeekByte());
            Assert.AreEqual(3, streamer.ReadByte());
            Assert.AreEqual(-1, streamer.PeekByte());
            Assert.AreEqual(-1, streamer.PeekByte());
            Assert.AreEqual(-1, streamer.ReadByte());
        }

        [TestMethod]
        public void Streamer_Read_1()
        {
            MemoryStream memory = _GetStream("\x01\x02\x03\x04\x05\x06");
            Streamer streamer = new Streamer(memory, 2);
            byte[] buffer = new byte[4];
            Assert.AreEqual(4, streamer.Read(buffer));
            Assert.AreEqual(1, buffer[0]);
            Assert.AreEqual(2, buffer[1]);
            Assert.AreEqual(3, buffer[2]);
            Assert.AreEqual(4, buffer[3]);
            Assert.AreEqual(2, streamer.Read(buffer));
            Assert.AreEqual(5, buffer[0]);
            Assert.AreEqual(6, buffer[1]);
            Assert.AreEqual(3, buffer[2]);
            Assert.AreEqual(0, streamer.Read(buffer));
            Assert.AreEqual(0, streamer.Read(buffer));
            Assert.AreEqual(5, buffer[0]);
        }

        [TestMethod]
        public void Streamer_Read_2()
        {
            MemoryStream memory = _GetStream();
            Streamer streamer = new Streamer(memory, 2);
            byte[] buffer = new byte[4];
            Assert.AreEqual(0, streamer.Read(buffer, 0, -1));
            Assert.AreEqual(0, streamer.Read(buffer, 0, 0));
        }

        [TestMethod]
        public void Streamer_ReadByte_1()
        {
            MemoryStream stream = _GetStream("\x01\x02\x03");
            Streamer streamer = new Streamer(stream, 2);
            Assert.AreEqual(1, streamer.ReadByte());
            Assert.AreEqual(2, streamer.ReadByte());
            Assert.AreEqual(3, streamer.ReadByte());
            Assert.AreEqual(-1, streamer.ReadByte());
        }

        [TestMethod]
        public void Streamer_ReadLine_1()
        {
            Streamer streamer = new Streamer(_GetStream(), 4);
            Assert.IsFalse(streamer.Eof);
            Assert.AreEqual("Hello,", streamer.ReadLine(Encoding.UTF8));
            Assert.IsFalse(streamer.Eof);
            Assert.AreEqual("World!", streamer.ReadLine(Encoding.UTF8));
            Assert.IsTrue(streamer.Eof);
            Assert.IsNull(streamer.ReadLine(Encoding.UTF8));
            Assert.IsTrue(streamer.Eof);
            Assert.IsNull(streamer.ReadLine(Encoding.UTF8));
        }

        [TestMethod]
        public void Streamer_Dispose_1()
        {
            Mock<Stream> mock = new Mock<Stream>();
            mock.SetupGet(one => one.CanRead).Returns(true);
            mock.Setup(one => one.Close());
            Stream stream = mock.Object;
            Streamer streamer = new Streamer(stream, 4);
            streamer.Dispose();
            mock.Verify(one => one.Close());
        }

        [TestMethod]
        public void Streamer_ToArray_1()
        {
            MemoryStream stream = _GetStream();
            byte[] expected = stream.ToArray();
            stream.Position = 0;
            Streamer streamer = new Streamer(stream, 4);
            byte[] actual = streamer.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Streamer_ToArray_2()
        {
            MemoryStream stream = new MemoryStream();
            Streamer streamer = new Streamer(stream, 4);
            byte[] array = streamer.ToArray();
            Assert.AreEqual(0, array.Length);
        }
    }
}
