/* UniforPlus3.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using AM;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class UniforPlus3
    {
        #region Private members

        #endregion

        #region Public methods

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
