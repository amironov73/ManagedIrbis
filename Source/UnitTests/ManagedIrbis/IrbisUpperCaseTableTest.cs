using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisUpperCaseTableTest
        : Common.CommonUnitTest
    {
        private IrbisUpperCaseTable _GetTable()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    IrbisUpperCaseTable.FileName
                );

            IrbisUpperCaseTable result
                = IrbisUpperCaseTable.ParseLocalFile(fileName);

            return result;
        }

        [TestMethod]
        public void IrbisUpperCaseTable_ToUpper()
        {
            IrbisUpperCaseTable table = _GetTable();

            Assert.AreEqual("HELLO", table.ToUpper("Hello"));
            Assert.AreEqual("СЪЕШЬ", table.ToUpper("Съешь"));
            Assert.AreEqual("ЕЩЕ", table.ToUpper("ещё"));
        }

        [TestMethod]
        public void IrbisUpperCaseTable_ToSourceCode()
        {
            IrbisUpperCaseTable table = _GetTable();
            StringWriter writer = new StringWriter();
            table.ToSourceCode(writer);
            string sourceCode = writer.ToString();
            Assert.IsNotNull(sourceCode);
        }
    }
}
