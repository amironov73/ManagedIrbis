using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

using DevExpress.Spreadsheet;

using AM;
using AM.Configuration;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Fields;
using ManagedIrbis.Readers;

namespace MaliciousDebtors
{
    class Program
    {
        //
        // Общие настройки
        //

        // Наименование библиотеки
        private const string LibraryName = "ИОГУНБ";

        // Формат для биб. описания
        private const string FormatName = "@brief";

        // Имя результирующего файла
        private const string OutputFile = "Debtors.xlsx";

        private static void Main()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                string connectionString = ConfigurationUtility
                    .GetString("connectionString")
                    .ThrowIfNull("connectionString not set");

                int delay = ConfigurationUtility
                    .GetInt32("delay");

                DateTime threshold = DateTime.Today
                    .AddMonths(-delay);

                using (IrbisConnection connection 
                    = new IrbisConnection(connectionString))
                {
                    DatabaseInfo[] databases 
                        = connection.ListDatabases();

                    DebtorManager manager
                        = new DebtorManager(connection)
                        {
                            ToDate = threshold
                        };
                    manager.BatchRead += (sender, args) =>
                    {
                        Console.Write(".");
                    };
                    DebtorInfo[] debtors = manager.GetDebtors
                        (
                            connection.Search("RB=$")
                        );
                    debtors = debtors.Where
                        (
                            debtor => !debtor.WorkPlace
                            .SafeContains(LibraryName)
                        )
                        .ToArray();
                    Console.WriteLine();
                    Console.WriteLine
                        (
                            "Debtors: {0}",
                            debtors.Length
                        );

                    VisitInfo[] allDebt = debtors.SelectMany
                        (
                            debtor => debtor.Debt
                        )
                        .ToArray();
                    Console.WriteLine
                        (
                            "Books in debt: {0}",
                            allDebt.Length
                        );


                    Workbook workbook = new Workbook();
                    workbook.CreateNewDocument();
                    Worksheet worksheet = workbook.Worksheets[0];

                    int row = 0;

                    worksheet.Cells[row, 0].Value = "ФИО";
                    worksheet.Cells[row, 1].Value = "Билет";
                    worksheet.Cells[row, 2].Value = "Краткое описание";
                    worksheet.Cells[row, 3].Value = "Год";
                    worksheet.Cells[row, 4].Value = "Номер";
                    worksheet.Cells[row, 5].Value = "Цена";
                    worksheet.Cells[row, 6].Value = "Хранение";
                    worksheet.Cells[row, 7].Value = "Дата";
                    worksheet.Cells[row, 8].Value = "Отдел";

                    row++;

                    for (int i = 0; i < allDebt.Length; i++)
                    {
                        if (i % 100 == 0)
                        {
                            Console.Write(".");
                        }

                        VisitInfo debt = allDebt[i];

                        string description = debt.Description;
                        string inventory = debt.Inventory;
                        string database = debt.Database;
                        string year = string.Empty;
                        string index = debt.Index;
                        string price = string.Empty;

                        if (!string.IsNullOrEmpty(index)
                            && !string.IsNullOrEmpty(database))
                        {
                            if (databases.FirstOrDefault
                                (
                                    db => db.Name.SameString(database)
                                )
                                == null)
                            {
                                continue;
                            }

                            try
                            {
                                connection.Database = database;
                                MarcRecord record
                                    = connection.SearchReadOneRecord
                                    (
                                        "\"I={0}\"",
                                        index
                                    );
                                if (!ReferenceEquals(record, null))
                                {
                                    description = connection.FormatRecord
                                    (
                                        FormatName,
                                        record.Mfn
                                    );
                                    year = GetYear(record);
                                    price = GetPrice(debt, record);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        worksheet.Cells[row, 0].Value = debt.Reader.FullName;
                        worksheet.Cells[row, 1].Value = debt.Reader.Ticket;
                        worksheet.Cells[row, 2].Value = description;
                        worksheet.Cells[row, 3].Value = year;
                        worksheet.Cells[row, 4].Value = inventory;
                        worksheet.Cells[row, 5].Value = price;
                        worksheet.Cells[row, 6].Value = debt.Sigla;
                        worksheet.Cells[row, 7].Value = debt.DateExpectedString;
                        worksheet.Cells[row, 8].Value = debt.Department;

                        for (int j = 0; j <= 6; j++)
                        {
                            Cell cell = worksheet.Cells[row, j];
                            cell.Borders.SetAllBorders
                                (
                                    Color.Black, 
                                    BorderLineStyle.Hair
                                );
                        }

                        row++;
                    }

                    workbook.SaveDocument(OutputFile);

                    Console.WriteLine("All done");

                    stopwatch.Stop();
                    TimeSpan elapsed = stopwatch.Elapsed;
                    Console.WriteLine("Elapsed: {0}", elapsed);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static string GetYear
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            string result = record.FM("210", 'd')
                ?? record.FM("934");

            return result;
        }

        private static string GetPrice
            (
                [NotNull] VisitInfo debt,
                [NotNull] MarcRecord bookRecord
            )
        {
            Code.NotNull(debt, "debt");

            string inventory = debt.Inventory;
            string barcode = debt.Barcode;

            RecordField[] fields = bookRecord.Fields
                .GetField("910");

            string result = null;

            foreach (RecordField field in fields)
            {
                ExemplarInfo exemplar = ExemplarInfo.Parse(field);

                if (!string.IsNullOrEmpty(inventory))
                {
                    if (exemplar.Number.SameString(inventory))
                    {
                        if (!string.IsNullOrEmpty(barcode))
                        {
                            if (exemplar.Barcode.SameString(barcode))
                            {
                                result = exemplar.Price;
                                break;
                            }
                        }
                        else
                        {
                            result = exemplar.Price;
                            break;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                result = bookRecord.FM("10", 'd');
            }

            return result;
        }
    }
}
