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
    public class NopCommandTest
        : CommandTest
    {
        [TestMethod]
        public void NopCommand_Construciton_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            NopCommand command = new NopCommand(connection);
            Assert.AreSame(connection, command.Connection);
        }

        [TestMethod]
        public void NopCommand_ExecuteRequest_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            NopCommand command = new NopCommand(connection);
            ResponseBuilder builder = new ResponseBuilder()
                .StandardHeader(CommandCode.Nop, 123, 456)
                .NewLine()
                .Append(0)
                .NewLine();
            TestingSocket socket = (TestingSocket) connection.Socket;
            socket.Response = builder.Encode();
            ClientQuery query = command.CreateQuery();
            ServerResponse response = command.Execute(query);
            Assert.AreEqual(0, response.ReturnCode);
        }

        [TestMethod]
        public void NopCommand_Verify_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            NopCommand command = new NopCommand(connection);
            Assert.IsTrue(command.Verify(false));
        }
    }
}
