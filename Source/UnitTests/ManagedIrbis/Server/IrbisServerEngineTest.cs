using System.IO;
using System.Net;
using System.Threading.Tasks;

using AM.IO;

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
    public class IrbisServerEngineTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private ServerIniFile _GetIniFile()
        {
            string fileName = Path.Combine(Irbis64RootPath, ServerIniFile.FileName);
            IniFile iniFile = new IniFile(fileName, IrbisEncoding.Ansi, false);
            ServerIniFile result = new ServerIniFile(iniFile);

            return result;
        }

        [TestMethod]
        public void IrbisServerEngine_Construction_1()
        {
            ServerIniFile iniFile = _GetIniFile();
            IrbisServerEngine engine = new IrbisServerEngine(iniFile);
            Assert.AreSame(iniFile, engine.IniFile);
            Assert.IsNotNull(engine.StopSignal);
            Assert.IsNotNull(engine.Listener);
            Assert.IsNotNull(engine.Workers);
            Assert.AreEqual(0, engine.Workers.Count);
            engine.Dispose();
        }

        [TestMethod]
        public void IrbisServerEngine_MainLoop_1()
        {
            ServerIniFile iniFile = _GetIniFile();
            IrbisServerEngine engine = new IrbisServerEngine(iniFile);

            Task mainLoop = Task.Factory.StartNew
                (
                    () =>
                    {
                        engine.MainLoop();
                    }
                );
            mainLoop.ContinueWith
                (
                    task =>
                    {
                        engine.Dispose();
                    }
                );
            Task.Delay(100);
            engine.StopSignal.Set();
            engine.WaitForWorkers();

            //engine.Dispose();
        }
    }
}
