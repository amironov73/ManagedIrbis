using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Configuration;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Client;
using ManagedIrbis.Fields;
using ManagedIrbis.Menus;
using ManagedIrbis.Reports;

namespace Crocodile
{
    class EffectiveEngine
    {
        private static IrbisConnection _connection;
        private static IrbisProvider _provider;
        private static bool _outputBooks;

        [NotNull]
        static EffectiveStat ProcessBook
            (
                [NotNull] MarcRecord record,
                [NotNull] string ksu
            )
        {
            Code.NotNull(record, "record");

            BookInfo book = new BookInfo(_provider, record);
            ExemplarInfo[] selected = book.Exemplars.Where
                (
                    ex => ex.KsuNumber1.SameString(ksu)
                )
                .ToArray();
            Debug.Assert(selected.Length != 0, "exemplars.Length != 0");
            EffectiveStat result = new EffectiveStat
            {
                Description = _outputBooks ? book.Description : string.Empty,
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

            result.LoanCount = (int)loanCount;
            result.Sigla = string.Join(" ", siglas.Distinct()
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.ToUpperInvariant()));

            return result;
        }

        [NotNull]
        static EffectiveStat ProcessKsu
            (
                [NotNull] MenuEntry entry
            )
        {
            Code.NotNull(entry, "entry");

            string ksu = entry.Code.ThrowIfNull("entry.Code");

            if (_outputBooks)
            {
                Console.WriteLine();
                Console.WriteLine("КСУ {0} {1}", ksu, entry.Comment);
                Console.WriteLine();
            }

            EffectiveStat result = new EffectiveStat
            {
                Description = string.Format
                    (
                        _outputBooks ? "Итого по КСУ {0}" : "{0} {1}",
                        ksu,
                        entry.Comment
                    )
            };

            string expression = string.Format("\"NKSU={0}\"", ksu);
            IEnumerable<MarcRecord> batch = BatchRecordReader.Search
                (
                    _connection,
                    _connection.Database,
                    expression,
                    500
                );
            foreach (MarcRecord record in batch)
            {
                EffectiveStat bookStat = ProcessBook(record, ksu);
                if (_outputBooks)
                {
                    bookStat.Output(false);
                }
                result.Add(bookStat);
            }

            return result;
        }

        public static void Process()
        {
            try
            {
                _outputBooks = ConfigurationUtility.GetBoolean("books", false);
                string connectionString
                    = IrbisConnectionUtility.GetStandardConnectionString();
                using (_connection = new IrbisConnection(connectionString))
                {
                    _provider = new ConnectedClient(_connection);
                    //_provider = new SemiConnectedClient(_connection);
                    EffectiveReport.Instance = new EffectiveReport(_provider);
                    MenuFile menu = MenuFile.ParseLocalFile("ksu.mnu");
                    EffectiveStat totalStat = new EffectiveStat
                    {
                        Description = "Всего по всем КСУ"
                    };
                    bool first = true;

                    foreach (MenuEntry entry in menu.Entries)
                    {
                        if (_outputBooks)
                        {
                            if (!first)
                            {
                                EffectiveReport.AddLine(string.Empty);
                            }

                            first = false;

                            string title = string.Format
                                (
                                    "{0} {1}",
                                    entry.Code,
                                    entry.Comment
                                );
                            EffectiveReport.BoldLine(title);
                        }

                        EffectiveStat ksuStat = ProcessKsu(entry);
                        ksuStat.Output(false);
                        totalStat.Add(ksuStat);
                    }

                    EffectiveReport.AddLine(string.Empty);
                    totalStat.Output(false);
                }

                ExcelDriver driver = new ExcelDriver();
                string fileName = "output.xlsx";
                FileUtility.DeleteIfExists(fileName);
                driver.OutputFile = fileName;
                EffectiveReport report = EffectiveReport.Instance;
                report.Context.SetDriver(driver);

                report.Render(report.Context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
