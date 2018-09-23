using System;
using System.IO;
using System.Threading.Tasks;

using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
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

        [NotNull]
        private IrbisServerEngine _GetEngine
            (
                [NotNull] ServerIniFile iniFile
            )
        {
            string serverRootPath = Irbis64RootPath;
            ServerSetup setup = new ServerSetup(iniFile)
            {
                RootPathOverride = serverRootPath,
                PortNumberOverride = new Random().Next(50000, 60000)
            };
            IrbisServerEngine result = new IrbisServerEngine(setup);
            return result;
        }

        [TestMethod]
        public void IrbisServerEngine_Construction_1()
        {
            ServerIniFile iniFile = _GetIniFile();
            IrbisServerEngine engine = _GetEngine(iniFile);
            Assert.AreSame(iniFile, engine.IniFile);
            Assert.IsNotNull(engine.Listeners);
            Assert.AreEqual(1, engine.Listeners.Length);
            Assert.IsNotNull(engine.Workers);
            Assert.AreEqual(0, engine.Workers.Count);
            engine.Dispose();
        }

        [TestMethod]
        public void IrbisServerEngine_MainLoop_1()
        {
            ServerIniFile iniFile = _GetIniFile();
            IrbisServerEngine engine = _GetEngine(iniFile);

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
            engine.CancelProcessing();
            engine.WaitForWorkers();

            engine.Dispose();
        }
    }
}
