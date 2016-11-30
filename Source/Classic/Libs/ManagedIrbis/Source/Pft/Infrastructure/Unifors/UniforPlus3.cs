// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlus3.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using AM;
using AM.Text;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class UniforPlus3
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert text from UTF8 to CP1251.
        /// </summary>
        public static void ConvertToAnsi
            (
                PftContext context,
                PftNode node,
                string expression
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
            }
        }

        /// <summary>
        /// Convert text from CP1251 to UTF8.
        /// </summary>
        public static void ConvertToUtf
            (
                PftContext context,
                PftNode node,
                string expression
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
            }
        }

        /// <summary>
        /// Replace '+' sign with %2B
        /// </summary>
        public static void ReplacePlus
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string clear = expression.Replace("+", "%2B");
                context.Write(node, clear);
                context.OutputFlag = true;
            }
        }

        /// <summary>
        /// Decode text from URL.
        /// </summary>
        public static void UrlDecode
            (
                PftContext context,
                PftNode node,
                string expression
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

        /// <summary>
        /// Encode text for URL.
        /// </summary>
        public static void UrlEncode
            (
                PftContext context,
                PftNode node,
                string expression
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
