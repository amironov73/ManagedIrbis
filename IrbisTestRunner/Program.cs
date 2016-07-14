/* Program.cs -- application entry point
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;

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
            if (args.Length != 1)
            {
                Console.WriteLine("USAGE: IrbisTestRunner <config.json>");
                return;
            }

            TestRunner engine = null;
            try
            {
                engine = new TestRunner();
                string configFileName = args[0];
                engine.LoadConfig(configFileName);
                engine.Verify(true);

                if (engine.FindLocalServer())
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

                engine.RunTests();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            finally
            {
                if (engine != null)
                {
                    engine.StopServer();
                }
            }
        }
    }
}
