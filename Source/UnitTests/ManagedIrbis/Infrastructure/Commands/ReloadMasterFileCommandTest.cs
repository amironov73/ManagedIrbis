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
    public class ReloadMasterFileCommandTest
        : CommandTest
    {
        [TestMethod]
        public void ReloadMasterFileCommand_Construciton_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ReloadMasterFileCommand command
                = new ReloadMasterFileCommand(connection);
            Assert.AreSame(connection, command.Connection);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void ReloadMasterFileCommand_CreateQuery_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ReloadMasterFileCommand command = new ReloadMasterFileCommand(connection);
            command.CreateQuery();
        }

        [TestMethod]
        public void ReloadMasterFileCommand_CreateQuery_2()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ReloadMasterFileCommand command = new ReloadMasterFileCommand(connection)
            {
                Database = "IBIS"
            };
            ClientQuery query = command.CreateQuery();
            Assert.IsNotNull(query);
        }

        [TestMethod]
        public void ReloadMasterFileCommand_ExecuteRequest_1()
        {
            int returnCode = 0;
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ReloadMasterFileCommand command = new ReloadMasterFileCommand(connection)
            {
                Database = "IBIS"
            };
            ResponseBuilder builder = new ResponseBuilder()
                .StandardHeader(CommandCode.ReloadMasterFile, 123, 456)
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
        public void ReloadMasterFileCommand_Verify_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ReloadMasterFileCommand command
                = new ReloadMasterFileCommand(connection);
            Assert.IsFalse(command.Verify(false));
        }

        [TestMethod]
        public void ReloadMasterFileCommand_Verify_2()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ReloadMasterFileCommand command
                = new ReloadMasterFileCommand(connection)
                {
                    Database = "IBIS"
                };
            Assert.IsTrue(command.Verify(false));
        }
    }
}
