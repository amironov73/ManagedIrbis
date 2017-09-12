// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RepeatKind.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public enum RepeatKind
    {
        /// <summary>
        /// All the repeats.
        /// </summary>
        All,

        /// <summary>
        /// By format.
        /// </summary>
        ByFormat,

        /// <summary>
        /// Last repeat.
        /// </summary>
        Last,

        /// <summary>
        /// Explicit specified repeat.
        /// </summary>
        Explicit
    }
}
