using System;
using System.IO;
using ManagedIrbis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisStopWordsTest
        : Common.CommonUnitTest
    {
        private const string ibis = "IBIS.STW";

        [TestMethod]
        public void IrbisStopWords_Construction_1()
        {
            IrbisStopWords words = new IrbisStopWords();
            Assert.IsNull(words.FileName);
        }

        [TestMethod]
        public void IrbisStopWords_Construction_2()
        {
            IrbisStopWords words = new IrbisStopWords(ibis);
            Assert.AreEqual(ibis, words.FileName);
        }

        [TestMethod]
        public void IrbisStopWords_ParseFile_1()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    ibis
                );

            IrbisStopWords words = IrbisStopWords.ParseFile
                (
                    fileName
                );
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
        public void IrbisStopWords_ToText_1()
        {
            IrbisStopWords words = new IrbisStopWords();
            string text = words.ToText();
            Assert.AreEqual(0, text.Length);
        }
    }
}
