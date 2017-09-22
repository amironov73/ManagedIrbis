// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StringUtility.cs -- string manipulation routines
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

using AM.Text;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// String manipulation routines.
    /// </summary>
    [PublicAPI]
    public static class StringUtility
    {
        #region Properties or fields

        /// <summary>
        /// Empty array of <see cref="string"/>.
        /// </summary>
        [NotNull]
        public static readonly string[] EmptyArray = new string[0];

        #endregion

        #region Private members

#if FW45

        private const MethodImplOptions Aggressive
            = MethodImplOptions.AggressiveInlining;

#else

        private const MethodImplOptions Aggressive
            = (MethodImplOptions)0;

#endif

        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                                     + "abcdefghijklmnopqrstuvwxyz";

        private const string Symbols = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                                       + "abcdefghijklmnopqrstuvwxyz"
                                       + "0123456789"
                                       + "_";

        private static readonly Random _random = new Random();
        private static readonly char[] _quotes = { '\'', '"', '[', ']' };

        #endregion

        #region Public methods

        /// <summary>
        /// Conditionally concatenates strings.
        /// </summary>
        /// <param name="to">Destination string.</param>
        /// <param name="what">Tail.</param>
        /// <returns>Resulting string.</returns>
        public static string CCat
            (
                [NotNull] this string to,
                string what
            )
        {
            return to.EndsWith(what)
                ? to
                : string.Concat(to, what);
        }

        /// <summary>
        /// Conditionally concatenates strings.
        /// </summary>
        /// <param name="to">Destination string.</param>
        /// <param name="end">Existing tail.</param>
        /// <param name="what">Tail to concat.</param>
        /// <returns>Resulting string.</returns>
        public static string CCat
            (
                [NotNull] this string to,
                string end,
                string what
            )
        {
            return to.EndsWith(end)
                ? to
                : string.Concat(to, what);
        }

        /// <summary>
        /// Changes the encoding of given string from one to other.
        /// </summary>
        /// <param name="fromEncoding">From encoding.</param>
        /// <param name="toEncoding">To encoding.</param>
        /// <param name="value">String to transcode.</param>
        /// <returns>Transcoded string</returns>
        [NotNull]
        public static string ChangeEncoding
            (
                [NotNull] Encoding fromEncoding,
                [NotNull] Encoding toEncoding,
                [NotNull] string value
            )
        {
            Code.NotNull(fromEncoding, "fromEncoding");
            Code.NotNull(toEncoding, "toEncoding");
            Code.NotNull(value, "value");

            if (fromEncoding.Equals(toEncoding))
            {
                return value;
            }

            byte[] bytes = fromEncoding.GetBytes(value);
            string result = EncodingUtility.GetString
                (
                    toEncoding,
                    bytes
                );

            return result;
        }

        /// <summary>
        /// Сравнивает две строки с точностью до регистра символов.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool CompareNoCase
            (
                string left,
                string right
            )
        {
            //return (string.Compare
            //            (left,
            //              right,
            //              true,
            //              _InvariantCulture) == 0);

            return CultureInfo.InvariantCulture.CompareInfo.Compare
                   (
                       left,
                       right,
                       CompareOptions.IgnoreCase
                   ) == 0;
        }

        /// <summary>
        /// Сравнивает два символа с точностью до регистра.
        /// </summary>
        public static bool CompareNoCase
            (
                char left,
                char right
            )
        {
            //return (char.ToUpper(left, _InvariantCulture)
            //         == char.ToUpper(right, _InvariantCulture));
            return (char.ToUpper(left)
                    == char.ToUpper(right));
        }

        /// <summary>
        /// Состоит ли строка только из указанного символа.
        /// </summary>
        public static bool ConsistOf
            (
                [CanBeNull] this string value,
                char c
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            foreach (char c1 in value)
            {
                if (c1 != c)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Состоит ли строка только из указанных символов.
        /// </summary>
        public static bool ConsistOf
            (
                [CanBeNull] this string value,
                params char[] array
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            foreach (char c in value)
            {
                if (Array.IndexOf(array, c) < 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Определяет, состоит ли строка только из цифр.
        /// </summary>
        public static bool ConsistOfDigits
            (
                string value,
                int from,
                int to
            )
        {
            if (string.IsNullOrEmpty(value)
                || @from >= value.Length)
            {
                return false;
            }
            to = Math.Min(to, value.Length);
            for (int i = @from; i < to; i++)
            {
                if (!char.IsDigit(value[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Определяет, состоит ли строка только из цифр.
        /// </summary>
        public static bool ConsistOfDigits
            (
                string value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

#if PORTABLE

            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;

#else

            return value.All(char.IsDigit);

#endif
        }

        /// <summary>
        /// Содержит ли строка любой из перечисленных символов.
        /// </summary>
        public static bool ContainsAnySymbol
            (
                [CanBeNull] this string text,
                params char[] symbols
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                foreach (char c in text)
                {
                    if (symbols.Contains(c))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the text contains specified character.
        /// </summary>
        /// <remarks>
        /// For portable library.
        /// </remarks>
        public static bool ContainsCharacter
            (
                [CanBeNull] this string text,
                char symbol
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                foreach (char c in text)
                {
                    if (c == symbol)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Count of given substrings in the text.
        /// </summary>
        public static int CountSubstrings
            (
                [CanBeNull] this string text,
                [CanBeNull] string substring
            )
        {
            int result = 0;

            if (!string.IsNullOrEmpty(text)
            && !string.IsNullOrEmpty(substring))
            {
                int length = substring.Length;
                int offset = 0;

                while (true)
                {
                    int index = text.IndexOf
                        (
                            substring,
                            offset,
                            StringComparison.OrdinalIgnoreCase
                        );
                    if (index < 0)
                    {
                        break;
                    }
                    result++;
                    offset = index + length;
                }
            }

            return result;
        }

        /// <summary>
        /// Get case-insensitive string comparer.
        /// </summary>
        public static IEqualityComparer<string> GetCaseInsensitiveComparer()
        {
#if NETCORE || UAP || WIN81 || PORTABLE

            return StringComparer.OrdinalIgnoreCase;

#else

            return StringComparer.InvariantCultureIgnoreCase;

#endif
        }

        /// <summary>
        /// Get positions of the symbol.
        /// </summary>
        [NotNull]
        public static int[] GetPositions
            (
                [CanBeNull] this string text,
                char c
            )
        {
            List<int> result = new List<int>();

            if (!string.IsNullOrEmpty(text))
            {
                int start = 0;
                int length = text.Length;

                while (start < length)
                {
                    int position = text.IndexOf(c, start);
                    if (position < 0)
                    {
                        break;
                    }
                    result.Add(position);
                    start = position + 1;
                }
            }

            return result.ToArray();
        }

        private static int HexToInt
            (
                char h
            )
        {
            return h >= '0' && h <= '9' 
                ? h - '0' 
                : h >= 'a' && h <= 'f'
                    ? h - 'a' + 10 
                    : h >= 'A' && h <= 'F'
                        ? h - 'A' + 10 
                        : -1;
        }

        private static char IntToHex
            (
                int n
            )
        {
            if (n <= 9)
            {
                return (char)(n + '0');
            }

            return (char)(n - 10 + 'A');
        }

        /// <summary>
        /// 
        /// </summary>
        public static string IfEmpty
            (
                this string first,
                params string[] others
            )
        {
            if (!string.IsNullOrEmpty(first))
            {
                return first;
            }
            return others
                .FirstOrDefault(s => !string.IsNullOrEmpty(s));
        }

        /// <summary>
        /// Check whether string is blank (consist of spaces) or empty.
        /// </summary>
        /// <param name="value">String.</param>
        /// <returns><c>true</c> if string is empty.</returns>
        public static bool IsBlank
            (
                [CanBeNull] this string value
            )
        {
            return string.IsNullOrEmpty(value)
                   || value.Trim().Length == 0;
        }

        /// <summary>
        /// Проверяет, можно ли трактовать строку как десятичное число.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDecimal
            (
                [NotNull] string value
            )
        {
            Code.NotNull(value, "value");

#if WINMOBILE || PocketPC

            try
            {
                decimal.Parse(value);
            }
            catch (Exception)
            {
                return false;
            }

            return true;

#else

            decimal temp;

            return decimal.TryParse
            (
                value,
                NumberStyles.AllowDecimalPoint
                | NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,
                out temp
            );

#endif
        }

        /// <summary>
        /// Checks whether one can convert given string to 32-bit 
        /// signed integer value.
        /// </summary>
        /// <param name="value">String to check.</param>
        /// <returns><c>true</c> if the string can be converted;
        /// <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is <c>null</c>.
        /// </exception>
        public static bool IsInteger
            (
                [NotNull] string value
            )
        {
            Code.NotNull(value, "value");

#if WINMOBILE || PocketPC

            try
            {
                int.Parse(value);
            }
            catch (Exception)
            {
                return false;
            }

            return true;

#else

            int temp;

            return int.TryParse
                (
                    value,
                    NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture,
                    out temp
                );

#endif
        }

        /// <summary>
        /// Checks whether one can convert given string to 64-bit 
        /// signed integer value.
        /// </summary>
        /// <param name="value">String to check.</param>
        /// <returns><c>true</c> if the string can be converted;
        /// <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is <c>null</c>.
        /// </exception>
        public static bool IsLongInteger
            (
                [NotNull] string value
            )
        {
            Code.NotNull(value, "value");

#if WINMOBILE || PocketPC

            try
            {
                long.Parse(value);
            }
            catch (Exception)
            {
                return false;
            }

            return true;

#else

            long temp;

            return long.TryParse
                (
                    value,
                    NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture,
                    out temp
                );

#endif
        }

        /// <summary>
        /// Проверяет, можно ли трактовать строку как число с плавающей 
        /// точкой (двойной точности).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric
            (
                [NotNull] string value
            )
        {
            Code.NotNull(value, "value");

#if WINMOBILE || PocketPC

            try
            {
                double.Parse(value);
            }
            catch (Exception)
            {
                return false;
            }

            return true;

#else

            double temp;

            return double.TryParse
            (
                value,
                NumberStyles.AllowDecimalPoint
                | NumberStyles.AllowExponent
                | NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,
                out temp
            );

#endif
        }

        /// <summary>
        /// Checks whether one can convert given string to 16-bit 
        /// signed integer value.
        /// </summary>
        /// <param name="value">String to check.</param>
        /// <returns><c>true</c> if the string can be converted;
        /// <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is <c>null</c>.
        /// </exception>
        public static bool IsShortInteger
            (
                [NotNull] string value
            )
        {
            Code.NotNull(value, "value");

#if WINMOBILE || PocketPC

            try
            {
                short.Parse(value);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
#else

            short temp;

            return short.TryParse
                (
                    value,
                    NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture,
                    out temp
                );

#endif
        }

        /// <summary>
        /// Replicates given string.
        /// </summary>
        /// <param name="text">String to replicate.</param>
        /// <param name="times">How many times.</param>
        /// <returns>Replicated string.</returns>
        /// <remarks><c>Replicate ( null, AnyNumber )</c>
        /// yields <c>null</c>.
        /// </remarks>
        [CanBeNull]
        public static string Replicate
            (
                [CanBeNull] string text,
                int times
            )
        {
            if (text == null)
            {
                return null;
            }

            int length = text.Length * times;
            if (length < 0)
            {
                length = 0;
            }

            StringBuilder result = new StringBuilder(length);
            for (; times > 0; times--)
            {
                result.Append(text);
            }

            return result.ToString();
        }

        /// <summary>
        /// Creates random string with given length.
        /// </summary>
        /// <param name="length">Desired length of string</param>
        public static string Random
            (
                int length
            )
        {
            StringBuilder result = new StringBuilder(length);

            if (length > 0)
            {
                result.Append(Chars[_random.Next(Chars.Length)]);
            }
            for (; length > 1; length--)
            {
                result.Append(Symbols[_random.Next(Symbols.Length)]);
            }

            return result.ToString();
        }


        /// <summary>
        /// Builds an array is regex matches.
        /// </summary>
        /// <param name="input">String to parse.</param>
        /// <param name="pattern">Regex.</param>
        /// <returns>Array of matches.</returns>
        public static string[] GetMatches
            (
                string input,
                string pattern
            )
        {
            MatchCollection matches = Regex.Matches(input, pattern);
            string[] result = new string[matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                result[i] = matches[i].Value;
            }

            return result;
        }

        /// <summary>
        /// Строит массив слов в строке.
        /// </summary>
        /// <param name="value">Разбираемая строка.</param>
        /// <returns>Массив слов.</returns>
        public static string[] GetWords
            (
                string value
            )
        {
            return GetMatches(value, @"\w+");
        }

        /// <summary>
        /// Строит массив групп совпадающих символов по регулярному выражению.
        /// </summary>
        /// <param name="input">Разбираемая строка.</param>
        /// <param name="pattern">Регулярное выражение.</param>
        /// <returns>Массив групп.</returns>
        [NotNull]
        public static string[] GetGroups
            (
                [NotNull] string input,
                [NotNull] string pattern
            )
        {
            Code.NotNull(input, "input");
            Code.NotNull(pattern, "pattern");

            Match match = Regex.Match(input, pattern);
            if (!match.Success)
            {
                return new string[0];
            }

            string[] result = new string[match.Groups.Count - 1];
            for (int i = 1; i < match.Groups.Count; i++)
            {
                result[i - 1] = match.Groups[i].Value;
            }

            return result;
        }

        /// <summary>
        /// Сравнивает строки с точностью до регистра.
        /// </summary>
        /// <param name="one">Первая строка.</param>
        /// <param name="two">Вторая строка.</param>
        /// <returns>Строки совпадают с точностью до регистра.</returns>
        public static bool SameString
            (
                this string one,
                string two
            )
        {
            return string.Compare
                   (
                       one,
                       two,
                       StringComparison.OrdinalIgnoreCase
                   ) == 0;
        }

        /// <summary>
        /// Сравнивает строки.
        /// </summary>
        public static bool SameStringSensitive
            (
                this string one,
                string two
            )
        {
            return string.Compare
                   (
                       one,
                       two,
                       StringComparison.Ordinal
                   ) == 0;
        }

        #region Sparce

        private const string after = ",.])!?:;";
        private const string before = "[(";
        private const string both = ""; // Тут был минус

        /// <summary>
        /// Sparse the string.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Sparse string.</returns>
        [CanBeNull]
        public static string Sparse
            (
                [CanBeNull] string input
            )
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            input = input.Trim();
            if (input.Length == 0)
            {
                return input;
            }

            StringBuilder output = new StringBuilder(input.Length);
            bool quot = false;
            bool apos = false;

            for (int i = 0; i < input.Length; i++)
            {
                char chr = input[i];
                char next = (i < (input.Length - 1))
                                ? input[i + 1]
                                : '\0';
                char prev = (i > 0)
                                ? input[i - 1]
                                : '\0';
                if (chr == ' ')
                {
                    if (next == ' ')
                    {
                        continue;
                    }
                    if (after.IndexOf(next) >= 0)
                    {
                        continue;
                    }
                }
                else if (chr == '"')
                {
                    if (quot)
                    {
                        output.Append(chr);
                        if ((next != ' ')
                             && (after.IndexOf(next) == -1))
                        {
                            output.Append(' ');
                        }
                    }
                    else
                    {
                        if (prev != ' ')
                        {
                            output.Append(' ');
                        }
                        output.Append(chr);
                    }
                    quot = !quot;
                    continue;
                }
                else if (chr == '\'')
                {
                    if (apos)
                    {
                        output.Append(chr);
                        if ((next != ' ')
                             && (after.IndexOf(next) == -1))
                        {
                            output.Append(' ');
                        }
                    }
                    else
                    {
                        if (prev != ' ')
                        {
                            output.Append(' ');
                        }
                        output.Append(chr);
                    }
                    apos = !apos;
                    continue;
                }
                else if (both.IndexOf(chr) >= 0)
                {
                    if (prev != ' ')
                    {
                        output.Append(' ');
                    }
                    output.Append(chr);
                    if (next != ' ')
                    {
                        output.Append(' ');
                    }
                    continue;
                }
                else if (before.IndexOf(chr) >= 0)
                {
                    if ((prev != ' ') && (i > 0)
                         && (before.IndexOf(next) == -1)
                         && (after.IndexOf(next) == -1))
                    {
                        output.Append(' ');
                    }
                }
                else if (after.IndexOf(chr) >= 0)
                {
                    output.Append(chr);
                    if (next == '-')
                    {
                        continue;
                    }
                    if (char.IsDigit(prev)
                         && char.IsDigit(next))
                    {
                        continue;
                    }
                    if ((next != ' ')
                         && (before.IndexOf(next) == -1)
                         && (after.IndexOf(next) == -1))
                    {
                        output.Append(' ');
                    }
                    {
                        continue;
                    }
                }
                output.Append(chr);
            }

            return output.ToString().TrimEnd();
        }

        #endregion

        /// <summary>
        /// Разбивает строку по указанному разделителю.
        /// </summary>
        public static string[] SplitFirst
            (
                [NotNull] this string line,
                char delimiter
            )
        {
            int index = line.IndexOf(delimiter);
            string[] result = (index < 0)
                ? new[] { line }
                : new[]
                {
                    line.Substring(0, index),
                    line.Substring(index + 1)
                };

            return result;
        }

        /// <summary>
        /// Превращает строку в видимую.
        /// Пример: "(null)".
        /// </summary>
        [NotNull]
        [MethodImpl(Aggressive)]
        public static string ToVisibleString
            (
                [CanBeNull] this string text
            )
        {
            if (ReferenceEquals(text, null))
            {
                return "(null)";
            }
            if (string.IsNullOrEmpty(text))
            {
                return "(empty)";
            }
#if FW40
            if (string.IsNullOrWhiteSpace(text))
            {
                return "(whitespace)";
            }
#endif

            return text;
        }

        /// <summary>
        /// Преобразует строку, содержащую escape-последовательности, 
        /// к нормальному виду.
        /// </summary>
        /// <param name="value">Исходная строка.</param>
        /// <returns>Результирующая строка.</returns>
        public static string Unescape
            (
                string value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder(value.Length);
            int current = 0;
            int next;

            sb.Length = 0;
            do
            {
                int index = value.IndexOf('%', current);

                if ((value.Length - index) < 3)
                {
                    index = -1;
                }

                next = index == -1
                        ? value.Length
                        : index;

                sb.Append(value.Substring(current, next - current));
                if (index != -1)
                {
                    byte[] bytes = new byte[value.Length - index];

#if CLASSIC || ANDROID

                    int byteCount = 0;
                    char ch;
                    
                    do
                    {
                        // Not supported in .NET Core
                        ch = Uri.HexUnescape(value, ref next);
                        if (ch < '\x80')
                        {
                            break;
                        }
                        bytes[byteCount++] = (byte)ch;
                    }
                    while (next < value.Length);

                    if (byteCount != 0)
                    {
                        int charCount = Encoding.UTF8.GetCharCount
                            (bytes, 0, byteCount);

                        if (charCount != 0)
                        {
                            char[] chrs = new char[value.Length - index];
                            Encoding.UTF8.GetChars
                                (bytes,
                                  0,
                                  byteCount,
                                  chrs,
                                  0);
                            sb.Append(Chars, 0, charCount);
                        }
                        else
                        {
                            for (int i = 0; i < byteCount; ++i)
                            {
                                sb.Append((char)bytes[i]);
                            }
                        }
                    }

                    if (ch < '\x80')
                    {
                        sb.Append(ch);
                    }
#endif
                }

                current = next;
            }
            while (next < value.Length);

            return sb.ToString();
        }


        /// <summary>
        /// Converts empty string to <c>null</c>.
        /// </summary>
        public static string EmptyToNull
            (
                this string value
            )
        {
            return string.IsNullOrEmpty(value)
                       ? null
                       : value;
        }

        /// <summary>
        /// Gets the first char of the text.
        /// </summary>
        public static char FirstChar
            (
                this string text
            )
        {
            return string.IsNullOrEmpty(text)
                ? '\0'
                : text[0];
        }

        /// <summary>
        /// Gets the last char of the text.
        /// </summary>
        public static char LastChar
            (
                this string text
            )
        {
            return string.IsNullOrEmpty(text)
                       ? '\0'
                       : text[text.Length - 1];
        }

        /// <summary>
        /// Trims matching open and close quotes from the string.
        /// </summary>
        /// <remarks>
        /// <code>Unquote("(text)");</code>
        /// </remarks>
        [NotNull]
        public static string Unquote
            (
                [NotNull] this string text
            )
        {
            return Unquote(text, '"');
        }

        /// <summary>
        /// Trims matching open and close quotes from the string.
        /// </summary>
        /// <remarks>
        /// <code>Unquote("(text)", '"');</code>
        /// </remarks>
        [NotNull]
        public static string Unquote
            (
                [NotNull] this string text,
                char quoteChar
            )
        {
            Code.NotNull(text, "text");

            int length = text.Length;
            if (length > 1)
            {
                if ((text[0] == quoteChar)
                    && (text[length - 1] == quoteChar))
                {
                    text = text.Substring(1, length - 2);
                }
            }

            return text;
        }

        /// <summary>
        /// Trims matching open and close quotes from the string.
        /// </summary>
        /// <remarks>
        /// <code>Unquote("(text)", '(', ')');</code>
        /// </remarks>
        [NotNull]
        public static string Unquote
            (
                [NotNull] this string text,
                char quoteChar1,
                char quoteChar2
            )
        {
            Code.NotNull(text, "text");

            int length = text.Length;
            if (length > 1)
            {
                if ((text[0] == quoteChar1)
                    && (text[length - 1] == quoteChar2))
                {
                    text = text.Substring(1, length - 2);
                }
            }

            return text;
        }

        /// <summary>
        /// Проверяет, является ли искомая строка одной
        /// из перечисленных. Регистр символов не учитывается.
        /// </summary>
        /// <param name="one">Искомая строка.</param>
        /// <param name="many">Источник проверяемых строк.</param>
        /// <returns>Найдена ли искомая строка.</returns>
        public static bool OneOf
            (
                this string one,
                IEnumerable<string> many
            )
        {
            return many
                .Any(_ => _.SameString(one));
        }

        /// <summary>
        /// Проверяет, является ли искомая строка одной
        /// из перечисленных. Регистр символов не учитывается.
        /// </summary>
        /// <param name="one">Искомая строка.</param>
        /// <param name="many">Массив проверяемых строк.</param>
        /// <returns>Найдена ли искомая строка.</returns>
        public static bool OneOf
            (
               this string one,
               params string[] many
            )
        {
            return one.OneOf(many.AsEnumerable());
        }

#if PORTABLE

        /// <summary>
        /// Проверяет, является ли искомый символ одним
        /// из перечисленных. Регистр символов не учитывается.
        /// </summary>
        /// <param name="one">Искомый символ.</param>
        /// <param name="many">Массив проверяемых символов.</param>
        /// <returns>Найден ли искомый символ.</returns>
        public static bool OneOf
            (
                this char one,
                [NotNull] string many
            )
        {
            return many.ToCharArray()
                .Any(_ => _.SameChar(one));
        }

#endif

        /// <summary>
        /// Проверяет, является ли искомый символ одним
        /// из перечисленных. Регистр символов не учитывается.
        /// </summary>
        /// <param name="one">Искомый символ.</param>
        /// <param name="many">Массив проверяемых символов.</param>
        /// <returns>Найден ли искомый символ.</returns>
        public static bool OneOf
            (
                this char one,
                IEnumerable<char> many
            )
        {
            return many
                .Any(_ => _.SameChar(one));
        }

        /// <summary>
        /// Проверяет, является ли искомый символ одним
        /// из перечисленных. Регистр символов не учитывается.
        /// </summary>
        /// <param name="one">Искомый символ.</param>
        /// <param name="many">Массив проверяемых символов.</param>
        /// <returns>Найден ли искомый символ.</returns>
        public static bool OneOf
            (
                this char one,
                params char[] many
            )
        {
            return one.OneOf(many.AsEnumerable());
        }

        /// <summary>
        /// Сравнивает символы с точностью до регистра.
        /// </summary>
        /// <param name="one">Первый символ.</param>
        /// <param name="two">Второй символ.</param>
        /// <returns>Символы совпадают с точностью до регистра.</returns>
        public static bool SameChar
            (
                this char one,
                char two
            )
        {
#if PocketPC
            return (char.ToUpper(one) == char.ToUpper(two));
#else
            return (char.ToUpperInvariant(one) == char.ToUpperInvariant(two));
#endif
        }

        /// <summary>
        /// Сравнение строк.
        /// </summary>
        public static int SafeCompare
            (
                this string s1,
                string s2
            )
        {
            return string.Compare
                (
                    s1,
                    s2,
#if NETCORE || UAP || WIN81 || PORTABLE

                    StringComparison.OrdinalIgnoreCase

#else

                    StringComparison.InvariantCultureIgnoreCase

#endif
                );
        }

        /// <summary>
        /// Сравнение строки с массивом.
        /// </summary>
        public static bool SafeCompare
            (
                string value,
                params string[] list
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            foreach (string s in list)
            {
                if (String.Equals
                    (
                        value,
                        s,
                        StringComparison.CurrentCultureIgnoreCase
                    ))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Поиск подстроки.
        /// </summary>
        public static bool SafeContains
            (
                [CanBeNull] this string s1,
                [CanBeNull] string s2
            )
        {
            if (string.IsNullOrEmpty(s1)
                || string.IsNullOrEmpty(s2))
            {
                return false;
            }


#if WINMOBILE || PocketPC

            return s1.ToLower().Contains(s2.ToLower());

#else

            return s1
                .ToLowerInvariant()
                .Contains
                (
                    s2.ToLowerInvariant()
                );

#endif
        }

        /// <summary>
        /// Поиск подстроки.
        /// </summary>
        public static bool SafeContains
            (
                [CanBeNull] this string value,
                params string[] list
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            foreach (string s in list)
            {
                if (value.ToUpper().Contains(s.ToUpper()))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Поиск начала строки.
        /// </summary>
        public static bool SafeStarts
            (
                [CanBeNull] this string text,
                [CanBeNull] string begin
            )
        {
            if (string.IsNullOrEmpty(text)
                || string.IsNullOrEmpty(begin))
            {
                return false;
            }

#if WINMOBILE || PocketPC

            // ReSharper disable once PossibleNullReferenceException
            return text.ToLower().StartsWith(begin.ToLower());
#else

            return text.ToLowerInvariant()
                .StartsWith(begin.ToLowerInvariant());

#endif
        }

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        public static string SafeSubstring
            (
                [CanBeNull] this string text,
                int offset,
                int width
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            int length = text.Length;
            if ((offset < 0)
                || (offset >= length)
                || (width <= 0))
            {
                return string.Empty;
            }
            if (offset + width > length)
            {
                width = length - offset;
            }
            if (width <= 0)
            {
                return string.Empty;
            }

            string result = text.Substring(offset, width);

            return result;
        }

        /// <summary>
        /// Разбивка текста на отдельные строки.
        /// </summary>
        /// <remarks>Пустые строки не удаляются.</remarks>
        /// <param name="text">Текст для разбиения.</param>
        /// <returns>Массив строк.</returns>
        public static string[] SplitLines
            (
                this string text
            )
        {
            text = text.Replace("\r\n", "\n");

            return text.Split
                (
                    '\n'
                );
        }

        /// <summary>
        /// Склейка строк в сплошной текст, разделенный переводами строки.
        /// </summary>
        /// <param name="lines">Строки для склейки.</param>
        /// <returns>Склеенный текст.</returns>
        public static string MergeLines
            (
                this IEnumerable<string> lines
            )
        {
            string result = string.Join
                (
                    Environment.NewLine,
                    lines.ToArray()
                );

            return result;
        }

        /// <summary>
        /// Строка содержит пробельные символы?
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool ContainsWhitespace
            (
                this string text
            )
        {
            return text.ContainsAnySymbol
                (
                    ' ', '\t', '\r', '\n'
                );
        }

        /// <summary>
        /// Determines whether the given string
        /// represents valid identifier or not.
        /// </summary>
        public static bool IsValidIdentifier
            (
                [CanBeNull] this string name
            )
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (!(char.IsLetter(name, 0) || name[0] == '_'))
            {
                return false;
            }

            for (int i = 1; i < name.Length; i++)
            {
                if (!(char.IsLetterOrDigit(name, i) || name[i] == '_'))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Split the string using the given function.
        /// </summary>
        public static IEnumerable<string> SplitString
            (
                [NotNull] this string text,
                [NotNull] Func<char, bool> func
            )
        {
            Code.NotNull(text, "text");
            Code.NotNull(func, "func");

            int start = 0;
            bool empty = true;
            string result;
            for (int index = 0; index < text.Length; index++)
            {
                char c = text[index];
                if (func(c))
                {
                    if (!empty)
                    {
                        result = text.Substring(start, index - start);
                        yield return result;
                    }
                    start = index + 1;
                    empty = true;
                }
                else
                {
                    empty = false;
                }
            }

            if (!empty)
            {
                result = text.Substring(start, text.Length - start);
                if (!string.IsNullOrEmpty(result))
                {
                    yield return result;
                }
            }
        }

        /// <summary>
        /// Join string representations of the given objects.
        /// </summary>
        [NotNull]
        public static string Join
            (
                [CanBeNull] string separator,
                [NotNull] IEnumerable objects
            )
        {
            Code.NotNull(objects, "objects");

            if (ReferenceEquals(separator, null))
            {
                separator = string.Empty;
            }

            StringBuilder result = new StringBuilder();
            IEnumerator enumerator = objects.GetEnumerator();
            if (enumerator.MoveNext())
            {
                object o = enumerator.Current;
                if (!ReferenceEquals(o, null))
                {
                    result.Append(o);
                }

                while (enumerator.MoveNext())
                {
                    result.Append(separator);
                    o = enumerator.Current;
                    if (!ReferenceEquals(o, null))
                    {
                        result.Append(o);
                    }
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Wrap the string with given prefix and suffix
        /// </summary>
        [CanBeNull]
        public static string Wrap
            (
                [CanBeNull] this string value,
                [CanBeNull] string prefix,
                [CanBeNull] string suffix
            )
        {
            if (ReferenceEquals(value, null))
            {
                return null;
            }

            string result = value;

            if (!string.IsNullOrEmpty(prefix))
            {
                result = prefix + value;
            }

            if (!string.IsNullOrEmpty(suffix))
            {
                result = result + suffix;
            }

            return result;
        }

        /// <summary>
        /// Mangle given text with the escape character.
        /// </summary>
        [CanBeNull]
        public static string Mangle
            (
                [CanBeNull] string text,
                char escape,
                [NotNull] char[] badCharacters
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            StringBuilder result = new StringBuilder();

            foreach (char c in text)
            {
                if (c.OneOf(badCharacters)
                    || (c == escape))
                {
                    result.Append(escape);
                }
                result.Append(c);
            }

            return result.ToString();
        }

        /// <summary>
        /// Is URL-safe char?
        /// </summary>
        /// <remarks>Set of safe chars, from RFC 1738.4 minus '+'</remarks>
        public static bool IsUrlSafeChar
            (
                char ch
            )
        {
            if ((ch >= 'a' && ch <= 'z')
                || (ch >= 'A' && ch <= 'Z')
                || (ch >= '0' && ch <= '9')
                )
            {
                return true;
            }

            switch (ch)
            {
                case '-':
                case '_':
                case '.':
                case '!':
                case '*':
                case '(':
                case ')':
                    {
                        return true;
                    }
            }

            return false;
        }

        // ===============================================================

        /// <summary>
        /// Decode string.
        /// </summary>
        [CanBeNull]
        public static string UrlDecode
            (
                [CanBeNull] string text,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(encoding, "encoding");
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            List<byte> bytes = new List<byte>();

            int count = text.Length;
            for (int pos = 0; pos < count; pos++)
            {
                char ch = text[pos];
                if (ch == '+')
                {
                    ch = ' ';
                }
                else if (ch == '%' && pos < count - 2)
                {
                    int h1 = HexToInt(text[pos + 1]);
                    int h2 = HexToInt(text[pos + 2]);

                    if (h1 >= 0 && h2 >= 0)
                    {
                        byte b = (byte)((h1 << 4) | h2);
                        pos += 2;

                        bytes.Add(b);
                        continue;
                    }
                }

                bytes.Add((byte)ch);
            }

            string result = EncodingUtility.GetString
                (
                    encoding,
                    bytes.ToArray()
                );

            return result;
        }

        /// <summary>
        /// Encode string.
        /// </summary>
        [CanBeNull]
        public static string UrlEncode
            (
                [CanBeNull] string text,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(encoding, "encoding");
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            byte[] bytes = encoding.GetBytes(text);

            StringBuilder result = new StringBuilder();

            foreach (byte b in bytes)
            {
                char c = (char)b;

                if (IsUrlSafeChar(c))
                {
                    result.Append(c);
                }
                else if (c == ' ')
                {
                    result.Append('+');
                }
                else
                {
                    result.Append('%');
                    result.Append(IntToHex((b >> 4) & 0x0F));
                    result.Append(IntToHex(b & 0x0F));
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Replace control characters in the text.
        /// </summary>
        [CanBeNull]
        public static string ReplaceControlCharacters
            (
                [CanBeNull] string text,
                char substitute
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            bool needReplace = false;
            foreach (char c in text)
            {
                if (c < ' ')
                {
                    needReplace = true;
                    break;
                }
            }

            if (!needReplace)
            {
                return text;
            }

            StringBuilder result = new StringBuilder(text.Length);

            foreach (char c in text)
            {
                result.Append
                    (
                        c < ' ' ? substitute : c
                    );
            }

            return result.ToString();
        }

        /// <summary>
        /// Replace control characters in the text.
        /// </summary>
        [CanBeNull]
        public static string ReplaceControlCharacters
            (
                [CanBeNull] string text
            )
        {
            return ReplaceControlCharacters(text, ' ');
        }

        /// <summary>
        /// Trim lines.
        /// </summary>
        [NotNull]
        public static IEnumerable<string> TrimLines
            (
                [NotNull] this IEnumerable<string> lines
            )
        {
            Code.NotNull(lines, "lines");

            foreach (string line in lines)
            {
                if (!ReferenceEquals(line, null))
                {
                    yield return line.Trim();
                }
            }
        }

        /// <summary>
        /// Trim lines.
        /// </summary>
        [NotNull]
        public static IEnumerable<string> TrimLines
            (
                [NotNull] this IEnumerable<string> lines,
                params char[] characters
            )
        {
            Code.NotNull(lines, "lines");

            foreach (string line in lines)
            {
                if (!ReferenceEquals(line, null))
                {
                    yield return line.Trim(characters);
                }
            }
        }

        /// <summary>
        /// Split the string.
        /// </summary>
        /// <remarks>For compatibility with WinMobile
        /// </remarks>
        [NotNull]
        public static string[] SplitString
            (
                [NotNull] string text,
                [NotNull] string separator
            )
        {
            Code.NotNull(text, "text");
            Code.NotNullNorEmpty(separator, "separator");

            List<string> result = new List<string>();

            int separatorLength = separator.Length;

            while (true)
            {
                int position = text.IndexOf(separator);
                if (position < 0)
                {
                    result.Add(text);
                    break;
                }
                string prefix = text.Substring(0, position);
                result.Add(prefix);
                text = text.Substring
                    (
                        position + separatorLength, 
                        text.Length - position - separatorLength
                    );
            }

            return result.ToArray();
        }

        /// <summary>
        /// Split the string.
        /// </summary>
        /// <remarks>For compatibility with WinMobile
        /// </remarks>
        [NotNull]
        public static string[] SplitString
            (
                [NotNull] string text,
                [NotNull] string[] separators
            )
        {
            Code.NotNull(text, "text");
            Code.NotNull(separators, "separators");

            List<string> result = new List<string>();

            while (true)
            {
                foreach (string separator in separators)
                {
                    int position = text.IndexOf(separator);
                    if (position >= 0)
                    {
                        int separatorLength = separator.Length;

                        string prefix = text.Substring(0, position);
                        result.Add(prefix);
                        text = text.Substring
                            (
                                position + separatorLength,
                                text.Length - position - separatorLength
                            );
                        goto DONE;
                    }
                }

                result.Add(text);
                break;

            DONE:;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Split the string.
        /// </summary>
        /// <remarks>For compatibility with WinMobile.</remarks>
        [NotNull]
        public static string[] SplitString
            (
                [NotNull] string text,
                [NotNull] char[] separators,
                int maxParts
            )
        {
            Code.NotNull(text, "text");
            Code.NotNull(separators, "separators");

            List<string> result = new List<string>();
            if (maxParts < 2)
            {
                result.Add(text);
            }

            while (result.Count < maxParts)
            {
                foreach (char separator in separators)
                {
                    int position = text.IndexOf(separator);
                    if (position >= 0)
                    {
                        string prefix = text.Substring(0, position);
                        result.Add(prefix);
                        text = text.Substring
                            (
                                position + 1,
                                text.Length - position - 1
                            );
                        goto DONE;
                    }
                }

                result.Add(text);
                break;

            DONE: ;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Convert string to upper case.
        /// </summary>
        /// <remarks>For WinMobile compatibility.</remarks>
        [MethodImpl(Aggressive)]
        public static string ToUpperInvariant
            (
                [NotNull]
#if WINMOBILE || PocketPC
                this
#endif
                string text
            )
        {
#if WINMOBILE || PocketPC

            return text.ToUpper();

#else

            return text.ToUpperInvariant();

#endif
        }

        /// <summary>
        /// Convert string to lower case.
        /// </summary>
        /// <remarks>For WinMobile compatibility.</remarks>
        [MethodImpl(Aggressive)]
        public static string ToLowerInvariant
            (
                [NotNull]
#if WINMOBILE || PocketPC
                this
#endif
                string text
            )
        {
#if WINMOBILE || PocketPC

            return text.ToLower();

#else

            return text.ToLowerInvariant();

#endif
        }


#if WINMOBILE || PocketPC || FW35

        /// <summary>
        /// Clear the <see cref="StringBuilder"/>.
        /// </summary>
        public static void Clear
            (
                [NotNull] this StringBuilder builder
            )
        {
            Code.NotNull(builder, "builder");

            builder.Length = 0;
        }

#endif

        #endregion
    }
}
