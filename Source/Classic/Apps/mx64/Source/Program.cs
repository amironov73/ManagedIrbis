/* Program.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.CommandLine;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mx;
using ManagedIrbis.Mx.Commands;
using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace mx64
{
    class Program
    {
        private static MxExecutive executive;

        static void Main(string[] args)
        {
            try
            {
                CommandLineParser parser = new CommandLineParser();
                ParsedCommandLine commandLine = parser.Parse(args);

                CommandLineSwitch flag = commandLine.GetSwitch("V");
                if (!ReferenceEquals(flag, null))
                {
                    Console.WriteLine("mx64 version {0}", MxExecutive.Version);
                    return;
                }

                string fileToExecute = commandLine.GetArgument(0, null);
                string commandToExecute = null;

                flag = commandLine.GetSwitch("c");
                if (!ReferenceEquals(flag, null))
                {
                    commandToExecute = flag.Value;
                }

                using (executive = new MxExecutive())
                {
                    flag = commandLine.GetSwitch("q");
                    if (!ReferenceEquals(flag, null))
                    {
                        executive.VerbosityLevel = 0;
                    }

                    if (!executive.ExecuteInitScript())
                    {
                        return;
                    }

                    if (!ReferenceEquals(commandToExecute, null))
                    {
                        executive.ExecuteLine(commandToExecute);
                    }
                    else if (ReferenceEquals(fileToExecute, null))
                    {
                        if (executive.VerbosityLevel >= 3)
                        {
                            executive.Banner();
                        }

                        executive.Repl();
                        executive.GetCommand<DisconnectCommand>()
                            .Execute(executive, MxArgument.EmptyArray);
                    }
                    else
                    {
                        executive.ExecuteFile(fileToExecute);
                    }
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
