// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Utility.cs -- bunch of useful routines.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

using AM.Logging;

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

#if FW45

        private const MethodImplOptions Aggressive
            = MethodImplOptions.AggressiveInlining;

#else

        private const MethodImplOptions Aggressive
            = (MethodImplOptions)0;

#endif

        #endregion

        #region Public methods

        // =========================================================

        /// <summary>
        /// Шестнадцатиричный дамп массива байт.
        /// </summary>
        public static string DumpBytes
            (
                byte[] buffer
            )
        {
            StringBuilder result = new StringBuilder(buffer.Length * 5);

            int offset;

            for (offset = 0; offset < buffer.Length; offset += 16)
            {
                result.AppendFormat
                    (
                        "{0:X6}:",
                        offset
                    );

                int run = Math.Min(buffer.Length - offset, 16);

                for (int i = 0; i < run; i++)
                {
                    result.AppendFormat
                        (
                            " {0:X2}",
                            buffer[offset + i]
                        );
                }

                result.AppendLine();
            }

            return result.ToString();
        }

        // =========================================================

        /// <summary>
        /// Compare two sequences.
        /// </summary>
        /// <remarks>Borrowed from StackOverflow:
        /// http://stackoverflow.com/questions/1680602/what-is-the-algorithm-used-by-the-memberwise-equality-test-in-net-structs
        /// </remarks>
        public static bool EnumerableEquals
            (
                [CanBeNull] IEnumerable left,
                [CanBeNull] IEnumerable right
            )
        {
            if (ReferenceEquals(left, null)
                || ReferenceEquals(right, null))
            {
                return false;
            }
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            IEnumerator rightEnumerator = right.GetEnumerator();
            rightEnumerator.Reset();

            foreach (object leftItem in left)
            {
                // unequal amount of items
                if (!rightEnumerator.MoveNext())
                {
                    return false;
                }
                {
                    if (!MemberwiseEquals
                        (
                            leftItem,
                            rightEnumerator.Current
                        ))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Aggregate hashcode for some objects.
        /// </summary>
        /// <remarks>Borrowed from Tom DuPont:
        /// http://www.tomdupont.net/2014/02/how-to-combine-hashcodes.html
        /// </remarks>
        public static int GetHashCodeAggregate<T>
            (
                [NotNull] this IEnumerable<T> source
            )
        {
            return GetHashCodeAggregate(source, 17);
        }

        /// <summary>
        /// Aggregate hashcode for some objects.
        /// </summary>
        /// <remarks>Borrowed from Tom DuPont:
        /// http://www.tomdupont.net/2014/02/how-to-combine-hashcodes.html
        /// </remarks>
        public static int GetHashCodeAggregate<T>
            (
                [NotNull] this IEnumerable<T> source,
                int hash
            )
        {
            unchecked
            {
                foreach (var item in source)
                {
                    if (!ReferenceEquals(item, null))
                    {
                        hash = hash * 31 + item.GetHashCode();
                    }
                }
            }

            return hash;
        }

        /// <summary>
        /// Implementation of a memberwise comparison
        /// for objects.
        /// </summary>
        /// <remarks>Borrowed from StackOverflow:
        /// http://stackoverflow.com/questions/1680602/what-is-the-algorithm-used-by-the-memberwise-equality-test-in-net-structs
        /// </remarks>
        public static bool MemberwiseEquals
            (
                [CanBeNull] object left,
                [CanBeNull] object right
            )
        {
            if (ReferenceEquals(left, null)
                || ReferenceEquals(right, null))
            {
                return false;
            }

            if (ReferenceEquals(left, right))
            {
                return true;
            }

            Type type = left.GetType();
            if (type != right.GetType())
            {
                return false;
            }

            if (TypeUtility.IsValueType(type))
            {
                return left.Equals(right);
            }

#if !WIN81 && !PORTABLE

            if (type == type.GetMethod("Equals").DeclaringType)
            {
                return left.Equals(right);
            }

#endif
            IEnumerable leftEnumerable = left as IEnumerable;
            IEnumerable rightEnumerable = right as IEnumerable;
            if (!ReferenceEquals(leftEnumerable, null))
            {
                return EnumerableEquals
                    (
                        leftEnumerable,
                        rightEnumerable
                    );
            }

#if !WIN81 && !PORTABLE

            // compare each property
            foreach (PropertyInfo info in type.GetProperties
                (
                    BindingFlags.Public
                    | BindingFlags.NonPublic
                    | BindingFlags.Instance
#if !UAP
                    | BindingFlags.GetProperty
#endif
                ))
            {
                // TODO: need to special-case indexable properties
                if (!MemberwiseEquals
                    (
                        info.GetValue(left, null),
                        info.GetValue(right, null)
                    ))
                {
                    return false;
                }
            }

            // compare each field
            foreach (FieldInfo info in type.GetFields
                (
#if !UAP
                    BindingFlags.GetField |
#endif
                    BindingFlags.NonPublic
                    | BindingFlags.Public
                    | BindingFlags.Instance
                ))
            {
                if (!MemberwiseEquals
                    (
                        info.GetValue(left),
                        info.GetValue(right))
                )
                {
                    return false;
                }
            }

#endif

            return true;
        }

        /// <summary>
        /// Compares two object by public instance properties.
        /// </summary>
        /// <remarks>Borrowed from StackOverflow:
        /// http://stackoverflow.com/questions/506096/comparing-object-properties-in-c-sharp
        /// </remarks>
        public static bool PropertyEquals
            (
                [CanBeNull] object left,
                [CanBeNull] object right
            )
        {
            if (ReferenceEquals(left, null)
                || ReferenceEquals(right, null))
            {
                return false;
            }
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            Type type = left.GetType();
            if (type != right.GetType())
            {
                return false;
            }

#if WIN81 || PORTABLE

            throw new NotImplementedException();

#else

            PropertyInfo[] properties = type.GetProperties
                (
                    BindingFlags.Public
                    | BindingFlags.Instance
                );

            foreach (PropertyInfo property in properties)
            {
                object leftValue = property.GetValue(left, null);
                object rightValue = property.GetValue(right, null);

                if (ReferenceEquals(leftValue, null)
                    || ReferenceEquals(rightValue, null))
                {
                    return false;
                }

                if (!leftValue.Equals(rightValue))
                {
                    return false;
                }
            }

#endif

            return true;
        }

        // =========================================================

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

            index = index >= 0
                ? index
                : array.Length + index;
            T result = index >= 0 && index < array.Length
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

            index = index >= 0
                ? index
                : list.Count + index;
            T result = index >= 0 && index < list.Count
                ? list[index]
                : defaultValue;

            return result;
        }

        /// <summary>
        /// Выборка элемента из массива.
        /// </summary>
        public static T GetItem<T>
            (
                [NotNull] this IList<T> list,
                int index
            )
        {
            return GetItem(list, index, default(T));
        }

        /// <summary>
        /// Determines whether is one of the specified values.
        /// </summary>
        public static bool IsOneOf<T>
            (
                T value,
                params T[] array
            )
            where T : IComparable<T>
        {
            foreach (T one in array)
            {
                if (value.CompareTo(one) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        ///// <summary>
        ///// Determines whether given object
        ///// is default value.
        ///// </summary>
        //[MethodImpl(Aggressive)]
        //public static bool NotDefault<T>
        //    (
        //        this T obj
        //    )
        //{
        //    return !EqualityComparer<T>.Default.Equals
        //        (
        //            obj,
        //            default(T)
        //        );
        //}

        ///// <summary>
        ///// Returns given value instead of
        ///// default(T) if happens.
        ///// </summary>
        //[MethodImpl(Aggressive)]
        //public static T NotDefault<T>
        //    (
        //        this T obj,
        //        T value
        //    )
        //{
        //    return EqualityComparer<T>.Default.Equals
        //        (
        //            obj,
        //            default(T)
        //        )
        //        ? value
        //        : obj;
        //}

        /// <summary>
        /// Преобразование любого значения в строку.
        /// </summary>
        /// <returns>Для <c>null</c> возвращается <c>null</c>.
        /// </returns>
        [CanBeNull]
        public static string NullableToString<T>
            (
                [CanBeNull] this T value
            )
            where T : class
        {
            return ReferenceEquals(value, null)
                ? null
                : value.ToString();
        }

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/>
        /// if given value is <c>null</c>.
        /// </summary>
        [NotNull]
        [DebuggerStepThrough]
        [MethodImpl(Aggressive)]
        public static T ThrowIfNull<T>
            (
                [CanBeNull] this T value
            )
            where T : class
        {
            if (ReferenceEquals(value, null))
            {
                Log.Error
                    (
                        "Utility::ThrowIfNull"
                    );

                throw new ArgumentException("value");
            }

            return value;
        }

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/>
        /// if given value is <c>null</c>.
        /// </summary>
        [NotNull]
        [DebuggerStepThrough]
        [MethodImpl(Aggressive)]
        public static T1 ThrowIfNull<T1, T2>
            (
                [CanBeNull] this T1 value
            )
            where T1 : class
            where T2 : Exception, new()
        {
            if (ReferenceEquals(value, null))
            {
                Log.Error
                    (
                        "Utility::ThrowIfNull"
                    );

                throw new T2();
            }

            return value;
        }

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/>
        /// if given value is <c>null</c>.
        /// </summary>
        [NotNull]
        [DebuggerStepThrough]
        [MethodImpl(Aggressive)]
        public static T ThrowIfNull<T>
            (
                [CanBeNull] this T value,
                [NotNull] string message
            )
            where T : class
        {
            Code.NotNull(message, "message");

            if (ReferenceEquals(value, null))
            {
                Log.Error
                    (
                        "Utility::ThrowIfNull: "
                        + message
                    );

                throw new ArgumentException(message);
            }

            return value;
        }

        /// <summary>
        /// Преобразование любого значения в строку.
        /// </summary>
        /// <returns>Для <c>null</c> возвращается "(null)".
        /// </returns>
        [NotNull]
        public static string ToVisibleString<T>
            (
                [CanBeNull] this T value
            )
        {
            if (ReferenceEquals(value, null))
            {
                return "(null)";
            }

            string result = value.ToString();

            return result.ToVisibleString();
        }

        #endregion
    }
}
