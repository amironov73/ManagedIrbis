// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NumericUtility.cs -- helper routines for numeric values
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// Helper methods for numeric values.
    /// </summary>
    [PublicAPI]
    public static class NumericUtility
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

        /// <summary>
        /// Преобразование набора целых чисел в строковое представление,
        /// учитывающее возможное наличие цепочек последовательных чисел,
        /// которые форматируются как диапазоны.
        /// </summary>
        /// <param name="n">Источник целых чисел.</param>
        /// <remarks>Источник должен поддерживать многократное считывание.
        /// Числа предполагаются предварительно упорядоченные. Повторения чисел
        /// не допускаются. Пропуски в последовательностях допустимы.
        /// Числа допускаются только неотрицательные.
        /// </remarks>
        /// <returns>Строковое представление набора чисел.</returns>
        [NotNull]
        public static string CompressRange
            (
                [CanBeNull] IEnumerable<int> n
            )
        {
            // TODO rewrite without .Any()

            if (ReferenceEquals(n, null))
            {
                return string.Empty;
            }

            // ReSharper disable PossibleMultipleEnumeration
            if (!n.Any())
            {
                return String.Empty;
            }

            var result = new StringBuilder();
            var first = true;
            var previous = n.First();
            var last = previous;
            foreach (var i in n.Skip(1))
            {
                if (i != last + 1)
                {
                    result.AppendFormat("{0}{1}", first ? "" : ", ",
                        FormatRange(previous, last));
                    previous = i;
                    first = false;
                }
                last = i;
            }
            result.AppendFormat("{0}{1}", first ? "" : ", ",
                FormatRange(previous, last));

            return result.ToString();
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Форматирование диапазона целых чисел.
        /// </summary>
        /// <remarks>Границы диапазона могут совпадать, однако
        /// левая не должна превышать правую.</remarks>
        /// <param name="first">Левая граница диапазона.</param>
        /// <param name="last">Правая граница диапазона.</param>
        /// <returns>Строковое представление диапазона.</returns>
        public static string FormatRange
            (
                int first,
                int last
            )
        {
            if (first == last)
            {
                return first.ToInvariantString();
            }
            if (first == last - 1)
            {
                return first.ToInvariantString() 
                    + ", " 
                    + last.ToInvariantString();
            }

            return first.ToInvariantString() 
                + "-" 
                + last.ToInvariantString();
        }

        /// <summary>
        /// Представляет ли строка положительное целое число.
        /// </summary>
        /// <param name="text"></param>
        [MethodImpl(Aggressive)]
        public static bool IsPositiveInteger
            (
                this string text
            )
        {
            return text.SafeToInt32() > 0;
        }

        /// <summary>
        /// One of many?
        /// </summary>
        public static bool OneOf
            (
                this int one,
                params int[] many
            )
        {
            foreach (int i in many)
            {
                if (i == one)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// One of many?
        /// </summary>
        public static bool OneOf
            (
                this int one,
                [NotNull] IEnumerable<int> many
            )
        {
            Code.NotNull(many, "many");

            foreach (int i in many)
            {
                if (i == one)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Parse decimal in standard manner.
        /// </summary>
        [MethodImpl(Aggressive)]
        public static decimal ParseDecimal
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            decimal result = decimal.Parse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture
                );

            return result;
        }

        /// <summary>
        /// Parse double in standard manner.
        /// </summary>
        [MethodImpl(Aggressive)]
        public static double ParseDouble
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            double result = double.Parse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture
                );

            return result;
        }

        /// <summary>
        /// Parse short integer in standard manner.
        /// </summary>
        [MethodImpl(Aggressive)]
        public static short ParseInt16
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            short result = short.Parse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture
                );

            return result;
        }

        /// <summary>
        /// Parse integer in standard manner.
        /// </summary>
        [MethodImpl(Aggressive)]
        public static int ParseInt32
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            int result = int.Parse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture
                );

            return result;
        }

        /// <summary>
        /// Parse long integer in standard manner.
        /// </summary>
        [MethodImpl(Aggressive)]
        public static long ParseInt64
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            long result = long.Parse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture
                );

            return result;
        }

        /// <summary>
        /// Безопасное преобразование строки
        /// в число с фиксированной точкой.
        /// </summary>
        public static decimal SafeToDecimal
            (
                [CanBeNull] this string text,
                decimal defaultValue
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return defaultValue;
            }

            decimal result;

#if WINMOBILE || PocketPC

            try
            {
                result = decimal.Parse(text);
            }
            catch (Exception)
            {
                result = defaultValue;
            }

#else

            if (!TryParseDecimal(text, out result))
            {
                result = defaultValue;
            }

#endif

            return result;
        }

        /// <summary>
        /// Безопасное преобразование строки в целое.
        /// </summary>
        public static int SafeToInt32
            (
                [CanBeNull] this string text,
                int defaultValue,
                int minValue,
                int maxValue
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return defaultValue;
            }

            int result;

#if WINMOBILE || PocketPC

            try
            {
                result = int.Parse(text);
            }
            catch (Exception)
            {
                result = defaultValue;
            }

#else

            if (!TryParseInt32(text, out result))
            {
                result = defaultValue;
            }

#endif

            if (result < minValue
                || result > maxValue)
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Безопасное преобразование строки в целое.
        /// </summary>
        public static int SafeToInt32
            (
                [CanBeNull] this string text,
                int defaultValue
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return defaultValue;
            }

            int result;

#if WINMOBILE || PocketPC

            try
            {
                result = int.Parse(text);
            }
            catch (Exception)
            {
                result = defaultValue;
            }

#else

            if (!TryParseInt32(text, out result))
            {
                result = defaultValue;
            }

#endif

            return result;
        }

        /// <summary>
        /// Безопасное преобразование строки в целое.
        /// </summary>
        public static int SafeToInt32
            (
                [CanBeNull] this string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            int result;

#if WINMOBILE || PocketPC

            try
            {
                result = int.Parse(text);
            }
            catch (Exception)
            {
                result = 0;
            }
#else

            if (!TryParseInt32(text, out result))
            {
                result = 0;
            }

#endif

            return result;
        }

        /// <summary>
        /// Безопасное преобразование строки в целое.
        /// </summary>
        public static long SafeToInt64
            (
                [CanBeNull] this string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            long result;

#if WINMOBILE || PocketPC

            try
            {
                result = long.Parse(text);
            }
            catch (Exception)
            {
                result = 0;
            }
#else

            if (!TryParseInt64(text, out result))
            {
                result = 0;
            }

#endif

            return result;
        }

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной 
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString
            (
                this short value
            )
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной 
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString
            (
                this int value
            )
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной 
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        [NotNull]
        public static string ToInvariantString
            (
                this double value
            )
        {
            return value.ToString
                (
                    CultureInfo.InvariantCulture
                );
        }

        /// <summary>
        /// Convert double to string using InvariantCulture.
        /// </summary>
        [NotNull]
        public static string ToInvariantString
            (
                this double value,
                [NotNull] string format
            )
        {
            Code.NotNullNorEmpty(format, "format");

            return value.ToString
                (
                    format,
                    CultureInfo.InvariantCulture
                );
        }

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной 
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        [NotNull]
        public static string ToInvariantString
            (
                this decimal value
            )
        {
            return value.ToString
                (
                    CultureInfo.InvariantCulture
                );
        }

        /// <summary>
        /// Convert decimal value to string using InvariantCulture.
        /// </summary>
        [NotNull]
        public static string ToInvariantString
            (
                this decimal value,
                [NotNull] string format
            )
        {
            Code.NotNullNorEmpty(format, "format");

            return value.ToString
                (
                    format,
                    CultureInfo.InvariantCulture
                );
        }

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной 
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString
            (
                this long value
            )
        {
            return value.ToString
                (
                    CultureInfo.InvariantCulture
                );
        }

        /// <summary>
        /// Convert to <see cref="System.String"/>
        /// using <see cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToInvariantString
            (
                this char value
            )
        {
            //return value.ToString(CultureInfo.InvariantCulture);
            return value.ToString();
        }

        /// <summary>
        /// Try parse decimal value in standard manner.
        /// </summary>
        public static bool TryParseDecimal
            (
                [CanBeNull] string text,
                out decimal value
            )
        {
            Code.NotNullNorEmpty(text, "text");

#if WINMOBILE || PocketPC

            bool result = false;

            try
            {
                value = decimal.Parse
                    (
                        text,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture
                    );

                result = true;
            }
            catch
            {
                value = 0;
            }

            return result;

#else

            bool result = decimal.TryParse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value
                );

            return result;

#endif
        }

        /// <summary>
        /// Try parse double precision value in standard manner.
        /// </summary>
        public static bool TryParseDouble
            (
                [CanBeNull] string text,
                out double value
            )
        {
#if WINMOBILE || PocketPC

            bool result = false;

            try
            {
                value = double.Parse
                    (
                        text,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture
                    );

                result = true;
            }
            catch
            {
                value = 0;
            }

            return result;

#else

            bool result = double.TryParse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value
                );

            return result;

#endif
        }

        /// <summary>
        /// Try parse single precision value in standard manner.
        /// </summary>
        public static bool TryParseFloat
            (
                [CanBeNull] string text,
                out float value
            )
        {
            Code.NotNullNorEmpty(text, "text");

#if WINMOBILE || PocketPC

            bool result = false;

            try
            {
                value = float.Parse
                    (
                        text,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture
                    );

                result = true;
            }
            catch
            {
                value = 0;
            }

            return result;

#else

            bool result = float.TryParse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value
                );

            return result;

#endif
        }

        /// <summary>
        /// Try parse integer in standard manner.
        /// </summary>
        public static bool TryParseInt16
            (
                [CanBeNull] string text,
                out short value
            )
        {
            Code.NotNullNorEmpty(text, "text");

#if WINMOBILE || PocketPC

            bool result = false;

            try
            {
                value = short.Parse
                    (
                        text,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture
                    );

                result = true;
            }
            catch
            {
                value = 0;
            }

            return result;

#else

            bool result = short.TryParse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value
                );

            return result;

#endif
        }

        /// <summary>
        /// Try to parse unsigned integer in standard manner.
        /// </summary>
        [CLSCompliant(false)]
        public static bool TryParseUInt16
            (
                [CanBeNull] string text,
                out ushort value
            )
        {
            Code.NotNullNorEmpty(text, "text");

#if WINMOBILE || PocketPC

            bool result = false;

            try
            {
                value = ushort.Parse
                    (
                        text,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture
                    );

                result = true;
            }
            catch
            {
                value = 0;
            }

            return result;

#else

            bool result = ushort.TryParse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value
                );

            return result;

#endif
        }

        /// <summary>
        /// Try parse integer in standard manner.
        /// </summary>
        public static bool TryParseInt32
            (
                [CanBeNull] string text,
                out int value
            )
        {
#if WINMOBILE || PocketPC

            bool result = false;

            try
            {
                value = int.Parse
                    (
                        text,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture
                    );

                result = true;
            }
            catch
            {
                value = 0;
            }

            return result;

#else

            bool result = int.TryParse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value
                );

            return result;

#endif
        }

        /// <summary>
        /// Try to parse unsigned integer in standard manner.
        /// </summary>
        [CLSCompliant(false)]
        public static bool TryParseUInt32
            (
                [CanBeNull] string text,
                out uint value
            )
        {
            Code.NotNullNorEmpty(text, "text");

#if WINMOBILE || PocketPC

            bool result = false;

            try
            {
                value = uint.Parse
                    (
                        text,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture
                    );

                result = true;
            }
            catch
            {
                value = 0;
            }

            return result;

#else

            bool result = uint.TryParse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value
                );

            return result;

#endif
        }

        /// <summary>
        /// Try parse integer in standard manner.
        /// </summary>
        public static bool TryParseInt64
            (
                [CanBeNull] string text,
                out long value
            )
        {
            Code.NotNullNorEmpty(text, "text");

#if WINMOBILE || PocketPC

            bool result = false;

            try
            {
                value = long.Parse
                    (
                        text,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture
                    );

                result = true;
            }
            catch
            {
                value = 0;
            }

            return result;

#else

            bool result = long.TryParse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value
                );

            return result;

#endif
        }

        /// <summary>
        /// Try to parse unsigned integer in standard manner.
        /// </summary>
        [CLSCompliant(false)]
        public static bool TryParseUInt64
            (
                [CanBeNull] string text,
                out ulong value
            )
        {
            Code.NotNullNorEmpty(text, "text");

#if WINMOBILE || PocketPC

            bool result = false;

            try
            {
                value = ulong.Parse
                    (
                        text,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture
                    );

                result = true;
            }
            catch
            {
                value = 0;
            }

            return result;

#else

            bool result = ulong.TryParse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value
                );

            return result;

#endif
        }

        #endregion
    }
}

