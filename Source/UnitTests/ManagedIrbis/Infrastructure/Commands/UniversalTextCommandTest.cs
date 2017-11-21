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
    public class UniversalTextCommandTest
        : CommandTest
    {
        const string commandCode = "code";

        [TestMethod]
        public void UniversalTextCommand_Construciton_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            UniversalTextCommand command
                = new UniversalTextCommand(connection, commandCode);
            Assert.AreSame(connection, command.Connection);
            Assert.AreEqual(commandCode, command.CommandCode);
        }

        [TestMethod]
        public void UniversalTextCommand_Verify_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            UniversalTextCommand command
                = new UniversalTextCommand(connection, commandCode);
            Assert.IsFalse(command.Verify(false));
        }
    }
}
