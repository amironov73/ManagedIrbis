using System;
using System.Collections.Generic;
using System.Linq;

using DevExpress.Spreadsheet;

using AM;
using AM.Text;
using AM.Text.Ranges;

using ManagedIrbis;
using ManagedIrbis.Fields;
using ManagedIrbis.Magazines;

// ReSharper disable StringLiteralTypo
// ReSharper disable LocalizableElement

namespace ComplectList
{
    class Program
    {
        private static string _connectionString;
        private static string _sigla;
        private static string _outputFileName;
        private static IrbisConnection _connection;
        private static Workbook _workbook;
        private static Worksheet _worksheet;
        private static int _currentRow;
        private static int _counter;
        private static string _currentYear;
        private static readonly List<ComplectInfo> Complects = new List<ComplectInfo>();

        static void WriteCell(int column, string text)
        {
            Cell result = _worksheet.Cells[_currentRow, column];
            result.Value = text;
        }

        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("ComplectList <connection> <sigla> output.xlsx");
                return;
            }

            _connectionString = args[0];
            _sigla = args[1];
            _outputFileName = args[2];

            _workbook= new Workbook();
            _worksheet = _workbook.Worksheets[0];
            _currentYear = DateTime.Today.Year.ToInvariantString();

            WriteCell(0, "№");
            WriteCell(1, "Газета");
            WriteCell(2, "Год");
            WriteCell(3, "Выпуски");
            WriteCell(4, "Отметки");
            _currentRow++;

            try
            {
                using (_connection = new IrbisConnection(_connectionString))
                {
                    Console.WriteLine("Loading magazines...");

                    MagazineManager manager = new MagazineManager(_connection);
                    MagazineInfo[] magazines = manager.GetAllMagazines();

                    Console.WriteLine
                        (
                            "Magazines loaded: {0}",
                            magazines.Length
                        );

                    // magazines = magazines.Take(10).ToArray();

                    _counter = 0;
                    foreach (MagazineInfo magazine in magazines)
                    {
                        _counter++;
                        if (!magazine.MagazineKind.SameString("c"))
                        {
                            continue;
                        }

                        string title = magazine.ExtendedTitle;
                        Console.WriteLine
                            (
                                "{0} of {1}) {2}",
                                _counter,
                                magazines.Length,
                                title
                            );

                        MagazineIssueInfo[] issues = manager.GetIssues(magazine);
                        foreach (MagazineIssueInfo issue in issues)
                        {
                            ExemplarInfo[] exemplars = issue.Exemplars;
                            if (!ReferenceEquals(exemplars, null))
                            {
                                foreach (ExemplarInfo exemplar in exemplars)
                                {
                                    if (!exemplar.Place.SameString(_sigla))
                                    {
                                        continue;
                                    }

                                    string year = issue.Year;
                                    if (string.IsNullOrEmpty(year))
                                    {
                                        continue;
                                    }

                                    if (string.CompareOrdinal(year, _currentYear) >= 0)
                                    {
                                        continue;
                                    }

                                    ComplectInfo complect = new ComplectInfo
                                    {
                                        Title = title,
                                        Year = year,
                                        Issue = issue.Number
                                    };
                                    Complects.Add(complect);
                                }
                            }
                        }
                    }
                }

                Console.WriteLine();
                Console.WriteLine();

                _counter = 1;
                foreach (var byTitle in Complects.GroupBy(c => c.Title).OrderBy(_ => _.Key))
                {
                    string title = byTitle.Key;
                    Console.WriteLine(title);

                    foreach (var byYear in byTitle.GroupBy(c => c.Year).OrderBy(_ => _.Key))
                    {
                        string year = byYear.Key;
                        Console.Write(" {0}", year);
                        string[] issues = byYear.Select(c => c.Issue)
                            .Distinct().ToArray();
                        List<NumberText> numbers = issues.Distinct()
                            .Select(i => new NumberText(i)).ToList();
                        NumberRangeCollection ranges = NumberRangeCollection.Cumulate(numbers);
                        string cumulated = ranges.ToString();
                        WriteCell(0, _counter.ToInvariantString());
                        WriteCell(1, title);
                        WriteCell(2, year);
                        WriteCell(3, cumulated);
                        _currentRow++;
                        _counter++;
                    }

                    Console.WriteLine();
                }

                Console.Write("Saving... ");
                _workbook.SaveDocument(_outputFileName);
                Console.WriteLine("saved");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
