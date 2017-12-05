using System;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

using JetBrains.Annotations;

using Moq;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class NonCloseableStreamReaderTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetFileName()
        {
            string result = Path.Combine(TestDataPath, "record.txt");

            return result;
        }

        [NotNull]
        private Mock<Stream> _GetMock()
        {
            Mock<Stream> result = new Mock<Stream>();

            result.SetupGet(s => s.CanRead).Returns(true);

            result.Setup(s => s.Close());

            return result;
        }

        [TestMethod]
        public void NonCloseableStreamReader_Construction_1()
        {
            Stream stream = Stream.Null;
            NonCloseableStreamReader reader = new NonCloseableStreamReader(stream);
            Assert.AreSame(stream, reader.BaseStream);
            reader.Dispose();
        }

        [TestMethod]
        public void NonCloseableStreamReader_Construction_2()
        {
            string fileName = _GetFileName();
            NonCloseableStreamReader reader = new NonCloseableStreamReader(fileName);
            reader.Dispose();
        }

        [TestMethod]
        public void NonCloseableStreamReader_Construction_3()
        {
            Stream stream = Stream.Null;
            NonCloseableStreamReader reader = new NonCloseableStreamReader(stream, false);
            Assert.AreSame(stream, reader.BaseStream);
            reader.Dispose();
        }

        [TestMethod]
        public void NonCloseableStreamReader_Construction_4()
        {
            Stream stream = Stream.Null;
            Encoding encoding = Encoding.ASCII;
            NonCloseableStreamReader reader = new NonCloseableStreamReader(stream, encoding);
            Assert.AreSame(stream, reader.BaseStream);
            Assert.AreSame(encoding, reader.CurrentEncoding);
            reader.Dispose();
        }

        [TestMethod]
        public void NonCloseableStreamReader_Construction_5()
        {
            string fileName = _GetFileName();
            NonCloseableStreamReader reader = new NonCloseableStreamReader(fileName, false);
            reader.Dispose();
        }

        [TestMethod]
        public void NonCloseableStreamReader_Construction_6()
        {
            string fileName = _GetFileName();
            Encoding encoding = Encoding.ASCII;
            NonCloseableStreamReader reader = new NonCloseableStreamReader(fileName, encoding);
            Assert.AreSame(encoding, reader.CurrentEncoding);
            reader.Dispose();
        }

        [TestMethod]
        public void NonCloseableStreamReader_Construction_7()
        {
            Stream stream = Stream.Null;
            Encoding encoding = Encoding.ASCII;
            NonCloseableStreamReader reader = new NonCloseableStreamReader(stream, encoding, false);
            Assert.AreSame(stream, reader.BaseStream);
            Assert.AreSame(encoding, reader.CurrentEncoding);
            reader.Dispose();
        }

        [TestMethod]
        public void NonCloseableStreamReader_Construction_8()
        {
            string fileName = _GetFileName();
            Encoding encoding = Encoding.ASCII;
            NonCloseableStreamReader reader = new NonCloseableStreamReader(fileName, encoding, false);
            Assert.AreSame(encoding, reader.CurrentEncoding);
            reader.Dispose();
        }

        [TestMethod]
        public void NonCloseableStreamReader_Construction_9()
        {
            Stream stream = Stream.Null;
            Encoding encoding = Encoding.ASCII;
            StreamReader innerReader = new StreamReader(stream, encoding);
            NonCloseableStreamReader outerReader = new NonCloseableStreamReader(innerReader);
            Assert.AreSame(stream, outerReader.BaseStream);
            Assert.AreSame(encoding, outerReader.CurrentEncoding);
            outerReader.Dispose();
            innerReader.Dispose();
        }

        [TestMethod]
        public void NonCloseableStreamReader_Close_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream stream = mock.Object;
            NonCloseableStreamReader reader = new NonCloseableStreamReader(stream);
            reader.Close();
            mock.Verify(s => s.Close(), Times.Never);
        }

        [TestMethod]
        public void NonCloseableStreamReader_Dispose_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream stream = mock.Object;
            NonCloseableStreamReader reader = new NonCloseableStreamReader(stream);
            ((IDisposable)reader).Dispose();
            mock.Verify(s => s.Close(), Times.Never);
        }

        [TestMethod]
        public void NonCloseableStreamReader_Dispose_2()
        {
            Mock<Stream> mock = _GetMock();
            Stream stream = mock.Object;
            NonCloseableStreamReader reader = new NonCloseableStreamReader(stream);
            reader.Dispose();
            mock.Verify(s => s.Close(), Times.Never);
        }

        [TestMethod]
        public void NonCloseableStreamReader_ReallyClose_1()
        {
            Mock<Stream> mock = _GetMock();
            Stream stream = mock.Object;
            NonCloseableStreamReader reader = new NonCloseableStreamReader(stream);
            reader.ReallyClose();
            mock.Verify(s => s.Close(), Times.Once);
        }
    }
}
