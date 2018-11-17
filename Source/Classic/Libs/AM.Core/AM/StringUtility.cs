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
using System.Diagnostics.CodeAnalysis;
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

        private static readonly char[] _after = { ',', '.', ']', '!', '?', ':', ';' };
        private static readonly char[] _before = { '[', '(' };
        private static readonly Random _random = new Random();
        private static readonly char[] _quotes = { '\'', '"', '[', ']' };
        private static readonly char[] _whitespace = { ' ', '\t', '\r', '\n' };

        private static int _HexToInt
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

        private static char _IntToHex
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
        /// <returns>Transcoded string.</returns>
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
        /// Сравнивает две строки независимо от текущей культуры.
        /// </summary>
        public static bool CompareInvariant
            (
                string left,
                string right
            )
        {
            return CultureInfo.InvariantCulture.CompareInfo.Compare
                (
                    left,
                    right
                ) == 0;
        }

        /// <summary>
        /// Сравнивает две строки с точностью до регистра символов.
        /// </summary>
        public static bool CompareNoCase
            (
                string left,
                string right
            )
        {
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
            return char.ToUpper(left)
                   == char.ToUpper(right);
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
                [CanBeNull] string value,
                int startIndex,
                int endIndex
            )
        {
            if (string.IsNullOrEmpty(value)
                || startIndex >= value.Length)
            {
                return false;
            }

            endIndex = Math.Min(endIndex, value.Length);
            for (int i = startIndex; i < endIndex; i++)
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

            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
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
        /// Строка содержит пробельные символы?
        /// </summary>
        public static bool ContainsWhitespace
            (
                [CanBeNull] this string text
            )
        {
            return text.ContainsAnySymbol(_whitespace);
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
        /// Converts empty string to <c>null</c>.
        /// </summary>
        [CanBeNull]
        public static string EmptyToNull
            (
                [CanBeNull] this string value
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
                [CanBeNull] this string text
            )
        {
            return string.IsNullOrEmpty(text)
                ? '\0'
                : text[0];
        }

        /// <summary>
        /// Get case-insensitive string comparer.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static IEqualityComparer<string> GetCaseInsensitiveComparer()
        {
#if UAP

            return StringComparer.OrdinalIgnoreCase;

#else

            return StringComparer.InvariantCultureIgnoreCase;

#endif
        }

        /// <summary>
        /// Get case-insensitive string comparer.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static StringComparison GetCaseInsensitiveComparison()
        {
#if UAP

            return StringComparison.OrdinalIgnoreCase;

#else

            return StringComparison.InvariantCultureIgnoreCase;

#endif
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
        /// Builds an array is regex matches.
        /// </summary>
        /// <param name="input">String to parse.</param>
        /// <param name="pattern">Regex.</param>
        /// <returns>Array of matches.</returns>
        [NotNull]
        public static string[] GetMatches
            (
                [NotNull] string input,
                [NotNull] string pattern
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

        /// <summary>
        /// Строит массив слов в строке.
        /// </summary>
        /// <param name="value">Разбираемая строка.</param>
        /// <returns>Массив слов.</returns>
        [NotNull]
        public static string[] GetWords
            (
                [NotNull] string value
            )
        {
            return GetMatches(value, @"\w+");
        }

        /// <summary>
        ///
        /// </summary>
        [CanBeNull]
        public static string IfEmpty
            (
                [CanBeNull] this string first,
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
        /// Reports the index of the first occurrence in this instance
        /// of any string in a specified array.
        /// </summary>
        public static int IndexOfAny
            (
                [NotNull] this string text,
                out int which,
                params string[] anyOf
            )
        {
            int result = -1;
            which = -1;

            for (int i = 0; i < anyOf.Length; i++)
            {
                string value = anyOf[i];
                int index = text.IndexOf(value, StringComparison.Ordinal);
                if (index >= 0)
                {
                    if (result >= 0)
                    {
                        if (index < result)
                        {
                            result = index;
                            which = i;
                        }
                    }
                    else
                    {
                        result = index;
                        which = i;
                    }
                }
            }

            return result;
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
        /// Is URL-safe char?
        /// </summary>
        /// <remarks>Set of safe chars, from RFC 1738.4 minus '+'</remarks>
        public static bool IsUrlSafeChar
            (
                char ch
            )
        {
            if (ch >= 'a' && ch <= 'z'
                || ch >= 'A' && ch <= 'Z'
                || ch >= '0' && ch <= '9'
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
                bool flag = false;
                object o = enumerator.Current;
                if (!ReferenceEquals(o, null))
                {
                    result.Append(o);
                    flag = true;
                }

                while (enumerator.MoveNext())
                {
                    o = enumerator.Current;
                    if (!ReferenceEquals(o, null))
                    {
                        if (flag)
                        {
                            result.Append(separator);
                        }
                        result.Append(o);
                        flag = true;
                    }
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Gets the last char of the text.
        /// </summary>
        public static char LastChar
            (
                [CanBeNull] this string text
            )
        {
            return string.IsNullOrEmpty(text)
                ? '\0'
                : text[text.Length - 1];
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

            StringBuilder result = new StringBuilder(text.Length);

            foreach (char c in text)
            {
                if (c.OneOf(badCharacters)
                    || c == escape)
                {
                    result.Append(escape);
                }
                result.Append(c);
            }

            return result.ToString();
        }

        /// <summary>
        /// Склейка строк в сплошной текст, разделенный переводами строки.
        /// </summary>
        /// <param name="lines">Строки для склейки.</param>
        /// <returns>Склеенный текст.</returns>
        public static string MergeLines
            (
                [NotNull] this IEnumerable<string> lines
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
        /// Проверяет, является ли искомая строка одной
        /// из перечисленных. Регистр символов не учитывается.
        /// </summary>
        /// <param name="one">Искомая строка.</param>
        /// <param name="many">Источник проверяемых строк.</param>
        /// <returns>Найдена ли искомая строка.</returns>
        public static bool OneOf
            (
                [CanBeNull] this string one,
                [NotNull] IEnumerable<string> many
            )
        {
            foreach (string s in many)
            {
                if (one.SameString(s))
                {
                    return true;
                }
            }

            return false;
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
                [CanBeNull] this string one,
                params string[] many
            )
        {
            foreach (string s in many)
            {
                if (one.SameString(s))
                {
                    return true;
                }
            }

            return false;
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
            foreach (char s in many)
            {
                if (one.SameChar(s))
                {
                    return true;
                }
            }

            return false;
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
                [NotNull] IEnumerable<char> many
            )
        {
            foreach (char s in many)
            {
                if (one.SameChar(s))
                {
                    return true;
                }
            }

            return false;
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
            foreach (char s in many)
            {
                if (one.SameChar(s))
                {
                    return true;
                }
            }

            return false;
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
            if (ReferenceEquals(text, null))
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
        /// Сравнение строк.
        /// </summary>
        public static int SafeCompare
            (
                [CanBeNull] this string s1,
                string s2
            )
        {
            return string.Compare
                (
                    s1,
                    s2,
#if UAP

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
                [CanBeNull] string value,
                params string[] list
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            foreach (string s in list)
            {
                if (string.Equals
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

            return ToLowerInvariant(s1)
                .Contains(ToLowerInvariant(s2));
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

            value = ToLowerInvariant(value);
            foreach (string s in list)
            {
                if (value.Contains(ToLowerInvariant(s)))
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

            return ToLowerInvariant(text)
                .StartsWith(ToLowerInvariant(begin));
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
            if (offset < 0
                || offset >= length
                || width <= 0)
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
            return char.ToUpperInvariant(one) == char.ToUpperInvariant(two);
#endif
        }

        /// <summary>
        /// Сравнивает строки с точностью до регистра.
        /// </summary>
        /// <param name="one">Первая строка.</param>
        /// <param name="two">Вторая строка.</param>
        /// <returns>Строки совпадают с точностью до регистра.</returns>
        public static bool SameString
            (
                [CanBeNull] this string one,
                [CanBeNull] string two
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
                [CanBeNull] this string one,
                [CanBeNull] string two
            )
        {
            return string.Compare
                   (
                       one,
                       two,
                       StringComparison.Ordinal
                   ) == 0;
        }

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
                char next = i < input.Length - 1
                                ? input[i + 1]
                                : '\0';
                char prev = i > 0
                                ? input[i - 1]
                                : '\0';
                if (chr == ' ')
                {
                    if (next == ' ')
                    {
                        continue;
                    }
                    if (Array.IndexOf(_after, next) >= 0)
                    {
                        continue;
                    }
                }
                else if (chr == '"')
                {
                    if (quot)
                    {
                        output.Append(chr);
                        if (next != ' '
                             && Array.IndexOf(_after, next) < 0)
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
                        if (next != ' '
                             && Array.IndexOf(_after, next) < 0)
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
                else if (Array.IndexOf(_before, chr) >= 0)
                {
                    if (prev != ' '
                        && i > 0
                        && Array.IndexOf(_before, next) < 0
                        && Array.IndexOf(_after, next) < 0)
                    {
                        output.Append(' ');
                    }
                }
                else if (Array.IndexOf(_after, chr) >= 0)
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
                    if (next != ' '
                         && Array.IndexOf(_before, next) < 0
                         && Array.IndexOf(_after, next) < 0)
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

        /// <summary>
        /// Slice the string.
        /// </summary>
        public static StringSlice Slice
            (
                [NotNull] this string text,
                int offset,
                int length
            )
        {
            return new StringSlice(text, offset, length);
        }

        /// <summary>
        /// Slice the string.
        /// </summary>
        public static StringSlice Slice
            (
                [NotNull] this string text,
                int offset
            )
        {
            return new StringSlice(text, offset, text.Length - offset);
        }

        /// <summary>
        /// Разбивает строку по указанному разделителю.
        /// </summary>
        [NotNull]
        public static string[] SplitFirst
            (
                [NotNull] this string line,
                char delimiter
            )
        {
            int index = line.IndexOf(delimiter);
            string[] result = index < 0
                ? new[] { line }
                : new[]
                {
                    line.Substring(0, index),
                    line.Substring(index + 1)
                };

            return result;
        }

        /// <summary>
        /// Разбивка текста на отдельные строки.
        /// </summary>
        /// <remarks>Пустые строки не удаляются.</remarks>
        /// <param name="text">Текст для разбиения.</param>
        /// <returns>Массив строк.</returns>
        [NotNull]
        public static string[] SplitLines
            (
                [NotNull] this string text
            )
        {
            text = text.Replace("\r\n", "\n");

            string[] result = text.Split('\n');

            return result;
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
            int start = 0;
            while (true)
            {
                int position = text.IndexOf(separator, start, StringComparison.Ordinal);
                if (position < 0)
                {
                    result.Add(text.Substring(start));
                    break;
                }
                string prefix = text.Substring(start, position - start);
                result.Add(prefix);
                start = position + separatorLength;
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
                int which;
                int position = text.IndexOfAny(out which, separators);
                if (position >= 0)
                {
                    string separator = separators[which];
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

            if (text.Length == 0)
            {
                return EmptyArray;
            }

            List<string> result = new List<string>();
            if (maxParts < 2)
            {
                result.Add(text);
            }

            int start = 0;
            while (result.Count < maxParts)
            {
                if (result.Count == maxParts - 1)
                {
                    result.Add(text.Substring(start));
                    break;
                }

                int position = text.IndexOfAny(separators, start);
                if (position >= 0)
                {
                    string prefix = text.Substring(start, position - start);
                    result.Add(prefix);
                    start = position + 1;
                }
                else
                {
                    result.Add(text.Substring(start));
                    break;
                }
            }

            return result.ToArray();
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
        /// Splits a string into substrings based on the characters
        /// in an array. You can specify whether the substrings
        /// include empty array elements.
        /// </summary>
        [NotNull]
        public static string[] SplitString
            (
                [NotNull] string text,
                [NotNull] char[] separator,
                StringSplitOptions options
            )
        {
#if WINMOBILE

            return text.Split(separator);

#else

            return text.Split(separator, options);

#endif
        }

        /// <summary>
        /// Splits a string into substrings based on the strings
        /// in an array. You can specify whether the substrings
        /// include empty array elements.
        /// </summary>
        [NotNull]
        public static string[] SplitString
            (
                [NotNull] string text,
                [NotNull] string[] separator,
                StringSplitOptions options
            )
        {
#if WINMOBILE

            return SplitString(text, separator);

#else

            return text.Split(separator, options);

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

            return text;
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
        /// Преобразует строку, содержащую escape-последовательности,
        /// к нормальному виду.
        /// </summary>
        /// <param name="value">Исходная строка.</param>
        /// <returns>Результирующая строка.</returns>
        [NotNull]
        public static string Unescape
            (
                [CanBeNull] string value
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

                if (value.Length - index < 3)
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
                if (text[0] == quoteChar
                    && text[length - 1] == quoteChar)
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
                if (text[0] == quoteChar1
                    && text[length - 1] == quoteChar2)
                {
                    text = text.Substring(1, length - 2);
                }
            }

            return text;
        }

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
                    int h1 = _HexToInt(text[pos + 1]);
                    int h2 = _HexToInt(text[pos + 2]);

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
                    result.Append(_IntToHex((b >> 4) & 0x0F));
                    result.Append(_IntToHex(b & 0x0F));
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
        /// Wrap the string with given prefix/suffix
        /// </summary>
        [CanBeNull]
        public static string Wrap
            (
                [CanBeNull] this string value,
                [CanBeNull] string prefixSuffix
            )
        {
            if (ReferenceEquals(value, null))
            {
                return null;
            }

            string result = value;

            if (!string.IsNullOrEmpty(prefixSuffix))
            {
                result = prefixSuffix + value + prefixSuffix;
            }

            return result;
        }

        // ===============================================================


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

        // ===============================================================

        /// <summary>
        /// Get very first index of substrings.
        /// </summary>
        public static int FirstIndexOfAny
            (
                [NotNull] string text,
                [NotNull] string[] fragments
            )
        {
            Code.NotNull(text, "text");
            Code.NotNull(fragments, "fragments");

            int result = -1;
            foreach (string fragment in fragments)
            {
                int index = text.IndexOf(fragment);
                if (index >= 0)
                {
                    if (result < 0)
                    {
                        result = index;
                    }
                    else
                    {
                        if (result > 0 && index < result)
                        {
                            result = index;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get very last index of substrings.
        /// </summary>
        public static int LastIndexOfAny
            (
                [NotNull] string text,
                [NotNull] string[] fragments
            )
        {
            Code.NotNull(text, "text");
            Code.NotNull(fragments, "fragments");

            int result = -1;
            foreach (string fragment in fragments)
            {
                int index = text.LastIndexOf(fragment);
                if (index > result)
                {
                    result = index;
                }
            }

            return result;
        }

#endregion
    }
}

