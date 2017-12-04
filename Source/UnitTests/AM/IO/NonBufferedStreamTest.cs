using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

using Moq;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class NonBufferedStreamTest
    {
        private Mock<Stream> _GetMock()
        {
            Mock<Stream> result = new Mock<Stream>();

            result.Setup(s => s.Flush());

            result.Setup(s => s.Seek(It.IsAny<long>(), It.IsAny<SeekOrigin>()))
                .Returns((long offset, SeekOrigin origin) => offset);

            result.Setup(s => s.SetLength(It.IsAny<long>()));

            result.Setup(s => s.Read(It.IsAny<byte[]>(),
                It.IsAny<int>(), It.IsAny<int>()))
                .Returns(1);

            result.Setup(s => s.Write(It.IsAny<byte[]>(),
                It.IsAny<int>(), It.IsAny<int>()));

            result.SetupGet(s => s.CanRead).Returns(true);

            result.SetupGet(s => s.CanSeek).Returns(true);

            result.SetupGet(s => s.CanWrite).Returns(true);

            result.SetupGet(s => s.Length).Returns(123);

            result.SetupGet(s => s.Position).Returns(123);
            result.SetupSet<long>(s => s.Position = It.IsAny<long>());

            result.Setup(s => s.ToString())
                .Returns("InnerStream");

            return result;
        }

        [TestMethod]
        public void NonBufferedStream_Flush_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonBufferedStream stream = new NonBufferedStream(innerStream);
            stream.Flush();
            mock.Verify(s => s.Flush(), Times.Once);
        }

        [TestMethod]
        public void NonBufferedStream_Seek_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonBufferedStream stream = new NonBufferedStream(innerStream);
            long expected = 123;
            long actual = stream.Seek(expected, SeekOrigin.Begin);
            Assert.AreEqual(expected, actual);
            mock.Verify(s => s.Seek(It.IsAny<long>(), It.IsAny<SeekOrigin>()),
                Times.Once);
        }

        [TestMethod]
        public void NonBufferedStream_SetLength_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonBufferedStream stream = new NonBufferedStream(innerStream);
            stream.SetLength(123);
            mock.Verify(s => s.SetLength(It.IsAny<long>()), Times.Once);
        }

        [TestMethod]
        public void NonBufferedStream_Read_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonBufferedStream stream = new NonBufferedStream(innerStream);
            byte[] buffer = new byte[100];
            stream.Read(buffer, 0, buffer.Length);
            mock.Verify(s => s.Read(It.IsAny<byte[]>(),
                It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mock.Verify(s => s.Flush(), Times.Once);
        }

        [TestMethod]
        public void NonBufferedStream_Write_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonBufferedStream stream = new NonBufferedStream(innerStream);
            byte[] buffer = new byte[100];
            stream.Write(buffer, 0, buffer.Length);
            mock.Verify(s => s.Write(It.IsAny<byte[]>(),
                It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mock.Verify(s => s.Flush(), Times.Once);
        }

        [TestMethod]
        public void NonBufferedStream_CanRead_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonBufferedStream stream = new NonBufferedStream(innerStream);
            Assert.IsTrue(stream.CanRead);
            mock.VerifyGet(s => s.CanRead, Times.Once);
        }

        [TestMethod]
        public void NonBufferedStream_CanSeek_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonBufferedStream stream = new NonBufferedStream(innerStream);
            Assert.IsTrue(stream.CanSeek);
            mock.VerifyGet(s => s.CanSeek, Times.Once);
        }

        [TestMethod]
        public void NonBufferedStream_CanWrite_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonBufferedStream stream = new NonBufferedStream(innerStream);
            Assert.IsTrue(stream.CanWrite);
            mock.VerifyGet(s => s.CanWrite, Times.Once);
        }

        [TestMethod]
        public void NonBufferedStream_Length_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonBufferedStream stream = new NonBufferedStream(innerStream);
            Assert.AreEqual(123, stream.Length);
            mock.VerifyGet(s => s.Length, Times.Once);
        }

        [TestMethod]
        public void NonBufferedStream_Position_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonBufferedStream stream = new NonBufferedStream(innerStream);
            Assert.AreEqual(123L, stream.Position);
            mock.VerifyGet(s => s.Position, Times.Once);
        }

        [TestMethod]
        public void NonBufferedStream_Position_2()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonBufferedStream stream = new NonBufferedStream(innerStream);
            stream.Position = 123;
            mock.VerifySet(s => s.Position = It.IsAny<long>(), Times.Once);
        }

        [TestMethod]
        public void NonBufferedStream_ToString_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonBufferedStream stream = new NonBufferedStream(innerStream);
            Assert.AreEqual("InnerStream", stream.ToString());
            mock.Verify(s => s.ToString(), Times.Once);
        }
    }
}
