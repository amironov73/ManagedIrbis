using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

using AM;
using AM.Configuration;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Client;
using ManagedIrbis.Fields;
using ManagedIrbis.Menus;

// ReSharper disable LocalizableElement
// ReSharper disable UseStringInterpolation
// ReSharper disable UseNameofExpression

namespace EffectiveBooks
{
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

            Console.WriteLine
                (
                    string.Format
                        (
                            CultureInfo.InvariantCulture,
                            "{0}\t{1}\t{2}\t{3:F2}\t{4}\t{5:F2}\t{6:F2}",
                            Description,
                            TitleCount,
                            ExemplarCount,
                            TotalCost,
                            LoanCount,
                            meanLoan,
                            loanCost
                        )
                );
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
                Description = book.Description,
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

            foreach (ExemplarInfo exemplar in selected)
            {
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

                result.TotalCost += amount * price;
            }

            decimal loanCount = book.UsageCount;
            if (result.ExemplarCount != totalExemplars)
            {
                loanCount = loanCount * result.ExemplarCount / totalExemplars;
            }

            result.LoanCount = (int) loanCount;

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
                        _outputBooks? "Итого по КСУ {0}" : "{0}\t{1}",
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
                    MenuFile menu = MenuFile.ParseLocalFile("ksu.mnu");
                    EffectiveStat totalStat = new EffectiveStat
                    {
                        Description = "Всего по всем КСУ"
                    };
                    foreach (MenuEntry entry in menu.Entries)
                    {
                        EffectiveStat ksuStat = ProcessKsu(entry);
                        ksuStat.Output(_outputBooks);
                        totalStat.Add(ksuStat);
                    }

                    totalStat.Output(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
