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

using JetBrains.Annotations;

#endregion

namespace UnsafeAM
{
    /// <summary>
    /// String manipulation routines.
    /// </summary>
    [PublicAPI]
    public static class StringUtility
    {
        #region Properties

        /// <summary>
        /// Empty array of <see cref="string"/>.
        /// </summary>
        [NotNull]
        public static readonly string[] EmptyArray = new string[0];

        #endregion

        #region Private members

        private static readonly char[] _whitespace = { ' ', '\t', '\r', '\n' };

        #endregion

        #region Public methods

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
            //Code.NotNull(fromEncoding, "fromEncoding");
            //Code.NotNull(toEncoding, "toEncoding");
            //Code.NotNull(value, "value");

            if (fromEncoding.Equals(toEncoding))
            {
                return value;
            }

            byte[] bytes = fromEncoding.GetBytes(value);
            string result = toEncoding.GetString(bytes);

            return result;
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
            return char.ToUpper(left) == char.ToUpper(right);
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
            return char.ToUpperInvariant(one) == char.ToUpperInvariant(two);
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
        /// Превращает строку в видимую.
        /// Пример: "(null)".
        /// </summary>
        [NotNull]
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

        #endregion
    }
}
