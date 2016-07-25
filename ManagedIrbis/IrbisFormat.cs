/* IrbisFormat.cs -- common format related stuff
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: moderate
 */

#region Using directives

using System.Text.RegularExpressions;
using AM;
using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Common format related stuff.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisFormat
    {
        #region Constants

        /// <summary>
        /// Format ALL.
        /// </summary>
        public const string All = "&uf('+0')";

        /// <summary>
        /// BRIEF format.
        /// </summary>
        public const string Brief = "@brief";

        /// <summary>
        /// IBIS format.
        /// </summary>
        public const string Ibis = "@ibiskw_h";

        /// <summary>
        /// Informational format.
        /// </summary>
        public const string Informational = "@info_w";

        /// <summary>
        /// Optimized format.
        /// </summary>
        public const string Optimized = "@";

        #endregion

        #region Properties

        /// <summary>
        /// Запрещенные символы.
        /// </summary>
        public static char[] ForbiddenCharacters
            = {'\r', '\n', '\t', '\x1F', '\x1E'};

        #endregion

        #region Public methods

        /// <summary>
        /// Prepare dynamic format string.
        /// </summary>
        /// <param name="text"></param>
        /// <remarks>Dynamic format string
        /// mustn't contains comments and
        /// string delimiters (no matter
        /// real or IRBIS).
        /// </remarks>
        /// <returns></returns>
        [CanBeNull]
        public static string PrepareFormat
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            text = Regex.Replace
                (
                    text,
                    "/[*].*?[\r\n]",
                    " "
                )
                .Replace('\r', ' ')
                .Replace('\n', ' ')
                .Replace('\t', ' ')
                .Replace('\x1F', ' ')
                .Replace('\x1E', ' ');

            return text;
        }

        /// <summary>
        /// Verify format string.
        /// </summary>
        public bool VerifyFormat
            (
                [CanBeNull] string text,
                bool throwOnError
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                if (throwOnError)
                {
                    throw new VerificationException("text is null");
                }
                return false;
            }

            // TODO more verification logic

            return true;
        }

        #endregion
    }
}
