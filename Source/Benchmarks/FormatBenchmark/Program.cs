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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Direct;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

#endregion

namespace FormatBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: FormatBenchmark <rootPath> <format>");

                return;
            }

            string rootPath = args[0];
            string format = args[1];

            try
            {
                IrbisEncoding.RelaxUtf8();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                using (LocalProvider provider = new LocalProvider(rootPath))
                {
                    provider.Database = "IBIS";
                    int maxMfn = provider.GetMaxMfn();
                    Console.WriteLine("Max MFN={0}", maxMfn);

                    PftContext context = new PftContext(null);
                    context.SetProvider(provider);
                    PftFormatter formatter = new PftFormatter(context);
                    formatter.ParseProgram(format);

                    for (int mfn = 1; mfn <= maxMfn; mfn++)
                    {
                        MarcRecord record = provider.ReadRecord(mfn);
                        if (ReferenceEquals(record, null))
                        {
                            continue;
                        }

                        string text = formatter.FormatRecord(record);
                        Console.WriteLine(text);
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
