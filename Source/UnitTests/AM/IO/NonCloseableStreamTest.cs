using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

using JetBrains.Annotations;

using Moq;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class NonCloseableStreamTest
    {
        [NotNull]
        private Mock<Stream> _GetMock()
        {
            Mock<Stream> result = new Mock<Stream>();

            result.Setup(c => c.Close());

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

            return result;
        }

        [TestMethod]
        public void NonCloseableStream_Construction_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonCloseableStream stream = new NonCloseableStream(innerStream);
            Assert.AreSame(innerStream, stream.InnerStream);
        }

        [TestMethod]
        public void NonCloseableStream_Flush_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonCloseableStream stream = new NonCloseableStream(innerStream);
            stream.Flush();
            mock.Verify(s => s.Flush(), Times.Once);
        }

        [TestMethod]
        public void NonCloseableStream_Seek_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonCloseableStream stream = new NonCloseableStream(innerStream);
            long expected = 123;
            long actual = stream.Seek(expected, SeekOrigin.Begin);
            Assert.AreEqual(expected, actual);
            mock.Verify(s => s.Seek(It.IsAny<long>(), It.IsAny<SeekOrigin>()),
                Times.Once);
        }

        [TestMethod]
        public void NonCloseableStream_SetLength_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonCloseableStream stream = new NonCloseableStream(innerStream);
            stream.SetLength(123);
            mock.Verify(s => s.SetLength(It.IsAny<long>()), Times.Once);
        }

        [TestMethod]
        public void NonCloseableStream_Read_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonCloseableStream stream = new NonCloseableStream(innerStream);
            byte[] buffer = new byte[100];
            stream.Read(buffer, 0, buffer.Length);
            mock.Verify(s => s.Read(It.IsAny<byte[]>(),
                It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void NonCloseableStream_Write_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonCloseableStream stream = new NonCloseableStream(innerStream);
            byte[] buffer = new byte[100];
            stream.Write(buffer, 0, buffer.Length);
            mock.Verify(s => s.Write(It.IsAny<byte[]>(),
                It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void NonCloseableStream_CanRead_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonCloseableStream stream = new NonCloseableStream(innerStream);
            Assert.IsTrue(stream.CanRead);
            mock.VerifyGet(s => s.CanRead, Times.Once);
        }

        [TestMethod]
        public void NonCloseableStream_CanSeek_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonCloseableStream stream = new NonCloseableStream(innerStream);
            Assert.IsTrue(stream.CanSeek);
            mock.VerifyGet(s => s.CanSeek, Times.Once);
        }

        [TestMethod]
        public void NonCloseableStream_CanWrite_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonCloseableStream stream = new NonCloseableStream(innerStream);
            Assert.IsTrue(stream.CanWrite);
            mock.VerifyGet(s => s.CanWrite, Times.Once);
        }

        [TestMethod]
        public void NonCloseableStream_Length_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonCloseableStream stream = new NonCloseableStream(innerStream);
            Assert.AreEqual(123, stream.Length);
            mock.VerifyGet(s => s.Length, Times.Once);
        }

        [TestMethod]
        public void NonCloseableStream_Position_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonCloseableStream stream = new NonCloseableStream(innerStream);
            Assert.AreEqual(123L, stream.Position);
            mock.VerifyGet(s => s.Position, Times.Once);
        }

        [TestMethod]
        public void NonCloseableStream_Position_2()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonCloseableStream stream = new NonCloseableStream(innerStream);
            stream.Position = 123;
            mock.VerifySet(s => s.Position = It.IsAny<long>(), Times.Once);
        }

        [TestMethod]
        public void NonCloseableStream_Close_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonCloseableStream stream = new NonCloseableStream(innerStream);
            stream.Close();
            mock.Verify(s => s.Close(), Times.Never);
        }

        [TestMethod]
        public void NonCloseableStream_Dispose_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonCloseableStream stream = new NonCloseableStream(innerStream);
            ((IDisposable)stream).Dispose();
            mock.Verify(s => s.Close(), Times.Never);
        }

        [TestMethod]
        public void NonCloseableStream_ReallyClose_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NonCloseableStream stream = new NonCloseableStream(innerStream);
            stream.ReallyClose();
        }
    }
}
