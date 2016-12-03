// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchLevel7.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// ( level6 )
    /// </summary>
    sealed class SearchLevel7
        : ComplexLevel<SearchLevel6>
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchLevel7()
            : base(null)
        {
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            SearchLevel6 item = Items[0];

            string result = item.ToString();

            return result;
        }

        #endregion
    }
}
