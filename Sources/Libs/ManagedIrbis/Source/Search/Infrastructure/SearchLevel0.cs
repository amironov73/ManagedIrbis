/* SearchLevel0.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// Term, reference or parenthesis.
    /// </summary>
    sealed class SearchLevel0
    {
        #region Properties

        /// <summary>
        /// Term.
        /// </summary>
        [CanBeNull]
        public SearchTerm Term { get; set; }

        /// <summary>
        /// Reference.
        /// </summary>
        [CanBeNull]
        public SearchReference Reference { get; set; }

        /// <summary>
        /// Parenthesis.
        /// </summary>
        [CanBeNull]
        public SearchLevel7 Parenthesis { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return 
                (
                    Term 
                    ?? (object)Reference
                    ?? Parenthesis
                )
                .NullableToVisibleString();
        }

        #endregion
    }
}
