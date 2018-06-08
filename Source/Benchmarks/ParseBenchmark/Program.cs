using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

namespace ParseBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: ParseBenchmark <format>");

                return;
            }

            string formatFile = args[0];
            string programText = File.ReadAllText(formatFile, IrbisEncoding.Ansi);

            try
            {
                IrbisEncoding.RelaxUtf8();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                using (NullProvider provider = new NullProvider())
                {
                    provider.Database = "IBIS";

                    PftContext context = new PftContext(null);
                    context.SetProvider(provider);

                    for (int i = 0; i < 10000; i++)
                    {
                        PftFormatter formatter = new PftFormatter(context);
                        formatter.ParseProgram(programText);
                    }
                }

                stopwatch.Stop();

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine
                    (
                        "Elapsed: {0} sec",
                        stopwatch.Elapsed.ToSecondString()
                    );
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
