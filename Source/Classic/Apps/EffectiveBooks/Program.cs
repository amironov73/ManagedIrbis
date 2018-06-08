using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Menus;

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
        }
    }

    class Program
    {
        private static IrbisConnection _connection;

        [NotNull]
        static EffectiveStat ProcessBook
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            EffectiveStat result = new EffectiveStat
            {
                Description = _connection.FormatRecord("@sbrief", record.Mfn)
            };

            return result;
        }

        [NotNull]
        static EffectiveStat ProcessKsu
            (
                [NotNull] MenuEntry entry
            )
        {
            Code.NotNull(entry, "entry");

            EffectiveStat result = new EffectiveStat
            {
                Description = string.Format("Итого по КСУ {0}", entry.Code)
            };

            string expression = string.Format("\"KSU={0}\"", entry.Code);
            IEnumerable<MarcRecord> batch = BatchRecordReader.Search
                (
                    _connection,
                    _connection.Database,
                    expression,
                    500
                );
            foreach (MarcRecord record in batch)
            {
                EffectiveStat bookStat = ProcessBook(record);
                bookStat.Output(false);
                result.Add(bookStat);
            }

            return result;
        }

        static void Main()
        {
            try
            {
                string connectionString
                    = IrbisConnectionUtility.GetStandardConnectionString();
                using (_connection = new IrbisConnection(connectionString))
                {
                    MenuFile menu = MenuFile.ParseLocalFile("ksu.mnu");
                    EffectiveStat totalStat = new EffectiveStat
                    {
                        Description = "Всего по всем КСУ"
                    };
                    foreach (MenuEntry entry in menu.Entries)
                    {
                        EffectiveStat ksuStat = ProcessKsu(entry);
                        ksuStat.Output(true);
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
