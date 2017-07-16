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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Logging;
using AM.Text.Output;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

namespace PftPrettyPrint
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                return;
            }

            string rootPath = args[0];
            string format = args[1];

            Log.ApplyDefaultsForConsoleApplication();

            try
            {
                using (LocalProvider provider = new LocalProvider(rootPath))
                {
                    FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        provider.Database,
                        format
                    );
                    string source = provider.ReadFile(specification);
                    if (string.IsNullOrEmpty(source))
                    {
                        Console.WriteLine("No file: {0}", format);
                    }
                    else
                    {
                        PftContext context = new PftContext(null);
                        context.SetProvider(provider);
                        PftFormatter formatter = new PftFormatter(context);
                        formatter.ParseProgram(source);

                        PftProgram program = formatter.Program;
                        AbstractOutput console = new ConsoleOutput();
                        PftPrettyPrinter printer = new PftPrettyPrinter();
                        console.WriteLine(string.Empty);
                        console.WriteLine(new string('=', 60));
                        console.WriteLine(string.Empty);
                        console.WriteLine(string.Empty);
                        program.PrettyPrint(printer);
                        console.WriteLine(printer.ToString());
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
