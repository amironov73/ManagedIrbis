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
