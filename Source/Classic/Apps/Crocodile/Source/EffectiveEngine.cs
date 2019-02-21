// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using AM;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Client;
using ManagedIrbis.Fields;

#endregion

// ReSharper disable StringLiteralTypo

namespace Crocodile
{
    class EffectiveEngine
    {
        public AbstractOutput Log { get; set; }
        public IrbisConnection Connection { get; set; }
        public IrbisProvider Provider { get; set; }
        public bool OutputBooks { get; set; }
        public string[] Selected { get; set; }
        public EffectiveSheet Sheet { get; set; }

        public void WriteLine()
        {
            Log.WriteLine(string.Empty);
        }

        public void WriteLine
            (
                string format,
                params object[] args
            )
        {
            Log.WriteLine(format, args);
        }

        [NotNull]
        public EffectiveStat ProcessBook
            (
                [NotNull] MarcRecord record,
                [NotNull] string ksu
            )
        {
            Code.NotNull(record, "record");

            BookInfo book = new BookInfo(Provider, record);
            ExemplarInfo[] selected = book.Exemplars.Where
                (
                    ex => ex.KsuNumber1.SameString(ksu)
                )
                .ToArray();
            Debug.Assert(selected.Length != 0, "exemplars.Length != 0");
            EffectiveStat result = new EffectiveStat
            {
                Description = OutputBooks ? book.Description : string.Empty,
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
        public EffectiveStat ProcessKsu
            (
                [NotNull] string ksu
            )
        {
            Code.NotNull(ksu, "ksu");

            WriteLine("КСУ {0}", ksu);
            Sheet.WriteLine("КСУ {0}", ksu);

            EffectiveStat result = new EffectiveStat
            {
                Description = string.Format
                    (
                        "Итого по КСУ {0}",
                        ksu
                    )
            };

            string expression = string.Format("\"NKSU={0}\"", ksu);
            IEnumerable<MarcRecord> batch = BatchRecordReader.Search
                (
                    Connection,
                    Connection.Database,
                    expression,
                    500
                );
            foreach (MarcRecord record in batch)
            {
                EffectiveStat bookStat = ProcessBook(record, ksu);
                if (OutputBooks)
                {
                    bookStat.Output(this);
                }
                result.Add(bookStat);
            }

            return result;
        }

        public void Process
            (
                string[] selected
            )
        {
            try
            {
                Provider = new ConnectedClient(Connection);
                EffectiveStat totalStat = new EffectiveStat
                {
                    Description = "Всего по всем КСУ"
                };
                bool first = true;

                foreach (string ksu in selected)
                {
                    if (OutputBooks)
                    {
                        if (!first)
                        {
                            Sheet.NewLine();
                        }

                        first = false;

                        //string title = string.Format
                        //(
                        //    "{0} {1}",
                        //    entry.Code,
                        //    entry.Comment
                        //);
                        //EffectiveReport.BoldLine(title);
                    }

                    EffectiveStat ksuStat = ProcessKsu(ksu);
                    ksuStat.Output(this);
                    totalStat.Add(ksuStat);
                }

                if (selected.Length > 1)
                {
                    Sheet.NewLine();
                    totalStat.Output(this);
                }
            }
            catch (Exception e)
            {
                WriteLine(e.ToString());
            }
        }
    }
}
