/* Utility.cs -- bunch of useful routines.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// Bunch of useful routines.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class Utility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Выборка элемента из массива.
        /// </summary>
        public static T GetItem<T>
            (
                [NotNull] this T[] array,
                int index,
                [CanBeNull] T defaultValue
            )
        {
            Code.NotNull(array, "array");

            index = (index >= 0)
                ? index
                : array.Length + index;
            T result = ((index >= 0) && (index < array.Length))
                ? array[index]
                : defaultValue;

            return result;
        }

        /// <summary>
        /// Выборка элемента из массива.
        /// </summary>
        public static T GetItem<T>
            (
                [NotNull] this T[] array,
                int index
            )
        {
            return GetItem(array, index, default(T));
        }

        /// <summary>
        /// Выборка элемента из списка.
        /// </summary>
        public static T GetItem<T>
            (
                [NotNull] this IList<T> list,
                int index,
                [CanBeNull] T defaultValue
            )
        {
            Code.NotNull(list, "list");

            index = (index >= 0)
                ? index
                : list.Count + index;
            T result = ((index >= 0) && (index < list.Count))
                ? list[index]
                : defaultValue;

            return result;
        }

        /// <summary>
        /// Выборка элемента из массива.
        /// </summary>
        public static T GetItem<T>
            (
                [NotNull] this IList<T>  list,
                int index
            )
        {
            return GetItem(list, index, default(T));
        }

        /// <summary>
        /// Determines whether given object
        /// is default value.
        /// </summary>
        public static bool NotDefault<T>
            (
                this T obj
            )
        {
            return !EqualityComparer<T>.Default.Equals
                (
                    obj,
                    default(T)
                );
        }

        /// <summary>
        /// Returns given value instead of
        /// default(T) if happens.
        /// </summary>
        public static T NotDefault<T>
            (
                this T obj,
                T value
            )
        {
            return EqualityComparer<T>.Default.Equals
                (
                    obj,
                    default(T)
                )
                ? value
                : obj;
        }

        #endregion
    }
}
