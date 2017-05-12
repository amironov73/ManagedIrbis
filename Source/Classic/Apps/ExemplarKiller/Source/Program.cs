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
using System.Threading.Tasks;

using AM;
using AM.Configuration;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace ExemplarKiller
{
    class Program
    {
        private static string prefix;
        private static string format;
        private static string readers;
        private static string[] databases;
        private static bool doDelete;
        private static IrbisConnection connection;

        private static void ReturnExemplar
            (
                [NotNull] MarcRecord record,
                [NotNull] string number,
                [CanBeNull] string barcode
            )
        {
            bool modified = false;

            Console.Write
                (
                    " <ticket {0}>",
                    record.FM("30")
                );

            RecordField[] fields = record.Fields
                .GetField("40");
            foreach (RecordField field in fields)
            {
                bool ok = field.GetFirstSubFieldValue('b')
                    .SameString(number);
                if (!ok && !string.IsNullOrEmpty(barcode))
                {
                    ok = field.GetFirstSubFieldValue('h')
                        .SameString(barcode);
                }

                if (ok)
                {
                    field.SetSubField('f', IrbisDate.TodayText);
                    modified = true;
                }
            }

            if (modified)
            {
                Console.Write(" <returned>");
                connection.WriteRecord(record);
            }
        }

        private static void DeleteExemplar
            (
                [NotNull] string database,
                int mfn,
                [NotNull] string number
            )
        {
            MarcRecord bookRecord = connection.ReadRecord
                (
                    database,
                    mfn,
                    false,
                    format
                );
            Console.Write
                (
                    " [{0}]",
                    bookRecord.Description
                );

            RecordField found = bookRecord.Fields
                .GetField("910")
                .GetField('b', number)
                .FirstOrDefault();

            if (ReferenceEquals(found, null))
            {
                Console.Write(" <missing>");
            }
            else
            {
                found.SetSubField('a', "6");
                Console.Write(" <written off>");
                connection.WriteRecord(bookRecord);

                string barcode = found.GetFirstSubFieldValue('h');
                BatchSearcher searcher = new BatchSearcher
                    (
                        connection,
                        readers,
                        "H="
                    );
                MarcRecord[] readerRecords = searcher.SearchRead
                    (
                        new[] { number, barcode }
                    );
                foreach (MarcRecord readerRecord in readerRecords)
                {
                    ReturnExemplar
                        (
                            readerRecord,
                            number,
                            barcode
                        );
                }
            }
        }

        private static void ProcessExemplar
            (
                int index,
                [NotNull] string number,
                [CanBeNull] string description
            )
        {
            if (string.IsNullOrEmpty(number))
            {
                return;
            }

            Console.Write
                (
                    "{0}: {1} [{2}]",
                    index,
                    number,
                    description
                );

            foreach (string database in databases)
            {
                connection.Database = database;
                int[] found = connection.Search
                    (
                        "\"{0}{1}\"",
                        prefix,
                        number
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
                            DeleteExemplar
                                (
                                    database,
                                    mfn,
                                    number
                                );
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
                        "Usage: ExemplarKiller <bookList>"
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
            prefix = ConfigurationUtility.GetString
                (
                    "prefix",
                    "IN="
                );
            format = ConfigurationUtility.GetString
                (
                    "format",
                    "@brief"
                );
            readers = ConfigurationUtility.GetString
                (
                    "readers",
                    "RDR"
                );

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                string connectionString = args.Length > 1
                    ? args[2]
                    : CM.AppSettings["connectionString"];

                string[] numbers = File.ReadAllLines
                    (
                        fileName,
                        Encoding.UTF8
                    );
                Console.WriteLine
                    (
                        "Numbers loaded: {0}",
                        numbers.Length
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

                    for (int i = 0; i < numbers.Length; i++)
                    {
                        string line = numbers[i];
                        string[] parts = line.Split
                            (
                                new[] { ';', ' ', '\t' },
                                2,
                                StringSplitOptions.RemoveEmptyEntries
                            );
                        string number = parts[0].Trim();
                        string description = null;
                        if (parts.Length != 1)
                        {
                            description = parts[1].Trim();
                        }

                        ProcessExemplar
                            (
                                i,
                                number,
                                description
                            );
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
