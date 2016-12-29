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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Monitoring;

using MoonSharp.Interpreter;

using CM=System.Configuration.ConfigurationManager;

#endregion

class Program
{
    private static IrbisMonitor monitor;

    static void Main()
    {
        try
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            using (IrbisConnection connection = new IrbisConnection())
            {
                string connectionString = CM.AppSettings["connectionString"];
                connection.ParseConnectionString(connectionString);
                connection.Connect();

                string database = CM.AppSettings["database"];

                monitor = new IrbisMonitor
                    (
                        connection,
                        database
                    );

                TextMonitoringSink sink = new TextMonitoringSink(Console.Out);
                monitor.Sink = sink;

                monitor.StartMonitoring();
                Console.WriteLine("Monitoring started");
                Console.WriteLine();

                while (monitor.Active)
                {
                    Thread.Sleep(100);
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    static void Console_CancelKeyPress
        (
            object sender,
            ConsoleCancelEventArgs e
        )
    {
        e.Cancel = true;

        monitor.StopMonitoring();
        Console.Write("Stopping");

        while (monitor.Active)
        {
            Console.Write('.');
            Thread.Sleep(500);
        }

        Console.WriteLine();
        Console.WriteLine("Stopped");
    }
}
