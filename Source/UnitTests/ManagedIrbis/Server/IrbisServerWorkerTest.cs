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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Server
{
    [TestClass]
    public class IrbisServerWorkerTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetFileName()
        {
            return Path.Combine(Irbis64RootPath, ServerIniFile.FileName);
        }

        [NotNull]
        private IrbisServerEngine _GetEngine()
        {
            IniFile iniFile = new IniFile(_GetFileName(), IrbisEncoding.Ansi, false);
            ServerIniFile serverIni = new ServerIniFile(iniFile);
            IrbisServerEngine result = new IrbisServerEngine(serverIni);

            return result;
        }

        [TestMethod]
        public void IrbisServerWorker_Construction_1()
        {
            IrbisServerEngine engine = _GetEngine();
            TcpClient client = new TcpClient();
            IrbisServerSocket socket = new IrbisServerSocket(client);
            IrbisServerWorker worker = new IrbisServerWorker(engine, socket);
            Assert.AreSame(engine, worker.Engine);
            Assert.AreSame(socket, worker.Socket);
            Assert.IsNotNull(worker.Task);
        }

        [TestMethod]
        public void IrbisServerWorker_DoWork_1()
        {
            IrbisServerEngine engine = _GetEngine();
            TcpClient client = new TcpClient();
            IrbisServerSocket socket = new IrbisServerSocket(client);
            IrbisServerWorker worker = new IrbisServerWorker(engine, socket);
            worker.Task.Start();
            worker.Task.Wait();
        }
    }
}
