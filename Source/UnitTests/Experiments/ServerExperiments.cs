using System;
using System.IO;

using AM.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Server;

namespace UnitTests.Experiments
{
    [TestClass]
    public class ServerExperiments
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void RunEngine_1()
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
        }
    }
}
