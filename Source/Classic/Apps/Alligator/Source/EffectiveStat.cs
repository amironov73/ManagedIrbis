// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System;

#endregion

namespace Alligator
{
    class EffectiveStat
    {
        /// <summary>
        /// Шифр в базе.
        /// </summary>
        public string Index;

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
        /// Раздел знаний.
        /// </summary>
        public string KnowledgeSection;

        /// <summary>
        /// Скорость выдачи.
        /// </summary>
        public double Speed;

        /// <summary>
        /// КСУ.
        /// </summary>
        public string Ksu;

        /// <summary>
        /// Автор.
        /// </summary>
        public string Author;

        /// <summary>
        /// Издательство.
        /// </summary>
        public string Publisher;

        /// <summary>
        /// Доля.
        /// </summary>
        public double Fraction;
    }
}
