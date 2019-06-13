// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Query.cs -- search query builder.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Linq;
using System.Text;

using AM;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Search
{
    /// <summary>
    /// Search query builder.
    /// </summary>
    public static class Query
    {
        #region Private members

        private static readonly char[] SpecialSymbols
            = { ' ', '+', '*', '(', ')', '"' };

        #endregion

        #region Public methods

        /// <summary>
        /// Логическое "ВСЕ".
        /// </summary>
        [NotNull]
        public static string All()
        {
            return "I=$";
        }

        /// <summary>
        /// Логическое умножение.
        /// </summary>
        [NotNull]
        public static string And
            (
                [NotNull] this string left,
                [NotNull] string right
            )
        {
            Code.NotNullNorEmpty(left, "left");
            Code.NotNullNorEmpty(right, "right");

            return "(" + WrapIfNeeded(left) + " * " + WrapIfNeeded(right) + ")";
        }

        /// <summary>
        /// Логическое умножение.
        /// </summary>
        [NotNull]
        public static string And
            (
                params string[] items
            )
        {
            if (items.Length == 0)
            {
                throw new SearchSyntaxException();
            }

            if (items.Length == 1)
            {
                if (string.IsNullOrEmpty(items[0]))
                {
                    throw new SearchSyntaxException();
                }

                return WrapIfNeeded(items[0]);
            }

            int length = items.Sum(item => item.Length + 3);
            StringBuilder result = new StringBuilder(length);
            result.Append('(');
            bool first = true;
            foreach (string item in items)
            {
                if (string.IsNullOrEmpty(item))
                {
                    throw new SearchSyntaxException();
                }

                if (!first)
                {
                    result.Append(" * ");
                }

                result.Append(WrapIfNeeded(item));
                first = false;
            }

            result.Append(')');

            return result.ToString();
        }

        /// <summary>
        /// Склеивает строки.
        /// </summary>
        [NotNull]
        public static string Concat
            (
                params string[] items
            )
        {
            string result = WrapIfNeeded(string.Concat(items));

            return result;
        }

        /// <summary>
        /// Поиск вида "префикс=значение".
        /// </summary>
        [NotNull]
        public static string Equals
            (
                [NotNull] this string prefix,
                [NotNull] string value
            )
        {
            Code.NotNullNorEmpty(prefix, "prefix");
            Code.NotNullNorEmpty(value, "value");

            return Concat(prefix, value);
        }

        /// <summary>
        /// Поиск вида "префикс=значение".
        /// </summary>
        [NotNull]
        public static string Equals
            (
                [NotNull] this string prefix,
                [NotNull] string value1,
                [NotNull] string value2
            )
        {
            Code.NotNullNorEmpty(prefix, "prefix");
            Code.NotNullNorEmpty(value1, "value1");
            Code.NotNullNorEmpty(value2, "value2");

            return Or
                (
                    Concat(prefix, value1),
                    Concat(prefix, value2)
                );
        }

        /// <summary>
        /// Поиск вида "префикс=значение1 + префикс=значение2".
        /// </summary>
        [NotNull]
        public static string Equals
            (
                [NotNull] this string prefix,
                params string[] values
            )
        {
            Code.NotNullNorEmpty(prefix, "prefix");

            if (values.Length == 0)
            {
                throw new SearchSyntaxException();
            }

            foreach (var value in values)
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new SearchSyntaxException();
                }
            }

            if (values.Length == 1)
            {
                return Equals(prefix, values[0]);
            }

            int length = values.Sum(item => item.Length + prefix.Length + 3);
            StringBuilder result = new StringBuilder(length);
            result.Append('(');
            bool first = true;
            foreach (string value in values)
            {
                if (!first)
                {
                    result.Append(" + ");
                }

                result.Append(Equals(prefix, value));
                first = false;
            }

            result.Append(')');

            return result.ToString();
        }

        /// <summary>
        /// Нужно ли заключить данный текст в кавычки?
        /// </summary>
        public static bool NeedWrap
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                // Пустую строку всегда нужно оборачивать.
                return true;
            }

            char first = text.FirstChar();
            if (first == '"' || first == '(')
            {
                // Строка уже заключена в кавычки
                // Бывают также случаи "K=очист$"/(200,922)
                return false;
            }

            return text.ContainsAnySymbol(SpecialSymbols);
        }

        /// <summary>
        /// Логическое "НЕ".
        /// </summary>
        [NotNull]
        public static string Not
            (
                [NotNull] this string left,
                [NotNull] string right
            )
        {
            Code.NotNullNorEmpty(left, "left");
            Code.NotNullNorEmpty(right, "right");

            return "(" + WrapIfNeeded(left) + " ^ " + WrapIfNeeded(right) + ")";
        }

        /// <summary>
        /// Логическое сложение.
        /// </summary>
        [NotNull]
        public static string Or
            (
                [NotNull] this string left,
                [NotNull] string right
            )
        {
            Code.NotNullNorEmpty(left, "left");
            Code.NotNullNorEmpty(right, "right");

            return "(" + WrapIfNeeded(left) + " + " + WrapIfNeeded(right) + ")";
        }

        /// <summary>
        /// Логическое сложение.
        /// </summary>
        [NotNull]
        public static string Or
            (
                params string[] items
            )
        {
            if (items.Length == 0)
            {
                throw new SearchSyntaxException();
            }

            if (items.Length == 1)
            {
                if (string.IsNullOrEmpty(items[0]))
                {
                    throw new SearchSyntaxException();
                }

                return WrapIfNeeded(items[0]);
            }

            int length = items.Sum(item => item.Length + 3);
            StringBuilder result = new StringBuilder(length);
            result.Append('(');
            bool first = true;
            foreach (string item in items)
            {
                if (string.IsNullOrEmpty(item))
                {
                    throw new SearchSyntaxException();
                }

                if (!first)
                {
                    result.Append(" + ");
                }

                result.Append(WrapIfNeeded(item));
                first = false;
            }

            result.Append(')');

            return result.ToString();
        }

        /// <summary>
        /// Логическое "В том же поле".
        /// </summary>
        [NotNull]
        public static string SameField
            (
                [NotNull] this string left,
                [NotNull] string right
            )
        {
            Code.NotNullNorEmpty(left, "left");
            Code.NotNullNorEmpty(right, "right");

            return "(" + WrapIfNeeded(left) + " (G) " + WrapIfNeeded(right) + ")";
        }

        /// <summary>
        /// Логическое "В том же поле".
        /// </summary>
        [NotNull]
        public static string SameField
            (
                params string[] items
            )
        {
            if (items.Length == 0)
            {
                throw new SearchSyntaxException();
            }

            if (items.Length == 1)
            {
                if (string.IsNullOrEmpty(items[0]))
                {
                    throw new SearchSyntaxException();
                }

                return WrapIfNeeded(items[0]);
            }

            int length = items.Sum(item => item.Length + 3);
            StringBuilder result = new StringBuilder(length);
            result.Append('(');
            bool first = true;
            foreach (string item in items)
            {
                if (string.IsNullOrEmpty(item))
                {
                    throw new SearchSyntaxException();
                }

                if (!first)
                {
                    result.Append(" (G) ");
                }

                result.Append(WrapIfNeeded(item));
                first = false;
            }

            result.Append(')');

            return result.ToString();
        }

        /// <summary>
        /// Логическое "В том же повторении поля".
        /// </summary>
        [NotNull]
        public static string SameRepeat
            (
                [NotNull] this string left,
                [NotNull] string right
            )
        {
            Code.NotNullNorEmpty(left, "left");
            Code.NotNullNorEmpty(right, "right");

            return "(" + WrapIfNeeded(left) + " (F) " + WrapIfNeeded(right) + ")";
        }

        /// <summary>
        /// Логическое "В том же повторении поля".
        /// </summary>
        [NotNull]
        public static string SameRepeat
            (
                params string[] items
            )
        {
            if (items.Length == 0)
            {
                throw new SearchSyntaxException();
            }

            if (items.Length == 1)
            {
                if (string.IsNullOrEmpty(items[0]))
                {
                    throw new SearchSyntaxException();
                }

                return WrapIfNeeded(items[0]);
            }

            int length = items.Sum(item => item.Length + 3);
            StringBuilder result = new StringBuilder(length);
            result.Append('(');
            bool first = true;
            foreach (string item in items)
            {
                if (string.IsNullOrEmpty(item))
                {
                    throw new SearchSyntaxException();
                }

                if (!first)
                {
                    result.Append(" (F) ");
                }

                result.Append(WrapIfNeeded(item));
                first = false;
            }

            result.Append(')');

            return result.ToString();
        }

        /// <summary>
        /// Обёртывает кавычками текст при необходимости.
        /// </summary>
        [NotNull]
        public static string WrapIfNeeded
            (
                [CanBeNull] string text
            )
        {
            return NeedWrap(text) ? "\"" + text + "\"" : text.ThrowIfNull();
        }

        #endregion
    }
}