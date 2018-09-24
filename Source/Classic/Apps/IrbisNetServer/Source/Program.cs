// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.CommandLine;
using AM.IO;
using AM.Logging;

using ManagedIrbis;
using ManagedIrbis.Server;
using ManagedIrbis.Server.Sockets;

#endregion

namespace IrbisNetServer
{
    class Program
    {
        private static ServerSetup Setup;

        private static IrbisServerEngine Engine;

        static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            try
            {
                Log.ApplyDefaultsForConsoleApplication();

                CommandLineParser parser = new CommandLineParser();
                ParsedCommandLine parsed = parser.Parse(args);

                string logPath = parsed.GetValue("log", null);
                if (!string.IsNullOrEmpty(logPath))
                {
                    TeeLogger tee = Log.Logger as TeeLogger;
                    tee?.Loggers.Add(new FileLogger(logPath));
                }

                Log.SetLogger(new TimeStampLogger(Log.Logger.ThrowIfNull()));

                string iniPath = parsed.GetArgument(0, "irbis_server.ini")
                    .ThrowIfNull("iniPath");
                iniPath = Path.GetFullPath(iniPath);

                IniFile iniFile = new IniFile(iniPath, IrbisEncoding.Ansi, false);
                ServerIniFile serverIniFile = new ServerIniFile(iniFile);
                Setup = new ServerSetup(serverIniFile)
                {
                    RootPathOverride = parsed.GetValue("root", null),
                    PortNumberOverride = parsed.GetValue("port", 0)
                };

                if (parsed.HaveSwitch("noipv4"))
                {
                    Setup.UseTcpIpV4 = false;
                }

                if (parsed.HaveSwitch("ipv6"))
                {
                    Setup.UseTcpIpV6 = true;
                }

                int httpPort = parsed.GetValue("http", 0);
                if (httpPort > 0)
                {
                    Setup.HttpPort = httpPort;
                }

                using (Engine = new IrbisServerEngine(Setup))
                {
                    Log.Trace(ServerUtility.GetServerVersion().ToString());
                    Log.Trace("BUILD: " + IrbisConnection.ClientVersion);

                    foreach (IrbisServerListener listener in Engine.Listeners)
                    {
                        Log.Trace("Listening " + listener.GetLocalAddress());
                    }

                    Log.Trace("Entering server main loop");
                    Engine.MainLoop();
                    Log.Trace("Leaved server main loop");
                }
            }
            catch (Exception exception)
            {
                Log.TraceException("Program::Main", exception);
            }

            Log.Trace("STOP");
        }

        private static void Console_CancelKeyPress
            (
                object sender,
                ConsoleCancelEventArgs e
            )
        {
            e.Cancel = true;
            Engine?.CancelProcessing();
            Log.Trace("Cancel key pressed");
        }
    }
}
