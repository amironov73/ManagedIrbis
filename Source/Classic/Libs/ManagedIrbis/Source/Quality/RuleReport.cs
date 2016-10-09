/* RuleReport.cs -- отчёт о работе правила.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace ManagedIrbis.Quality
{
    /// <summary>
    /// Отчёт о работе правила.
    /// </summary>
    public sealed class RuleReport
    {
        #region Properties

        /// <summary>
        /// Дефекты, обнаруженные правилом.
        /// </summary>
        public List<FieldDefect> Defects { get; set; }

        /// <summary>
        /// Общий урон.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Начисленный бонус.
        /// </summary>
        public int Bonus { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public RuleReport()
        {
            Defects = new List<FieldDefect>();
        }

        #endregion
    }
}
