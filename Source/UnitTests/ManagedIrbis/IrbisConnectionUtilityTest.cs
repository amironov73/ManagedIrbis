using System;
using System.Collections.Generic;
using System.IO;

using AM;
using AM.IO;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Menus;
using ManagedIrbis.Search;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

// ReSharper disable InvokeAsExtensionMethod

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisConnectionUtilityTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void IrbisConnectionUtility_DefaultConnectionString_1()
        {
            string expected = "qqq";
            string save = IrbisConnectionUtility.DefaultConnectionString;
            try
            {
                IrbisConnectionUtility.DefaultConnectionString = expected;
                Assert.AreSame(expected, IrbisConnectionUtility.DefaultConnectionString);
            }
            finally
            {
                IrbisConnectionUtility.DefaultConnectionString = save;
            }
        }

        [TestMethod]
        public void IrbisConnectionUtility_ActualizeDatabase_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ActualizeRecord(It.IsAny<string>(), It.IsAny<int>()));

            IIrbisConnection connection = mock.Object;
            IrbisConnectionUtility.ActualizeDatabase(connection, "IBIS");

            mock.Verify(c => c.ActualizeRecord(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_DeleteAnyFile_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<int>()));
            mock.Setup(c => c.GetServerVersion()).Returns(new IrbisVersion() { Version = "64.2017.1" });

            IIrbisConnection connection = mock.Object;
            IrbisConnectionUtility.DeleteAnyFile(connection, "any.file");

            mock.Verify(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            mock.Verify(c => c.GetServerVersion(), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_DeleteRecord_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadRecord(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(new MarcRecord());
            mock.Setup(c => c.WriteRecord(It.IsAny<MarcRecord>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()));

            IIrbisConnection connnection = mock.Object;
            IrbisConnectionUtility.DeleteRecord(connnection, 1);

            mock.Verify(c => c.ReadRecord(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()),
                Times.Once);
            mock.Verify(c => c.WriteRecord(It.IsAny<MarcRecord>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_DeleteRecord_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadRecord(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(new MarcRecord());
            mock.Setup(c => c.WriteRecord(It.IsAny<MarcRecord>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()));

            IIrbisConnection connnection = mock.Object;
            IrbisConnectionUtility.DeleteRecord(connnection, 1, true);

            mock.Verify(c => c.ReadRecord(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()),
                Times.Once);
            mock.Verify(c => c.WriteRecord(It.IsAny<MarcRecord>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                Times.Once);
        }

        //[TestMethod]
        //public void IrbisConnectionUtility_DeleteRecords_1()
        //{
        //}

        [TestMethod]
        public void IrbisConnectionUtility_ExecuteArbitraryCommand_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            CommandFactory factory = new CommandFactory(connection);
            mock.SetupGet(c => c.CommandFactory).Returns(factory);
            ServerResponse response = ServerResponse.GetEmptyResponse(connection);
            mock.Setup(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()))
                .Returns(response);

            IrbisConnectionUtility.ExecuteArbitraryCommand
                (
                    connection,
                    "command",
                    "argument1",
                    "argument2"
                );

            mock.VerifyGet(c => c.CommandFactory, Times.Once);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()), Times.Once);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IrbisConnectionUtility_ExtendedSearch_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            mock.SetupGet(c => c.CommandFactory).Returns(CommandFactory.GetDefaultFactory(connection));
            mock.Setup(c => c.ExecuteCommand(It.IsAny<SearchReadCommand>()))
                .Returns((SearchReadCommand command) =>
                {
                    command.Records = new MarcRecord[0];

                    return ServerResponse.GetEmptyResponse(connection);
                });

            SearchParameters parameters = new SearchParameters();
            IrbisConnectionUtility.ExtendedSearch(connection, parameters);
        }

        [TestMethod]
        public void IrbisConnectionUtility_FormatRecord_1()
        {
            string expected = "Результат расформатирования";
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            AbstractEngine engine = new StandardEngine(connection, null);
            mock.SetupGet(c => c.Executive).Returns(engine);
            CommandFactory factory = CommandFactory.GetDefaultFactory(connection);
            mock.SetupGet(c => c.CommandFactory).Returns(factory);
            ResponseBuilder builder = new ResponseBuilder()
                .StandardHeader(CommandCode.FormatRecord, 123456, 123)
                .Append(0).NewLine()
                .AppendUtf(expected).NewLine();
            byte[] request = EmptyArray<byte>.Value;
            byte[] answer = builder.Encode();
            ServerResponse response = new ServerResponse(connection, answer, request, false);
            mock.Setup(c => c.ExecuteCommand(It.IsAny<FormatCommand>()))
                .Returns((FormatCommand command) =>
                {
                    command.FormatResult = new[] {expected};
                    return response;
                });

            string actual = IrbisConnectionUtility.FormatRecord(connection, "IBIS", "Some format", 1);
            Assert.AreEqual(expected, actual);

            mock.VerifyGet(c => c.Executive);
            mock.VerifyGet(c => c.CommandFactory);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<FormatCommand>()));
        }

        [TestMethod]
        public void IrbisConnectionUtility_FormatUtf8_1()
        {
            string expected = "Результат расформатирования";
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            AbstractEngine engine = new StandardEngine(connection, null);
            mock.SetupGet(c => c.Executive).Returns(engine);
            CommandFactory factory = CommandFactory.GetDefaultFactory(connection);
            mock.SetupGet(c => c.CommandFactory).Returns(factory);
            ResponseBuilder builder = new ResponseBuilder()
                .StandardHeader(CommandCode.FormatRecord, 123456, 123)
                .Append(0).NewLine()
                .AppendUtf(expected).NewLine();
            byte[] request = EmptyArray<byte>.Value;
            byte[] answer = builder.Encode();
            ServerResponse response = new ServerResponse(connection, answer, request, false);
            mock.Setup(c => c.ExecuteCommand(It.IsAny<FormatCommand>()))
                .Returns((FormatCommand command) =>
                {
                    command.FormatResult = new[] {expected};
                    return response;
                });

            string actual = IrbisConnectionUtility.FormatUtf8(connection, "Some format", 1);
            Assert.AreEqual(expected, actual);

            mock.VerifyGet(c => c.Executive);
            mock.VerifyGet(c => c.CommandFactory);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<FormatCommand>()));
        }

        [TestMethod]
        public void IrbisConnectionUtility_FormatUtf8_2()
        {
            string expected = "Результат расформатирования";
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            AbstractEngine engine = new StandardEngine(connection, null);
            mock.SetupGet(c => c.Executive).Returns(engine);
            CommandFactory factory = CommandFactory.GetDefaultFactory(connection);
            mock.SetupGet(c => c.CommandFactory).Returns(factory);
            ResponseBuilder builder = new ResponseBuilder()
                .StandardHeader(CommandCode.FormatRecord, 123456, 123)
                .Append(0).NewLine()
                .AppendUtf(expected).NewLine();
            byte[] request = EmptyArray<byte>.Value;
            byte[] answer = builder.Encode();
            ServerResponse response = new ServerResponse(connection, answer, request, false);
            mock.Setup(c => c.ExecuteCommand(It.IsAny<FormatCommand>()))
                .Returns((FormatCommand command) =>
                {
                    command.FormatResult = new[] {expected};
                    return response;
                });

            MarcRecord record = new MarcRecord();
            string actual = IrbisConnectionUtility.FormatUtf8(connection, "Some format", record);
            Assert.AreEqual(expected, actual);

            mock.VerifyGet(c => c.Executive);
            mock.VerifyGet(c => c.CommandFactory);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<FormatCommand>()));
        }

        [TestMethod]
        public void IrbisConnectionUtility_FormatUtf8_3()
        {
            string[] expected = {"Результат 1", "Результат 2", "Результат 3"};
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            AbstractEngine engine = new StandardEngine(connection, null);
            mock.SetupGet(c => c.Executive).Returns(engine);
            CommandFactory factory = CommandFactory.GetDefaultFactory(connection);
            mock.SetupGet(c => c.CommandFactory).Returns(factory);
            ResponseBuilder builder = new ResponseBuilder()
                .StandardHeader(CommandCode.FormatRecord, 123456, 123)
                .Append(0).NewLine();
            foreach (string s in expected)
            {
                builder.AppendUtf(s).NewLine();
            }
            byte[] request = EmptyArray<byte>.Value;
            byte[] answer = builder.Encode();
            ServerResponse response = new ServerResponse(connection, answer, request, false);
            mock.Setup(c => c.ExecuteCommand(It.IsAny<FormatCommand>()))
                .Returns((FormatCommand command) =>
                {
                    command.FormatResult = expected;
                    return response;
                });

            int[] mfn = {1, 2, 3};
            string[] actual = IrbisConnectionUtility.FormatUtf8(connection, "BIS", "Some format", mfn);
            CollectionAssert.AreEqual(expected, actual);

            mock.VerifyGet(c => c.Executive);
            mock.VerifyGet(c => c.CommandFactory);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<FormatCommand>()));
        }

        [TestMethod]
        public void IrbisConnectionUtility_ListDatabases_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string text = "IBIS\nIBIS\nISTU\nISTU\n*****";
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(text);
            IIrbisConnection connection = mock.Object;

            DatabaseInfo[] databases = IrbisConnectionUtility.ListDatabases(connection, "list.mnu");
            Assert.AreEqual(2, databases.Length);

            mock.Verify(c => c.ReadTextFile(It.IsAny<FileSpecification>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_ListDatabases_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string text = "IBIS\nIBIS\nISTU\nISTU\n*****";
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(text);
            IIrbisConnection connection = mock.Object;

            DatabaseInfo[] databases = IrbisConnectionUtility.ListDatabases(connection);
            Assert.AreEqual(2, databases.Length);

            mock.Verify(c => c.ReadTextFile(It.IsAny<FileSpecification>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_ListStandardConnectionStrings_1()
        {
            Assert.IsNotNull(IrbisConnectionUtility.ListStandardConnectionStrings());
        }

        [TestMethod]
        public void IrbisConnectionUtility_LockRecord_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            CommandFactory factory = CommandFactory.GetDefaultFactory(connection);
            mock.SetupGet(c => c.CommandFactory).Returns(factory);
            mock.Setup(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()));

            IrbisConnectionUtility.LockRecord(connection, "IBIS", 1);

            mock.VerifyGet(c => c.CommandFactory, Times.Once);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_LockRecords_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            CommandFactory factory = CommandFactory.GetDefaultFactory(connection);
            mock.SetupGet(c => c.CommandFactory).Returns(factory);
            mock.Setup(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()));

            int[] records = { 1, 2 };
            IrbisConnectionUtility.LockRecords(connection, "IBIS", records);

            Times twice = Times.Exactly(2);
            mock.VerifyGet(c => c.CommandFactory, twice);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()), twice);
        }

        [TestMethod]
        public void IrbisConnectionUtility_ReadAnyBinaryFile_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.SetupGet(c => c.ServerVersion).Returns(new IrbisVersion { Version = "64.2017.1" });
            mock.Setup(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<int>()))
                .Returns("^aqqq.bin^b%01%02%03");
            IIrbisConnection connection = mock.Object;

            byte[] expected = { 0x01, 0x02, 0x03 };
            byte[] actual = IrbisConnectionUtility.ReadAnyBinaryFile(connection, "qqq.bin");
            CollectionAssert.AreEqual(expected, actual);

            mock.VerifyGet(c => c.ServerVersion, Times.Once);
            mock.Verify(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_ReadAnyBinaryFile_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.SetupGet(c => c.ServerVersion).Returns(new IrbisVersion { Version = "64.2017.1" });
            mock.Setup(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(string.Empty);
            IIrbisConnection connection = mock.Object;

            byte[] actual = IrbisConnectionUtility.ReadAnyBinaryFile(connection, "qqq.bin");
            Assert.IsNull(actual);

            mock.VerifyGet(c => c.ServerVersion, Times.Once);
            mock.Verify(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_ReadAnyTextFile_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.SetupGet(c => c.ServerVersion).Returns(new IrbisVersion { Version = "64.2017.1" });
            mock.Setup(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<int>()))
                .Returns("^aqqq.txt^b" + IrbisUtility.EncodePercentString(IrbisEncoding.Ansi.GetBytes("Hello, world")));
            IIrbisConnection connection = mock.Object;

            string expected = "Hello, world";
            string actual = IrbisConnectionUtility.ReadAnyTextFile(connection, "qqq.txt");
            Assert.AreEqual(expected, actual);

            mock.VerifyGet(c => c.ServerVersion, Times.Once);
            mock.Verify(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_ReadAnyTextFile_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.SetupGet(c => c.ServerVersion).Returns(new IrbisVersion { Version = "64.2017.1" });
            mock.Setup(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<int>()))
                .Returns("^aqqq.txt^b" + IrbisUtility.EncodePercentString(IrbisEncoding.Ansi.GetBytes("Hello, world")));
            IIrbisConnection connection = mock.Object;

            string expected = "Hello, world";
            string actual = IrbisConnectionUtility.ReadAnyTextFile(connection, "qqq.txt", IrbisEncoding.Ansi);
            Assert.AreEqual(expected, actual);

            mock.VerifyGet(c => c.ServerVersion, Times.Once);
            mock.Verify(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_ReadIniFile_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.SetupGet(c => c.Database).Returns("IBIS");
            string text = "[Main]\nFirst=Second\n";
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(text);
            IIrbisConnection connection = mock.Object;

            IniFile iniFile = IrbisConnectionUtility.ReadIniFile(connection, "any.ini");
            Assert.AreEqual("Second", iniFile["Main"]?["First"]);

            mock.VerifyGet(c => c.Database, Times.Once);
            mock.Verify(c => c.ReadTextFile(It.IsAny<FileSpecification>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void IrbisConnectionUtility_ReadIniFile_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.SetupGet(c => c.Database).Returns("IBIS");
            string text = "[Main\nFirst=Second\n";
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(text);
            IIrbisConnection connection = mock.Object;

            IrbisConnectionUtility.ReadIniFile(connection, "any.ini");
        }

        [TestMethod]
        public void IrbisConnectionUtility_ReadMenu_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string text = "First\nSecond\n*****";
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(text);
            IIrbisConnection connection = mock.Object;

            MenuFile menu = IrbisConnectionUtility.ReadMenu(connection, "any.mnu");
            Assert.AreEqual("Second", menu.GetString("First"));

            mock.Verify(c => c.ReadTextFile(It.IsAny<FileSpecification>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_ReadMenu_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string text = "First\nSecond\n*****";
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(text);
            IIrbisConnection connection = mock.Object;

            FileSpecification specification = new FileSpecification(IrbisPath.MasterFile, "IBIS", "any.mnu");
            MenuFile menu = IrbisConnectionUtility.ReadMenu(connection, specification);
            Assert.AreEqual("Second", menu.GetString("First"));

            mock.Verify(c => c.ReadTextFile(It.IsAny<FileSpecification>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_ReadRawRecord_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            mock.SetupGet(c => c.CommandFactory).Returns(CommandFactory.GetDefaultFactory(connection));
            RawRecord expected = new RawRecord
            {
                Database = "IBIS",
                Mfn = 1,
                Status = RecordStatus.Last
            };
            mock.Setup(c => c.ExecuteCommand(It.IsAny<ReadRawRecordCommand>()))
                .Returns((ReadRawRecordCommand command) => { command.RawRecord = expected; return ServerResponse.GetEmptyResponse(connection); });

            RawRecord actual = IrbisConnectionUtility.ReadRawRecord(connection, "IBIS", 1);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Database, actual.Database);
            Assert.AreEqual(expected.Mfn, actual.Mfn);
            Assert.AreEqual(expected.Status, actual.Status);

            mock.VerifyGet(c => c.CommandFactory, Times.Once);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<ReadRawRecordCommand>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_ReadRawRecord_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            mock.SetupGet(c => c.CommandFactory).Returns(CommandFactory.GetDefaultFactory(connection));
            RawRecord expected = new RawRecord
            {
                Database = "IBIS",
                Mfn = 1,
                Status = RecordStatus.Last
            };
            mock.Setup(c => c.ExecuteCommand(It.IsAny<ReadRawRecordCommand>()))
                .Returns((ReadRawRecordCommand command) => { command.RawRecord = expected; return ServerResponse.GetEmptyResponse(connection); });

            RawRecord actual = IrbisConnectionUtility.ReadRawRecord(connection, "IBIS", 1, false, "@brief");
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Database, actual.Database);
            Assert.AreEqual(expected.Mfn, actual.Mfn);
            Assert.AreEqual(expected.Status, actual.Status);

            mock.VerifyGet(c => c.CommandFactory, Times.Once);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<ReadRawRecordCommand>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_ReadRawRecord_3()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            mock.SetupGet(c => c.CommandFactory).Returns(CommandFactory.GetDefaultFactory(connection));
            RawRecord expected = new RawRecord
            {
                Database = "IBIS",
                Mfn = 1,
                Status = RecordStatus.Last
            };
            mock.Setup(c => c.ExecuteCommand(It.IsAny<ReadRawRecordCommand>()))
                .Returns((ReadRawRecordCommand command) => { command.RawRecord = expected; return ServerResponse.GetEmptyResponse(connection); });

            RawRecord actual = IrbisConnectionUtility.ReadRawRecord(connection, "IBIS", 1, 1, "@brief");
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Database, actual.Database);
            Assert.AreEqual(expected.Mfn, actual.Mfn);
            Assert.AreEqual(expected.Status, actual.Status);

            mock.VerifyGet(c => c.CommandFactory, Times.Once);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<ReadRawRecordCommand>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_ReadRawRecords_1()
        {

        }

        [TestMethod]
        public void IrbisConnectionUtility_ReadRecords_1()
        {

        }

        [TestMethod]
        public void IrbisConnectionUtility_ReadSearchScenario_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string fileName = Path.Combine(TestDataPath, @"Irbis64/Datai/IBIS/ibis.ini");
            string content = File.ReadAllText(fileName, IrbisEncoding.Ansi);
            mock.SetupGet(c => c.Database).Returns("IBIS");
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(content);
            IIrbisConnection connection = mock.Object;

            SearchScenario[] scenarios = IrbisConnectionUtility.ReadSearchScenario(connection, "ibis.ini");
            Assert.AreEqual(73, scenarios.Length);

            mock.VerifyGet(c => c.Database, Times.Once);
            mock.Verify(c => c.ReadTextFile(It.IsAny<FileSpecification>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_RemoveLogging_1()
        {

        }

        [TestMethod]
        public void IrbisConnectionUtility_RecordHistory_1()
        {

        }

        [TestMethod]
        public void IrbisConnectionUtility_ReadRecord_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            MarcRecord expected = new MarcRecord
            {
                Database = "IBIS",
                Mfn = 1,
                Status = RecordStatus.Last
            };
            mock.Setup(c => c.ReadRecord(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            MarcRecord actual = IrbisConnectionUtility.ReadRecord(connection, 1);
            Assert.AreEqual(expected.Database, actual.Database);
            Assert.AreEqual(expected.Mfn, actual.Mfn);
            Assert.AreEqual(expected.Status, actual.Status);

            mock.Verify(c => c.ReadRecord(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_ReadTextFile_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.SetupGet(c => c.Database).Returns("IBIS");
            string expected = "Some text";
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>())).Returns(expected);
            IIrbisConnection connection = mock.Object;

            string actual = IrbisConnectionUtility.ReadTextFile(connection, IrbisPath.System, "some.txt");
            Assert.AreEqual(expected, actual);

            mock.VerifyGet(c => c.Database, Times.Once);
            mock.Verify(c => c.ReadTextFile(It.IsAny<FileSpecification>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_RequireClientVersion_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;

            Assert.IsTrue(IrbisConnectionUtility.RequireClientVersion(connection, "1.0.0", false));
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void IrbisConnectionUtility_RequireClientVersion_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;

            IrbisConnectionUtility.RequireClientVersion(connection, "100500.0.0", true);
        }

        [TestMethod]
        public void IrbisConnectionUtility_RequireServerVersion_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.SetupGet(c => c.ServerVersion).Returns(new IrbisVersion { Version = "64.2017.1" });
            IIrbisConnection connection = mock.Object;

            Assert.IsTrue(IrbisConnectionUtility.RequireServerVersion(connection, "2010.1", false));

            mock.VerifyGet(c => c.ServerVersion);
        }

        [TestMethod]
        public void IrbisConnectionUtility_RequireServerVersion_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.SetupGet(c => c.ServerVersion).Returns((IrbisVersion)null);
            mock.Setup(c => c.GetServerVersion()).Returns(new IrbisVersion { Version = "64.2017.1" });
            IIrbisConnection connection = mock.Object;

            Assert.IsTrue(IrbisConnectionUtility.RequireServerVersion(connection, "2010.1", false));

            mock.VerifyGet(c => c.ServerVersion);
            mock.Verify(c => c.GetServerVersion());
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void IrbisConnectionUtility_RequireServerVersion_3()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.SetupGet(c => c.ServerVersion).Returns(new IrbisVersion { Version = "64.2007.1" });
            IIrbisConnection connection = mock.Object;

            IrbisConnectionUtility.RequireServerVersion(connection, "2010.1", true);
        }

        [TestMethod]
        public void IrbisConnectionUtility_Search_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            int[] expected = { 1, 2, 3 };
            mock.Setup(c => c.Search(It.IsAny<string>())).Returns(expected);
            IIrbisConnection connection = mock.Object;

            int[] actual = IrbisConnectionUtility.Search(connection, "K=ANY");
            CollectionAssert.AreEqual(expected, actual);

            mock.Verify(c => c.Search(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_SearchCount_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            mock.SetupGet(c => c.CommandFactory).Returns(CommandFactory.GetDefaultFactory(connection));
            mock.Setup(c => c.ExecuteCommand(It.IsAny<SearchCommand>()))
                .Returns((SearchCommand command) =>
                {
                    command.FoundCount = 123;
                    return ServerResponse.GetEmptyResponse(connection);
                });

            Assert.AreEqual(123, IrbisConnectionUtility.SearchCount(connection, "K=ANY"));

            mock.Verify(c => c.ExecuteCommand(It.IsAny<SearchCommand>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_SearchFormat_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            mock.SetupGet(c => c.CommandFactory).Returns(CommandFactory.GetDefaultFactory(connection));
            mock.Setup(c => c.ExecuteCommand(It.IsAny<SearchCommand>()))
                .Returns((SearchCommand command) =>
                {
                    command.Found = new List<FoundItem>
                    {
                        new FoundItem
                        {
                            Mfn = 1,
                            Record = new MarcRecord {Mfn = 1},
                            Text = "Some text"
                        }
                    };
                    command.FoundCount = 1;
                    return ServerResponse.GetEmptyResponse(connection);
                });

            FoundItem[] found = IrbisConnectionUtility.SearchFormat(connection, "K=ANY", "Some format");
            Assert.AreEqual(1, found.Length);
            Assert.AreEqual(1, found[0].Mfn);
            Assert.AreEqual("Some text", found[0].Text);

            mock.Verify(c => c.ExecuteCommand(It.IsAny<SearchCommand>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_SearchFormatUtf8_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            mock.SetupGet(c => c.CommandFactory).Returns(CommandFactory.GetDefaultFactory(connection));
            mock.Setup(c => c.ExecuteCommand(It.IsAny<SearchCommand>()))
                .Returns((SearchCommand command) =>
                {
                    command.Found = new List<FoundItem>
                    {
                        new FoundItem
                        {
                            Mfn = 1,
                            Record = new MarcRecord {Mfn = 1},
                            Text = "Some text"
                        }
                    };
                    command.FoundCount = 1;
                    return ServerResponse.GetEmptyResponse(connection);
                });

            FoundItem[] found = IrbisConnectionUtility.SearchFormatUtf8(connection, "K=ANY", "Some format");
            Assert.AreEqual(1, found.Length);
            Assert.AreEqual(1, found[0].Mfn);
            Assert.AreEqual("Some text", found[0].Text);

            mock.Verify(c => c.ExecuteCommand(It.IsAny<SearchCommand>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_SearchRaw_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            mock.SetupGet(c => c.CommandFactory).Returns(CommandFactory.GetDefaultFactory(connection));
            mock.Setup(c => c.ExecuteCommand(It.IsAny<SearchRawCommand>()))
                .Returns((SearchRawCommand command) =>
                {
                    command.Found = new[] {"first", "second"};
                    return ServerResponse.GetEmptyResponse(connection);
                });

            SearchParameters parameters = new SearchParameters
            {
                Database = "IBIS",
                SearchExpression = "K=ANY"
            };
            string[] found = IrbisConnectionUtility.SearchRaw(connection, parameters);
            Assert.AreEqual(2, found.Length);

            mock.Verify(c => c.ExecuteCommand(It.IsAny<SearchRawCommand>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_SearchRead_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            mock.SetupGet(c => c.CommandFactory).Returns(CommandFactory.GetDefaultFactory(connection));
            mock.Setup(c => c.ExecuteCommand(It.IsAny<SearchReadCommand>()))
                .Returns((SearchReadCommand command) =>
                {
                    command.Records = new[]
                    {
                        new MarcRecord {Mfn = 1},
                        new MarcRecord {Mfn = 2},
                        new MarcRecord {Mfn = 3}
                    };
                    return ServerResponse.GetEmptyResponse(connection);
                });

            MarcRecord[] found = IrbisConnectionUtility.SearchRead(connection, "K=ANY");
            Assert.AreEqual(3, found.Length);
            Assert.AreEqual(1, found[0].Mfn);
            Assert.AreEqual(2, found[1].Mfn);
            Assert.AreEqual(3, found[2].Mfn);

            mock.Verify(c => c.ExecuteCommand(It.IsAny<SearchReadCommand>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_SearchReadOneRecord_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            mock.SetupGet(c => c.CommandFactory).Returns(CommandFactory.GetDefaultFactory(connection));
            mock.Setup(c => c.ExecuteCommand(It.IsAny<SearchReadCommand>()))
                .Returns((SearchReadCommand command) =>
                {
                    command.Records = new[]
                    {
                        new MarcRecord {Mfn = 1},
                        new MarcRecord {Mfn = 2},
                        new MarcRecord {Mfn = 3}
                    };
                    return ServerResponse.GetEmptyResponse(connection);
                });

            MarcRecord found = IrbisConnectionUtility.SearchReadOneRecord(connection, "K=ANY");
            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Mfn);

            mock.Verify(c => c.ExecuteCommand(It.IsAny<SearchReadCommand>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_SequentialSearchRaw_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            mock.SetupGet(c => c.Executive).Returns(new StandardEngine(connection, null));
            mock.SetupGet(c => c.CommandFactory).Returns(CommandFactory.GetDefaultFactory(connection));
            mock.Setup(c => c.ExecuteCommand(It.IsAny<UniversalCommand>()))
                .Returns((UniversalCommand command) =>
                {
                    ResponseBuilder builder = new ResponseBuilder();
                    builder.StandardHeader(CommandCode.Search, 12345, 123)
                        .Append(3).NewLine()
                        .AppendUtf("Первая").NewLine()
                        .AppendUtf("Вторая").NewLine()
                        .AppendUtf("Третья").NewLine();
                    byte[] request = EmptyArray<byte>.Value;
                    byte[] answer = builder.Encode();
                    ServerResponse result = new ServerResponse(connection, answer, request, false);

                    return result;
                });

            string[] found = IrbisConnectionUtility.SequentialSearchRaw
                (
                    connection,
                    "IBIS",
                    "K=ANY",
                    0,
                    0,
                    0,
                    0,
                    "p(v200)",
                    null
                );
            Assert.AreEqual(3, found.Length);
            Assert.AreEqual("Первая", found[0]);
            Assert.AreEqual("Вторая", found[1]);
            Assert.AreEqual("Третья", found[2]);

            mock.VerifyGet(c => c.Executive, Times.AtLeastOnce);
            mock.VerifyGet(c => c.CommandFactory, Times.Once);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<UniversalCommand>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_UndeleteRecord_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadRecord(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(new MarcRecord { Deleted = true });
            mock.Setup(c => c.WriteRecord(It.IsAny<MarcRecord>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()));
            mock.SetupGet(c => c.Database).Returns("IBIS");

            IIrbisConnection connnection = mock.Object;
            IrbisConnectionUtility.UndeleteRecord(connnection, 1);

            mock.Verify(c => c.ReadRecord(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()),
                Times.Once);
            mock.Verify(c => c.WriteRecord(It.IsAny<MarcRecord>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                Times.Once);
            mock.VerifyGet(c => c.Database, Times.Once);
        }

        //[TestMethod]
        //public void IrbisConnectionUtility_UndeleteRecords_1()
        //{
        //}

        [TestMethod]
        public void IrbisConnectionUtility_UnlockRecordAlternative_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            CommandFactory factory = new CommandFactory(connection);
            mock.SetupGet(c => c.CommandFactory).Returns(factory);
            ServerResponse response = ServerResponse.GetEmptyResponse(connection);
            mock.Setup(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()))
                .Returns(response);

            IrbisConnectionUtility.UnlockRecordAlternative(connection, "IBIS", 1);

            mock.VerifyGet(c => c.CommandFactory, Times.Once);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_WriteRawRecord_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            CommandFactory factory = new CommandFactory(connection);
            mock.SetupGet(c => c.CommandFactory).Returns(factory);
            ServerResponse response = ServerResponse.GetEmptyResponse(connection);
            mock.Setup(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()))
                .Returns(response);

            IrbisConnectionUtility.WriteRawRecord
                (
                    connection,
                    "IBIS",
                    "record",
                    false,
                    true
                );

            mock.VerifyGet(c => c.CommandFactory, Times.Once);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_WriteRawRecords_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            CommandFactory factory = new CommandFactory(connection);
            mock.SetupGet(c => c.CommandFactory).Returns(factory);
            AbstractEngine engine = new StandardEngine(connection, null);
            mock.SetupGet(c => c.Executive).Returns(engine);
            ServerResponse response = ServerResponse.GetEmptyResponse(connection);
            mock.Setup(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()))
                .Returns(response);

            IrbisConnectionUtility.WriteRawRecords
                (
                    connection,
                    "IBIS",
                    new string[2],
                    false,
                    true
                );

            mock.VerifyGet(c => c.CommandFactory, Times.Once);
            mock.VerifyGet(c => c.Executive, Times.Once);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()), Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_WriteRecord_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.WriteRecord(It.IsAny<MarcRecord>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()));

            IIrbisConnection connnection = mock.Object;
            MarcRecord record = new MarcRecord();
            IrbisConnectionUtility.WriteRecord(connnection, record);

            mock.Verify(c => c.WriteRecord(It.IsAny<MarcRecord>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                Times.Once);

        }

        [TestMethod]
        public void IrbisConnectionUtility_WriteRecord_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.WriteRecord(It.IsAny<MarcRecord>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()));

            IIrbisConnection connnection = mock.Object;
            MarcRecord record = new MarcRecord();
            IrbisConnectionUtility.WriteRecord(connnection, record, false, true);

            mock.Verify(c => c.WriteRecord(It.IsAny<MarcRecord>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                Times.Once);
        }

        [TestMethod]
        public void IrbisConnectionUtility_WriteRecord_3()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.WriteRecord(It.IsAny<MarcRecord>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()));

            IIrbisConnection connnection = mock.Object;
            MarcRecord record = new MarcRecord();
            IrbisConnectionUtility.WriteRecord(connnection, record, true);

            mock.Verify(c => c.WriteRecord(It.IsAny<MarcRecord>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),
                Times.Once);
        }

    }
}
