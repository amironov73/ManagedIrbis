// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using AM;
using AM.Collections;

using DevExpress.Spreadsheet;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Fields;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Search;
using static System.Console;

#endregion

// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement

namespace Casquette
{
    class Program
    {
        private const int STEP = 50;

        private static readonly string connectionString
            = "host=127.0.0.1;port=6666;user=librarian;password=secret;db=KNIK;arm=C;";

        private static IrbisConnection connection;
        private static IrbisProvider provider;
        private static Workbook workbook;
        private static Worksheet worksheet;
        private static int currentLine;
        private static readonly DictionaryCounterInt32<int> loanCounter
            = new DictionaryCounterInt32<int>();
        private static readonly DictionaryCounterInt32<int> exemplarCounter
            = new DictionaryCounterInt32<int>();

        static int CountLoans(MarcRecord record)
        {
            int result = record.FM(999).SafeToInt32();
            foreach (string counter in record.FMA(2999, 'b'))
            {
                result += counter.SafeToInt32();
            }

            return result;
        }

        static void ProcessBook(MarcRecord record)
        {
            BookInfo book = new BookInfo(provider, record);
            if (!book.DocumentType.SameString("a"))
            {
                return;
            }

            ExemplarInfo[] exemplars = book.Exemplars;

            if (exemplars.Length == 0)
            {
                return;
            }

            int pages = book.Pages;
            if (pages <= 0)
            {
                return;
            }

            int exemplarCount = book.ExemplarCount;
            int loanCount = CountLoans(record);

            WriteCell(0, book.Description);
            WriteCell(1, exemplarCount);
            WriteCell(2, pages);
            WriteCell(3, loanCount);
            currentLine++;

            int index = pages / STEP;
            loanCounter.Augment(index, loanCount);
            exemplarCounter.Augment(index, exemplarCount);
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        static Cell WriteCell(int column, string value)
        {
            if (int.TryParse(value, out var number))
            {
                return WriteCell(column, number);
            }

            Cell cell = worksheet.Cells[currentLine, column];
            cell.Value = value;
            cell.NumberFormat = "@";
            return cell;
        }

        static Cell WriteCell(int column, int value)
        {
            Cell cell = worksheet.Cells[currentLine, column];
            cell.Value = value;
            return cell;
        }

        static Cell WriteCell(int column, double value)
        {
            Cell cell = worksheet.Cells[currentLine, column];
            cell.Value = value;
            cell.NumberFormat = "0.00";
            return cell;
        }

        static void DumpCounter()
        {
            currentLine += 2;

            int maxKey = loanCounter.Keys.Max();
            for (int i = 0; i <= maxKey; i++)
            {
                double ratio = 0.0;
                var exemplars = exemplarCounter.GetValue(i);
                var loans = loanCounter.GetValue(i);
                if (exemplars != 0)
                {
                    ratio = ((double) loans) / exemplars;
                }

                WriteCell(1, i * STEP);
                WriteCell(2, exemplars);
                WriteCell(3, loans);
                WriteCell(4, ratio);
                currentLine++;
            }
        }

        static void ProcessAll()
        {
            ReadRecordCommand.ThrowOnVerify = false;
            //const string expression = "\"V=KN\"";
            const string expression = "\"I=$\"";
            int[] allBooks = connection.Search(expression);

            //List<int> allBooks = new List<int>(50_000);
            //int first = 1;
            //int total = connection.SearchCount(expression);
            //while (first < total)
            //{
            //    SearchParameters parameters = new SearchParameters
            //    {
            //        Database = connection.Database,
            //        SearchExpression = expression,
            //        FirstRecord = first,
            //        NumberOfRecords = 10000
            //    };

            //    int[] found = connection.SequentialSearch(parameters);
            //    if (found.Length == 0)
            //    {
            //        break;
            //    }

            //    allBooks.AddRange(found);
            //    first += found.Length;
            //}

            WriteLine($"Found={allBooks.Length}");

            int index = 0;
            foreach (int mfn in allBooks)
            {
                if ((index % 50) == 0)
                {
                    WriteLine();
                    Write($"{index:000000}> ");
                }

                index++;
                Write(".");

                try
                {
                    MarcRecord record = connection.ReadRecord(mfn);
                    ProcessBook(record);
                }
                catch (Exception ex)
                {
                    WriteLine("{0}: {1}", mfn, ex.Message);
                }
            }
        }

        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                workbook = new Workbook
                {
                    Unit = DevExpress.Office.DocumentUnit.Millimeter
                };
                worksheet = workbook.Worksheets[0];

                using (connection = new IrbisConnection(connectionString))
                {
                    provider = new ConnectedClient(connection);
                    ProcessAll();
                }

                DumpCounter();

                workbook.SaveDocument("effective.xlsx");
            }
            catch (Exception exception)
            {
                WriteLine(exception.ToString());
            }

            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed;
            WriteLine();
            WriteLine($"Elapsed={elapsed.ToAutoString()}");
        }
    }
}
