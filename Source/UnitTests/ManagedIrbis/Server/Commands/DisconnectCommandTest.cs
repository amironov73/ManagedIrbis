using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using AM.IO;
using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Search;
using ManagedIrbis.Server;
using ManagedIrbis.Server.Commands;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.ManagedIrbis.Server.Commands
{
    [TestClass]
    public class DisconnectCommandTest
        : CommonCommandTest
    {
        [TestMethod]
        public void DisconnectCommand_Construction_1()
        {
            Mock<ClientRequest> mock = new Mock<ClientRequest>();
            ClientRequest request = mock.Object;
            ServerContext context = _GetContext();
            DisconnectCommand command = new DisconnectCommand(request, context);
            Assert.AreSame(context, command.Context);
        }
    }
}
