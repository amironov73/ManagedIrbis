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
    public class ReadFileCommandTest
        : CommandTest
    {
        [TestMethod]
        public void ReadFileCommand_Construciton_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ReadFileCommand command
                = new ReadFileCommand(connection);
            Assert.AreSame(connection, command.Connection);
        }

        [TestMethod]
        public void ReadFileCommand_CreateQuery_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ReadFileCommand command = new ReadFileCommand(connection)
            {
                Files =
                {
                    new FileSpecification(IrbisPath.MasterFile, "IBIS", "file.txt")
                }
            };
            ClientQuery query = command.CreateQuery();
            Assert.IsNotNull(query);
        }

        [TestMethod]
        public void ReadFileCommand_ExecuteRequest_1()
        {
            int returnCode = 0;
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ReadFileCommand command = new ReadFileCommand(connection)
            {
                Files =
                {
                    new FileSpecification(IrbisPath.MasterFile, "IBIS", "file.txt")
                }
            };
            ResponseBuilder builder = new ResponseBuilder()
                .StandardHeader(CommandCode.ReadDocument, 123, 456)
                .NewLine()
                .Append(returnCode)
                .NewLine();
            TestingSocket socket = (TestingSocket) connection.Socket;
            socket.Response = builder.Encode();
            ClientQuery query = command.CreateQuery();
            ServerResponse response = command.Execute(query);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void ReadFileCommand_Verify_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ReadFileCommand command
                = new ReadFileCommand(connection);
            Assert.IsFalse(command.Verify(false));
        }
    }
}
