// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ArrayUtility.cs -- array manipulation helpers
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using UnsafeAM.Logging;

using System;
using System.Collections.Generic;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM
{
    /// <summary>
    /// <see cref="Array"/> manipulation helper methods.
    /// </summary>
    [PublicAPI]
    public static class ArrayUtility
    {
        #region Public methods

        /// <summary>
        /// Changes type of given array to the specified type.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <typeparam name="TFrom">Type of source array.</typeparam>
        /// <typeparam name="TTo">Type of destination array.</typeparam>
        /// <returns>Allocated array with converted items.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sourceArray"/> is <c>null</c>.
        /// </exception>
        [NotNull]
        public static TTo[] ChangeType<TFrom, TTo>
            (
                [NotNull] TFrom[] sourceArray
            )
        {
            Code.NotNull(sourceArray, "sourceArray");

            TTo[] result = new TTo[sourceArray.Length];
            Array.Copy(sourceArray, result, sourceArray.Length);

            return result;
        }

        /// <summary>
        /// Changes type of given array to the specified type.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <typeparam name="TTo">Type of destination array.</typeparam>
        /// <returns>Allocated array with converted items.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sourceArray"/> is <c>null</c>.
        /// </exception>
        [NotNull]
        public static TTo[] ChangeType<TTo>
            (
                [NotNull] Array sourceArray
            )
        {
            Code.NotNull(sourceArray, "sourceArray");

            TTo[] result = new TTo[sourceArray.Length];
            Array.Copy(sourceArray, result, sourceArray.Length);

            return result;
        }

        /// <summary>
        /// Clone the array.
        /// </summary>
        public static T[] Clone<T>
            (
                [NotNull] T[] array
            )
            where T : ICloneable
        {
            Code.NotNull(array, "array");

            T[] result = (T[])array.Clone();

            for (int i = 0; i < array.Length; i++)
            {
                result[i] = (T)array[i].Clone();
            }

            return result;
        }

        /// <summary>
        /// Whether segment of first array
        /// coincides with segment of second array.
        /// </summary>
        public static bool Coincide<T>
            (
                [NotNull] T[] firstArray,
                int firstOffset,
                [NotNull] T[] secondArray,
                int secondOffset,
                int length
            )
            where T : IEquatable<T>
        {
            Code.NotNull(firstArray, "firstArray");
            Code.NotNull(secondArray, "secondArray");
            Code.Nonnegative(firstOffset, "firstOffset");
            Code.Nonnegative(secondOffset, "secondOffset");
            Code.Nonnegative(length, "length");

            // Совпадают ли два куска массивов?
            // Куски нулевой длины считаются совпадающими.

            for (int i = 0; i < length; i++)
            {
                T first = firstArray[firstOffset + i];
                T second = secondArray[secondOffset + i];
                if (!first.Equals(second))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares two specified arrays by elements.
        /// </summary>
        /// <param name="firstArray">First array to compare.</param>
        /// <param name="secondArray">Second array to compare.</param>
        /// <returns><para>Less than zero - first array is less.</para>
        /// <para>Zero - arrays are equal.</para>
        /// <para>Greater than zero - first array is greater.</para>
        /// </returns>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="firstArray"/> or
        /// <paramref name="secondArray"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">Length of
        /// <paramref name="firstArray"/> is not equal to length of
        /// <paramref name="secondArray"/>.
        /// </exception>
        public static int Compare<T>
            (
                [NotNull] T[] firstArray,
                [NotNull] T[] secondArray
            )
            where T : IComparable<T>
        {
            Code.NotNull(firstArray, "firstArray");
            Code.NotNull(secondArray, "secondArray");

            if (firstArray.Length
                 != secondArray.Length)
            {
                Log.Error
                    (
                        "ArrayUtility::Compare: "
                        + "length not equal"
                    );

                throw new ArgumentException();
            }

            for (int i = 0; i < firstArray.Length; i++)
            {
                int result = firstArray[i].CompareTo(secondArray[i]);
                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        /// <summary>
        /// Converts the specified array.
        /// </summary>
        [NotNull]
        public static TTo[] Convert<TFrom, TTo>
            (
                [NotNull] TFrom[] array
            )
        {
            Code.NotNull(array, "array");

            TTo[] result = new TTo[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                result[i] = ConversionUtility.ConvertTo<TTo>(array[i]);
            }

            return result;
        }

        /// <summary>
        /// Creates the array of specified length initializing it with
        /// specified value.
        /// </summary>
        /// <param name="length">Desired length of the array.</param>
        /// <param name="initialValue">The initial value of
        /// array items.</param>
        /// <returns>Created and initialized array.</returns>
        /// <typeparam name="T">Type of array item.</typeparam>
        [NotNull]
        public static T[] Create<T>
            (
                int length,
                T initialValue
            )
        {
            Code.Nonnegative(length, "length");

            T[] result = new T[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = initialValue;
            }

            return result;
        }

        /// <summary>
        /// Выборка элемента из массива.
        /// </summary>
        /// <remarks>
        /// Возможна отрицательная нумерация
        /// (означает индекс с конца массива).
        /// При выходе за границы массива
        /// выдаётся значение по умолчанию.
        /// </remarks>
        [CanBeNull]
        public static T GetOccurrence<T>
            (
                [NotNull] this T[] array,
                int occurrence
            )
        {
            Code.NotNull(array, "array");

            int length = array.Length;

            occurrence = occurrence >= 0
                ? occurrence
                : length + occurrence;

            T result = default(T);

            if (length != 0
                && occurrence >= 0
                && occurrence < length)
            {
                result = array[occurrence];
            }

            return result;
        }

        /// <summary>
        /// Выборка элемента из массива.
        /// </summary>
        /// <remarks>
        /// Возможна отрицательная нумерация
        /// (означает индекс с конца массива).
        /// При выходе за границы массива
        /// выдаётся значение по умолчанию.
        /// </remarks>
        [CanBeNull]
        public static T GetOccurrence<T>
            (
                [NotNull] this T[] array,
                int occurrence,
                [CanBeNull] T defaultValue
            )
        {
            Code.NotNull(array, "array");

            int length = array.Length;

            occurrence = occurrence >= 0
                ? occurrence
                : length + occurrence;

            T result = defaultValue;

            if (length != 0
                && occurrence >= 0
                && occurrence < length)
            {
                result = array[occurrence];
            }

            return result;
        }

        /// <summary>
        /// Get span of the array.
        /// </summary>
        [NotNull]
        public static T[] GetSpan<T>
            (
                [NotNull] this T[] array,
                int offset,
                int count
            )
        {
            Code.NotNull(array, "array");
            Code.Nonnegative(offset, "offset");
            Code.Nonnegative(count, "count");

            if (offset > array.Length)
            {
                return new T[0];
            }
            if (offset + count > array.Length)
            {
                count = array.Length - offset;
            }
            if (count <= 0)
            {
                return new T[0];
            }

            T[] result = new T[count];
            Array.Copy(array, offset, result, 0, count);

            return result;
        }

        /// <summary>
        /// Get span of the array.
        /// </summary>
        [NotNull]
        public static T[] GetSpan<T>
            (
                [NotNull] this T[] array,
                int offset
            )
        {
            Code.NotNull(array, "array");
            Code.Nonnegative(offset, "offset");

            if (offset >= array.Length)
            {
                return new T[0];
            }

            int count = array.Length - offset;
            T[] result = array.GetSpan(offset, count);

            return result;
        }

        /// <summary>
        /// Searches for the specified pattern
        /// and returns the index of its first occurrence
        /// in a one-dimensional array.
        /// </summary>
        public static int IndexOf
            (
                [NotNull] byte[] data,
                [NotNull] byte[] pattern
            )
        {
            Code.NotNull(data, "data");
            Code.NotNull(pattern, "pattern");

            int patternLength = pattern.Length;
            int dataLength = data.Length - patternLength;
            if (patternLength == 0 || dataLength < 0)
            {
                return -1;
            }

            for (int i = 0; i <= dataLength; i++)
            {
                bool found = true;
                for (int j = 0; j < patternLength; j++)
                {
                    if (data[i + j] != pattern[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Determines whether the specified array is null or empty
        /// (has zero length).
        /// </summary>
        /// <param name="array">Array to check.</param>
        /// <returns><c>true</c> if the array is null or empty;
        /// otherwise, <c>false</c>.
        /// </returns>
        [AssertionMethod]
        public static bool IsNullOrEmpty
            (
                [AssertionCondition(AssertionConditionType.IS_NOT_NULL)]
                [CanBeNull] Array array
            )
        {
            return ReferenceEquals(array, null)
                     || array.Length == 0;
        }

        /// <summary>
        /// Merges the specified arrays.
        /// </summary>
        /// <param name="arrays">Arrays to merge.</param>
        /// <returns>Array that consists of all <paramref name="arrays"/>
        /// items.</returns>
        /// <typeparam name="T">Type of array item.</typeparam>
        /// <exception cref="ArgumentNullException">
        /// At least one of <paramref name="arrays"/> is <c>null</c>.
        /// </exception>
        [NotNull]
        public static T[] Merge<T>
            (
                [NotNull] params T[][] arrays
            )
        {
            int resultLength = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                if (ReferenceEquals(arrays[i], null))
                {
                    Log.Error
                        (
                            "ArrayUtility::Merge: "
                            + "array["
                            + i
                            + "] is null"
                        );

                    throw new ArgumentNullException("arrays");
                }
                resultLength += arrays[i].Length;
            }

            T[] result = new T[resultLength];
            int offset = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                arrays[i].CopyTo(result, offset);
                offset += arrays[i].Length;
            }

            return result;
        }

        /// <summary>
        /// Get range of the array.
        /// </summary>
        [NotNull]
        public static T[] Range<T>
            (
                [NotNull] T[] input,
                int offset,
                int length
            )
        {
            // TODO GetSpan?

            Code.NotNull(input, "input");
            Code.Nonnegative(offset, "offset");

            int inputLength = input.Length;
            if (offset > inputLength || length <= 0)
            {
                return EmptyArray<T>.Value;
            }

            if (offset + length > inputLength)
            {
                length = inputLength - offset;
            }

            T[] result = new T[length];
            Array.Copy(input, offset, result, 0, length);

            return result;
        }

        /// <summary>
        /// Безопасное вычисление длины массива.
        /// </summary>
        public static int SafeLength
            (
                [CanBeNull] this Array array
            )
        {
            return ReferenceEquals(array, null) ? 0 : array.Length;
        }

        /// <summary>
        /// Разбиение массива на (почти) равные части.
        /// </summary>
        [NotNull]
        public static T[][] SplitArray<T>
            (
                [NotNull] T[] array,
                int partCount
            )
        {
            Code.NotNull(array, "array");
            Code.Positive(partCount, "partCount");

            List<T[]> result = new List<T[]>(partCount);
            int length = array.Length;
            int chunkSize = length / partCount;
            while (chunkSize * partCount < length)
            {
                chunkSize++;
            }
            int offset = 0;
            for (int i = 0; i < partCount; i++)
            {
                int size = Math.Min(chunkSize, length - offset);
                T[] chunk = new T[size];
                Array.Copy(array, offset, chunk, 0, size);
                result.Add(chunk);
                offset += size;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Converts to string array using
        /// <see cref="object.ToString"/> method.
        /// </summary>
        public static string[] ToString<T>
            (
                [NotNull] T[] array
            )
        {
            Code.NotNull(array, "array");

            string[] result = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                object o = array[i];
                if (o != null)
                {
                    result[i] = array[i].ToString();
                }
            }

            return result;
        }

        #endregion
    }
}
