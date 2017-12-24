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
    public class MaxMfnCommandTest
        : CommandTest
    {
        [TestMethod]
        public void MaxMfnCommand_Construciton_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            MaxMfnCommand command = new MaxMfnCommand(connection);
            Assert.AreSame(connection, command.Connection);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void MaxMfnCommand_CreateQuery_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            MaxMfnCommand command = new MaxMfnCommand(connection);
            command.CreateQuery();
        }

        [TestMethod]
        public void MaxMfnCommand_ExecuteRequest_1()
        {
            int maxMfn = 123456;
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            MaxMfnCommand command = new MaxMfnCommand(connection)
            {
                Database = "IBIS"
            };
            ResponseBuilder builder = new ResponseBuilder()
                .StandardHeader(CommandCode.GetMaxMfn, 123, 456)
                .NewLine()
                .Append(maxMfn)
                .NewLine();
            TestingSocket socket = (TestingSocket) connection.Socket;
            socket.Response = builder.Encode();
            ClientQuery query = command.CreateQuery();
            ServerResponse response = command.Execute(query);
            Assert.AreEqual(maxMfn, response.ReturnCode);
        }

        [TestMethod]
        public void MaxMfnCommand_Verify_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            MaxMfnCommand command = new MaxMfnCommand(connection);
            Assert.IsFalse(command.Verify(false));
        }

        [TestMethod]
        public void MaxMfnCommand_Verify_2()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            MaxMfnCommand command = new MaxMfnCommand(connection)
            {
                Database = "IBIS"
            };
            Assert.IsTrue(command.Verify(false));
        }
    }
}
