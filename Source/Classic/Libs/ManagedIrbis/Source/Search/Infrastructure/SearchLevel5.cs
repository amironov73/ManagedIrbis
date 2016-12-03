// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchLevel5.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// level4 ^ level4
    /// </summary>
    sealed class SearchLevel5
        : ComplexLevel<SearchLevel4>
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchLevel5()
            : base(" ^ ")
        {
        }

        #endregion
    }
}
