// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

using AM;

using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Charts;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Fields;
using ManagedIrbis.Infrastructure;

using static System.Console;

#endregion

// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement

namespace Alligator
{
    static class Program
    {
        const int NumberOfSections = 15;

        private static readonly string connectionString
            = "host=127.0.0.1;port=6666;user=librarian;password=secret;db=IBIS;arm=C;";
        private static readonly string[] fonds =
        {
            "Ф201", "Ф202", "Ф302", "Ф303", "Ф404",
            "Ф501", "Ф502", "Ф503", "Ф504", "Ф505", "Ф506"
        };
        private static IrbisConnection connection;
        private static IrbisProvider provider;
        private static List<EffectiveStat> stat;

        private static Workbook workbook;
        private static Worksheet worksheet;
        private static int currentLine;

        static void ProcessBook(MarcRecord record, string ksu)
        {
            BookInfo book = new BookInfo(provider, record);
            ExemplarInfo[] selected = book.Exemplars.Where
                (
                    ex => ex.KsuNumber1.SameString(ksu)
                )
                .ToArray();

            if (selected.Length == 0)
            {
                return;
            }

            EffectiveStat result = new EffectiveStat
            {
                Description = connection.FormatRecord("@sbrief", record.Mfn),
                TitleCount = 1
            };

            int totalExemplars = 0;
            foreach (ExemplarInfo exemplar in book.Exemplars)
            {
                int amount = exemplar.Amount.SafeToInt32();
                if (amount == 0)
                {
                    amount = 1;
                }

                totalExemplars += amount;
            }

            List<string> siglas = new List<string>();

            foreach (ExemplarInfo exemplar in selected)
            {
                DateTime date = IrbisDate.ConvertStringToDate(exemplar.Date);
                if (date != DateTime.MinValue)
                {
                    if (result.Date == DateTime.MinValue)
                    {
                        result.Date = date;
                    }
                    else if (date < result.Date)
                    {
                        result.Date = date;
                    }
                }

                int amount = exemplar.Amount.SafeToInt32();
                if (amount == 0)
                {
                    amount = 1;
                }

                result.ExemplarCount += amount;

                decimal price = exemplar.Price.SafeToDecimal(0);
                if (price == 0)
                {
                    price = book.Price;
                }

                siglas.Add(exemplar.Place);

                result.TotalCost += amount * price;
            }

            decimal loanCount = book.UsageCount;
            if (result.ExemplarCount != totalExemplars)
            {
                loanCount = loanCount * result.ExemplarCount / totalExemplars;
            }

            result.Bbk = record.FM(621).SafeSubstring(0, 2);
            result.KnowledgeSection = record.FM(60) ?? "?";

            result.LoanCount = (int)loanCount;
            result.Sigla = string.Join(" ", siglas.Distinct()
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.ToUpperInvariant()));

            double years = (DateTime.Today - result.Date).Days / 365.0;
            result.Speed = result.LoanCount / years;

            stat.Add(result);
        }

        static void ProcessKsu(string ksu)
        {
            WriteLine($"KSU={ksu}");
            var records = connection.SearchRead("\"NKSU={0}\"", ksu);
            WriteLine($"\trecords={records.Length}");
            foreach (var record in records)
            {
                ProcessBook(record, ksu);
            }
        }

        static bool FilterFond(EffectiveStat est, string fond)
        {
            if (string.IsNullOrEmpty(est.Sigla))
            {
                return false;
            }

            return est.Sigla.Contains(fond);
        }

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

        static void WriteBook(EffectiveStat book, Color color)
        {
            WriteCell(0, book.Description)
                .SetBorders().Background(color);
            WriteCell(1, book.Date.ToShortDateString())
                .SetBorders().Background(color);
            WriteCell(2, book.Bbk)
                .SetBorders().Background(color);
            WriteCell(3, book.KnowledgeSection)
                .SetBorders().Background(color);
            WriteCell(4, book.LoanCount)
                .SetBorders().Background(color);
            WriteCell(5, book.Speed)
                .SetBorders().Background(color);
        }

        static void ProcessSelection(string name, Color color, EffectiveStat[] selected)
        {
            if (selected.Length == 0)
            {
                return;
            }

            currentLine += 1;
            WriteCell(0, name).Bold();
            currentLine++;
            selected = selected.OrderByDescending(s => s.Speed).ToArray();
            foreach (var book in selected)
            {
                WriteBook(book, color);
                currentLine++;
            }
        }

        static CellValue[] CountSections(EffectiveStat[] books)
        {
            var result = new CellValue[NumberOfSections];
            for (int section = 1; section <= NumberOfSections; section++)
            {
                string text = section.ToInvariantString();
                result[section - 1] = books.Count(b => b.KnowledgeSection == text);
            }

            return result;
        }

        static void ProcessFond(int year, string fond)
        {
            fond = fond.ToUpperInvariant();
            WriteLine($"FOND={fond}");
            var filtered = stat.Where(s => FilterFond(s, fond)).ToArray();
            WriteLine($"\trecords={filtered.Length}");
            if (filtered.Length == 0)
            {
                return;
            }

            var name = $"{year.ToInvariantString()}-{fond}";
            worksheet = workbook.Worksheets.Add(name);
            worksheet.Columns[0].Width = 150;
            worksheet.Columns[1].Width = 20;
            worksheet.Columns[2].Width = 10;
            worksheet.Columns[3].Width = 12;
            worksheet.Columns[4].Width = 12;
            worksheet.Columns[5].Width = 12;
            currentLine = 16;

            var lightGray = Color.FromArgb(200, 200, 200);
            var lightRed = Color.FromArgb(255, 200, 200);

            WriteCell(0, "Библиографическое описание")
                .Bold().Background(lightGray).SetBorders();
            WriteCell(1, "Дата")
                .Bold().Background(lightGray).SetBorders();
            WriteCell(2, "ББК")
                .Bold().Background(lightGray).SetBorders();
            WriteCell(3, "Разд.")
                .Bold().Background(lightGray).SetBorders();
            WriteCell(4, "Выд.")
                .Bold().Background(lightGray).SetBorders();
            WriteCell(5, "V")
                .Bold().Background(lightGray).SetBorders();
            worksheet.FreezeRows(currentLine);
            currentLine++;

            var poorBooks = filtered.Where(b => b.Speed < 2.0).ToArray();
            var moderateBooks = filtered.Where
                (
                    b => (b.Speed < 7.0) && (b.Speed >= 2.0)
                ).ToArray();
            var goodBooks = filtered.Where(b => b.Speed >= 7.0).ToArray();


            ProcessSelection("Лидеры", Color.LightGreen, goodBooks);
            ProcessSelection("Аутсайдеры", lightRed, poorBooks);
            ProcessSelection("Середняки", Color.Yellow, moderateBooks);

            Chart chart = worksheet.Charts.Add(ChartType.ColumnStacked);
            chart.TopLeftCell = worksheet.Cells[0, 0];
            chart.BottomRightCell = worksheet.Cells[14, 0];
            chart.Title.SetValue($"{fond} в {year} году");
            chart.Title.Font.Size = 12.0;
            chart.Title.Visible = true;
            chart.Legend.Visible = false;
            chart.Legend.Position = LegendPosition.Right;

            var arguments = ChartData.FromArray
                (
                    Enumerable.Range(1, NumberOfSections)
                        .Select(i => CellValue.FromObject(i))
                        .ToArray()
                );
            chart.Series.Add
                (
                    "Аутсайдеры",
                    arguments,
                    ChartData.FromArray(CountSections(poorBooks))
                );
            chart.Series[0].Fill.SetSolidFill(lightRed);

            chart.Series.Add
                (
                    "Середняки",
                    arguments,
                    ChartData.FromArray(CountSections(moderateBooks))
                );
            chart.Series[1].Fill.SetSolidFill(Color.Yellow);

            chart.Series.Add
                (
                    "Лидеры",
                    arguments,
                    ChartData.FromArray(CountSections(goodBooks))
                );
            chart.Series[2].Fill.SetSolidFill(Color.LightGreen);

            chart.Views[0].GapWidth = 15;
            var title = chart.PrimaryAxes[0].Title;
            title.SetValue("Разделы знаний");
            title.Visible = true;
            title = chart.PrimaryAxes[1].Title;
            title.SetValue("Кол-во экземпляров");
            title.Visible = true;
        }

        static void ProcessYear(int year, params int[] lines)
        {
            WriteLine($"YEAR: {year}");
            stat = new List<EffectiveStat>();
            foreach (int line in lines)
            {
                string ksu = $"{year.ToInvariantString()}/{line.ToInvariantString()}";
                ProcessKsu(ksu);
            }

            WriteLine();

            foreach (var fond in fonds)
            {
                ProcessFond(year, fond);
            }

            WriteLine();
        }

        static void SetDirectory()
        {
            worksheet = workbook.Worksheets[0];
            worksheet.Name = "Справочник";
            worksheet.Columns[1].Width = 150;
            currentLine = 0;
            WriteCell(0, "Разделы знаний").Bold();
            currentLine++; currentLine++;

            var specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    connection.Database,
                    "rzn.mnu"
                );
            var menu = connection.ReadMenu(specification);
            foreach (var entry in menu.Entries)
            {
                WriteCell(0, entry.Code).Bold();
                WriteCell(1, entry.Comment);
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

                using (connection = new IrbisConnection(connectionString))
                {
                    SetDirectory();

                    provider = new ConnectedClient(connection);
                    ProcessYear(2016, 38, 43, 45, 47, 48, 49, 53, 56, 62, 71, 73, 79, 80, 82, 90, 93, 97);
                    ProcessYear(2017, 51, 61, 65, 72, 73, 78, 83, 87, 92, 103, 112, 114, 115, 116, 118);
                    ProcessYear(2018, 20, 23, 30, 40, 49, 50, 90, 93, 95);
                }

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
