// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConversionUtility.cs -- set of type conversion routines.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Reflection;
using System.Text;

using UnsafeAM.Logging;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM
{
    /// <summary>
    /// Type conversion helpers.
    /// </summary>
    public static class ConversionUtility
    {
        #region Private members

        private static readonly char[] ALPHABET = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz"
            .ToCharArray();
        private static readonly int BASE_58 = ALPHABET.Length;
        private static readonly int BASE_256 = 256;
        private static readonly int[] INDEXES = new int[128];

        private static byte divmod58(byte[] number, int startAt)
        {
            int remainder = 0;
            for (int i = startAt; i < number.Length; i++)
            {
                int digit256 = number[i] & 0xFF;
                int temp = remainder * BASE_256 + digit256;

                number[i] = (byte) (temp / BASE_58);

                remainder = temp % BASE_58;
            }

            return (byte) remainder;
        }

        private static byte divmod256(byte[] number58, int startAt)
        {
            int remainder = 0;
            for (int i = startAt; i < number58.Length; i++)
            {
                int digit58 = (int) number58[i] & 0xFF;
                int temp = remainder * BASE_58 + digit58;

                number58[i] = (byte) (temp / BASE_256);

                remainder = temp % BASE_256;
            }

            return (byte) remainder;
        }

        #endregion

        #region Construction

        static ConversionUtility()
        {
            for (int i = 0; i < INDEXES.Length; i++)
            {
                INDEXES[i] = -1;
            }

            for (int i = 0; i < ALPHABET.Length; i++)
            {
                INDEXES[ALPHABET[i]] = i;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether given value can be converted to
        /// the specified type.
        /// </summary>
        /// <param name="value">Value to be converted.</param>
        /// <returns>
        /// <c>true</c> if value can be converted;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool CanConvertTo<T>
            (
                [CanBeNull] object value
            )
        {
            if (value != null)
            {
                Type sourceType = value.GetType();
                Type targetType = typeof(T);

                if (targetType == sourceType)
                {
                    return true;
                }

                if (targetType.IsAssignableFrom(sourceType))
                {
                    return true;
                }

                IConvertible convertible = value as IConvertible;
                if (convertible != null)
                {
                    return true; // ???
                }

                //TypeConverter converterFrom
                //    = TypeDescriptor.GetConverter(value);
                //if (converterFrom.CanConvertTo(targetType))
                //{
                //    return true;
                //}

                //TypeConverter converterTo
                //    = TypeDescriptor.GetConverter(targetType);
                //if (converterTo.CanConvertFrom(sourceType))
                //{
                //    return true;
                //}
            }
            return false;
        }

        /// <summary>
        /// Converts given value to the specified type.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <returns>Converted value.</returns>
        public static T ConvertTo<T>
            (
                [CanBeNull] object value
            )
        {
            if (value == null)
            {
                return default(T);
            }

            Type sourceType = value.GetType();
            Type targetType = typeof(T);

            if (targetType == typeof(string))
            {
                return (T)(object)value.ToString();
            }

            if (targetType.IsAssignableFrom(sourceType))
            {
                return (T)value;
            }

            IConvertible convertible = value as IConvertible;
            if (convertible != null)
            {
                return (T)Convert.ChangeType(value, targetType);
            }

            //TypeConverter converterFrom
            //    = TypeDescriptor.GetConverter(value);
            //if (converterFrom.CanConvertTo(targetType))
            //{
            //    return (T)converterFrom.ConvertTo
            //                (
            //                    value,
            //                    targetType
            //                );
            //}

            //TypeConverter converterTo
            //    = TypeDescriptor.GetConverter(targetType);
            //if (converterTo.CanConvertFrom(sourceType))
            //{
            //    return (T)converterTo.ConvertFrom(value);
            //}

            //foreach (MethodInfo miOpChangeType in
            //    sourceType.GetMethods(BindingFlags.Public | BindingFlags.Static))
            //{
            //    if (miOpChangeType.IsSpecialName
            //         && (miOpChangeType.Name == "op_Implicit"
            //              || miOpChangeType.Name == "op_Explicit"
            //            )
            //         && miOpChangeType.ReturnType.IsAssignableFrom(targetType)
            //        )
            //    {
            //        ParameterInfo[] psOpChangeType = miOpChangeType.GetParameters();
            //        if ((psOpChangeType.Length == 1)
            //             && (psOpChangeType[0].ParameterType == sourceType)
            //            )
            //        {
            //            return (T)miOpChangeType.Invoke(null, new [] { value });
            //        }
            //    }
            //}

            throw new ArsMagnaException();
        }

        //
        // https://ru.wikipedia.org/wiki/Base58
        //
        // Base58 - вариант кодирования цифрового кода в виде
        // буквенно-цифрового текста на основе латинского алфавита.
        // Алфавит кодирования содержит 58 символов. Применяется
        // для передачи данных в разнородных сетях (транспортное
        // кодирование). Стандарт похож на Base64, но отличается тем,
        // что в результатах нет не только служебных кодов,
        // но и алфавитно-цифровых символов, которые могут человеком
        // восприниматься неоднозначно. Исключены 0 (ноль),
        // O (заглавная латинская o), I (заглавная латинская i),
        // l (маленькая латинская L).
        // Также исключены символы + (плюс) и / (косая черта),
        // которые при кодировании URL могут приводить
        // к неверной интерпретации.
        //
        // Стандарт был разработан для уменьшения визуальной путаницы
        // у пользователей, которые вручную вводят данные на основе
        // распечатанного текста или фотографии, т.е. без возможности
        // машинного копирования и вставки.
        //
        // В отличие от Base64, при кодировании не сохраняется
        // однозначное побайтное соответствие с исходными данными
        // (разные комбинации одинакового количества байт кодируются
        // строкой с разной длиной символов). По этой причине,
        // способ хорошо подходит для кодирования больших целых чисел,
        // но не предназначены для кодирования более длинных частей
        // двоичных данных.
        //

        /// <summary>
        /// Converts the specified string, which encodes binary data
        /// as base-58 digits, to an equivalent 8-bit unsigned
        /// integer array.
        /// </summary>
        public static byte[] FromBase58String
            (
                [NotNull] string input
            )
        {
            Code.NotNull(input, "input");

            if (input.Length == 0)
            {
                return EmptyArray<byte>.Value;
            }

            byte[] input58 = new byte[input.Length];

            // Transform the String to a base58 byte sequence
            for (int i = 0; i < input.Length; ++i)
            {
                char c = input[i];

                int digit58 = -1;
                if (c < 128)
                {
                    digit58 = INDEXES[c];
                }
                if (digit58 < 0)
                {
                    throw new Exception("Not a Base58 input: " + input);
                }

                input58[i] = (byte) digit58;
            }

            // Count leading zeroes
            int zeroCount = 0;
            while (zeroCount < input58.Length && input58[zeroCount] == 0)
            {
                ++zeroCount;
            }

            // The encoding
            byte[] temp = new byte[input.Length];
            int j = temp.Length;

            int startAt = zeroCount;
            while (startAt < input58.Length)
            {
                byte mod = divmod256(input58, startAt);
                if (input58[startAt] == 0) {
                    ++startAt;
                }

                temp[--j] = mod;
            }

            // Do no add extra leading zeroes, move j to first non null byte
            while (j < temp.Length && temp[j] == 0)
            {
                ++j;
            }

            int start = j - zeroCount;

            return ArrayUtility.Range(temp, start, temp.Length - start);
        }

        /// <summary>
        /// Converts an array of 8-bit unsigned integers
        /// to its equivalent string representation that
        /// is encoded with base-58 digits.
        /// </summary>
        [NotNull]
        public static string ToBase58String
            (
                [NotNull] byte[] input
            )
        {
            Code.NotNull(input, "input");

            int inputLength = input.Length;
            if (inputLength == 0)
            {
                return string.Empty;
            }

            // Count leading zeroes
            int zeroCount = 0;
            while (zeroCount < inputLength && input[zeroCount] == 0)
            {
                zeroCount++;
            }

            // The actual encoding
            byte[] temp = new byte[inputLength * 2];
            int j = temp.Length;

            int startAt = zeroCount;
            while (startAt < inputLength)
            {
                byte mod = divmod58(input, startAt);
                if (input[startAt] == 0)
                {
                    ++startAt;
                }

                temp[--j] = (byte) ALPHABET[mod];
            }

            // Strip extra '1' if any
            while (j < temp.Length && temp[j] == ALPHABET[0])
            {
                j++;
            }

            // Add as many leading '1' as there were leading zeros.
            while (--zeroCount >= 0)
            {
                temp[--j] = (byte) ALPHABET[0];
            }

            string result = Encoding.ASCII.GetString(temp, j, temp.Length-j);

            return result;
        }

        /// <summary>
        /// Converts given object to boolean value.
        /// </summary>
        /// <param name="value">Object to be converted.</param>
        /// <returns>Converted value.</returns>
        /// <exception cref="FormatException">
        /// Value can't be converted.
        /// </exception>
        public static bool ToBoolean
            (
                [NotNull] object value
            )
        {
            Code.NotNull(value, "value");

            if (value is bool)
            {
                return (bool)value;
            }

            //if (value is bool?)
            //{
            //    return ((bool?)value).Value;
            //}

            bool result;

            if (bool.TryParse(value as string, out result))
            {
                return result;
            }

            string svalue = value as string;
            if (svalue == "false"
                 || svalue == "0"
                 || svalue == "no"
                 || svalue == "n"
                 || svalue == "off"
                 || svalue == "negative"
                 || svalue == "neg"
                 || svalue == "disabled"
                 || svalue == "incorrect"
                 || svalue == "wrong"
                 || svalue == "нет"
                )
            {
                return false;
            }

            if (svalue == "true"
                 || svalue == "1"
                 || svalue == "yes"
                 || svalue == "y"
                 || svalue == "on"
                 || svalue == "positiva"
                 || svalue == "pos"
                 || svalue == "enabled"
                 || svalue == "correct"
                 || svalue == "right"
                 || svalue == "да"
                )
            {
                return true;
            }

            if (value is int
                 || value is uint
                 || value is byte
                 || value is sbyte)
            {
                int intValue = (int)value;
                return intValue != 0;
            }

            if (value is long
                 || value is ulong
                )
            {
                long longValue = (long)value;
                return longValue != 0L;
            }

            if (value is decimal)
            {
                decimal doubleValue = (decimal)value;
                return doubleValue != 0m;
            }

            if (value is float
                 || value is double)
            {
                double doubleValue = (double)value;

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                return doubleValue != 0.0;
            }

            Log.Error
                (
                    "ConversionUtility::ToBoolean: "
                    + "bad value="
                    + value
                );

            throw new FormatException
                (
                    "Bad value " + value
                );
        }

        #endregion
    }
}
