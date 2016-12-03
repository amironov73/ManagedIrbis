// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchLevel3.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// level2 (G) level2
    /// </summary>
    sealed class SearchLevel3
        : ComplexLevel<SearchLevel2>
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchLevel3()
            : base(" (G) ")
        {
        }

        #endregion
    }
}
