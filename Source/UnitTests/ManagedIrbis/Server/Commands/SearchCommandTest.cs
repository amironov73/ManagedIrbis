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
using ManagedIrbis.Menus;
using ManagedIrbis.Search;
using ManagedIrbis.Server;
using ManagedIrbis.Server.Commands;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.ManagedIrbis.Server.Commands
{
    [TestClass]
    public class SearchCommandTest
        : CommonCommandTest
    {
        [TestMethod]
        public void SearchCommand_Construction_1()
        {
            WorkData data = new WorkData();
            SearchCommand command = new SearchCommand(data);
            Assert.AreSame(data, command.Data);
        }
    }
}
