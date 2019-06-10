// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System;
using System.Diagnostics;
using System.Linq;

using AM;
using AM.Collections;

using DevExpress.Spreadsheet;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Fields;

using static System.Console;

#endregion

// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement

namespace Gavialis
{
    class Program
    {
        private const int STEP = 50;

        private static readonly string connectionString
            = "host=127.0.0.1;port=6666;user=librarian;password=secret;db=IBIS;arm=C;";

        private static IrbisConnection connection;
        private static IrbisProvider provider;
        private static Workbook workbook;
        private static Worksheet worksheet;
        private static int currentLine;
        private static readonly DictionaryCounterInt32<int> loanCounter
            = new DictionaryCounterInt32<int>();
        private static readonly DictionaryCounterInt32<int> exemplarCounter
            = new DictionaryCounterInt32<int>();

        static void ProcessBook(MarcRecord record, string ksu)
        {
            BookInfo book = new BookInfo(provider, record);
            if (!book.DocumentType.SameString("a"))
            {
                return;
            }

            ExemplarInfo[] selected = book.Exemplars.Where
                (
                    ex => ex.KsuNumber1.SameString(ksu)
                )
                .ToArray();

            if (selected.Length == 0)
            {
                return;
            }

            int pages = book.Pages;
            if (pages <= 0 || pages > 1000)
            {
                return;
            }

            int exemplarCount = book.ExemplarCount;
            int loanCount = book.UsageCount;

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

        static void ProcessKsu(string ksu)
        {
            WriteLine($"KSU={ksu}");
            var records = connection.SearchRead("\"NKSU={0}\"", ksu);
            WriteLine($"\trecords={records.Length}");
            foreach (var record in records)
            {
                try
                {
                    ProcessBook(record, ksu);
                }
                catch (Exception exception)
                {
                    WriteLine($"MFN={record.Mfn}: {exception.Message}");
                }
            }
        }

        static void ProcessYear(int year, params int[] lines)
        {
            WriteLine($"YEAR: {year}");
            foreach (int line in lines)
            {
                string ksu = $"{year.ToInvariantString()}/{line.ToInvariantString()}";
                ProcessKsu(ksu);
            }

            WriteLine();
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

        static void Main()
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
                    ProcessYear(2016, 38, 43, 45, 47, 48, 49, 53, 56, 62, 71, 73, 79, 80, 82, 90, 93, 97);
                    ProcessYear(2017, 51, 61, 65, 72, 73, 78, 83, 87, 92, 103, 112, 114, 115, 116, 118);
                    ProcessYear(2018, 20, 23, 30, 40, 49, 50, 56, 90, 93, 95);
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
            WriteLine($"Elapsed={elapsed.ToAutoString()}");
        }
    }
}
