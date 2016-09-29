/* SearchLevel4.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// level3 * level3
    /// </summary>
    sealed class SearchLevel4
        : ComplexLevel<SearchLevel3>
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchLevel4()
            : base(" * ")
        {
        }

        #endregion
    }
}
