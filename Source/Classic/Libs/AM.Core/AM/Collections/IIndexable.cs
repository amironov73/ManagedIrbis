// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IIndexable.cs -- indexable object interface
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// Indexable object interface.
    /// </summary>
    [PublicAPI]
    public interface IIndexable<T>
    {
        /// <summary>
        /// Gets item at the specified index.
        /// </summary>
        [CanBeNull]
        T this[int index] { get; }

        /// <summary>
        /// Gets the count of items.
        /// </summary>
        int Count { get; }
    }
}
