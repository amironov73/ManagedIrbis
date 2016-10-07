/* SearchLevel1.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// level0 . level0
    /// </summary>
    sealed class SearchLevel1
        : ComplexLevel<SearchLevel0>
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchLevel1()
            : base(" . ")
        {
        }

        #endregion
    }
}
