// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

using AM;
using AM.Text.Output;

using CodeJam;
using DevExpress.Spreadsheet;
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
        public string Fond { get; set; }
        public Reference<bool> StopFlag { get; set; }

        public bool IsStop => StopFlag.Target;

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

        [CanBeNull]
        public EffectiveStat ProcessBook
            (
                [NotNull] MarcRecord record,
                [NotNull] string ksu
            )
        {
            Code.NotNull(record, nameof(record));

            if (IsStop)
            {
                return null;
            }

            BookInfo book = new BookInfo(Provider, record);
            ExemplarInfo[] selected = book.Exemplars.Where
                (
                    ex => ex.KsuNumber1.SameString(ksu)
                )
                .ToArray();

            if (Fond != "*")
            {
                selected = selected.Where
                    (
                        ex => ex.Place.SameString(Fond)
                    )
                    .ToArray();
            }

            if (selected.Length == 0)
            {
                return null;
            }

            EffectiveStat result = new EffectiveStat
            {
                Description = OutputBooks ? book.Description : string.Empty,
                TitleCount = 1,
                PageCount = book.Pages
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
            Code.NotNull(ksu, nameof(ksu));

            WriteLine("КСУ {0}", ksu);
            if (OutputBooks)
            {
                Sheet.Invoke(() => Sheet.WriteLine("КСУ {0}", ksu).Bold());
            }

            EffectiveStat result = new EffectiveStat
            {
                Description = $"Итого по КСУ {ksu}"
            };

            string expression = $"\"NKSU={ksu}\"";
            IEnumerable<MarcRecord> batch = BatchRecordReader.Search
                (
                    Connection,
                    Connection.Database,
                    expression,
                    500
                );
            int topRow = Sheet.CurrentRow;
            foreach (MarcRecord record in batch)
            {
                if (IsStop)
                {
                    break;
                }

                EffectiveStat bookStat = ProcessBook(record, ksu);
                if (IsStop)
                {
                    break;
                }

                if (!ReferenceEquals(bookStat, null))
                {
                    if (OutputBooks)
                    {
                        bookStat.Output(this);
                    }

                    result.Add(bookStat);
                }
            }

            int bottomRow = Sheet.CurrentRow;
            if (topRow != bottomRow)
            {
                bottomRow--;
                Range range = Sheet.GetRange(8, topRow, bottomRow);
                Sheet.Invoke(() => range.Conditional2Colors
                        (
                            Color.FromArgb(255, 255, 150, 150),
                            Color.FromArgb(255, 50, 255, 50)
                        )
                );
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
                    Description = "Итого по всем КСУ"
                };
                bool first = true;

                foreach (string ksu in selected)
                {
                    if (IsStop)
                    {
                        break;
                    }

                    if (OutputBooks)
                    {
                        if (!first)
                        {
                            Sheet.NewLine();
                        }

                        first = false;
                    }

                    EffectiveStat ksuStat = ProcessKsu(ksu);
                    ksuStat.Output(this, true);
                    totalStat.Add(ksuStat);
                }

                if (selected.Length > 1 && !IsStop)
                {
                    Sheet.NewLine();
                    totalStat.Output(this, true);
                }
            }
            catch (Exception e)
            {
                WriteLine(e.ToString());
            }
        }
    }
}
