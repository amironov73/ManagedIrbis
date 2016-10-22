/* SearchLevel6.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// level5 + level5
    /// </summary>
    sealed class SearchLevel6
        : ComplexLevel<SearchLevel5>
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchLevel6()
            : base(" + ")
        {
        }

        #endregion
    }
}
