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

using AM;
using AM.Collections;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Fields;
using ManagedIrbis.Readers;
using ManagedIrbis.Search;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace Dupolov
{
    class Program
    {
        private const string TermPrefix = "I=";

        // Признак прерывания
        private static bool Cancel;

        // Используемое подключение
        private static IrbisConnection connection;

        // Поля, остающиеся в записи
        private static readonly int[] ResiduaryTags = { 907, 999 };

        // Дублей всего (шифры)
        private static int DoubleCount;

        // Полных дублей (записи)
        private static int FullDoubleCount;

        // Удалять дубли?
        private static bool DeleteDoubles = false;

        // ==========================================================

        static void DumpTerm
            (
                [NotNull] string term
            )
        {
            MarcRecord[] records = connection.SearchRead("\"" + term + "\"");
            Console.WriteLine
                (
                    "{0} MFN={1}",
                    term,
                    StringUtility.Join(", ", records.Select(r => r.Mfn))
                );
            if (records.Length > 1)
            {
                DoubleCount++;
            }

            for (int i = 1; i < records.Length; i++)
            {
                FieldDifference[] diff = RecordComparator.FindDifference2
                    (
                        records[0],
                        records[i],
                        ResiduaryTags
                    )
                    .ToArray();

                FieldDifference[] modified = diff
                    .Where(line => line.State != FieldState.Unchanged)
                    .ToArray();

                if (modified.Length == 0)
                {
                    Console.WriteLine("No difference found");
                    FullDoubleCount++;
                    if (DeleteDoubles)
                    {
                        connection.DeleteRecord(records[i].Mfn);
                        Console.WriteLine("Deleted");
                    }
                }
                foreach (FieldDifference line in diff)
                {
                    Console.WriteLine(line.ToString());
                }

                Console.WriteLine();
            }

            Console.WriteLine(new string('=', 70));
            Console.WriteLine();
        }

        static void DumpTerms()
        {
            bool first = true;
            TermParameters parameters = new TermParameters
            {
                Database = connection.Database,
                StartTerm = TermPrefix,
                NumberOfTerms = 1000
            };
            while (true)
            {
                if (Cancel)
                {
                    break;
                }

                TermInfo[] terms = connection.ReadTerms(parameters);
                if (terms.Length == 0)
                {
                    break;
                }

                int start = first ? 0 : 1;
                for (int i = start; i < terms.Length; i++)
                {
                    if (Cancel)
                    {
                        break;
                    }

                    TermInfo term = terms[i];
                    if (!term.Text.SafeStarts(TermPrefix))
                    {
                        break;
                    }
                    if (term.Count > 1)
                    {
                        DumpTerm(term.Text);
                    }
                }

                if (terms.Length < 2)
                {
                    break;
                }
                string lastTerm = terms.Last().Text;
                if (!lastTerm.SafeStarts(TermPrefix))
                {
                    break;
                }
                parameters.StartTerm = lastTerm;
                first = false;
            }
        }

        static void Main()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            try
            {
                DeleteDoubles = CM.AppSettings["delete"].SameString("yes");

                string connectionString = CM.AppSettings["irbis-connection-string"];

                using (connection = new IrbisConnection())
                {
                    connection.ParseConnectionString(connectionString);
                    connection.Connect();
                    //Console.WriteLine("Connected");

                    DumpTerms();

                    Console.WriteLine();
                    Console.WriteLine("Doubled indexes: {0}", DoubleCount);
                    Console.WriteLine("Full double records: {0}", FullDoubleCount);

                    //Console.WriteLine("Disconnected");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private static void Console_CancelKeyPress
            (
                object sender,
                ConsoleCancelEventArgs e
            )
        {
            Console.WriteLine("Interrupted");
            Cancel = true;
            e.Cancel = true;
        }
    }
}
