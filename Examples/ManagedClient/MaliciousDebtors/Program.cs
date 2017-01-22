using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevExpress.Spreadsheet;

using AM;
using AM.Configuration;

using ManagedIrbis;
using ManagedIrbis.Readers;

namespace MaliciousDebtors
{
    class Program
    {
        private static void Main()
        {
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
                    DebtorInfo[] debtors = manager.GetDebtors();
                    debtors = debtors.Where
                        (
                            debtor => !debtor.WorkPlace.SafeContains("ИОГУНБ")
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

                    worksheet.Columns[0].Width = 200;

                    worksheet.Cells[row, 0].Value = "Краткое описание";
                    worksheet.Cells[row, 1].Value = "Год";
                    worksheet.Cells[row, 2].Value = "Номер";
                    worksheet.Cells[row, 3].Value = "Цена";
                    worksheet.Cells[row, 4].Value = "Хранение";
                    worksheet.Cells[row, 5].Value = "Дата";
                    worksheet.Cells[row, 6].Value = "Отдел";

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
                                ) == null)
                            {
                                continue;
                            }

                            try
                            {
                                connection.Database = database;
                                int[] found = connection.Search
                                (
                                    "\"I={0}\"",
                                    index
                                );
                                if (found.Length == 1)
                                {
                                    int mfn = found[0];
                                    description = connection.FormatRecord
                                    (
                                        "@sbrief",
                                        mfn
                                    );
                                    year = connection.FormatRecord
                                    (
                                        "&uf('Av210^d#1'),v934",
                                        mfn
                                    );
                                    price = connection.FormatRecord
                                    (
                                        "if p(v10^d) then v10^d else &uf('Av910^e#1') fi",
                                        mfn
                                    );
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        worksheet.Cells[row, 0].Value = description;
                        worksheet.Cells[row, 1].Value = year;
                        worksheet.Cells[row, 2].Value = inventory;
                        worksheet.Cells[row, 3].Value = price;
                        worksheet.Cells[row, 4].Value = debt.Sigla;
                        worksheet.Cells[row, 5].Value = debt.DateExpectedString;
                        worksheet.Cells[row, 6].Value = debt.Department;

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

                    workbook.SaveDocument("Debtors.xlsx");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
