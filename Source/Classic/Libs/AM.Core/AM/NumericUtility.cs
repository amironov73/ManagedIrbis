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

            if (n == null)
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
                if (i != (last + 1))
                {
                    result.AppendFormat("{0}{1}", (first ? "" : ", "),
                        FormatRange(previous, last));
                    previous = i;
                    first = false;
                }
                last = i;
            }
            result.AppendFormat("{0}{1}", (first ? "" : ", "),
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
        /// <returns></returns>
        public static bool IsPositiveInteger
            (
                this string text
            )
        {
            return text.SafeToInt32() > 0;
        }

        /// <summary>
        /// Parse integer in standard manner.
        /// </summary>
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

            if ((result < minValue)
                || (result > maxValue))
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
                [NotNull] string text,
                out decimal value
            )
        {
            Code.NotNullNorEmpty(text, "text");

            bool result = decimal.TryParse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value
                );

            return result;
        }

        /// <summary>
        /// Try parse double precision value in standard manner.
        /// </summary>
        public static bool TryParseDouble
            (
                [NotNull] string text,
                out double value
            )
        {
            Code.NotNullNorEmpty(text, "text");

            bool result = double.TryParse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value
                );

            return result;
        }

        /// <summary>
        /// Try parse single precision value in standard manner.
        /// </summary>
        public static bool TryParseFloat
            (
                [NotNull] string text,
                out float value
            )
        {
            Code.NotNullNorEmpty(text, "text");

            bool result = float.TryParse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value
                );

            return result;
        }

        /// <summary>
        /// Try parse integer in standard manner.
        /// </summary>
        public static bool TryParseInt16
            (
                [NotNull] string text,
                out short value
            )
        {
            Code.NotNullNorEmpty(text, "text");

            bool result = short.TryParse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value
                );

            return result;
        }

        /// <summary>
        /// Try parse integer in standard manner.
        /// </summary>
        public static bool TryParseInt32
            (
                [NotNull] string text,
                out int value
            )
        {
            Code.NotNullNorEmpty(text, "text");

            bool result = int.TryParse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value
                );

            return result;
        }

        /// <summary>
        /// Try parse integer in standard manner.
        /// </summary>
        public static bool TryParseInt64
            (
                [NotNull] string text,
                out long value
            )
        {
            Code.NotNullNorEmpty(text, "text");

            bool result = long.TryParse
                (
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value
                );

            return result;
        }

        #endregion
    }
}

