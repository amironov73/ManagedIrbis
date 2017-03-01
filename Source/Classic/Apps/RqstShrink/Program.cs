// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RqstShrink.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Linq;

using AM;
using AM.CommandLine;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Requests;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace RqstShrink
{
    class Program
    {
        static int Main(string[] arguments)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                string connectionString = CM.AppSettings["connectionString"];

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

                using (IrbisConnection connection
                    = new IrbisConnection(connectionString))
                {
                    string expression = RequestPrefixes.Unfulfilled /* I=0 */
                                        + " + "
                                        + RequestPrefixes.Reserved; /* I=2 */

                    if (parsed.HaveSwitch("expression"))
                    {
                        expression = parsed.GetSwitch("expression")
                            .ThrowIfNull()
                            .Value;
                    }

                    expression = expression.ThrowIfNull("expression");

                    Console.Write("Reading good records ");

                    MarcRecord[] goodRecords = BatchRecordReader.Search
                        (
                            connection,
                            connection.Database,
                            expression,
                            1000,
                            batch => Console.Write(".")
                        )
                        .ToArray();

                    Console.WriteLine();
                    Console.WriteLine
                        (
                            "Good records loaded: {0}",
                            goodRecords.Length
                        );

                    connection.TruncateDatabase(connection.Database);

                    Console.WriteLine("Database truncated");


                    using (BatchRecordWriter writer = new BatchRecordWriter
                        (
                            connection,
                            connection.Database,
                            500
                        ))
                    {
                        foreach (MarcRecord record in goodRecords)
                        {
                            record.Version = 0;
                            record.Mfn = 0;
                            writer.Append(record);
                        }
                    }

                    Console.WriteLine("Good records restored");

                    stopwatch.Stop();
                    Console.WriteLine
                        (
                            "Elapsed: {0}",
                            stopwatch.Elapsed.ToAutoString()
                        );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return 1;
            }

            return 0;
        }
    }
}
