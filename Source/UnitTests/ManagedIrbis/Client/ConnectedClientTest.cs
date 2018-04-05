using System;
using System.Collections.Generic;
using System.IO;

using AM;
using AM.IO;
using AM.Threading;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Pft;
using ManagedIrbis.Search;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

// ReSharper disable PossibleNullReferenceException

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class ConnectedClientTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void ConnectedClient_Construction_1()
        {
            using (ConnectedClient client = new ConnectedClient())
            {
                Assert.IsNotNull(client.Connection);
                Assert.IsFalse(client.Connected);
                Assert.IsFalse(client.BusyState);
                Assert.AreEqual("IBIS", client.Database);
            }
        }

        [TestMethod]
        public void ConnectedClient_Construction_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.SetupGet(c => c.Busy).Returns(new BusyState());
            mock.SetupGet(c => c.Database).Returns("IBIS");
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                Assert.AreSame(connection, client.Connection);
                Assert.IsNotNull(client.Connection);
                Assert.IsFalse(client.Connected);
                Assert.IsFalse(client.BusyState);
                Assert.AreEqual("IBIS", client.Database);
            }
        }

        [TestMethod]
        public void ConnectedClient_Database_1()
        {
            using (ConnectedClient client = new ConnectedClient())
            {
                Assert.IsNotNull(client.Connection);
                Assert.AreEqual("IBIS", client.Database);
                client.Database = "ISTU";
                Assert.AreEqual("ISTU", client.Connection.Database);
            }
        }

        [TestMethod]
        public void ConnectedClient_Connect_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.Connect());
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                client.Connect();
            }

            mock.Verify(c => c.Connect());
        }

        [TestMethod]
        public void ConnectedClient_Disconnect_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.Dispose());
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                client.Disconnect();
            }

            mock.Verify(c => c.Dispose());
        }

        [TestMethod]
        public void ConnectedClient_ParseConnectionString_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ParseConnectionString(It.IsAny<string>()));
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                client.ParseConnectionString("connection string");
            }

            mock.Verify(c => c.ParseConnectionString(It.IsAny<string>()));
        }

        [TestMethod]
        public void ConnectedClient_AcquireFormatter_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                IPftFormatter formatter = client.AcquireFormatter();
                Assert.IsNotNull(formatter);
                client.ReleaseFormatter(formatter);
            }
        }

        [TestMethod]
        public void ConnectedClient_Configure_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ParseConnectionString(It.IsAny<string>()));
            mock.Setup(c => c.Connect());
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                client.Configure("connection string");
            }

            mock.Verify(c => c.ParseConnectionString(It.IsAny<string>()));
            mock.Verify(c => c.Connect());
        }

        [TestMethod]
        public void ConnectedClient_ExactSearchLinks_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.SetupGet(c => c.Database).Returns("IBIS");
            TermPosting[] postings =
            {
                new TermPosting { Count = 1, Mfn = 2, Occurrence = 3, Text = "Text1" },
                new TermPosting { Count = 4, Mfn = 5, Occurrence = 6, Text = "Text2" }
            };
            mock.Setup(c => c.ReadPostings(It.IsAny<PostingParameters>()))
                .Returns(postings);
            mock.Setup(c => c.Connect());
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                TermLink[] links = client.ExactSearchLinks("term");
                Assert.AreEqual(2, links.Length);
            }

            mock.VerifyGet(c => c.Database);
            mock.Verify(c => c.ReadPostings(It.IsAny<PostingParameters>()));
        }

        [TestMethod]
        public void ConnectedClient_FormatRecord_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string expected = "formatted";
            mock.Setup(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<MarcRecord>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                MarcRecord record = new MarcRecord();
                string actual = client.FormatRecord(record, "format");
                Assert.AreEqual(expected, actual);
            }

            mock.Verify(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<MarcRecord>()));
        }

        [TestMethod]
        public void ConnectedClient_FormatRecords_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.SetupGet(c => c.Database).Returns("IBIS");
            mock.Setup(c => c.FormatRecords(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<IEnumerable<int>>()))
                .Returns((string database, string format, IEnumerable<int> mfns) =>
                {
                    List<string> result = new List<string>();
                    foreach (int mfn in mfns)
                    {
                        result.Add("MFN" + mfn.ToInvariantString());
                    }

                    return result.ToArray();
                });
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                int[] mfns = { 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144,
                    233, 377, 610, 987, 1597, 2584, 4181, 6765, 10946, 17711 };
                string[] actual = client.FormatRecords(mfns, "format");
                Assert.AreEqual(mfns.Length, actual.Length);
            }

            mock.VerifyGet(c => c.Database);
            mock.Verify(c => c.FormatRecords(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<IEnumerable<int>>()));
        }

        [TestMethod]
        public void ConnectedClient_GetAlphabetTable_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string fileName = Path.Combine(TestDataPath, "ISISACW.TAB");
            string text = File.ReadAllText(fileName);
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(text);
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                IrbisAlphabetTable table = client.GetAlphabetTable();
                Assert.IsNotNull(table);
                Assert.IsNotNull(table.Characters);
                Assert.AreEqual(182, table.Characters.Length);
            }

            mock.Verify(c => c.ReadTextFile(It.IsAny<FileSpecification>()));
        }

        [TestMethod]
        public void ConnectedClient_GetCatalogState_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            int maxMfn = 3;
            mock.Setup(c => c.GetMaxMfn(It.IsAny<string>())).Returns(maxMfn);
            string[] formatted =
            {
                "0\n1#0\n0#1\n951#^",
                "0\n2#0\n0#1\n694#^",
                "0\n3#0\n0#1\n225#^"
            };
            mock.Setup(c => c.FormatRecords(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<IEnumerable<int>>()))
                .Returns(formatted);
            DatabaseInfo info = new DatabaseInfo
            {
                DatabaseLocked = false,
                Description = "description",
                LockedRecords = new int[0],
                LogicallyDeletedRecords = new[] { 1 },
                MaxMfn = maxMfn,
                NonActualizedRecords = new int[0],
                PhysicallyDeletedRecords = new int[0],
                Name = "IBIS",
                ReadOnly = false
            };
            mock.Setup(c => c.GetDatabaseInfo(It.IsAny<string>()))
                .Returns(info);
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                CatalogState state = client.GetCatalogState("IBIS");
                Assert.IsNotNull(state);
                Assert.AreEqual(maxMfn, state.MaxMfn);
            }
        }

        [TestMethod]
        public void ConnectedClient_GetMaxMfn_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            int expected = 123;
            mock.Setup(c => c.GetMaxMfn()).Returns(expected);
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                int actual = client.GetMaxMfn();
                Assert.AreEqual(expected, actual);
            }

            mock.Verify(c => c.GetMaxMfn());
        }

        [TestMethod]
        public void ConnectedClient_GetStopWords_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.SetupGet(c => c.Database).Returns("IBIS");
            string fileName = Path.Combine(Irbis64RootPath, "Datai/IBIS/ibis.stw");
            string content = File.ReadAllText(fileName, IrbisEncoding.Ansi);
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(content);
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                IrbisStopWords words = client.GetStopWords();
                Assert.IsTrue(words.IsStopWord("about"));
                Assert.IsFalse(words.IsStopWord("Irbis"));
            }

            mock.VerifyGet(c => c.Database);
        }

        [TestMethod]
        public void ConnectedClient_NoOp_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.NoOp());
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                client.NoOp();
            }

            mock.Verify(c => c.NoOp());
        }

        [TestMethod]
        public void ConnectedClient_ReadFile_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string expected = "content";
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                FileSpecification specification = new FileSpecification
                    (
                        path: IrbisPath.MasterFile,
                        database: "IBIS",
                        fileName: "someFile.txt"
                    );
                string actual = client.ReadFile(specification);
                Assert.AreEqual(expected, actual);
            }

            mock.Verify(c => c.ReadTextFile(It.IsAny<FileSpecification>()));
        }

        [TestMethod]
        public void ConnectedClient_ReadIniFile_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string fileName = Path.Combine(TestDataPath, "IniFile1.ini");
            string content = File.ReadAllText(fileName);
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(content);
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                FileSpecification specification = new FileSpecification
                    (
                        path: IrbisPath.MasterFile,
                        database: "IBIS",
                        fileName: "someFile.ini"
                    );
                IniFile iniFile = client.ReadIniFile(specification);
                Assert.IsNotNull(iniFile);
                Assert.AreEqual("1", iniFile["Main"]["FirstParameter"]);
            }

            mock.Verify(c => c.ReadTextFile(It.IsAny<FileSpecification>()));
        }

        [TestMethod]
        public void ConnectedClient_ReadIniFile_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string content = string.Empty;
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(content);
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                FileSpecification specification = new FileSpecification
                    (
                        path: IrbisPath.MasterFile,
                        database: "IBIS",
                        fileName: "someFile.ini"
                    );
                IniFile iniFile = client.ReadIniFile(specification);
                Assert.IsNull(iniFile);
            }

            mock.Verify(c => c.ReadTextFile(It.IsAny<FileSpecification>()));
        }

        [TestMethod]
        public void ConnectedClient_ReadMenu_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string fileName = Path.Combine(TestDataPath, "ORG.MNU");
            string content = File.ReadAllText(fileName);
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(content);
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                FileSpecification specification = new FileSpecification
                    (
                        path: IrbisPath.MasterFile,
                        database: "IBIS",
                        fileName: "ORG.MNU"
                    );
                MenuFile menu = client.ReadMenuFile(specification);
                string expected = "RU";
                string actual = menu.GetString("1");
                Assert.AreEqual(expected, actual);
            }

            mock.Verify(c => c.ReadTextFile(It.IsAny<FileSpecification>()));
        }

        [TestMethod]
        public void ConnectedClient_ReadRecord_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            MarcRecord expected = new MarcRecord
            {
                Mfn = 123
            };
            mock.Setup(c => c.ReadRecord(It.IsAny<string>(), It.IsAny<int>(),
                    It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                MarcRecord actual = client.ReadRecord(123);
                Assert.AreSame(expected, actual);
            }

            mock.Verify(c => c.ReadRecord(It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<bool>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void ConnectedClient_ReadTerms_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            TermInfo[] expected =
            {
                new TermInfo { Text = "К=БЕТОН", Count=123 },
                new TermInfo { Text = "К=БЕТОНИРОВАНИЕ", Count=234 },
                new TermInfo { Text = "К=БЕТОНКА", Count=345 },
            };
            mock.Setup(c => c.ReadTerms(It.IsAny<TermParameters>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                TermParameters parameters = new TermParameters
                {
                    Database = "IBIS",
                    StartTerm = "K=БЕТОН",
                    NumberOfTerms = 3
                };
                TermInfo[] actual = client.ReadTerms(parameters);
                Assert.AreEqual(expected.Length, actual.Length);
            }

            mock.Verify(c => c.ReadTerms(It.IsAny<TermParameters>()));
        }

        [TestMethod]
        public void ConnectedClient_Reconnect_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.Reconnect());
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                client.Reconnect();
            }

            mock.Verify(c => c.Reconnect());
        }

        [TestMethod]
        public void ConnectedClient_Search_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            int[] expected = { 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144,
                233, 377, 610, 987, 1597, 2584, 4181, 6765, 10946, 17711 };
            mock.Setup(c => c.Search(It.IsAny<string>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                int[] actual = client.Search("expression");
                CollectionAssert.AreEqual(expected, actual);
            }

            mock.Setup(c => c.Search(It.IsAny<string>()));
        }

        [TestMethod]
        public void ConnectedClient_Search_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            int[] expected = new int[0];
            mock.Setup(c => c.Search(It.IsAny<string>())).Returns(expected);
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                int[] actual = client.Search(string.Empty);
                CollectionAssert.AreEqual(expected, actual);
            }

            mock.Setup(c => c.Search(It.IsAny<string>()));
        }

        [TestMethod]
        public void ConnectedClient_WriteRecord_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.WriteRecord(It.IsAny<MarcRecord>(), It.IsAny<bool>(),
                    It.IsAny<bool>(), It.IsAny<bool>()));
            IIrbisConnection connection = mock.Object;

            using (ConnectedClient client = new ConnectedClient(connection))
            {
                MarcRecord record = new MarcRecord();
                client.WriteRecord(record);
            }

            mock.Setup(c => c.WriteRecord(It.IsAny<MarcRecord>(), It.IsAny<bool>(),
                It.IsAny<bool>(), It.IsAny<bool>()));
        }
    }
}

