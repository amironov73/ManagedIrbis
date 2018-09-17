using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;
using JetBrains.Annotations;
using ManagedIrbis;
using ManagedIrbis.Server;

namespace UnitTests.Experiments
{
    [TestClass]
    public class ServerExperiments
        : Common.CommonUnitTest
    {
        [NotNull]
        private IrbisServerEngine _GetEngine()
        {
            string serverRootPath = Irbis64RootPath;

            string fileName = Path.Combine(serverRootPath, "irbis_server.ini");
            IniFile simpleIni = new IniFile(fileName, IrbisEncoding.Ansi, false);
            ServerIniFile serverIni = new ServerIniFile(simpleIni);
            IrbisServerEngine engine = new IrbisServerEngine(serverIni, serverRootPath);
            Assert.IsNotNull(engine.IniFile);
            Assert.IsNotNull(engine.ClientIni);
            Assert.IsNotNull(engine.Contexts);
            Assert.IsNotNull(engine.DataPath);
            Assert.IsNotNull(engine.Listener);
            Assert.IsNotNull(engine.Mapper);
            Assert.IsNotNull(engine.StopSignal);
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
                            Host = "127.0.0.1",
                            Port = 6666,
                            Username = "librarian",
                            Password = "secret",
                            Workstation = IrbisWorkstation.Cataloger
                        };
                        connection.Connect();

                        action(connection);

                        connection.Dispose();
                        engine.StopSignal.Set();
                    }
                    catch (Exception ex)
                    {
                        engine.StopSignal.Set();
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

        [TestMethod]
        public void Server_GetMaxMfn_1()
        {
            Exception ex = _RunAction(connection =>
            {
                int maxMfn = connection.GetMaxMfn("IBIS");
                Assert.AreEqual(332, maxMfn);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_ReadRecord_1()
        {
            Exception ex = _RunAction(connection =>
            {
                MarcRecord record = connection.ReadRecord("IBIS", 1, false, null);
                Assert.AreEqual(100, record.Fields.Count);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_GetServerVersion_1()
        {
            Exception ex = _RunAction(connection =>
            {
                IrbisVersion expected = ServerUtility.GetServerVersion();
                IrbisVersion actual = connection.GetServerVersion();
                Assert.AreEqual(expected.Organization, actual.Organization);
                Assert.AreEqual(expected.Version, actual.Version);
                Assert.AreEqual(1, actual.ConnectedClients);
                Assert.AreEqual(expected.MaxClients, actual.MaxClients);
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }

        [TestMethod]
        public void Server_UpdateIniFile_1()
        {
            Exception ex = _RunAction(connection =>
            {
                List<string> lines = new List<string>();
                lines.Add("[MIRON]");
                lines.Add(string.Format("LastAccess={0}", DateTime.Now));
                connection.UpdateIniFile(lines.ToArray());
            });
            if (!ReferenceEquals(ex, null))
            {
                throw ex;
            }
        }
    }
}
