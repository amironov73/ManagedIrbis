// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NonNullCollection.cs -- collection with items that can't be null
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// <see cref="Collection{T}"/> with items that can't be <c>null</c>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class NonNullCollection<T>
        : Collection<T>
        where T : class
    {
        #region Collection<T> members

        /// <inheritdoc cref="Collection{T}.InsertItem" />
        protected override void InsertItem
            (
                int index,
                T item
            )
        {
            Code.NotNull(item, "item");

            base.InsertItem(index, item);
        }

        /// <inheritdoc cref="Collection{T}.SetItem" />
        protected override void SetItem
            (
                int index,
                T item
            )
        {
            Code.NotNull(item, "item");

            base.SetItem(index, item);
        }

        #endregion

        #region Public members

        /// <summary>
        /// Converts the collection to <see cref="Array"/> of elements
        /// of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>Array of items of type <typeparamref name="T"/>.
        /// </returns>
        [NotNull]
        public T[] ToArray()
        {
            List<T> result = new List<T>(this);

            return result.ToArray();
        }

        /// <summary>
        /// Add several elements to the collection.
        /// </summary>
        [NotNull]
        public NonNullCollection<T> AddRange
            (
                [NotNull] IEnumerable<T> range
            )
        {
            Code.NotNull(range, "range");

            foreach (T item in range)
            {
                Add(item);
            }

            return this;
        }

        #endregion
    }
}
