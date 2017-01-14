using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Search;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class KeywordPermutatorTest
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

        private IrbisStopWords _GetStopWords()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "IBIS.STW"
                );

            IrbisStopWords result
                = IrbisStopWords.ParseFile(fileName);

            return result;
        }

        [TestMethod]
        public void KeywordPermutator_PermutateWords()
        {
            IrbisAlphabetTable table = _GetTable();
            IrbisStopWords stopWords = _GetStopWords();
            KeywordPermutator permutator
                = new KeywordPermutator
                    (
                        table,
                        stopWords
                    );

            string sourceText = "В лесу родилась елочка";
            string[] result = permutator.PermutateWords(sourceText);
            Assert.AreEqual(4, result.Length);
        }
    }
}
