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
    public class DynamicCommandTest
        : CommandTest
    {
        [TestMethod]
        public void DynamicCommand_Construciton_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            DynamicCommand command
                = new DynamicCommand(connection);
            Assert.AreSame(connection, command.Connection);
        }

        [TestMethod]
        public void DynamicCommand_Verify_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            DynamicCommand command
                = new DynamicCommand(connection);
            Assert.IsTrue(command.Verify(false));
        }
    }
}
