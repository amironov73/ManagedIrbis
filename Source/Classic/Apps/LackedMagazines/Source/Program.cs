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
using System.IO;
using System.Linq;

using AM;
using AM.Text;

using DevExpress.Spreadsheet;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Fields;
using ManagedIrbis.Magazines;

#endregion

namespace LackedMagazines
{
    using Properties;

    class Program
    {
        private static IrbisConnection _connection;
        private static MagazineManager _manager;
        private static TextWriter _writer;
        private static string _year;
        private static string _period;
        private static List<PeriodInfo> _list;
        private static Workbook workbook;
        private static Worksheet worksheet;
        private static int currentRow;

        static Cell WriteCell(int column, string text)
        {
            Cell result = worksheet.Cells[currentRow, column];
            //if (int.TryParse(text, out int value))
            //{
            //    result.Value = value;
            //}
            //else
            //{
                result.Value = text;
            //}

            return result;
        }

        private static bool Match
            (
                [CanBeNull] string issue,
                [NotNull] NumberText required
            )
        {
            if (string.IsNullOrEmpty(issue))
            {
                return false;
            }
            if (issue.Contains('/'))
            {
                string[] parts = issue.Split('/');
                if (parts.Length < 2)
                {
                    return false;
                }
                NumberText first = parts[0];
                NumberText second = parts[1];

                return first <= required && second >= required;
            }

            return required == issue;
        }

        [NotNull]
        private static string CompressIfPossible
            (
                [NotNull] IEnumerable<string> numbers
            )
        {
            string[] array1 = numbers.ToArray();
            if (array1.All(n => n.IsPositiveInteger()))
            {
                int[] array2 = array1.Select(NumericUtility.ParseInt32).ToArray();
                Array.Sort(array2);

                return NumericUtility.CompressRange(array2);
            }

            return StringUtility.Join(", ", array1);
        }

        private static void ProcessMagazine
            (
                [NotNull] MagazineInfo magazine
            )
        {
            Console.WriteLine(magazine);
            if (ReferenceEquals(magazine.QuarterlyOrders, null))
            {
                Console.WriteLine(Resources.HaventOrders);
                return;
            }

            QuarterlyOrderInfo order = magazine.QuarterlyOrders.FirstOrDefault
                (
                    o => o.Period.SameString(_period)
                );
            if (ReferenceEquals(order, null)
                || string.IsNullOrEmpty(order.FirstIssue)
                || string.IsNullOrEmpty(order.LastIssue))
            {
                Console.WriteLine(Resources.NoOrderInfoFor, _period);
                return;
            }

            MagazineIssueInfo[] issues = _manager.GetIssues(magazine, _year);

            string[] registered = issues.Select(i => i.Number).ToArray();
            registered = NumberText.Sort(registered).ToArray();

            NumberText first = order.FirstIssue;
            NumberText current = first.Clone();
            NumberText last = order.LastIssue;

            PeriodInfo info = new PeriodInfo
            {
                Title = magazine.ExtendedTitle,
                Expected = first + "-" + last,
                Registered = CompressIfPossible(registered)
            };
            _list.Add(info);

            List<string> missingIssues = new List<string>();
            while (current <= last)
            {
                MagazineIssueInfo found = issues.FirstOrDefault
                    (
                        i => Match(i.Number, current)
                    );
                if (ReferenceEquals(found, null))
                {
                    missingIssues.Add(current.ToString());
                }

                current = current.Increment();
            }

            info.Missing = CompressIfPossible(missingIssues);

            Console.WriteLine(Resources.Present, info.Registered);
            Console.WriteLine(Resources.Missing, info.Missing);
        }

        static void PrintMagazine
            (
                [NotNull] PeriodInfo info
            )
        {
            WriteCell(0, info.Title);
            WriteCell(1, info.Expected);
            WriteCell(2, info.Registered);
            WriteCell(3, info.Missing);
            currentRow++;

            //_writer.WriteLine("<tr>");
            //_writer.WriteLine("<td>");
            //_writer.WriteLine(HtmlText.Encode(info.Title));
            //_writer.WriteLine("</td>");
            //_writer.WriteLine("<td>");
            //_writer.WriteLine(HtmlText.Encode(info.Expected));
            //_writer.WriteLine("</td>");
            //_writer.WriteLine("<td>");
            //_writer.WriteLine(HtmlText.Encode(info.Registered));
            //_writer.WriteLine("</td>");
            //_writer.WriteLine("<td>");
            //_writer.WriteLine(HtmlText.Encode(info.Missing));
            //_writer.WriteLine("</td>");
            //_writer.WriteLine("</tr>");
        }

        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                return;
            }

            string connectionString = args[0];
            _period = args[1];
            string fileName = args[2];

            try
            {
                _list = new List<PeriodInfo>();
                _year = _period.Substring(0, 4);

                //using (_writer = new StreamWriter(fileName))
                using (_connection = new IrbisConnection(connectionString))
                {
                    _manager = new MagazineManager(_connection);
                    MagazineInfo[] allMagazines = _manager.GetAllMagazines();
                    foreach (MagazineInfo magazine in allMagazines)
                    {
                        ProcessMagazine(magazine);
                    }

                    _list = _list.OrderBy(i => i.Title).ToList();

                    workbook= new Workbook();
                    worksheet = workbook.Worksheets[0];
                    WriteCell(0, "Заглавие");
                    WriteCell(1, "Ожидались");
                    WriteCell(2, "Поступили");
                    WriteCell(3, "Отсутствуют");
                    currentRow++;
                    foreach (PeriodInfo info in _list)
                    {
                        PrintMagazine(info);
                    }

                    Console.Write("Saving... ");
                    workbook.SaveDocument(fileName);
                    Console.WriteLine("saved");

                    //_writer.WriteLine("<h3>");
                    //_writer.WriteLine(Resources.MagazineRegistrationForPeriod);
                    //_writer.WriteLine(HtmlText.Encode(_period));
                    //_writer.WriteLine("</h3>");
                    //_writer.WriteLine("<table border='1' cellpadding='3' cellspacing='0'>");
                    //_writer.WriteLine("<tr>");
                    //_writer.WriteLine(Resources.MagazineTitle);
                    //_writer.WriteLine(Resources.ExpectedIssueNumbers);
                    //_writer.WriteLine(Resources.RegisteredIssues);
                    //_writer.WriteLine(Resources.MissedIssues);
                    //_writer.WriteLine("</tr>");
                    //foreach (PeriodInfo info in _list)
                    //{
                    //    PrintMagazine(info);
                    //}
                    //_writer.WriteLine("</table>");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
