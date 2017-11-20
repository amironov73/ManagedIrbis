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
    public class ConnectCommandTest
    {
        [TestMethod]
        public void ConnectCommand_Construciton_1()
        {
            IIrbisConnection connection = new IrbisConnection();
            ConnectCommand command
                = new ConnectCommand(connection);
            Assert.AreSame(connection, command.Connection);
        }
    }
}
