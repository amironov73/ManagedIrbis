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
using ManagedIrbis.Server;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Server
{
    [TestClass]
    public class ServerWorkerTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void IrbisServerWorker_Construction_1()
        {
            WorkData data = new WorkData();
            ServerWorker worker = new ServerWorker(data);
            Assert.AreSame(data, worker.Data);
        }
    }
}
