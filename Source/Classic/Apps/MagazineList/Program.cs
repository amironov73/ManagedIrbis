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
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.CommandLine;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Fields;
using ManagedIrbis.Magazines;

#endregion

namespace MagazineList
{
    class Program
    {
        static int Main(string[] arguments)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                string connectionString = ConfigurationManager.AppSettings["connectionString"];

                if (string.IsNullOrEmpty(connectionString))
                {
                    Console.WriteLine("Connection string not specified");
                    return 1;
                }

                CommandLineParser parser = new CommandLineParser();
                ParsedCommandLine parsed = parser.Parse(arguments);
                if (parsed.PositionalArguments.Count != 0)
                {
                    connectionString = parsed.PositionalArguments[0];
                }

                string searchExpression = "V=01  + V=02";
                string filterExpression = null;

                if (parsed.HaveSwitch("search"))
                {
                    searchExpression = parsed.GetSwitch("search")
                        .ThrowIfNull().Value;
                }

                searchExpression = searchExpression.ThrowIfNull();

                if (parsed.HaveSwitch("filter"))
                {
                    filterExpression = parsed.GetSwitch("filter")
                        .ThrowIfNull().Value;
                }

                using (IrbisConnection connection
                    = new IrbisConnection(connectionString))
                {
                    IEnumerable<MarcRecord> allRecords = BatchRecordReader.Search
                        (
                            connection,
                            connection.Database,
                            searchExpression,
                            500,
                            batch => Console.Write(".")
                        );


                }

                stopwatch.Stop();
                Console.WriteLine
                    (
                        "Elapsed: {0}",
                        stopwatch.Elapsed.ToAutoString()
                    );
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return 1;
            }

            return 0;
        }
    }
}
