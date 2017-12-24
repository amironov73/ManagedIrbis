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

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.ManagedIrbis.Infrastructure.Commands
{
    [TestClass]
    public class CreateDatabaseCommandTest
        : CommandTest
    {
        [TestMethod]
        public void CreateDatabaseCommand_Construciton_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            CreateDatabaseCommand command = new CreateDatabaseCommand(connection);
            Assert.AreSame(connection, command.Connection);
        }

        [TestMethod]
        public void CreateDatabaseCommand_Properties_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            CreateDatabaseCommand command = new CreateDatabaseCommand(connection);
            string database = "NEWIBIS";
            command.Database = database;
            Assert.AreEqual(database, command.Database);
            string description = "New catalog";
            command.Description = description;
            Assert.AreEqual(description, command.Description);
            command.ReaderAccess = true;
            Assert.IsTrue(command.ReaderAccess);
            string template = "IBIS";
            command.Template = template;
            Assert.AreEqual(template, command.Template);
        }

        [TestMethod]
        public void CreateDatabaseCommand_CreateQuery_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            CreateDatabaseCommand command = new CreateDatabaseCommand(connection);
            ClientQuery query = command.CreateQuery();
            Assert.IsNotNull(query);
        }

        [TestMethod]
        public void CreateDatabaseCommand_ExecuteRequest_1()
        {
            int returnCode = 0;
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            CreateDatabaseCommand command = new CreateDatabaseCommand(connection)
            {
                Database = "NEWIBIS",
                Description = "New catalog"
            };
            ResponseBuilder builder = new ResponseBuilder()
                .StandardHeader(CommandCode.CreateDatabase, 123, 456)
                .NewLine()
                .Append(returnCode)
                .NewLine();
            TestingSocket socket = (TestingSocket) connection.Socket;
            socket.Response = builder.Encode();
            ClientQuery query = command.CreateQuery();
            ServerResponse response = command.Execute(query);
            Assert.AreEqual(returnCode, response.ReturnCode);
        }

        [TestMethod]
        public void CreateDatabaseCommand_Verify_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            CreateDatabaseCommand command = new CreateDatabaseCommand(connection);
            Assert.IsFalse(command.Verify(false));
        }

        [TestMethod]
        public void CreateDatabaseCommand_Verify_2()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            CreateDatabaseCommand command = new CreateDatabaseCommand(connection)
            {
                Database = "NEWIBIS",
                Description = "New catalog"
            };
            Assert.IsTrue(command.Verify(false));
        }
    }
}
