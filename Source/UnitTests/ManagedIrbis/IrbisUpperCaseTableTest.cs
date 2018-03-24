using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AM;
using AM.Runtime;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

// ReSharper disable MustUseReturnValue
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable PossibleNullReferenceException

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisUpperCaseTableTest
        : Common.CommonUnitTest
    {
        private IrbisUpperCaseTable _GetTable()
        {
            string fileName = Path.Combine(TestDataPath, IrbisUpperCaseTable.FileName);
            IrbisUpperCaseTable result = IrbisUpperCaseTable.ParseLocalFile(fileName);

            return result;
        }

        [TestMethod]
        public void IrbisUpperCaseTable_Construction_1()
        {
            IrbisUpperCaseTable table = new IrbisUpperCaseTable();
            Assert.IsNotNull(table);
        }

        [TestMethod]
        public void IrbisUpperCaseTable_Construction_2()
        {
            byte[] bytes =
                {
                    000, 001, 002, 003, 004, 005, 006, 007,
                    008, 009, 010, 011, 012, 013, 014, 015,
                    016, 017, 018, 019, 020, 021, 022, 023,
                    024, 025, 026, 027, 028, 028, 030, 031,
                    032, 033, 034, 035, 036, 037, 038, 039,
                    040, 041, 042, 043, 044, 045, 046, 047,
                    048, 049, 050, 051, 052, 053, 054, 055,
                    056, 057, 058, 059, 060, 061, 062, 063,
                    064, 065, 066, 067, 068, 069, 070, 071,
                    072, 073, 074, 075, 076, 077, 078, 079,
                    080, 081, 082, 083, 084, 085, 086, 087,
                    088, 089, 090, 091, 092, 093, 094, 095,
                    096, 065, 066, 067, 068, 069, 070, 071,
                    072, 073, 074, 075, 076, 077, 078, 079,
                    080, 081, 082, 083, 084, 085, 086, 087,
                    088, 089, 090, 123, 124, 125, 126, 127,
                    128, 129, 130, 131, 132, 133, 134, 135,
                    136, 137, 138, 139, 140, 141, 142, 143,
                    144, 145, 146, 147, 148, 149, 150, 151,
                    152, 153, 154, 155, 156, 157, 158, 159,
                    160, 161, 161, 163, 164, 165, 166, 167,
                    168, 169, 170, 171, 172, 173, 174, 175,
                    176, 177, 178, 178, 165, 181, 182, 183,
                    168, 185, 170, 187, 163, 189, 189, 175,
                    192, 193, 194, 195, 196, 197, 198, 199,
                    200, 201, 202, 203, 204, 205, 206, 207,
                    208, 209, 210, 211, 212, 213, 214, 215,
                    216, 217, 218, 219, 220, 221, 222, 223,
                    192, 193, 194, 195, 196, 197, 198, 199,
                    200, 201, 202, 203, 204, 205, 206, 207,
                    208, 209, 210, 211, 212, 213, 214, 215,
                    216, 217, 218, 219, 220, 221, 222, 223
                };
            IrbisUpperCaseTable table = new IrbisUpperCaseTable(IrbisEncoding.Ansi, bytes);
            Assert.IsNotNull(table);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void IrbisUpperCaseTable_Construction_3()
        {
            new IrbisUpperCaseTable(Encoding.Unicode, EmptyArray<byte>.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void IrbisUpperCaseTable_Construction_4()
        {
            new IrbisUpperCaseTable(IrbisEncoding.Ansi, EmptyArray<byte>.Value);
        }

        [TestMethod]
        public void IrbisUpperCaseTable_FromServer_1()
        {
            string fileName = Path.Combine(TestDataPath, IrbisUpperCaseTable.FileName);
            string result = File.ReadAllText(fileName, IrbisEncoding.Ansi);
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>())).Returns(result);
            IIrbisConnection connection = mock.Object;
            IrbisUpperCaseTable table = IrbisUpperCaseTable.FromServer(connection, IrbisUpperCaseTable.FileName);
            Assert.IsNotNull(table);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisNetworkException))]
        public void IrbisUpperCaseTable_FromServer_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>())).Returns(string.Empty);
            IIrbisConnection connection = mock.Object;
            IrbisUpperCaseTable.FromServer(connection, IrbisUpperCaseTable.FileName);
        }

        [TestMethod]
        public void IrbisUpperCaseTable_GetInstance_1()
        {
            string fileName = Path.Combine(TestDataPath, IrbisUpperCaseTable.FileName);
            string result = File.ReadAllText(fileName, IrbisEncoding.Ansi);
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>())).Returns(result);
            IIrbisConnection connection = mock.Object;
            IrbisUpperCaseTable table = IrbisUpperCaseTable.GetInstance(connection);
            Assert.IsNotNull(table);
        }

        [TestMethod]
        public void IrbisUpperCaseTable_ResetInstance_1()
        {
            IrbisUpperCaseTable.ResetInstance();
        }

        private void _TestSerialization
            (
                [NotNull] IrbisUpperCaseTable first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IrbisUpperCaseTable second = bytes.RestoreObjectFromMemory<IrbisUpperCaseTable>();
            Dictionary<char, char>.KeyCollection firstKeys = first.Mapping.Keys;
            Dictionary<char, char>.KeyCollection secondKeys = second.Mapping.Keys;
            CollectionAssert.AreEqual(firstKeys, secondKeys);
        }

        [TestMethod]
        public void IrbisUpperCaseTable_Serialization_1()
        {
            IrbisUpperCaseTable table = _GetTable();
            _TestSerialization(table);
        }

        [TestMethod]
        public void IrbisUpperCaseTable_ToUpper_1()
        {
            IrbisUpperCaseTable table = _GetTable();

            Assert.AreEqual('H', table.ToUpper('h'));
            Assert.AreEqual('Ъ', table.ToUpper('ъ'));
            Assert.AreEqual('Е', table.ToUpper('ё'));
            Assert.AreEqual('Е', table.ToUpper('ё'));
            Assert.AreEqual('œ', table.ToUpper('œ'));
        }

        [TestMethod]
        public void IrbisUpperCaseTable_ToUpper_2()
        {
            IrbisUpperCaseTable table = _GetTable();

            Assert.AreEqual("HELLO", table.ToUpper("Hello"));
            Assert.AreEqual("СЪЕШЬ", table.ToUpper("Съешь"));
            Assert.AreEqual("ЕЩЕ", table.ToUpper("ещё"));
            Assert.AreEqual("NOëL", table.ToUpper("Noël"));
        }

        [TestMethod]
        public void IrbisUpperCaseTable_ToSourceCode_1()
        {
            IrbisUpperCaseTable table = _GetTable();
            StringWriter writer = new StringWriter();
            table.ToSourceCode(writer);
            string sourceCode = writer.ToString();
            Assert.IsNotNull(sourceCode);
        }

        [TestMethod]
        public void IrbisUpperCaseTable_WriteLocalFile_1()
        {
            IrbisUpperCaseTable table = _GetTable();
            string fileName = Path.GetTempFileName();
            table.WriteLocalFile(fileName);
            int length = File.ReadAllText(fileName, IrbisEncoding.Ansi).DosToUnix().Length;
            Assert.AreEqual(1024, length);
        }

        [TestMethod]
        public void IrbisUpperCaseTable_WriteTable_1()
        {
            IrbisUpperCaseTable table = _GetTable();
            StringWriter writer = new StringWriter();
            table.WriteTable(writer);
            Assert.AreEqual(1024, writer.ToString().DosToUnix().Length);
        }

        [TestMethod]
        public void IrbisUpperCaseTable_Verify_1()
        {
            IrbisUpperCaseTable table = _GetTable();
            Assert.IsTrue(table.Verify(true));
        }
    }
}
