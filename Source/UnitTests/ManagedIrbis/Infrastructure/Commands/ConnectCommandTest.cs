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
    public class ConnectCommandTest
        : CommandTest
    {
        [TestMethod]
        public void ConnectCommand_Construciton_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ConnectCommand command = new ConnectCommand(connection);
            Assert.AreSame(connection, command.Connection);
            Assert.IsFalse(command.RequireConnection);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void ConnectCommand_CreateQuery_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ConnectCommand command = new ConnectCommand(connection);
            command.CreateQuery();
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void CreateDictionayCommand_CreateQuery_2()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ConnectCommand command = new ConnectCommand(connection)
            {
                Username = "user"
            };
            command.CreateQuery();
        }

        [TestMethod]
        public void ConnectCommand_CreateQuery_3()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ConnectCommand command = new ConnectCommand(connection)
            {
                Username = "user",
                Password = "pass"
            };
            ClientQuery query = command.CreateQuery();
            Assert.IsNotNull(query);
        }

        [TestMethod]
        public void ConnectCommand_ExecuteRequest_1()
        {
            int returnCode = 0;
            string configuration = "Some=Text";
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ConnectCommand command = new ConnectCommand(connection)
            {
                Username = "user",
                Password = "pass"
            };
            ResponseBuilder builder = new ResponseBuilder()
                .AppendAnsi(CommandCode.RegisterClient).NewLine()
                .AppendAnsi("12345678").NewLine()
                .AppendAnsi("1").NewLine()
                .AppendAnsi("123").NewLine()
                .AppendAnsi("64.2014").NewLine()
                .NewLine()
                .NewLine()
                .NewLine()
                .NewLine()
                .NewLine()
                .Append(returnCode).NewLine()
                .AppendAnsi("30").NewLine()
                .AppendAnsi(configuration);
            TestingSocket socket = (TestingSocket)connection.Socket;
            socket.Response = builder.Encode();
            ClientQuery query = command.CreateQuery();
            ServerResponse response = command.Execute(query);
            Assert.AreEqual(returnCode, response.ReturnCode);
            Assert.AreEqual(configuration, command.Configuration);
            Assert.AreEqual(30, command.ConfirmationInterval);
            Assert.AreEqual("64.2014", command.ServerVersion);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void ConnectCommand_ExecuteRequest_2()
        {
            int returnCode = 0;
            string configuration = "Some=Text";
            Mock<IIrbisConnection> mock = GetConnectionMock();
            mock.SetupGet(c => c.Connected).Returns(true);
            IIrbisConnection connection = mock.Object;
            ConnectCommand command = new ConnectCommand(connection)
            {
                Username = "user",
                Password = "pass"
            };
            ResponseBuilder builder = new ResponseBuilder()
                .AppendAnsi(CommandCode.RegisterClient).NewLine()
                .AppendAnsi("12345678").NewLine()
                .AppendAnsi("1").NewLine()
                .AppendAnsi("123").NewLine()
                .AppendAnsi("64.2014").NewLine()
                .NewLine()
                .NewLine()
                .NewLine()
                .NewLine()
                .NewLine()
                .Append(returnCode).NewLine()
                .AppendAnsi("30").NewLine()
                .AppendAnsi(configuration);
            TestingSocket socket = (TestingSocket)connection.Socket;
            socket.Response = builder.Encode();
            ClientQuery query = command.CreateQuery();
            command.Execute(query);
        }

        [TestMethod]
        public void ConnectCommand_Verify_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ConnectCommand command = new ConnectCommand(connection);
            Assert.IsFalse(command.Verify(false));
        }

        [TestMethod]
        public void ConnectCommand_Verify_2()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ConnectCommand command = new ConnectCommand(connection)
            {
                Username = "user",
                Password = "pass"
            };
            Assert.IsTrue(command.Verify(false));
        }
    }
}
