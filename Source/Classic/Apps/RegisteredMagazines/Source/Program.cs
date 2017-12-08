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

using JetBrains.Annotations;


using ManagedIrbis;
using ManagedIrbis.Fields;
using ManagedIrbis.Magazines;

#endregion

namespace RegisteredMagazines
{
    using Properties;

    class Program
    {
        private static IrbisConnection _connection;
        private static MagazineManager _manager;
        private static TextWriter _writer;
        private static string _period;
        private static string _year;
        private static List<PeriodInfo> _list;

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
            MagazineIssueInfo[] issues = _manager.GetIssues(magazine, _year);

            List<string> registered = new List<string>(issues.Length);
            foreach (MagazineIssueInfo issue in issues)
            {
                ExemplarInfo[] exemplars = issue.Exemplars;
                if (!ReferenceEquals(exemplars, null))
                {
                    foreach (ExemplarInfo exemplar in exemplars)
                    {
                        string number = issue.Number;
                        if (!ReferenceEquals(number, null)
                            && exemplar.KsuNumber1.SameString(_period))
                        {
                            registered.Add(issue.Number);
                            break;
                        }
                    }
                }
            }

            registered = NumberText.Sort(registered).Distinct().ToList();

            if (registered.Count == 0)
            {
                Console.WriteLine(Resources.T0, string.Empty);
            }
            else
            {
                PeriodInfo info = new PeriodInfo
                {
                    Title = magazine.ExtendedTitle,
                    Registered = CompressIfPossible(registered)
                };
                _list.Add(info);
                Console.WriteLine(Resources.T0, info.Registered);
            }
        }

        static void PrintMagazine
            (
                [NotNull] PeriodInfo info
            )
        {
            _writer.WriteLine("<tr>");
            _writer.WriteLine("<td>");
            _writer.WriteLine(HtmlText.Encode(info.Title));
            _writer.WriteLine("</td>");
            _writer.WriteLine("<td>");
            _writer.WriteLine(HtmlText.Encode(info.Registered));
            _writer.WriteLine("</td>");
            _writer.WriteLine("</tr>");
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

                using (_writer = new StreamWriter(fileName))
                using (_connection = new IrbisConnection(connectionString))
                {
                    _manager = new MagazineManager(_connection);
                    MagazineInfo[] allMagazines = _manager.GetAllMagazines();
                    foreach (MagazineInfo magazine in allMagazines)
                    {
                        ProcessMagazine(magazine);
                    }

                    _list = _list.OrderBy(i => i.Title).ToList();

                    _writer.WriteLine("<h3>");
                    _writer.WriteLine(Resources.RegisteredPeriodicalsInYear);
                    _writer.WriteLine(HtmlText.Encode(_year));
                    _writer.WriteLine("</h3>");
                    _writer.WriteLine("<table border='1' cellpadding='3' cellspacing='0'>");
                    _writer.WriteLine("<tr>");
                    _writer.WriteLine(Resources.MagazineTitle);
                    _writer.WriteLine(Resources.RegisteredIssuesTh);
                    _writer.WriteLine("</tr>");
                    foreach (PeriodInfo info in _list)
                    {
                        PrintMagazine(info);
                    }
                    _writer.WriteLine("</table>");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
