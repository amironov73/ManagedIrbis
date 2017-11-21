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
    public class ListUsersCommandTest
        : CommandTest
    {
        [TestMethod]
        public void ListUsersCommand_Construciton_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ListUsersCommand command
                = new ListUsersCommand(connection);
            Assert.AreSame(connection, command.Connection);
        }

        [TestMethod]
        public void ListUsersCommand_Verify_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            ListUsersCommand command
                = new ListUsersCommand(connection);
            Assert.IsTrue(command.Verify(false));
        }
    }
}
