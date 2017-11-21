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
    public class CreateDatabaseCommandTest
        : CommandTest
    {
        [TestMethod]
        public void CreateDatabaseCommand_Construciton_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            CreateDatabaseCommand command
                = new CreateDatabaseCommand(connection);
            Assert.AreSame(connection, command.Connection);
        }
    }
}
