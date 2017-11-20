using System;
using System.IO;
using System.Text;
using AM.IO;
using AM.Runtime;
using AM.Text;
using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

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
        public void IrbisAlphabetTable_Construction_1()
        {
            IrbisAlphabetTable table = new IrbisAlphabetTable();
            Assert.AreEqual(182, table.Characters.Length);
        }

        [TestMethod]
        public void IrbisAlphabetTable_Construction_2()
        {
            Encoding encoding = IrbisEncoding.Ansi;
            byte[] bytes =
            {
                038, 064, 065, 066, 067, 068, 069, 070, 071, 072,
                073, 074, 075, 076, 077, 078, 079, 080, 081, 082,
                083, 084, 085, 086, 087, 088, 089, 090, 097, 098,
                099, 100, 101, 102, 103, 104, 105, 106, 107, 108,
                109, 110, 111, 112, 113, 114, 115, 116, 117, 118,
                119, 120, 121, 122, 128, 129, 130, 131, 132, 133,
                134, 135, 136, 137, 138, 139, 140, 141, 142, 143,
                144, 145, 146, 147, 148, 149, 150, 151, 152, 153,
                154, 155, 156, 157, 158, 159, 160, 161, 162, 163,
                164, 165, 166, 167, 168, 169, 170, 171, 172, 173,
                174, 175, 176, 177, 178, 179, 180, 181, 182, 183,
                184, 185, 186, 187, 188, 189, 190, 191, 192, 193,
                194, 195, 196, 197, 198, 199, 200, 201, 202, 203,
                204, 205, 206, 207, 208, 209, 210, 211, 212, 213,
                214, 215, 216, 217, 218, 219, 220, 221, 222, 223,
                224, 225, 226, 227, 228, 229, 230, 231, 232, 233,
                234, 235, 236, 237, 238, 239, 240, 241, 242, 243,
                244, 245, 246, 247, 248, 249, 250, 251, 252, 253,
                254, 255
            };
            IrbisAlphabetTable table = new IrbisAlphabetTable(encoding, bytes);
            Assert.AreEqual(182, table.Characters.Length);
        }

        [TestMethod]
        public void IrbisAlphabetTable_Construction_3()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    IrbisAlphabetTable.FileName
                );
            string text = File.ReadAllText(fileName);
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(text);

            IIrbisConnection connection = mock.Object;
            IrbisAlphabetTable table = new IrbisAlphabetTable(connection);
            Assert.AreEqual(182, table.Characters.Length);

            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void IrbisAlphabetTable_Construction_4()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    IrbisAlphabetTable.FileName
                );
            string text = File.ReadAllText(fileName);
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(text);

            IIrbisConnection connection = mock.Object;
            IrbisAlphabetTable table = new IrbisAlphabetTable
                (
                    connection,
                    "unusual.file"
                );
            Assert.AreEqual(182, table.Characters.Length);

            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Once
                );
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

        [TestMethod]
        public void IrbisAlphabetTable_WriteTable_1()
        {
            IrbisAlphabetTable table = _GetTable();
            StringWriter writer = new StringWriter();
            table.WriteTable(writer);
            Assert.AreEqual
                (
                    "038 064 065 066 067 068 069 070 071 072 073 074 075 076 077 078 079 080 081 082 083 084 085 086 087 088 089 090 097 098 099 100\n"
                    + "101 102 103 104 105 106 107 108 109 110 111 112 113 114 115 116 117 118 119 120 121 122 128 129 130 131 132 133 134 135 136 137\n"
                    + "138 139 140 141 142 143 144 145 146 147 148 149 150 151 152 153 154 155 156 157 158 159 160 161 162 163 164 165 166 167 168 169\n"
                    + "170 171 172 173 174 175 176 177 178 179 180 181 182 183 184 185 186 187 188 189 190 191 192 193 194 195 196 197 198 199 200 201\n"
                    + "202 203 204 205 206 207 208 209 210 211 212 213 214 215 216 217 218 219 220 221 222 223 224 225 226 227 228 229 230 231 232 233\n"
                    + "234 235 236 237 238 239 240 241 242 243 244 245 246 247 248 249 250 251 252 253 254 255",
                    writer.ToString().DosToUnix()
                );
        }

        [TestMethod]
        public void IrbisAlphabetTable_WriteLocalFile_1()
        {
            string fileName = Path.GetTempFileName();
            IrbisAlphabetTable table = _GetTable();
            table.WriteLocalFile(fileName);
            string text = File.ReadAllText(fileName).DosToUnix();
            Assert.AreEqual
                (
                    "038 064 065 066 067 068 069 070 071 072 073 074 075 076 077 078 079 080 081 082 083 084 085 086 087 088 089 090 097 098 099 100\n"
                    + "101 102 103 104 105 106 107 108 109 110 111 112 113 114 115 116 117 118 119 120 121 122 128 129 130 131 132 133 134 135 136 137\n"
                    + "138 139 140 141 142 143 144 145 146 147 148 149 150 151 152 153 154 155 156 157 158 159 160 161 162 163 164 165 166 167 168 169\n"
                    + "170 171 172 173 174 175 176 177 178 179 180 181 182 183 184 185 186 187 188 189 190 191 192 193 194 195 196 197 198 199 200 201\n"
                    + "202 203 204 205 206 207 208 209 210 211 212 213 214 215 216 217 218 219 220 221 222 223 224 225 226 227 228 229 230 231 232 233\n"
                    + "234 235 236 237 238 239 240 241 242 243 244 245 246 247 248 249 250 251 252 253 254 255",
                    text
                );
        }

        [TestMethod]
        public void IrbisAlphabetTable_Verify_1()
        {
            IrbisAlphabetTable table = _GetTable();
            Assert.IsTrue(table.Verify(false));
        }

        [TestMethod]
        public void IrbisAlphabetTable_Verify_2()
        {
            Encoding encoding = IrbisEncoding.Ansi;
            byte[] bytes =
            {
                038, 064, 064, 066, 067, 068, 069, 070, 071, 072,
                073, 074, 075, 076, 077, 078, 079, 080, 081, 082,
                083, 084, 085, 086, 087, 088, 089, 090, 097, 098,
                099, 100, 101, 102, 103, 104, 105, 106, 107, 108,
                109, 110, 111, 112, 113, 114, 115, 116, 117, 118,
                119, 120, 121, 122, 128, 129, 130, 131, 132, 133,
                134, 135, 136, 137, 138, 139, 140, 141, 142, 143,
                144, 145, 146, 147, 148, 149, 150, 151, 152, 153,
                154, 155, 156, 157, 158, 159, 160, 161, 162, 163,
                164, 165, 166, 167, 168, 169, 170, 171, 172, 173,
                174, 175, 176, 177, 178, 179, 180, 181, 182, 183,
                184, 185, 186, 187, 188, 189, 190, 191, 192, 193,
                194, 195, 196, 197, 198, 199, 200, 201, 202, 203,
                204, 205, 206, 207, 208, 209, 210, 211, 212, 213,
                214, 215, 216, 217, 218, 219, 220, 221, 222, 223,
                224, 225, 226, 227, 228, 229, 230, 231, 232, 233,
                234, 235, 236, 237, 238, 239, 240, 241, 242, 243,
                244, 245, 246, 247, 248, 249, 250, 251, 252, 253,
                254, 255
            };
            IrbisAlphabetTable table = new IrbisAlphabetTable(encoding, bytes);
            Assert.IsFalse(table.Verify(false));
        }

        [TestMethod]
        public void IrbisAlphabetTable_GetInstance_1()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    IrbisAlphabetTable.FileName
                );
            string text = File.ReadAllText(fileName);
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(text);

            IIrbisConnection connection = mock.Object;
            IrbisAlphabetTable table1 = IrbisAlphabetTable.GetInstance(connection);
            Assert.AreEqual(182, table1.Characters.Length);

            IrbisAlphabetTable table2 = IrbisAlphabetTable.GetInstance(connection);
            Assert.AreSame(table1, table2);

            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Once
                );
        }

    }
}
