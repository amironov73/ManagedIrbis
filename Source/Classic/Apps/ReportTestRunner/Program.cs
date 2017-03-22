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

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Client;
using ManagedIrbis.Reports;

using MoonSharp.Interpreter;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace ReportTestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("ReportTestRunner <folder>");
                return;
            }

            try
            {
                string rootPath = CM.AppSettings["rootPath"];
                AbstractClient environment
                    = new LocalClient(rootPath);

                ReportTester tester = new ReportTester(args[0]);
                tester.SetEnvironment(environment);

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

                ConsoleColor foreColor;
                foreach (ReportTestResult result in tester.Results)
                {
                    foreColor = Console.ForegroundColor;
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

                Console.WriteLine();
                foreColor = Console.ForegroundColor;
                Console.ForegroundColor = failed == 0
                    ? ConsoleColor.Green
                    : ConsoleColor.Red;
                Console.WriteLine
                    (
                        "Total tests: {0}, failed: {1}",
                        total,
                        failed
                    );
                if (failed != 0)
                {
                    Console.Write
                        (
                            "Failed tests: {0}",
                            StringUtility.Join
                                (
                                    ", ",
                                    tester.Results
                                    .Where(t => t.Failed)
                                    .Select(t => t.Name)
                                )
                        );
                }
                Console.ForegroundColor = foreColor;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
