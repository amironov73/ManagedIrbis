/* SearchReference.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// #N
    /// </summary>
    sealed class SearchReference
    {
        #region Properties

        /// <summary>
        /// Number.
        /// </summary>
        [CanBeNull]
        public string Number { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return "#" + Number;
        }

        #endregion
    }
}
