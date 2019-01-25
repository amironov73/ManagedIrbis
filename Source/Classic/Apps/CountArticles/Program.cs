// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConsoleUtility.cs -- useful routines for console manipulation
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Configuration;
using AM.Text;

using DevExpress.Spreadsheet;

using ManagedIrbis;
using ManagedIrbis.Magazines;

#endregion

// ReSharper disable LocalizableElement

namespace CountArticles
{
    static class Program
    {
        private static IrbisConnection connection;
        private static Workbook workbook;
        private static Worksheet worksheet;
        private static MagazineManager magazineManager;
        private static MagazineInfo[] magazines;
        private static string threshold = "2011";

        private static int currentRow;

        static Cell WriteCell(int column, string text)
        {
            Cell result = worksheet.Cells[currentRow, column];
            if (int.TryParse(text, out int value))
            {
                result.Value = value;
            }
            else
            {
                result.Value = text;
            }

            return result;
        }

        static Cell WriteCell(int column, int value)
        {
            Cell result = worksheet.Cells[currentRow, column];
            if (value != 0)
            {
                result.Value = value;
            }

            return result;
        }

        static Cell TextColor(this Cell cell, Color color)
        {
            cell.Font.Color = color;

            return cell;
        }

        static Cell Center(this Cell cell)
        {
            cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            cell.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

            return cell;
        }

        static Cell Bold(this Cell cell)
        {
            cell.Font.Bold = true;

            return cell;
        }

        static Cell RightJustify(this Cell cell)
        {
            cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;

            return cell;
        }

        static Cell SetBorders(this Cell cell)
        {
            cell.Borders.SetAllBorders(Color.DarkBlue, BorderLineStyle.Thin);

            return cell;
        }

        static Cell Background(this Cell cell, Color color)
        {
            cell.FillColor = color;

            return cell;
        }

        static bool HaveIssues(MagazineIssueInfo[] issues, string year, string number)
        {
            foreach (var issue in issues)
            {
                if (issue.Year == year && issue.Number == number)
                {
                    return true;
                }
            }

            return false;
        }

        static int CountInnerArticles(MagazineIssueInfo[] issues, string year, string number)
        {
            int result = 0;

            foreach (var issue in issues)
            {
                if (issue.Year == year && issue.Number == number)
                {
                    int? delta = issue.Articles?.Length;
                    result += delta ?? 0;
                }
            }

            return result;
        }

        static int CountExternalArticles(MagazineIssueInfo[] issues, string year, string number)
        {
            int result = 0;

            foreach (var issue in issues)
            {
                if (issue.Year == year && issue.Number == number)
                {
                    int delta = magazineManager.CountExternalArticles(issue);
                    result += delta;
                }
            }

            return result;
        }

        static void ProcessMagazine(MagazineInfo magazine)
        {
            Color haveIssuesBackColor = Color.FromArgb(220, 255, 255);
            Color headerBackColor = Color.FromArgb(180, 180, 255);
            string title = magazine.ExtendedTitle;

            Console.WriteLine($"[{magazine.Mfn}] [{magazine.Index}] {title}");

            MagazineIssueInfo[] issues = magazineManager.GetIssues(magazine);
            issues = issues
                .Where(i => !string.IsNullOrEmpty(i.Year) && !string.IsNullOrEmpty(i.Number))
                .Where(i => i.Year.SafeCompare(threshold) >= 0)
                .Where(i => !i.IsBinding)
                .ToArray();
            Console.WriteLine($"\tIssues: {issues.Length}");
            if (issues.Length == 0)
            {
                return;
            }

            WriteCell(0, title).Bold().TextColor(Color.Blue);
            currentRow++;

            string[] numbers = issues.Select(i => i.Number).Distinct().ToArray();
            numbers = NumberText.Sort(numbers).ToArray();
            WriteCell(0, "").Background(headerBackColor);
            for(int i = 0; i < numbers.Length; i++)
            {
                string number = numbers[i];
                WriteCell(1 + i, number).Bold().SetBorders().Background(headerBackColor).RightJustify();
            }
            currentRow++;

            foreach (var grouped in issues.GroupBy(i => i.Year))
            {
                string year = grouped.Key;
                Console.Write($"\t{year}>");
                Range range = worksheet.Range.FromLTRB(0, currentRow, 0, currentRow + 1);
                worksheet.MergeCells(range);
                WriteCell(0, year).Bold().SetBorders().Background(headerBackColor).Center();

                // Inner articles
                for (int i = 0; i < numbers.Length; i++)
                {
                    string number = numbers[i];
                    bool haveIssues = HaveIssues(issues, year, number);
                    if (haveIssues)
                    {
                        Console.Write($" {number}");
                    }
                    int count = haveIssues ? CountInnerArticles(issues, year, number) : 0;
                    Cell cell = WriteCell(1 + i, count).SetBorders().TextColor(Color.Red);
                    if (haveIssues)
                    {
                        cell.Background(haveIssuesBackColor);
                    }
                }

                Console.Write(" |||");
                currentRow++;

                // External articles
                for (int i = 0; i < numbers.Length; i++)
                {
                    string number = numbers[i];
                    bool haveIssues = HaveIssues(issues, year, number);
                    if (haveIssues)
                    {
                        Console.Write($" {number}");
                    }
                    int count = haveIssues ? CountExternalArticles(issues, year, number) : 0;
                    Cell cell = WriteCell(1 + i, count).SetBorders().TextColor(Color.Blue);
                    if (haveIssues)
                    {
                        cell.Background(haveIssuesBackColor);
                    }
                }

                Console.WriteLine();
                currentRow++;
            }

            currentRow++;
        }

        static MagazineInfo[] FilterMagazines()
        {
            List<MagazineInfo> result = new List<MagazineInfo>(magazines.Length);
            foreach (MagazineInfo magazine in magazines)
            {
                if (string.IsNullOrEmpty(magazine.Title)
                    || string.IsNullOrEmpty(magazine.Index))
                {
                    continue;
                }

                string worklist = magazine.Record?.FM(920);
                if (worklist != "J")
                {
                    continue;
                }

                result.Add(magazine);
            }

            return result.ToArray();
        }

        static void Main()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                threshold = ConfigurationUtility.GetString("threshold", threshold);

                workbook= new Workbook();
                worksheet = workbook.Worksheets[0];

                using (connection = IrbisConnectionUtility.GetClientFromConfig())
                {
                    Console.WriteLine("Connected");
                    magazineManager = new MagazineManager(connection);
                    magazines = magazineManager.GetAllMagazines();
                    Console.WriteLine($"Magazines loaded: {magazines.Length}");
                    magazines = FilterMagazines();
                    Console.WriteLine($"Magazines remaining: {magazines.Length}");
                    magazines = magazines.OrderBy(m => m.Title).ToArray();
                    // magazines = magazines.Take(60).ToArray();

                    foreach (MagazineInfo magazine in magazines)
                    {
                        try
                        {
                            ProcessMagazine(magazine);
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception.Message);
                            currentRow++;
                            WriteCell(0, exception.Message).Bold().TextColor(Color.Red);
                            currentRow++;
                        }
                    }
                }

                Console.WriteLine("Disconnected");

                Console.WriteLine("Saving...");
                workbook.SaveDocument("Articles.xlsx");
                Console.WriteLine("Saved");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            stopwatch.Stop();
            TimeSpan elapsed = stopwatch.Elapsed;
            Console.WriteLine($"Elapsed: {elapsed}");
        }
    }
}
