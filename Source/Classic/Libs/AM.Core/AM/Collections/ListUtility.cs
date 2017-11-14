// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ListUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// Useful routines for <see cref="IList{T}"/>.
    /// </summary>
    /// <remarks>Borrowed from Json.NET.</remarks>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ListUtility
    {
        #region Public methods

        /// <summary>
        /// Add to list if don't have yet.
        /// </summary>
        public static bool AddDistinct<T>
            (
                [NotNull] this IList<T> list,
                T value
            )
        {
            return list.AddDistinct
                (
                    value,
                    EqualityComparer<T>.Default
                );
        }

        /// <summary>
        /// Add to list if don't have yet.
        /// </summary>
        public static bool AddDistinct<T>
            (
                [NotNull] this IList<T> list,
                T value,
                [NotNull] IEqualityComparer<T> comparer
            )
        {
            Code.NotNull(list, "list");
            Code.NotNull(comparer, "comparer");

            if (list.ContainsValue(value, comparer))
            {
                return false;
            }

            list.Add(value);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool AddRangeDistinct<T>
            (
                [NotNull] this IList<T> list,
                [NotNull] IEnumerable<T> values,
                [NotNull] IEqualityComparer<T> comparer
            )
        {
            Code.NotNull(list, "list");
            Code.NotNull(values, "values");
            Code.NotNull(comparer, "comparer");

            bool allAdded = true;
            foreach (T value in values)
            {
                if (!list.AddDistinct(value, comparer))
                {
                    allAdded = false;
                }
            }

            return allAdded;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool ContainsValue<TSource>
            (
                [NotNull] this IEnumerable<TSource> source,
                TSource value,
                [NotNull] IEqualityComparer<TSource> comparer
            )
        {
            Code.NotNull(comparer, "comparer");
            Code.NotNull(source, "source");

            foreach (TSource local in source)
            {
                if (comparer.Equals(local, value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public static int IndexOf<T>
            (
                [NotNull] this IEnumerable<T> collection,
                [NotNull] Func<T, bool> predicate
            )
        {
            Code.NotNull(collection, "collection");
            Code.NotNull(predicate, "predicate");

            int index = 0;
            foreach (T value in collection)
            {
                if (predicate(value))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

        /// <summary>
        /// Is the list is <c>null</c> or empty?
        /// </summary>
        public static bool IsNullOrEmpty<T>
            (
                [CanBeNull] this IList<T> list
            )
        {
            if (!ReferenceEquals(list, null))
            {
                return list.Count == 0;
            }

            return true;
        }

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/>
        /// if the list is <c>null</c> or empty.
        /// </summary>
        [NotNull]
        public static IList<T> ThrowIfNullOrEmpty<T>
            (
                [CanBeNull] this IList<T> list
            )
        {
            if (ReferenceEquals(list, null))
            {
                Log.Error
                    (
                        "ListUtility::ThrowIfNullOrEmpty: "
                        + "list is null"
                    );

                throw new ArgumentNullException();
            }

            if (list.Count == 0)
            {
                Log.Error
                    (
                        "ListUtility::ThrowIfNullOrEmpty: "
                        + "list is empty"
                    );

                throw new ArgumentException();
            }

            return list;
        }

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/>
        /// if the list is <c>null</c> or empty.
        /// </summary>
        [NotNull]
        public static IList<T> ThrowIfNullOrEmpty<T>
            (
                [CanBeNull] this IList<T> list,
                [NotNull] string message
            )
        {
            Code.NotNullNorEmpty(message, "message");

            if (ReferenceEquals(list, null))
            {
                Log.Error
                (
                    "ListUtility::ThrowIfNullOrEmpty: "
                    + "list is null"
                );

                throw new ArgumentNullException(message);
            }

            if (list.Count == 0)
            {
                Log.Error
                (
                    "ListUtility::ThrowIfNullOrEmpty: "
                    + "list is empty"
                );

                throw new ArgumentException(message);
            }

            return list;
        }

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/>
        /// if the array is <c>null</c> or empty.
        /// </summary>
        [NotNull]
        public static T[] ThrowIfNullOrEmpty<T>
            (
                [CanBeNull] this T[] array
            )
        {
            if (ReferenceEquals(array, null))
            {
                Log.Error
                (
                    "ListUtility::ThrowIfNullOrEmpty: "
                    + "array is null"
                );

                throw new ArgumentNullException();
            }

            if (array.Length == 0)
            {
                Log.Error
                (
                    "ListUtility::ThrowIfNullOrEmpty: "
                    + "array is empty"
                );

                throw new ArgumentException();
            }

            return array;
        }

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/>
        /// if the array is <c>null</c> or empty.
        /// </summary>
        [NotNull]
        public static T[] ThrowIfNullOrEmpty<T>
            (
                [CanBeNull] this T[] array,
                [NotNull] string message
            )
        {
            Code.NotNullNorEmpty(message, "message");

            if (ReferenceEquals(array, null))
            {
                Log.Error
                (
                    "ListUtility::ThrowIfNullOrEmpty: "
                    + "array is null"
                );

                throw new ArgumentNullException(message);
            }

            if (array.Length == 0)
            {
                Log.Error
                (
                    "ListUtility::ThrowIfNullOrEmpty: "
                    + "array is empty"
                );

                throw new ArgumentException(message);
            }

            return array;
        }

        #endregion
    }
}
