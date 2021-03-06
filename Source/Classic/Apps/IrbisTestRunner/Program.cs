﻿/* Program.cs -- application entry point
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    [PublicAPI]
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("USAGE: IrbisTestRunner <config.json> [testToRun]");
                return;
            }

            IrbisEncoding.RelaxUtf8();

            TestRunner engine = null;
            string testToRun = null;
            if (args.Length > 1)
            {
                testToRun = args[1];
            }

            try
            {
                engine = new TestRunner();
                string configFileName = args[0];
                engine.LoadConfig(configFileName);
                engine.Verify(true);

                if (!engine.ForeignServer
                    && engine.FindLocalServer())
                {
                    engine.WriteLine
                        (
                            ConsoleColor.Red,
                            "Server already running"
                        );
                    return;
                }

                engine.StartServer();

                engine.HideServerWindow();

                engine.PingTheServer();

                engine.DiscoverTests();

                engine.CompileTests();

                engine.RunTests(testToRun);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            finally
            {
                if (!ReferenceEquals(engine, null))
                {
                    engine.StopServer();
                }
            }

            if (!ReferenceEquals(engine, null))
            {
                engine.PrintReport();
            }

        }
    }
}
