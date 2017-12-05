using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

using JetBrains.Annotations;

using Moq;

// ReSharper disable UseObjectOrCollectionInitializer

namespace UnitTests.AM.IO
{
    [TestClass]
    public class NotifyStreamTest
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
        public void NotifyStream_Construction_1()
        {
            Stream innerStream = Stream.Null;
            NotifyStream stream = new NotifyStream(innerStream);
            Assert.AreSame(innerStream, stream.InnerStream);
        }

        [TestMethod]
        public void NotifyStream_Flush_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NotifyStream stream = new NotifyStream(innerStream);
            stream.Flush();
            mock.Verify(s => s.Flush(), Times.Once);
        }

        [TestMethod]
        public void NotifyStream_Seek_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NotifyStream stream = new NotifyStream(innerStream);
            long expected = 123;
            long actual = stream.Seek(expected, SeekOrigin.Begin);
            Assert.AreEqual(expected, actual);
            mock.Verify(s => s.Seek(It.IsAny<long>(), It.IsAny<SeekOrigin>()),
                Times.Once);
        }

        [TestMethod]
        public void NotifyStream_SetLength_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NotifyStream stream = new NotifyStream(innerStream);
            stream.SetLength(123);
            mock.Verify(s => s.SetLength(It.IsAny<long>()), Times.Once);
        }

        [TestMethod]
        public void NotifyStream_Read_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NotifyStream stream = new NotifyStream(innerStream);
            byte[] buffer = new byte[100];
            stream.Read(buffer, 0, buffer.Length);
            mock.Verify(s => s.Read(It.IsAny<byte[]>(),
                It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void NotifyStream_Write_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NotifyStream stream = new NotifyStream(innerStream);
            byte[] buffer = new byte[100];
            stream.Write(buffer, 0, buffer.Length);
            mock.Verify(s => s.Write(It.IsAny<byte[]>(),
                It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void NotifyStream_CanRead_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NotifyStream stream = new NotifyStream(innerStream);
            Assert.IsTrue(stream.CanRead);
            mock.VerifyGet(s => s.CanRead, Times.Once);
        }

        [TestMethod]
        public void NotifyStream_CanSeek_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NotifyStream stream = new NotifyStream(innerStream);
            Assert.IsTrue(stream.CanSeek);
            mock.VerifyGet(s => s.CanSeek, Times.Once);
        }

        [TestMethod]
        public void NotifyStream_CanWrite_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NotifyStream stream = new NotifyStream(innerStream);
            Assert.IsTrue(stream.CanWrite);
            mock.VerifyGet(s => s.CanWrite, Times.Once);
        }

        [TestMethod]
        public void NotifyStream_Length_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NotifyStream stream = new NotifyStream(innerStream);
            Assert.AreEqual(123, stream.Length);
            mock.VerifyGet(s => s.Length, Times.Once);
        }

        [TestMethod]
        public void NotifyStream_Position_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NotifyStream stream = new NotifyStream(innerStream);
            Assert.AreEqual(123L, stream.Position);
            mock.VerifyGet(s => s.Position, Times.Once);
        }

        [TestMethod]
        public void NotifyStream_Position_2()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NotifyStream stream = new NotifyStream(innerStream);
            stream.Position = 123;
            mock.VerifySet(s => s.Position = It.IsAny<long>(), Times.Once);
        }

        [TestMethod]
        public void NotifyStream_Close_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NotifyStream stream = new NotifyStream(innerStream);
            stream.Close();
            mock.Verify(s => s.Close(), Times.Never);
        }

        [TestMethod]
        public void NotifyStream_Dispose_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream innerStream = mock.Object;
            NotifyStream stream = new NotifyStream(innerStream);
            ((IDisposable)stream).Dispose();
            mock.Verify(s => s.Close(), Times.Once);
        }


        [TestMethod]
        public void NotifyStream_StreamChanged_1()
        {
            Stream baseStream = Stream.Null;
            NotifyStream notifyStream = new NotifyStream(baseStream);
            int counter = 0;
            notifyStream.StreamChanged += (sender, args) =>
            {
                counter++;
            };

            const int expected = 10;
            for (int i = 0; i < expected; i++)
            {
                byte[] bytes = new byte[10];
                notifyStream.Write(bytes, 0, bytes.Length);
            }

            Assert.AreEqual(expected, counter);
        }
    }
}
