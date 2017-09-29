using System;
using System.IO;

using AM.IO;
using AM.Runtime;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis
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
        public void IrbisAlphabetTable_ParseLocalFile_1()
        {
            IrbisAlphabetTable table = _GetTable();
            Assert.AreEqual(182, table.Characters.Length);
        }

        [TestMethod]
        public void IrbisAlphabetTable_SplitWords_1()
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
        public void IrbisAlphabetTable_TrimText_1()
        {
            IrbisAlphabetTable table = _GetTable();

            Assert.AreEqual("", table.TrimText(""));
            Assert.AreEqual("", table.TrimText("!?!"));

            Assert.AreEqual("Hello", table.TrimText("Hello"));
            Assert.AreEqual("Hello", table.TrimText("(Hello)"));

            Assert.AreEqual("Привет", table.TrimText("Привет"));
            Assert.AreEqual("Привет", table.TrimText("(Привет)"));

            Assert.AreEqual("Happy New Year", table.TrimText("Happy New Year"));
            Assert.AreEqual("Happy New Year", table.TrimText("Happy New Year!"));
        }

        [TestMethod]
        public void IrbisAlphabetTable_ToSourceCode_1()
        {
            IrbisAlphabetTable table = _GetTable();
            StringWriter writer = new StringWriter();
            table.ToSourceCode(writer);
            string sourceCode = writer.ToString();
            Assert.IsNotNull(sourceCode);
        }

        [TestMethod]
        public void IrbisAlphabetTable_Serialize_1()
        {
            IrbisAlphabetTable table1 = _GetTable();

            byte[] bytes = table1.SaveToMemory();

            IrbisAlphabetTable table2 = bytes
                .RestoreObjectFromMemory<IrbisAlphabetTable>();

            Assert.AreEqual
                (
                    table1.Characters.Length,
                    table2.Characters.Length
                );

            for (int i = 0; i < table1.Characters.Length; i++)
            {
                Assert.AreEqual
                    (
                        table1.Characters[i],
                        table2.Characters[i]
                    );
            }
        }
    }
}
