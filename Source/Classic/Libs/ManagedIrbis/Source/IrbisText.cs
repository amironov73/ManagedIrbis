// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisText.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IrbisText
    {
        #region Constants

        /// <summary>
        /// Irbis line delimiter.
        /// </summary>
        public const string IrbisDelimiter = "\x001F\x001E";

        /// <summary>
        /// Standard Windows line delimiter.
        /// </summary>
        public const string StandardDelimiter = "\r\n";

        /// <summary>
        /// Standard Windows line delimiter.
        /// </summary>
        public const string WindowsDelimiter = "\r\n";

        #endregion

        #region Properties

        #endregion

        #region Private members

        private static char[] _delimiters = { '\x1F' };

        private static string _CleanupEvaluator
            (
                Match match
            )
        {
            int length = match.Value.Length;

            if ((length & 1) == 0)
            {
                return new string('.', length / 2);
            }

            return new string('.', length / 2 + 2);
        }

        #endregion

        #region Public methods


        /// <summary>
        /// Cleanup the text.
        /// </summary>
        [CanBeNull]
        public static string CleanupText
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            string result = Regex.Replace
                (
                    text,
                    @"(\. - ){2,}",
                    ". - "
                );

            result = Regex.Replace
                (
                    result,
                    @"\.{2,}",
                    _CleanupEvaluator
                );

            return result;
        }

        /// <summary>
        /// Convert IRBIS line endings to standard.
        /// </summary>
        [CanBeNull]
        public static string IrbisToWindows
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (!text.Contains(IrbisDelimiter))
            {
                return text;
            }

            string result = text.Replace
                (
                    IrbisDelimiter,
                    WindowsDelimiter
                );

            return result;
        }

        /// <summary>
        /// Split IRBIS-delimited text to lines.
        /// </summary>
        [NotNull]
        public static string[] SplitIrbisToLines
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return new string[0];
            }

            text = IrbisToWindows(text);

            string[] result = text.Split
                (
                    _delimiters,
                    StringSplitOptions.None
                );

            return result;
        }

        /// <summary>
        /// Convert standard line endings to IRBIS.
        /// </summary>
        [CanBeNull]
        public static string WindowsToIrbis
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (!text.Contains(WindowsDelimiter))
            {
                return text;
            }

            string result = text.Replace
                (
                    WindowsDelimiter,
                    IrbisDelimiter
                );

            return result;
        }

        #endregion
    }
}
