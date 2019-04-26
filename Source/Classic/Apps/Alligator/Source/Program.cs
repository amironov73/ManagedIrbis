// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;

using AM;
using AM.Collections;

using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Charts;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Fields;
using ManagedIrbis.Infrastructure;

using static System.Console;

#endregion

// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement

namespace Alligator
{
    class LoanComparer : IComparer<LoanInfo>
    {
        public static LoanComparer Instance = new LoanComparer();

        public int Compare(LoanInfo x, LoanInfo y)
        {
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                throw new ArgumentNullException();
            }
            return string.Compare(x.Index, y.Index, StringComparison.InvariantCultureIgnoreCase);
        }
    }

    class LoanInfo
    {
        public string Index;
        public int Count;

        public static LoanInfo[] LoadFromFile(string fileName)
        {
            var result = new LocalList<LoanInfo>();
            foreach (var line in File.ReadLines(fileName))
            {
                var parts = line.Split('\t');
                if (parts.Length == 2)
                {
                    LoanInfo loan = new LoanInfo
                    {
                        Index = parts[0],
                        Count = FastNumber.ParseInt32(parts[1])
                    };
                    result.Add(loan);
                }
            }

            return result.ToArray();
        }

        public static int CountLoans(LoanInfo[] array, string index, double fraction)
        {
            if (string.IsNullOrEmpty(index))
            {
                return 0;
            }

            var loan = new LoanInfo{Index = index};
            var found = Array.BinarySearch(array, loan, LoanComparer.Instance);
            return found < 0 ? 0 : (int)(array[found].Count * fraction);
        }

        public static int CountLoans(LoanInfo[] array, IEnumerable<EffectiveStat> books)
        {
            var result = 0;
            foreach (var book in books)
            {
                result += CountLoans(array, book.Index, book.Fraction);
            }

            return result;
        }
    }

    class LoanArchive
    {
        public DateTime Date;
        public LoanInfo[] Loans;

        public static LoanArchive LoadFromFile(string fileName)
        {
            var nameOnly = Path.GetFileNameWithoutExtension(fileName);
            var date = DateTime.ParseExact(nameOnly, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var loans = LoanInfo.LoadFromFile(fileName);
            var result = new LoanArchive{Date = date, Loans = loans};
            return result;
        }

        public static LoanArchive[] LoadFromFolder(string folder)
        {
            Write("Read archives... ");
            var result = new List<LoanArchive>();
            var files = Directory.GetFiles(folder, "*.txt", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var loan = LoadFromFile(file);
                result.Add(loan);
            }

            WriteLine("done");
            return result.OrderBy(a => a.Date).ToArray();
        }
    }

    class LoanIndication
    {
        public DateTime Date;
        public int Count;

        public static LinkedList<LoanIndication> CountLoans(IEnumerable<LoanArchive> archives,
            EffectiveStat[] books)
        {
            var result = new LinkedList<LoanIndication>();
            var flag = false;
            foreach (var archive in archives)
            {
                var count = LoanInfo.CountLoans(archive.Loans, books);
                if (!flag && count == 0)
                {
                    continue;
                }

                flag = true;
                var indication = new LoanIndication{Count = count, Date = archive.Date};
                result.AddLast(indication);
            }

            return result;
        }
    }

    static class Program
    {
        const int NumberOfSections = 15;

        private static readonly string connectionString
            = "host=127.0.0.1;port=6666;user=librarian;password=secret;db=IBIS;arm=C;";
        private static readonly string[] fonds =
        {
            "Ф201", "Ф202", "Ф302", "Ф303", "Ф404",
            "Ф501", "Ф502", "Ф503", "Ф504", "Ф505",
            "Ф506", "ФКХ"
        };
        private static IrbisConnection connection;
        private static IrbisProvider provider;
        private static List<EffectiveStat> stat;
        private static LoanArchive[] archives;

        private static Workbook workbook;
        private static Worksheet worksheet;
        private static int currentLine;

        static string GetPublisher(MarcRecord record)
        {
            var result = record.FM(210, 'c')
                         ?? record.FM(461, 'g');
            if (!string.IsNullOrEmpty(result))
            {
                result = result
                    .Trim()
                    .Unquote('[', ']')
                    .Unquote('"')
                    .ToUpperInvariant();
            }

            return result;
        }

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

                result.Index = record.FM(903);
                result.Status = exemplar.Status;
                result.Author = record.FM(700, 'a');
                result.Ksu = exemplar.KsuNumber1;
                result.Publisher = GetPublisher(record);

                int amount = exemplar.Amount.SafeToInt32();
                if (amount == 0)
                {
                    amount = 1;
                }

                result.ExemplarCount += amount;
                result.Fraction = 1.0 / amount;

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

            // Проверяем, не портит ли экземпляр статистику.
            // Плохие (выпадающие) экземпляры не берём.

            if (result.Status == "1" || result.Status == "6")
            {
                if (result.LoanCount < 3)
                {
                    return;
                }
            }

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
            WriteCell(6, book.Ksu)
                .SetBorders().Background(color);
            WriteCell(7, book.Publisher)
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

        static void CreateFirstDiagramAndList(EffectiveStat[] books, string fond, int year)
        {
            var lightRed = Color.FromArgb(255, 200, 200);

            var poorBooks = books.Where(b => b.Speed < 2.0).ToArray();
            var moderateBooks = books.Where
                (
                    b => (b.Speed < 7.0) && (b.Speed >= 2.0)
                ).ToArray();
            var goodBooks = books.Where(b => b.Speed >= 7.0).ToArray();


            ProcessSelection("Лидеры", Color.LightGreen, goodBooks);
            ProcessSelection("Аутсайдеры", lightRed, poorBooks);
            ProcessSelection("Середняки", Color.Yellow, moderateBooks);

            var chart = worksheet.Charts.Add(ChartType.ColumnStacked);
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
            //chart.Series[0].Outline.SetSolidFill(lightRed);
            chart.Series[0].Fill.SetSolidFill(lightRed);

            chart.Series.Add
                (
                    "Середняки",
                    arguments,
                    ChartData.FromArray(CountSections(moderateBooks))
                );
            //chart.Series[1].Outline.SetSolidFill(Color.Yellow);
            chart.Series[1].Fill.SetSolidFill(Color.Yellow);

            chart.Series.Add
                (
                    "Лидеры",
                    arguments,
                    ChartData.FromArray(CountSections(goodBooks))
                );
            //chart.Series[2].Outline.SetSolidFill(Color.LightGreen);
            chart.Series[2].Fill.SetSolidFill(Color.LightGreen);

            chart.Views[0].GapWidth = 15;
            var title = chart.PrimaryAxes[0].Title;
            title.SetValue("Разделы знаний");
            title.Visible = true;
            title = chart.PrimaryAxes[1].Title;
            title.SetValue("Кол-во экземпляров");
            title.Visible = true;
        }

        static LinkedList<LoanIndication> GetIndications(EffectiveStat[] books)
        {
            var firstDate = books.Min(b => b.Date);
            var indications = LoanIndication.CountLoans(archives, books);
            while (indications.Count != 0)
            {
                if (indications.First.Value.Date >= firstDate)
                {
                    break;
                }
                indications.RemoveFirst();
            }

            var first = true;
            var previous = 0;
            foreach (var indication in indications)
            {
                if (!first)
                {
                    if (indication.Count < previous)
                    {
                        indication.Count = previous;
                    }
                }

                first = false;
                previous = indication.Count;
            }

            return indications;
        }

        static void CreateSecondDiagram(EffectiveStat[] books)
        {
            if (ReferenceEquals(archives, null))
            {
                return;
            }

            var indications = GetIndications(books);
            if (ReferenceEquals(indications.First, null))
            {
                return;
            }

            currentLine += 2;
            var chart = worksheet.Charts.Add(ChartType.LineMarker);
            chart.TopLeftCell = worksheet.Cells[0, 1];
            chart.BottomRightCell = worksheet.Cells[14, 9];
            chart.Title.SetValue("Динамика выдачи");
            chart.Title.Font.Size = 12.0;
            chart.Title.Visible = true;
            chart.Legend.Visible = false;
            chart.Legend.Position = LegendPosition.Right;

            var arguments = indications
                .Select(i => CellValue.FromObject(i.Date.ToString("MMM yy")))
                .ToArray();
            var values = indications.Select(i => CellValue.FromObject(i.Count)).ToArray();
            chart.Series.Add
                (
                    "Динамика",
                    arguments,
                    values
                );
            chart.Series[0].Outline.SetSolidFill(Color.Blue);
        }

        static void SetupColumnWidth()
        {
            worksheet.Columns[0].Width = 150;
            worksheet.Columns[1].Width = 20;
            worksheet.Columns[2].Width = 10;
            worksheet.Columns[3].Width = 12;
            worksheet.Columns[4].Width = 12;
            worksheet.Columns[5].Width = 12;
            worksheet.Columns[6].Width = 20;
            worksheet.Columns[7].Width = 50;
        }

        static void ProcessFond(int year, string fond)
        {
            fond = fond.ToUpperInvariant();
            WriteLine($"FOND={fond}");
            var books = stat.Where(s => FilterFond(s, fond)).ToArray();
            WriteLine($"\trecords={books.Length}");
            if (books.Length == 0)
            {
                return;
            }

            var name = $"{year.ToInvariantString()}-{fond}";
            worksheet = workbook.Worksheets.Add(name);
            currentLine = 16;

            var lightGray = Color.FromArgb(200, 200, 200);

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
            WriteCell(6, "КСУ")
                .Bold().Background(lightGray).SetBorders();
            WriteCell(7, "Издательство")
                .Bold().Background(lightGray).SetBorders();
            var saveLine = currentLine;
            currentLine++;

            CreateFirstDiagramAndList(books, fond, year);
            CreateSecondDiagram(books);

            SetupColumnWidth();
            worksheet.FreezeRows(saveLine);
        }

        static void ProcessPublishers(int year)
        {
            var name = $"{year.ToInvariantString()}-изд";
            worksheet = workbook.Worksheets.Add(name);
            worksheet.Columns[0].Width = 150;
            worksheet.Columns[1].Width = 20;
            currentLine = 0;

            var lightGray = Color.FromArgb(200, 200, 200);
            WriteCell(0, "Издательство")
                .Bold().Background(lightGray).SetBorders();
            WriteCell(1, "Выдач")
                .Bold().Background(lightGray).SetBorders();
            currentLine++;

            var counter = new DictionaryCounterInt32<string>();

            foreach (var book in stat)
            {
                if (!string.IsNullOrEmpty(book.Publisher))
                {
                    counter.Augment(book.Publisher, book.LoanCount);
                }
            }

            var pairs = counter.OrderByDescending(p=>p.Value).ToArray();

            foreach (var pair in pairs)
            {
                WriteCell(0, pair.Key).SetBorders();
                WriteCell(1, pair.Value).SetBorders();
                currentLine++;
            }
        }

        static void ProcessAuthors(int year)
        {
            var name = $"{year.ToInvariantString()}-авт";
            worksheet = workbook.Worksheets.Add(name);
            worksheet.Columns[0].Width = 70;
            worksheet.Columns[1].Width = 20;
            currentLine = 0;

            var lightGray = Color.FromArgb(200, 200, 200);
            WriteCell(0, "Автор")
                .Bold().Background(lightGray).SetBorders();
            WriteCell(1, "Выдач")
                .Bold().Background(lightGray).SetBorders();
            currentLine++;

            var counter = new DictionaryCounterInt32<string>();

            foreach (var book in stat)
            {
                if (!string.IsNullOrEmpty(book.Author))
                {
                    counter.Augment(book.Author, book.LoanCount);
                }
            }

            var pairs = counter.OrderByDescending(p=>p.Value).ToArray();

            foreach (var pair in pairs)
            {
                WriteCell(0, pair.Key).SetBorders();
                WriteCell(1, pair.Value).SetBorders();
                currentLine++;
            }
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

            ProcessPublishers(year);
            ProcessAuthors(year);

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
                string archiveFolder = "/Archive";
                if (Directory.Exists(archiveFolder)) //-V3039
                {
                    archives = LoanArchive.LoadFromFolder(archiveFolder);
                }

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
                    ProcessYear(2018, 20, 23, 30, 40, 49, 50, 56, 90, 93, 95);
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
