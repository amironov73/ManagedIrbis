using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;

namespace UnitTests.ManagedClient
{
    [TestClass]
    public class IrbisAlphabetTableTest
        : Common.CommonUnitTest
    {
        private IrbisAlphabetTable _GetTable()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    IrbisAlphabetTable.FileName
                );

            IrbisAlphabetTable result
                = IrbisAlphabetTable.ParseLocalFile(fileName);

            return result;
        }

        [TestMethod]
        public void TestIrbisAlphabetTableParseLocalFile()
        {
            IrbisAlphabetTable table = _GetTable();
            Assert.AreEqual(182, table.Characters.Length);
        }

        [TestMethod]
        public void TestIrbisAlphabetTableSplitText()
        {
            IrbisAlphabetTable table = _GetTable();
            const string text = "Hello, world! Съешь ещё(этих)мягких "
                + "французских булок?12345 вышел зайчик погулять.";
            string[] words = table.SplitWords(text);
            Assert.AreEqual(11, words.Length);
            Assert.AreEqual("Hello", words[0]);
            Assert.AreEqual("world", words[1]);
            Assert.AreEqual("Съешь", words[2]);
            Assert.AreEqual("ещё", words[3]);
            Assert.AreEqual("этих", words[4]);
            Assert.AreEqual("мягких", words[5]);
            Assert.AreEqual("французских", words[6]);
            Assert.AreEqual("булок", words[7]);
            Assert.AreEqual("вышел", words[8]);
            Assert.AreEqual("зайчик", words[9]);
            Assert.AreEqual("погулять", words[10]);
        }

        [TestMethod]
        public void TestIrbisAlphabetTableToSourceCode()
        {
            IrbisAlphabetTable table = _GetTable();
            StringWriter writer = new StringWriter();
            table.ToSourceCode(writer);
            string sourceCode = writer.ToString();
            Assert.IsNotNull(sourceCode);
        }
    }
}
