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
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.Configuration;

using ManagedIrbis;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace ReaderKiller
{
    class Program
    {
        private static string[] databases;
        private static bool doDelete;
        private static IrbisConnection connection;

        private static void ProcessReader
            (
                int index,
                string ticket
            )
        {
            if (string.IsNullOrEmpty(ticket))
            {
                return;
            }

            Console.Write
                (
                    "{0}: {1}",
                    index,
                    ticket
                );

            foreach (string database in databases)
            {
                connection.Database = database;
                int[] found = connection.Search
                    (
                        "\"RI={0}\"",
                        ticket
                    );
                if (found.Length == 0)
                {
                    Console.Write(" {0} [not found]", database);
                }
                else if (found.Length > 1)
                {
                    Console.Write(" {0} [too many!]", database);
                }
                else
                {
                    int mfn = found[0];
                    Console.Write
                        (
                            " {0} [MFN {1}]",
                            database,
                            mfn
                        );
                    if (doDelete)
                    {
                        try
                        {
                            connection.DeleteRecord(mfn, true);
                            Console.Write(" <deleted>");
                        }
                        catch (Exception exception)
                        {
                            Console.Write
                                (
                                    " <exception>: {0}",
                                    exception.GetType().Name
                                );
                        }
                    }
                }
            }

            Console.WriteLine(" done");
        }

        private static bool ExceptionResolver
            (
                Exception exception
            )
        {
            Console.WriteLine
                (
                    " {0}",
                    exception.Message
                );

            return true;
        }

        static void Main(string[] args)
        {
            if (args.Length < 1
                || args.Length > 2)
            {
                Console.WriteLine
                    (
                        "Usage: ReaderKiller <readerList>"
                    );

                return;
            }

            string fileName = args[0];
            doDelete = ConfigurationUtility.GetBoolean
                (
                    "delete",
                    false
                );
            databases = ConfigurationUtility.GetString
                (
                    "databases",
                    null
                )
                .ThrowIfNull("Databases not specified")
                .Split
                    (
                        new[] { ';', ',', ' ' },
                        StringSplitOptions.RemoveEmptyEntries
                    );
            if (databases.Length == 0)
            {
                throw new Exception("Empty database list");
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                string connectionString = args.Length > 1
                    ? args[2]
                    : CM.AppSettings["connectionString"];

                string[] tickets = File.ReadAllLines
                    (
                        fileName,
                        Encoding.UTF8
                    );
                Console.WriteLine
                    (
                        "Tickets loaded: {0}",
                        tickets.Length
                    );

                using (connection = new IrbisConnection())
                {
                    connection.SetRetry(10, ExceptionResolver);

                    connection.ParseConnectionString
                        (
                            connectionString
                        );
                    connection.Connect();

                    Console.WriteLine("Connected");

                    for (int i = 0; i < tickets.Length; i++)
                    {
                        string ticket = tickets[i];
                        ProcessReader(i, ticket);
                    }
                }

                Console.WriteLine("Disconnected");

                stopwatch.Stop();
                Console.WriteLine
                    (
                        "Time elapsed: {0}",
                        stopwatch.Elapsed.ToAutoString()
                    );
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
