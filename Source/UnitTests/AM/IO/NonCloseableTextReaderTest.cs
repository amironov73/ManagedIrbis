using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

using JetBrains.Annotations;

using Moq;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class NonCloseableTextReaderTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private Mock<TextReader> _GetReader()
        {
            Mock<TextReader> mock = new Mock<TextReader>();

            mock.Setup(r => r.Close());
            mock.Setup(r => r.Peek()).Returns(-1);
            mock.Setup(r => r.Read()).Returns(-1);
            mock.Setup(r => r.Read(It.IsAny<char[]>(), It.IsAny<int>(), It.IsAny<int>())).Returns(0);
            mock.Setup(r => r.ReadBlock(It.IsAny<char[]>(), It.IsAny<int>(), It.IsAny<int>())).Returns(0);
            mock.Setup(r => r.ReadLine()).Returns(string.Empty);
            mock.Setup(r => r.ReadToEnd()).Returns(string.Empty);

            return mock;
        }

        [TestMethod]
        public void NonCloseableTextReader_Construction_1()
        {
            TextReader textReader = new StringReader("hello");
            NonCloseableTextReader reader = new NonCloseableTextReader(textReader);
            reader.Dispose();
        }

        //[TestMethod]
        //public void NonCloseableTextReader_ReallyClose_1()
        //{
        //    Mock<TextReader> mock = _GetReader();
        //    NonCloseableTextReader reader = new NonCloseableTextReader(mock.Object);
        //    reader.ReallyClose();
        //    mock.Verify(r => r.Close(), Times.Once);
        //}

        [TestMethod]
        public void NonCloseableTextReader_Close_1()
        {
            Mock<TextReader> mock = _GetReader();
            NonCloseableTextReader reader = new NonCloseableTextReader(mock.Object);
            reader.Close();
            mock.Verify(r => r.Close(), Times.Never);
        }

        [TestMethod]
        public void NonCloseableTextReader_Dispose_1()
        {
            Mock<TextReader> mock = _GetReader();
            NonCloseableTextReader reader = new NonCloseableTextReader(mock.Object);
            reader.Dispose();
            mock.Verify(r => r.Close(), Times.Never);
        }

        [TestMethod]
        public void NonCloseableTextReader_Peek_1()
        {
            Mock<TextReader> mock = _GetReader();
            NonCloseableTextReader reader = new NonCloseableTextReader(mock.Object);
            Assert.AreEqual(-1, reader.Peek());
            mock.Verify(r => r.Peek(), Times.Once);
        }

        [TestMethod]
        public void NonCloseableTextReader_Read_1()
        {
            Mock<TextReader> mock = _GetReader();
            NonCloseableTextReader reader = new NonCloseableTextReader(mock.Object);
            Assert.AreEqual(-1, reader.Read());
            mock.Verify(r => r.Read(), Times.Once);
        }

        [TestMethod]
        public void NonCloseableTextReader_Read_2()
        {
            Mock<TextReader> mock = _GetReader();
            NonCloseableTextReader reader = new NonCloseableTextReader(mock.Object);
            char[] buffer = new char[10];
            Assert.AreEqual(0, reader.Read(buffer, 0, buffer.Length));
            mock.Verify(r => r.Read(It.IsAny<char[]>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void NonCloseableTextReader_ReadBlock_1()
        {
            Mock<TextReader> mock = _GetReader();
            NonCloseableTextReader reader = new NonCloseableTextReader(mock.Object);
            char[] buffer = new char[10];
            Assert.AreEqual(0, reader.ReadBlock(buffer, 0, buffer.Length));
            mock.Verify(r => r.ReadBlock(It.IsAny<char[]>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void NonCloseableTextReader_ReadLine_1()
        {
            Mock<TextReader> mock = _GetReader();
            NonCloseableTextReader reader = new NonCloseableTextReader(mock.Object);
            Assert.AreEqual(string.Empty, reader.ReadLine());
            mock.Verify(r => r.ReadLine(), Times.Once);
        }

        [TestMethod]
        public void NonCloseableTextReader_ReadToEnd_1()
        {
            Mock<TextReader> mock = _GetReader();
            NonCloseableTextReader reader = new NonCloseableTextReader(mock.Object);
            Assert.AreEqual(string.Empty, reader.ReadToEnd());
            mock.Verify(r => r.ReadToEnd(), Times.Once);
        }
    }
}
