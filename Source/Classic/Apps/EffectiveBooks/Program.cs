using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

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

// ReSharper disable LocalizableElement
// ReSharper disable UseStringInterpolation
// ReSharper disable UseNameofExpression

namespace EffectiveBooks
{
    class EffectiveReport
        : IrbisReport
    {
        #region Properties

        [NotNull]
        public IrbisProvider Provider { get; private set; }

        [NotNull]
        public ReportContext Context { get; private set; }

        [NotNull]
        // ReSharper disable once NotNullMemberIsNotInitialized
        public static EffectiveReport Instance { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public EffectiveReport
            (
                [NotNull] IrbisProvider provider
            )
        {
            Provider = provider;
            Context = new ReportContext(provider);

            Header = new HeaderBand();
            Header.Cells.Add(new TextCell("КСУ",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Дата",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Сигла",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("ББК",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Назв.",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Экз.",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Руб.",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Выдач",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Выд./экз.",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Руб/выд.",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Выд/день",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Чит. эффект.",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Фин. эффект.",
                new ReportAttribute(ReportAttribute.Bold, true)));
        }

        #endregion

        #region Public methods

        public static void AddLine(string text)
        {
            Instance.Body.Add(new ReportBand(new TextCell(text)));
        }

        public static void BoldLine(string text)
        {
            Instance.Body.Add(new ReportBand(new TextCell(text,
                new ReportAttribute(ReportAttribute.Bold, true))));
        }

        #endregion
    }

    class EffectiveStat
    {
        /// <summary>
        /// Описание.
        /// </summary>
        public string Description;

        /// <summary>
        /// Количество названий.
        /// </summary>
        public int TitleCount;

        /// <summary>
        /// Количество экземпляров.
        /// </summary>
        public int ExemplarCount;

        /// <summary>
        /// Общая стоимость экземпляров.
        /// </summary>
        public decimal TotalCost;

        /// <summary>
        /// Количество выдач.
        /// </summary>
        public int LoanCount;

        /// <summary>
        /// Дата поступления экземпляров.
        /// </summary>
        public DateTime Date;

        /// <summary>
        /// Сигла (фонд).
        /// </summary>
        public string Sigla;

        /// <summary>
        /// ББК.
        /// </summary>
        public string Bbk;

        public void Add
            (
                [NotNull] EffectiveStat other
            )
        {
            Code.NotNull(other, "other");

            TitleCount += other.TitleCount;
            ExemplarCount += other.ExemplarCount;
            TotalCost += other.TotalCost;
            LoanCount += other.LoanCount;
            if (Date == DateTime.MinValue)
            {
                Date = other.Date;
            }
            else if (other.Date != DateTime.MinValue && other.Date < Date)
            {
                Date = other.Date;
            }
        }

        public void Output
            (
                bool indent
            )
        {
            if (indent)
            {
                Console.WriteLine();
            }

            decimal loanCost = LoanCount == 0
                ? TotalCost
                : TotalCost / LoanCount;

            decimal meanLoan = (decimal)LoanCount / ExemplarCount;

            int days = Date == DateTime.MinValue
                ? 0
                : (DateTime.Today - Date).Days + 1;
            decimal dayLoan = days == 0
                ? 0
                : (decimal) LoanCount / days;
            decimal rdrEff = days == 0
                ? 0
                : (decimal) LoanCount / ExemplarCount / days * 1000m;
            decimal finEff = days == 0
                ? 0
                : TotalCost == 0
                  ? 0
                  : LoanCount / TotalCost / days * 100000m;

            Console.WriteLine
                (
                    string.Format
                        (
                            CultureInfo.InvariantCulture,
                            "{0}\t{1:d}\t{2}\t{3}\t{4}\t{5}\t{6:F0}\t{7}\t{8:F2}\t{9:F2}\t{10:F2}\t{11:F2}\t{12:F2}",
                            Description,
                            Date,
                            Sigla,
                            Bbk,
                            TitleCount,
                            ExemplarCount,
                            TotalCost,
                            LoanCount,
                            meanLoan,
                            loanCost,
                            dayLoan,
                            rdrEff,
                            finEff
                        )
                );

            ReportBand band = new ReportBand();
            band.Cells.Add(new TextCell(Description));
            band.Cells.Add(new TextCell(Date.ToShortDateString()));
            band.Cells.Add(new TextCell(Sigla));
            band.Cells.Add(new TextCell(Bbk));
            band.Cells.Add(new TextCell(TitleCount.ToInvariantString(),
                new ReportAttribute(ReportAttribute.Number, "0")));
            band.Cells.Add(new TextCell(ExemplarCount.ToInvariantString(),
                new ReportAttribute(ReportAttribute.Number, "0")));
            band.Cells.Add(new TextCell(TotalCost.ToInvariantString("F0"),
                new ReportAttribute(ReportAttribute.Number, "0")));
            band.Cells.Add(new TextCell(LoanCount.ToInvariantString(),
                new ReportAttribute(ReportAttribute.Number, "0")));
            band.Cells.Add(new TextCell(meanLoan.ToInvariantString("F2"),
                new ReportAttribute(ReportAttribute.Number, "0.00")));
            band.Cells.Add(new TextCell(loanCost.ToInvariantString("F2"),
                new ReportAttribute(ReportAttribute.Number, "0.00")));
            band.Cells.Add(new TextCell(dayLoan.ToInvariantString("F2"),
                new ReportAttribute(ReportAttribute.Number, "0.00")));
            band.Cells.Add(new TextCell(rdrEff.ToInvariantString("F2"),
                new ReportAttribute(ReportAttribute.Number, "0.00")));
            band.Cells.Add(new TextCell(finEff.ToInvariantString("F2"),
                new ReportAttribute(ReportAttribute.Number, "0.00")));
            EffectiveReport.Instance.Body.Add(band);
        }
    }

    class Program
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

        static void Main()
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
