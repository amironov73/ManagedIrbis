// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisFormat.cs -- common format related stuff
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: moderate
 */

#region Using directives

using System.Text;

using AM;
using AM.Text;

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
    public static class IrbisFormat
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
        /// Remove comments from the format.
        /// </summary>
        [CanBeNull]
        public static string RemoveComments
            (
                [CanBeNull] string text
            )
        {
            const char ZERO = '\0';

            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (!text.Contains("/*"))
            {
                return text;
            }

            StringBuilder result = new StringBuilder(text.Length);
            TextNavigator navigator = new TextNavigator(text);
            char state = ZERO;

            while (!navigator.IsEOF)
            {
                char c = navigator.ReadChar();

                switch (state)
                {
                    case '\'':
                        if (c == '\'')
                        {
                            state = ZERO;
                        }
                        result.Append(c);
                        break;

                    case '"':
                        if (c == '"')
                        {
                            state = ZERO;
                        }
                        result.Append(c);
                        break;

                    case '|':
                        if (c == '|')
                        {
                            state = ZERO;
                        }
                        result.Append(c);
                        break;

                    default:
                        if (c == '/')
                        {
                            if (navigator.PeekChar() == '*')
                            {
                                navigator.ReadTo('\r', '\n');
                            }
                            else
                            {
                                result.Append(c);
                            }
                        }
                        else if (c == '\'' || c == '"' || c == '|')
                        {
                            state = c;
                            result.Append(c);
                        }
                        else
                        {
                            result.Append(c);
                        }
                        break;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Prepare the dynamic format string.
        /// </summary>
        /// <remarks>Dynamic format string
        /// mustn't contains comments and
        /// string delimiters (no matter
        /// real or IRBIS).
        /// </remarks>
        [CanBeNull]
        public static string PrepareFormat
            (
                [CanBeNull] string text
            )
        {
            const char ZERO = '\0';

            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            text = RemoveComments(text);
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("\r", string.Empty)
                    .Replace("\n", string.Empty);
            }

            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            StringBuilder result = new StringBuilder(text.Length);
            TextNavigator navigator = new TextNavigator(text);

            char state = ZERO;

            // Replace all forbidden characters with spaces
            while (!navigator.IsEOF)
            {
                char c = navigator.ReadChar();

                switch (state)
                {
                    case '\'':
                        if (c == '\'')
                        {
                            state = ZERO;
                        }
                        result.Append(c);
                        break;

                    case '"':
                        if (c == '"')
                        {
                            state = ZERO;
                        }
                        result.Append(c);
                        break;

                    case '|':
                        if (c == '|')
                        {
                            state = ZERO;
                        }
                        result.Append(c);
                        break;

                    default:
                        if (c < ' ')
                        {
                            c = ' ';
                        }
                        result.Append(c);
                        break;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Verify format string.
        /// </summary>
        public static bool VerifyFormat
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
