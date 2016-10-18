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

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Testing;

using MoonSharp.Interpreter;

#endregion

namespace PftTestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("PftTestRunner <folder>");
                return;
            }

            try
            {
                PftTester tester = new PftTester(args[0]);

                tester.DiscoverTests();

                tester.RunTests();

                string fileName = DateTime.Now.ToString
                    (
                        "yyyy-MM-dd-hh-mm-ss"
                    )
                    + ".json";
                tester.WriteResults(fileName);

                int total = tester.Results.Count;
                int failed = tester.Results.Count(t => t.Failed);

                foreach (PftTestResult result in tester.Results)
                {
                    ConsoleColor foreColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(result.Name);
                    Console.Write('\t');
                    Console.ForegroundColor = result.Failed
                        ? ConsoleColor.Red
                        : ConsoleColor.Green;
                    Console.Write(result.Failed ? "FAILED" : "OK");
                    Console.Write('\t');
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write
                        (
                            "{0:0},{1:000}",
                            result.Duration.TotalSeconds,
                            result.Duration.Milliseconds
                        );
                    Console.ForegroundColor = foreColor;
                    Console.Write('\t');
                    Console.WriteLine(result.Description);
                }

                Console.WriteLine
                    (
                        "Total tests: {0}, failed: {1}",
                        total,
                        failed
                    );
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
