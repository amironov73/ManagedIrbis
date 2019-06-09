// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System;

using AM;

using CodeJam;

using JetBrains.Annotations;

// ReSharper disable UseNameofExpression

#endregion

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

        /// <summary>
        /// Количество страниц.
        /// </summary>
        public int PageCount;

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
                [NotNull] EffectiveEngine engine,
                bool bold = false
            )
        {
            if (ExemplarCount == 0)
            {
                return;
            }

            decimal loanCost = LoanCount == 0
                ? TotalCost
                : TotalCost / LoanCount;

            decimal meanLoan = ExemplarCount == 0
                ? LoanCount
                : (decimal)LoanCount / ExemplarCount;

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

            EffectiveSheet sheet = engine.Sheet;
            sheet.WriteCell(0, Description);
            sheet.WriteCell(1, Date.ToShortDateString());
            sheet.WriteCell(2, Sigla);
            sheet.WriteCell(3, Bbk);
            sheet.WriteCell(4, PageCount);
            sheet.WriteCell(5, TitleCount);
            sheet.WriteCell(6, ExemplarCount);
            sheet.WriteCell(7, TotalCost, "0.00");
            sheet.WriteCell(8, LoanCount);
            sheet.WriteCell(9, meanLoan, "0.00");
            sheet.WriteCell(10, loanCost, "0.00");
            sheet.WriteCell(11, dayLoan, "0.00");
            sheet.WriteCell(12, rdrEff, "0.00");
            sheet.WriteCell(13, finEff, "0.00");

            if (bold)
            {
                sheet.Invoke(() => sheet.CurrentLine().Bold());
            }

            sheet.NewLine();
        }
    }
}
