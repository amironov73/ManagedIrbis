using System;
using System.IO;
using System.Text;

using AM.IO;
using AM.Runtime;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Infrastructure.Sockets;
using ManagedIrbis.Search;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.ManagedIrbis.Infrastructure.Commands
{
    [TestClass]
    public class SearchRawCommandTest
        : CommandTest
    {
        [TestMethod]
        public void SearchRawCommand_Construciton_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            SearchRawCommand command = new SearchRawCommand(connection);
            Assert.AreSame(connection, command.Connection);
        }

        [TestMethod]
        public void SearchRawCommand_ApplyParameters_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            SearchRawCommand command = new SearchRawCommand(connection);
            SearchParameters parameters = new SearchParameters
            {
                Database = "IBIS",
                FirstRecord = 1,
                NumberOfRecords = 10,
                MinMfn = 100,
                MaxMfn = 200,
                SearchExpression = "A=AUTHOR$",
                FormatSpecification = "@brief",
                SequentialSpecification = "p(v300)"
            };
            command.ApplyParameters(parameters);
            Assert.AreEqual(parameters.Database, command.Database);
            Assert.AreEqual(parameters.FirstRecord, command.FirstRecord);
            Assert.AreEqual(parameters.NumberOfRecords, command.NumberOfRecords);
            Assert.AreEqual(parameters.MinMfn, command.MinMfn);
            Assert.AreEqual(parameters.MaxMfn, command.MaxMfn);
            Assert.AreEqual(parameters.SearchExpression, command.SearchExpression);
            Assert.AreEqual(parameters.FormatSpecification, command.FormatSpecification);
            Assert.AreEqual(parameters.SequentialSpecification, command.SequentialSpecification);
        }

        [TestMethod]
        public void SearchRawCommand_Clone_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            SearchRawCommand first = new SearchRawCommand(connection)
            {
                Database = "IBIS",
                FirstRecord = 1,
                NumberOfRecords = 10,
                MinMfn = 100,
                MaxMfn = 200,
                SearchExpression = "A=AUTHOR$",
                FormatSpecification = "@brief",
                SequentialSpecification = "p(v300)"
            };
            SearchRawCommand second = first.Clone();
            Assert.AreEqual(first.Database, second.Database);
            Assert.AreEqual(first.FirstRecord, second.FirstRecord);
            Assert.AreEqual(first.NumberOfRecords, second.NumberOfRecords);
            Assert.AreEqual(first.MinMfn, second.MinMfn);
            Assert.AreEqual(first.MaxMfn, second.MaxMfn);
            Assert.AreEqual(first.SearchExpression, second.SearchExpression);
            Assert.AreEqual(first.FormatSpecification, second.FormatSpecification);
            Assert.AreEqual(first.SequentialSpecification, second.SequentialSpecification);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void SearchRawCommand_CreateQuery_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            SearchRawCommand command = new SearchRawCommand(connection);
            ClientQuery query = command.CreateQuery();
            Assert.IsNotNull(query);
        }

        [TestMethod]
        public void SearchRawCommand_CreateQuery_2()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            SearchRawCommand command = new SearchRawCommand(connection)
            {
                Database = "IBIS",
                SearchExpression = "A=AUTHOR$"
            };
            ClientQuery query = command.CreateQuery();
            Assert.IsNotNull(query);
        }

        [TestMethod]
        public void SearchRawCommand_ExecuteRequest_1()
        {
            int returnCode = 0;
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            SearchRawCommand command = new SearchRawCommand(connection)
            {
                Database = "IBIS",
                SearchExpression = "A=AUTHOR$"
            };
            ResponseBuilder builder = new ResponseBuilder()
                .StandardHeader(CommandCode.Search, 123, 456)
                .NewLine()
                .Append(returnCode)
                .NewLine();
            TestingSocket socket = (TestingSocket) connection.Socket;
            socket.Response = builder.Encode();
            ClientQuery query = command.CreateQuery();
            ServerResponse response = command.Execute(query);
            Assert.AreEqual(returnCode, response.ReturnCode);
            Assert.IsNotNull(command.Found);
            Assert.AreEqual(0, command.Found.Length);
        }

        [TestMethod]
        public void SearchRawCommand_ExecuteRequest_2()
        {
            int returnCode = 0;
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            SearchRawCommand command = new SearchRawCommand(connection)
            {
                Database = "IBIS",
                SearchExpression = "A=AUTHOR$",
                MinMfn = 0,
                MaxMfn = 0,
                SequentialSpecification = "p(v300)",
                FormatSpecification = "(v300/)"
            };
            ResponseBuilder builder = new ResponseBuilder()
                .StandardHeader(CommandCode.Search, 123, 456)
                .NewLine()
                .Append(returnCode)
                .NewLine();
            TestingSocket socket = (TestingSocket) connection.Socket;
            socket.Response = builder.Encode();
            ClientQuery query = command.CreateQuery();
            ServerResponse response = command.Execute(query);
            Assert.AreEqual(returnCode, response.ReturnCode);
            Assert.IsNotNull(command.Found);
            Assert.AreEqual(0, command.Found.Length);
        }

        [TestMethod]
        public void SearchRawCommand_GatherParameters_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            SearchRawCommand command = new SearchRawCommand(connection)
            {
                Database = "IBIS",
                FirstRecord = 1,
                NumberOfRecords = 10,
                MinMfn = 100,
                MaxMfn = 200,
                SearchExpression = "A=AUTHOR$",
                FormatSpecification = "@brief",
                SequentialSpecification = "p(v300)"
            };
            SearchParameters parameters = command.GatherParameters();
            Assert.AreEqual(command.Database, parameters.Database);
            Assert.AreEqual(command.FirstRecord, parameters.FirstRecord);
            Assert.AreEqual(command.NumberOfRecords, parameters.NumberOfRecords);
            Assert.AreEqual(command.MinMfn, parameters.MinMfn);
            Assert.AreEqual(command.MaxMfn, parameters.MaxMfn);
            Assert.AreEqual(command.SearchExpression, parameters.SearchExpression);
            Assert.AreEqual(command.FormatSpecification, parameters.FormatSpecification);
            Assert.AreEqual(command.SequentialSpecification, parameters.SequentialSpecification);
        }

        [TestMethod]
        public void SearchRawCommand_Verify_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            SearchRawCommand command = new SearchRawCommand(connection);
            Assert.IsTrue(command.Verify(false));
        }
    }
}
