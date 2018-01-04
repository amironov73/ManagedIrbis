// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlus3.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text;

using AM;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Работа со строками.
    //

    static class UniforPlus3
    {
        #region Public methods

        // ================================================================

        //
        // Декодирование строки из UTF-8 – &uf('+3W')
        // Вид функции: +3W.
        // Назначение: Декодирование строки из UTF-8.
        // Формат(передаваемая строка):
        // +3W<данные>
        //

        /// <summary>
        /// Convert text from UTF8 to CP1251.
        /// </summary>
        public static void ConvertToAnsi
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = EncodingUtility.ChangeEncoding
                    (
                        expression,
                        IrbisEncoding.Utf8,
                        IrbisEncoding.Ansi
                    );
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // Кодирование строки в UTF-8 – &uf('+3U')
        // Вид функции: +3U.
        // Назначение: Кодирование строки в UTF-8.
        // Формат(передаваемая строка):
        // +3U<данные>
        //

        /// <summary>
        /// Convert text from CP1251 to UTF8.
        /// </summary>
        public static void ConvertToUtf
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = EncodingUtility.ChangeEncoding
                    (
                        expression,
                        IrbisEncoding.Ansi,
                        IrbisEncoding.Utf8
                    );
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // Перевод знака + в %2B – &uf('+3+')
        // Вид функции: +3+.
        // Назначение: Перевод знака + в %2B.
        // Формат (передаваемая строка):
        // +3+<данные>
        //

        /// <summary>
        /// Replace '+' sign with %2B
        /// </summary>
        public static void ReplacePlus
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string clear = expression.Replace("+", "%2B");
                context.Write(node, clear);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // Декодирование данных из URL – &uf('+3D')
        // Вид функции: +3D.
        // Назначение: Декодирование данных из URL.
        // Присутствует в версиях ИРБИС с 2005.2.
        // Формат (передаваемая строка):
        // +3D<данные>
        //

        /// <summary>
        /// Decode text from the URL.
        /// </summary>
        public static void UrlDecode
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = StringUtility.UrlDecode
                    (
                        expression,
                        IrbisEncoding.Utf8
                    );
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // Кодирование данных для представления в URL – &uf('+3E')
        // Вид функции: +3E.
        // Назначение: Кодирование данных для представления в URL.
        // Присутствует в версиях ИРБИС с 2005.2.
        // Формат (передаваемая строка):
        //  +3E<данные>
        //
        // Пример:
        //
        // &unifor('+3E', v1007)
        //

        /// <summary>
        /// Encode the text to URL format.
        /// </summary>
        public static void UrlEncode
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = StringUtility.UrlEncode
                    (
                        expression,
                        IrbisEncoding.Utf8
                    );
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // ibatrak
        //
        // Форматирование полей записи в клиентское представление без заголовка
        //
        // &uf ('+3A')

        /// <summary>
        /// Encode the record to the plain text format.
        /// </summary>
        public static void FieldsToText
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            MarcRecord record = context.Record;
            if (!ReferenceEquals(record, null))
            {
                string output = record.ToPlainText();
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        private static readonly char[] _specialChars = {'&', '"', '<', '>'};

        //
        // ibatrak
        //
        // Замена специальных символов HTML.
        //
        // Неописанная функция
        // &unifor('+3H')
        // Кривая реализация htmlspecialchars
        // заменяет 
        // & на &quot; (здесь ошибка -- надо на &amp;)
        // " на &quot;
        // < на &lt;
        // > на &gt;
        // одинарные кавычки не кодирует
        //
        public static void HtmlSpecialChars
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                if (expression.ContainsAnySymbol(_specialChars))
                {
                    StringBuilder builder = new StringBuilder(expression);
                    builder.Replace("&", "&quot;");
                    builder.Replace("\"", "&quot;");
                    builder.Replace("<", "&lt;");
                    builder.Replace(">", "&gt;");
                    expression = builder.ToString();
                }

                context.Write(node, expression);
                context.OutputFlag = true;
            }
        }

        #endregion
    }
}
