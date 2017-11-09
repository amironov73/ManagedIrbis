using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Search;

using Moq;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class AsyncExtensionsTest
    {
        [TestMethod]
        public void AsyncExtensions_ConnectAsync_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IniFile iniFile1 = new IniFile();
            mock.Setup(c => c.Connect()).Returns(iniFile1);
            IIrbisConnection connection = mock.Object;

            IniFile iniFile2 = connection.ConnectAsync().Result;
            Assert.AreSame(iniFile1, iniFile2);

            mock.Verify
                (
                    c => c.Connect(),
                    Times.Once
                );
        }
        
        [TestMethod]
        public void AsyncExtensions_DisconnectAsync_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.Dispose());
            IIrbisConnection connection = mock.Object;

            connection.DisconnectAsync().Wait();

            mock.Verify
                (
                    c => c.Dispose(),
                    Times.Once
                );
        }

        [TestMethod]
        public void AsyncExtensions_FormatRecordAsync_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string expected = "FORMATTED";
            mock.Setup(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            string actual = connection.FormatRecordAsync("@format", 1).Result;
            Assert.AreEqual(expected, actual);

            mock.Verify
                (
                    c => c.FormatRecord(It.IsAny<string>(), It.IsAny<int>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void AsyncExtensions_FormatRecordAsync_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string expected = "FORMATTED";
            mock.Setup(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<MarcRecord>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            string actual = connection.FormatRecordAsync("@format", new MarcRecord()).Result;
            Assert.AreEqual(expected, actual);

            mock.Verify
                (
                    c => c.FormatRecord(It.IsAny<string>(), It.IsAny<MarcRecord>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void AsyncExtensions_GetMaxMfnAsync_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            int expected = 123;
            mock.Setup(c => c.GetMaxMfn()).Returns(expected);
            IIrbisConnection connection = mock.Object;

            int actual = connection.GetMaxMfnAsync().Result;
            Assert.AreEqual(expected, actual);

            mock.Verify
                (
                    c => c.GetMaxMfn(),
                    Times.Once
                );
        }

        [TestMethod]
        public void AsyncExtensions_ListDatabasesAsync_1()
        {
            Mock<IrbisConnection> mock = new Mock<IrbisConnection>();
            DatabaseInfo[] expected =
            {
                new DatabaseInfo
                {
                    Name = "IBIS",
                    Description = "Электронный каталог"
                },
            };
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns("IBIS\r\nЭлектронный каталог\r\n*****\r\n");
            IIrbisConnection connection = mock.Object;

            DatabaseInfo[] actual = connection.ListDatabasesAsync("dbnam1.mnu").Result;
            Assert.AreEqual(expected.Length, actual.Length);

            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void AsyncExtensions_NoOpAsync_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.NoOp());
            IIrbisConnection connection = mock.Object;

            connection.NoOpAsync().Wait();

            mock.Verify
                (
                    c => c.NoOp(),
                    Times.Once
                );
        }

        [TestMethod]
        public void AsyncExtensions_ReadIniFileAsync_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string text = "[Main]\r\nKey=Value";
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(text);
            IIrbisConnection connection = mock.Object;

            IniFile iniFile = connection.ReadIniFileAsync("file.ini").Result;
            Assert.IsNotNull(iniFile);
            IniFile.Section section = iniFile["Main"];
            Assert.IsNotNull(section);
            IniFile.Line line = section.First();
            Assert.AreEqual("Key", line.Key);
            Assert.AreEqual("Value", line.Value);

            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void AsyncExtensions_ReadMenuAsync_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string text = "Key\r\nValue\r\n*****";
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(text);
            IIrbisConnection connection = mock.Object;

            MenuFile menu = connection.ReadMenuAsync("menu.mnu").Result;
            Assert.IsNotNull(menu);
            Assert.AreEqual(1, menu.Entries.Count);
            Assert.AreEqual("Key", menu.Entries[0].Code);
            Assert.AreEqual("Value", menu.Entries[0].Comment);

            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void AsyncExtensions_ReadTermsAsync_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            TermInfo[] expected = new TermInfo[0];
            mock.Setup(c => c.ReadTerms(It.IsAny<TermParameters>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            TermInfo[] actual = connection.ReadTermsAsync("start", 10).Result;
            Assert.AreSame(expected, actual);

            mock.Verify
                (
                    c => c.ReadTerms(It.IsAny<TermParameters>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void AsyncExtensions_ReadSearchScenarioAsync_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string text = "[SEARCH]\r\nItemNumb=2\r\nItemName0=Автор\r\nItemPref0=A=\r\nItemDictionType0=0\r\nItemLogic0=\r\nItemMenu0=\r\nItemF8For0=!F8avt\r\nItemModByDic0=\r\nItemTranc0=\r\nItemHint0=\r\nItemModByDicAuto0=\r\nItemAdv0=ATHRA,A=,@sadv\r\nItemPft0=\r\nItemExactly0=\r\nItemName1=Заглавие/Название\r\nItemPref1=T=\r\nItemDictionType1=0\r\nItemLogic1=\r\nItemMenu1=\r\nItemF8For1=!F8TIT\r\nItemModByDic1=!DMODT\r\nItemTranc1=\r\nItemHint1=\r\nItemModByDicAuto1=\r\nItemAdv1=\r\nItemPft1=\r\nItemExactly1=\r\n";
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(text);
            IIrbisConnection connection = mock.Object;

            SearchScenario[] scenarios = connection.ReadSearchScenarioAsync("istu.ini").Result;
            Assert.AreEqual(2, scenarios.Length);
            Assert.AreEqual("Автор", scenarios[0].Name);
            Assert.AreEqual("A=", scenarios[0].Prefix);
            Assert.AreEqual("Заглавие/Название", scenarios[1].Name);
            Assert.AreEqual("T=", scenarios[1].Prefix);

            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void AsyncExtensions_ReadRecordAsync_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            MarcRecord expected = new MarcRecord();
            mock.Setup(c => c.ReadRecord(It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            MarcRecord actual = connection.ReadRecordAsync(1).Result;
            Assert.AreSame(expected, actual);

            mock.Verify
                (
                    c => c.ReadRecord(It.IsAny<string>(), It.IsAny<int>(),
                    It.IsAny<bool>(), It.IsAny<string>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void AsyncExtensions_ReadTextFileAsync_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string expected = "TEXT FILE";
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            string actual = connection.ReadTextFileAsync(new FileSpecification()).Result;
            Assert.AreEqual(expected, actual);

            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void AsyncExtensions_SearchAsync_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            int[] expected = { 1, 2, 3 };
            mock.Setup(c => c.Search(It.IsAny<string>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            int[] actual = connection.SearchAsync("search").Result;
            Assert.AreSame(expected, actual);

            mock.Verify
                (
                    c => c.Search(It.IsAny<string>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void AsyncExtensions_WriteRecordAsync_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            MarcRecord expected = new MarcRecord();
            mock.Setup(c => c.WriteRecord(It.IsAny<MarcRecord>(),
                It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            MarcRecord input = new MarcRecord();
            MarcRecord actual = connection.WriteRecordAsync(input).Result;
            Assert.AreSame(expected, actual);

            mock.Verify
                (
                    c => c.WriteRecord(It.IsAny<MarcRecord>(),
                        It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                    Times.Once
                );
        }
    }
}
