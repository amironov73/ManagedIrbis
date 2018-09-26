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
        private static IrbisServerEngine Engine;

        static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            try
            {
                Log.ApplyDefaultsForConsoleApplication();

                using (Engine = ServerUtility.CreateEngine(args))
                {
                    ServerUtility.DumpEngineSettings(Engine);
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
