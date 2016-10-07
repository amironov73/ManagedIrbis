/* SearchLevel2.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// level1 (F) level1
    /// </summary>
    sealed class SearchLevel2
        : ComplexLevel<SearchLevel1>
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchLevel2()
            : base(" (F) ")
        {
        }

        #endregion
    }
}
