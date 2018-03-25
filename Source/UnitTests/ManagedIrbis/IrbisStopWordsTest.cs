using System.IO;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisStopWordsTest
        : Common.CommonUnitTest
    {
        private const string Ibis = "IBIS.STW";

        [TestMethod]
        public void IrbisStopWords_Construction_1()
        {
            IrbisStopWords words = new IrbisStopWords();
            Assert.IsNull(words.FileName);
        }

        [TestMethod]
        public void IrbisStopWords_Construction_2()
        {
            IrbisStopWords words = new IrbisStopWords(Ibis);
            Assert.AreEqual(Ibis, words.FileName);
        }

        [TestMethod]
        public void IrbisStopWords_FromServer_1()
        {
            string fileName = Path.Combine(TestDataPath, Ibis);
            string content = File.ReadAllText(fileName, IrbisEncoding.Ansi);
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>())).Returns(content);
            mock.SetupGet(c => c.Database).Returns("IBIS");
            IIrbisConnection connection = mock.Object;
            IrbisStopWords words = IrbisStopWords.FromServer(connection);
            Assert.IsTrue(words.IsStopWord("О"));
        }

        [TestMethod]
        public void IrbisStopWords_FromServer_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>())).Returns(string.Empty);
            mock.SetupGet(c => c.Database).Returns("IBIS");
            IIrbisConnection connection = mock.Object;
            IrbisStopWords words = IrbisStopWords.FromServer(connection);
            Assert.IsFalse(words.IsStopWord("О"));
        }

        [TestMethod]
        public void IrbisStopWords_FromServer_3()
        {
            string fileName = Path.Combine(TestDataPath, Ibis);
            string content = File.ReadAllText(fileName, IrbisEncoding.Ansi);
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>())).Returns(content);
            IIrbisConnection connection = mock.Object;
            IrbisStopWords words = IrbisStopWords.FromServer(connection, "IBIS", "ANY.STW");
            Assert.IsTrue(words.IsStopWord("О"));
        }

        [TestMethod]
        public void IrbisStopWords_ParseFile_1()
        {
            string fileName = Path.Combine(TestDataPath, Ibis);
            IrbisStopWords words = IrbisStopWords.ParseFile(fileName);
            Assert.IsTrue(words.IsStopWord("О"));
        }

        [TestMethod]
        public void IrbisStopWords_IsStopWord_1()
        {
            IrbisStopWords words = new IrbisStopWords();
            Assert.IsTrue(words.IsStopWord(string.Empty));
        }

        [TestMethod]
        public void IrbisStopWords_IsStopWord_2()
        {
            IrbisStopWords words = new IrbisStopWords();
            Assert.IsTrue(words.IsStopWord(" "));
        }

        [TestMethod]
        public void IrbisStopWords_ToLines_1()
        {
            IrbisStopWords words = new IrbisStopWords();
            string[] lines = words.ToLines();
            Assert.AreEqual(0, lines.Length);
        }

        [TestMethod]
        public void IrbisStopWords_ToLines_2()
        {
            IrbisStopWords words = IrbisStopWords.ParseLines
                (
                    "IBIS",
                    new[] { "WORLD", "Hello" }
                );
            string[] lines = words.ToLines();
            Assert.AreEqual(2, lines.Length);
        }

        [TestMethod]
        public void IrbisStopWords_ToText_1()
        {
            IrbisStopWords words = new IrbisStopWords();
            string text = words.ToText();
            Assert.AreEqual(0, text.Length);
        }
    }
}
