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
    public class GblCommandTest
        : CommandTest
    {
        [TestMethod]
        public void GblCommand_Construciton_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            GblCommand command
                = new GblCommand(connection);
            Assert.AreSame(connection, command.Connection);
        }

        [TestMethod]
        public void GblCommand_Verify_1()
        {
            Mock<IIrbisConnection> mock = GetConnectionMock();
            IIrbisConnection connection = mock.Object;
            GblCommand command
                = new GblCommand(connection);

            // TODO Fix this
            Assert.IsTrue(command.Verify(false));
        }
    }
}
