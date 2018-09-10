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
            IrbisServerEngine engine = new IrbisServerEngine(serverIni);

        }
    }
}
