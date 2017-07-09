﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlus3.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class UniforPlus3
    {
        #region Private members

        #endregion

        #region Public methods

        // ================================================================

        //
        // Декодирование строки из UTF-8 – &uf('+3W…
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
        // Кодирование строки в UTF-8 – &uf('+3U…
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
        // Перевод знака + в %2B – &uf('+3+…
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
        // Декодирование данных из URL – &uf('+3D…
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
        // Кодирование данных для представления в URL – &uf('+3E…
        // Вид функции: +3E.
        // Назначение: Кодирование данных для представления в URL.
        // Присутствует в версиях ИРБИС с 2005.2.
        // Формат (передаваемая строка):
        //  +3E<данные>
        // Примеры:
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

        #endregion
    }
}
