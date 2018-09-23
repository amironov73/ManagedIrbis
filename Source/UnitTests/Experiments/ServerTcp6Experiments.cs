using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Search;
using ManagedIrbis.Server;
using ManagedIrbis.Server.Sockets;

namespace UnitTests.Experiments
{
    [TestClass]
    public class ServerTcp6Experiments
        : Common.CommonUnitTest
    {
        [NotNull]
        private IrbisServerEngine _GetEngine()
        {
            string serverRootPath = Irbis64RootPath;

            string fileName = Path.Combine(serverRootPath, "irbis_server.ini");
            IniFile simpleIni = new IniFile(fileName, IrbisEncoding.Ansi, false);
            ServerIniFile serverIni = new ServerIniFile(simpleIni);
            ServerSetup setup = new ServerSetup(serverIni)
            {
                RootPathOverride = serverRootPath,
                PortNumberOverride = new Random().Next(50000, 60000),
                UseTcpIpV4 = false,
                UseTcpIpV6 = true
            };
            IrbisServerEngine engine = new IrbisServerEngine(setup);
            Assert.IsNotNull(engine.IniFile);
            Assert.IsNotNull(engine.ClientIni);
            Assert.IsNotNull(engine.Contexts);
            Assert.IsNotNull(engine.DataPath);
            Assert.IsNotNull(engine.Listeners);
            Assert.AreEqual(1, engine.Listeners.Length);
            Assert.IsNotNull(engine.Mapper);
            Assert.IsNotNull(engine.SystemPath);
            Assert.IsNotNull(engine.Users);
            Assert.IsNotNull(engine.WorkDir);
            Assert.IsNotNull(engine.Workers);

            Assert.AreEqual(2, engine.Users.Length);
            Assert.AreEqual("librarian", engine.Users[0].Name);
            Assert.AreEqual("secret", engine.Users[0].Password);

            return engine;
        }

        private Exception _RunAction
            (
                [NotNull] Action<IrbisConnection> action
            )
        {
            Exception result = null;

            using (IrbisServerEngine engine = _GetEngine())
            {
                Task actionTask = new Task(() =>
                {
                    try
                    {
                        Task.Delay(100).Wait();
                        IrbisConnection connection = new IrbisConnection
                        {
                            Host = "localhost",
                            Port = engine.PortNumber,
                            Username = "librarian",
                            Password = "secret",
                            Workstation = IrbisWorkstation.Administrator
                        };
                        connection.SetSocket(new SimpleClientSocketV6(connection));
                        connection.Connect();

                        action(connection);

                        connection.Dispose();
                        engine.CancelProcessing();
                    }
                    catch (Exception ex)
                    {
                        engine.CancelProcessing();
                        result = ex;
                    }
                });
                try
                {
                    actionTask.Start();
                    engine.MainLoop();
                    actionTask.Wait();

                    if (!ReferenceEquals(result, null))
                    {
                        return result;
                    }

                    Assert.AreEqual(0, engine.Contexts.Count);
                }
                catch (Exception ex)
                {
                    return ex;
                }

                return result;
            }
        }

        [TestMethod]
        public void Server_NoOp_1()
        {
            Exception ex = _RunAction(connection =>
            {
                connection.NoOp();
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }
    }
}
