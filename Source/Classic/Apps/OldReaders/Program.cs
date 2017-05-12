// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

using AM;
using AM.Configuration;

using DevExpress.Spreadsheet;

using ManagedIrbis;
using ManagedIrbis.Readers;

#endregion

namespace OldReaders
{
    internal class Program
    {
        // Наименование библиотеки
        private const string LibraryName = "иогунб";

        // Имя результирующего файла
        private const string OutputFile = "OldReaders.xlsx";

        private const string Litres = "литре";

        private static BlockingCollection<ReaderInfo> OldReaders;

        private static DateTime Threshold;

        private static void Main()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                string connectionString = ConfigurationUtility
                    .GetString("connectionString")
                    .ThrowIfNull("connectionString not set");
                Threshold = IrbisDate.ConvertStringToDate
                    (
                        ConfigurationUtility
                            .GetString("threshold")
                            .ThrowIfNull("threshold not set")
                    );

                Console.WriteLine("Loading readers");

                List<ReaderInfo> allReaders = new List<ReaderInfo>();
                OldReaders = new BlockingCollection<ReaderInfo>();

                using (IrbisConnection connection
                    = new IrbisConnection(connectionString))
                {
                    ReaderManager manager = new ReaderManager(connection)
                    {
                        OmitDeletedRecords = true
                    };
                    manager.BatchRead += (obj, ea) => Console.Write(".");

                    string[] databases = ConfigurationUtility.GetString("databases")
                        .ThrowIfNull("databases not specified")
                        .Split
                        (
                            new[] { ' ', ';', ',' },
                            StringSplitOptions.RemoveEmptyEntries
                        );

                    foreach (string database in databases)
                    {
                        Console.WriteLine
                            (
                                "Database: {0}, records: {1}",
                                database,
                                connection.GetMaxMfn(database) - 1
                            );

                        allReaders.AddRange
                            (
                                manager.GetAllReaders(database)
                            );

                        Console.WriteLine();
                    }

                }

                WriteDelimiter();

                Console.WriteLine("Merging");
                Console.WriteLine("Records before merging: {0}", allReaders.Count);

                allReaders = ReaderManager.MergeReaders(allReaders);

                Console.WriteLine("Records after merging: {0}", allReaders.Count);
                WriteDelimiter();

                Console.WriteLine("Filtering");

                ParallelOptions options = new ParallelOptions
                {
                    MaxDegreeOfParallelism = 4
                };
                Parallel.ForEach(allReaders, options, ProcessReader);

                ReaderInfo[] oldReaders = OldReaders.ToArray();

                WriteDelimiter();

                Console.WriteLine("Sorting");

                oldReaders = oldReaders.OrderBy
                    (
                        reader => reader.FullName
                    )
                    .ToArray();

                WriteDelimiter();

                Console.WriteLine
                    (
                        "Create table: {0} lines",
                        oldReaders.Length
                    );

                Workbook workbook = new Workbook();
                workbook.CreateNewDocument();
                Worksheet worksheet = workbook.Worksheets[0];

                int row = 0;

                worksheet.Cells[row, 0].Value = "ФИО";
                worksheet.Cells[row, 1].Value = "Билет";
                worksheet.Cells[row, 2].Value = "Регистрация";
                worksheet.Cells[row, 3].Value = "Кол-во";
                worksheet.Cells[row, 4].Value = "Последнее событие";
                worksheet.Cells[row, 5].Value = "Отделы";

                DrawBorders(worksheet, row);

                row++;

                for (int i = 0; i < oldReaders.Length; i++)
                {
                    if (i % 100 == 0)
                    {
                        Console.Write(".");
                    }

                    ReaderInfo reader = oldReaders[i];
                    string lastDate = (string)reader.UserData;
                    if (string.IsNullOrEmpty(lastDate))
                    {
                        lastDate = "--";
                    }

                    string departments = StringUtility.Join
                        (
                            ", ",
                            reader.Registrations
                                .Select(reg => reg.Chair)
                                .Concat
                                    (
                                        reader.Visits
                                            .Select(visit => visit.Department)
                                    )
                                .NonEmptyLines()
                                .Distinct()
                        );

                    worksheet.Cells[row, 0].Value = reader.FullName;
                    worksheet.Cells[row, 1].Value = reader.Ticket;
                    worksheet.Cells[row, 2].Value = reader.RegistrationDate
                        .ToShortDateString();
                    worksheet.Cells[row, 3].Value = reader.Visits.Length
                                                    + reader.Registrations.Length;
                    worksheet.Cells[row, 4].Value = lastDate;
                    worksheet.Cells[row, 5].Value = departments;

                    DrawBorders(worksheet, row);

                    row++;
                }

                WriteDelimiter();

                workbook.SaveDocument(OutputFile);

                Console.WriteLine("All done");

                stopwatch.Stop();
                TimeSpan elapsed = stopwatch.Elapsed;
                Console.WriteLine("Elapsed: {0}", elapsed);
                Console.WriteLine("Old readers: {0}", oldReaders.Length);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private static void ProcessReader
            (
                ReaderInfo reader
            )
        {
            string ticket = reader.Ticket;
            if (string.IsNullOrEmpty(ticket))
            {
                return;
            }
            ticket = ticket.ToLower();
            if (ticket.Contains(Litres))
            {
                return;
            }

            string workplace = reader.WorkPlace;
            if (!string.IsNullOrEmpty(workplace))
            {
                workplace = workplace.ToLower();
                if (workplace.Contains(LibraryName))
                {
                    return;
                }
            }

            VisitInfo[] debt = reader.Visits
                .GetLoans()
                .GetDebt();
            if (debt.Length != 0)
            {
                return;
            }

            VisitInfo lastEvent = null;
            if (reader.Visits.Length != 0)
            {
                VisitInfo[] visits = reader.Visits;
                string maxDate = visits[0].DateGivenString;
                lastEvent = visits[0];

                for (int i = 1; i < visits.Length; i++)
                {
                    string date = visits[i].DateGivenString;
                    if (string.CompareOrdinal(date, maxDate) > 0)
                    {
                        maxDate = date;
                        lastEvent = visits[i];
                    }
                }
            }

            if (ReferenceEquals(lastEvent, null))
            {
                ReaderRegistration lastRegistration
                    = GetLastRegistration(reader.Registrations);

                if (!ReferenceEquals(lastRegistration, null))
                {
                    if (lastRegistration.Date >= Threshold)
                    {
                        return;
                    }
                    reader.UserData = lastRegistration.Date
                        .ToShortDateString() + " перерегистрация";
                }
                else
                {
                    lastRegistration
                        = GetLastRegistration(reader.Enrollment);

                    if (!ReferenceEquals(lastRegistration, null))
                    {
                        if (lastRegistration.Date >= Threshold)
                        {
                            return;
                        }
                        reader.UserData = lastRegistration.Date
                            .ToShortDateString() + " регистрация";
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                if (lastEvent.DateGiven >= Threshold)
                {
                    return;
                }
                reader.UserData = lastEvent.DateGiven
                    .ToShortDateString() + " посещение";
            }

            OldReaders.Add(reader);
            Console.Write(".");
        }

        private static void DrawBorders
            (
                Worksheet worksheet,
                int row
            )
        {
            for (int j = 0; j <= 5; j++)
            {
                Cell cell = worksheet.Cells[row, j];
                cell.Borders.SetAllBorders
                    (
                        Color.Black,
                        BorderLineStyle.Thin
                    );
            }
        }

        private static void WriteDelimiter()
        {
            Console.WriteLine();
            Console.WriteLine(new string('#', 70));
            Console.WriteLine();
        }

        private static ReaderRegistration GetLastRegistration
            (
                ReaderRegistration[] registrations
            )
        {
            ReaderRegistration result = null;

            if (registrations.Length != 0)
            {
                string maxDate = registrations[0].DateString;
                result = registrations[0];

                for (int i = 1; i < registrations.Length; i++)
                {
                    string date = registrations[i].DateString;
                    if (string.CompareOrdinal(date, maxDate) > 0)
                    {
                        maxDate = date;
                        result = registrations[i];
                    }
                }
            }

            return result;
        }
    }
}
