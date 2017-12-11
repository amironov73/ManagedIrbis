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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Server.Commands
{
    public class CommonCommandTest
        : Common.CommonUnitTest
    {
        [NotNull]
        protected string _GetIniFileName()
        {
            return Path.Combine(Irbis64RootPath, ServerIniFile.FileName);
        }

        [NotNull]
        protected ServerContext _GetContext()
        {
            IniFile iniFile = new IniFile(_GetIniFileName());
            ServerIniFile serverIni = new ServerIniFile(iniFile);
            IrbisServerEngine engine = new IrbisServerEngine(serverIni);

            return new ServerContext
            {
                Address = "127.0.0.1",
                CommandCount = 123,
                Connected = new DateTime(2017, 12, 11, 17, 29, 0),
                Id = "1234567",
                LastActivity = new DateTime(2017, 12, 11, 17, 30, 0),
                Password = "password",
                Engine = engine,
                Username = "username",
                Workstation = IrbisWorkstation.Administrator,
                UserData = "User data"
            };
        }
    }
}
