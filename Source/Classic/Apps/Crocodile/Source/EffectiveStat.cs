using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Reports;

namespace Crocodile
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
}
