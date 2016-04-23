/* Sequence.cs -- 
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// Inspired by LINQ Sequence.cs.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class Sequence
    {
        #region Private members

        #endregion

        #region public methods

        /// <summary>
        /// Repeats the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="count">The count.</param>
        /// <returns>Sequence of specified values.</returns>
        [NotNull]
        public static IEnumerable<T> Repeat<T>
            (
                [CanBeNull] T value,
                int count
            )
        {
            while (count-- > 0)
            {
                yield return value;
            }
        }

        /// <summary>
        /// Repeats the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        [NotNull]
        public static IEnumerable<T> Repeat<T>
            (
                [NotNull] IEnumerable<T> list,
                int count
            )
        {
            Code.NotNull(list, "list");

            while (count-- > 0)
            {
                // ReSharper disable once PossibleMultipleEnumeration
                foreach (T value in list)
                {
                    yield return value;
                }
            }
        }

        /// <summary>
        /// Replaces items in the specified list.
        /// </summary>
        /// <param name="list">The list to process to.</param>
        /// <param name="replaceFrom">Item to replace from.</param>
        /// <param name="replaceTo">Replacement.</param>
        /// <returns>List with replaced items.</returns>
        [NotNull]
        public static IEnumerable<T> Replace<T>
            (
                [NotNull] IEnumerable<T> list,
                [CanBeNull] T replaceFrom,
                [CanBeNull] T replaceTo
            )
            where T : IEquatable<T>
        {
            Code.NotNull(list, "list");

            foreach (T item in list)
            {
                if (item.Equals(replaceFrom))
                {
                    yield return replaceTo;
                }
                else
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Extracts segment from the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>Segment.</returns>
        [NotNull]
        public static IEnumerable<T> Segment<T>
            (
                [NotNull] IEnumerable<T> list,
                int offset,
                int count
            )
        {
            Code.NotNull(list, "list");
            Code.Nonnegative(offset, "offset");
            Code.Nonnegative(count, "count");

            int index = 0;
            foreach (T obj in list)
            {
                if (index < offset)
                {
                    index++;
                }
                else if (count > 0)
                {
                    yield return obj;
                    count--;
                }
                else
                {
                    break;
                }
            }
        }

        #endregion
    }
}
