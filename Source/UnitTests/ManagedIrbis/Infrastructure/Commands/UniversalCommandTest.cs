using System;
using System.IO;
using System.Text;

using AM.IO;
using AM.Runtime;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.ManagedIrbis.Infrastructure.Commands
{
    [TestClass]
    public class UniversalCommandTest
        : CommandTest
    {
        const string commandCode = "code";

        [TestMethod]
        public void UniversalCommand_Construciton_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            UniversalCommand command
                = new UniversalCommand(connection, commandCode);
            Assert.AreSame(connection, command.Connection);
            Assert.AreEqual(commandCode, command.CommandCode);
        }

        [TestMethod]
        public void UniversalCommand_Verify_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            UniversalCommand command
                = new UniversalCommand(connection, commandCode);
            Assert.IsTrue(command.Verify(false));
        }
    }
}
