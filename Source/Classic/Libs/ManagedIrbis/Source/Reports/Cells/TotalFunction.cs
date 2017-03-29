// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TotalFunction.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Function for <see cref="TotalCell"/>.
    /// </summary>
    public enum TotalFunction
    {
        /// <summary>
        /// Cell count.
        /// </summary>
        Count,

        /// <summary>
        /// Count of non-empty cells.
        /// </summary>
        CountNonEmpty,

        /// <summary>
        /// Count of unique cells.
        /// </summary>
        CountUnique,

        /// <summary>
        /// Sum.
        /// </summary>
        Sum,

        /// <summary>
        /// Average.
        /// </summary>
        Average,

        /// <summary>
        /// Maximum.
        /// </summary>
        Maximum,

        /// <summary>
        /// Minimum.
        /// </summary>
        Minimum,

        /// <summary>
        /// Standard deviation.
        /// </summary>
        StandardDeviation,

        /// <summary>
        /// Variation.
        /// </summary>
        Variation,
    }
}
