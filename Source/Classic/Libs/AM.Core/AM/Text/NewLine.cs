// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NewLine.cs -- new line symbol.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Text
{
    //
    // Carriage Return     = ASCII 13 (0x0D), '\r'
    // Line Feed           = ASCII 10 (0x0A), '\n'
    // Next Line           = U+0085
    // Line Separator      = U+2028
    // Paragraph Separator = U+2029
    //

    /// <summary>
    /// New line symbol in different OSes.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class NewLine
    {
        #region Constants

        /// <summary>
        /// Apple.
        /// </summary>
        public const string Apple = "\r";

        /// <summary>
        /// Carriage Return.
        /// </summary>
        public const string CarriageReturn = "\r";

        /// <summary>
        /// Carriage Return.
        /// </summary>
        public const string CR = "\r";

        /// <summary>
        /// Carriage Return + Line Feed.
        /// </summary>
        public const string CRLF = "\r\n";

        /// <summary>
        /// Default value;
        /// </summary>
        public const string Default = "\r\n";

        /// <summary>
        /// Line Feed.
        /// </summary>
        public const string LF = "\n";

        /// <summary>
        /// Line Feed.
        /// </summary>
        public const string LineFeed = "\n";

        /// <summary>
        /// UNICODE Line Separator, U+2028.
        /// </summary>
        public const string LineSeparator = "\u2028";

        /// <summary>
        /// Linux.
        /// </summary>
        public const string Linux = "\n";

        /// <summary>
        /// Mac OS.
        /// </summary>
        public const string MacOS = "\r";

        /// <summary>
        /// MS-DOS.
        /// </summary>
        public const string MsDos = "\r\n";

        /// <summary>
        /// UNICODE Next Line, U+0085.
        /// </summary>
        public const string NextLine = "\u8085";

        /// <summary>
        /// UNICODE Paragraph Separator, U+2029.
        /// </summary>
        public const string ParagraphSeparator = "\u2029";

        /// <summary>
        /// UNIX.
        /// </summary>
        public const string Unix = "\n";

        /// <summary>
        /// Windows.
        /// </summary>
        public const string Windows = "\r\n";

        #endregion

        #region Public methods

        /// <summary>
        /// Determine line endings for the <paramref name="text"/>.
        /// </summary>
        [NotNull]
        public static string DetermineLineEndings
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return Default;
            }

            if (text.Contains(CRLF))
            {
                return CRLF;
            }

            if (text.Contains(LF))
            {
                return LF;
            }

            if (text.Contains(CR))
            {
                return CR;
            }

            return Default;
        }

        /// <summary>
        /// Change MS-DOS to UNIX line endings.
        /// </summary>
        [CanBeNull]
        public static string DosToUnix
            (
                [CanBeNull] this string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (!text.Contains(MsDos))
            {
                return text;
            }

            string result = text.Replace
                (
                    MsDos,
                    Unix
                );

            return result;
        }

        /// <summary>
        /// Remove NewLine symbol.
        /// </summary>
        [CanBeNull]
        public static string RemoveLineBreaks
            (
                [CanBeNull] this string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (!text.Contains(CarriageReturn)
                && !text.Contains(LineFeed))
            {
                return text;
            }

            StringBuilder result = new StringBuilder(text);

            result.Replace(CarriageReturn, string.Empty);
            result.Replace(LineFeed, string.Empty);

            return result.ToString();
        }

        /// <summary>
        /// Change UNICODE to MS-DOS line endings.
        /// </summary>
        [CanBeNull]
        public static string UnicodeToWindows
            (
                [CanBeNull] this string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            string result = text
                .Replace(NextLine, MsDos)
                .Replace(LineSeparator, MsDos)
                .Replace(ParagraphSeparator, MsDos);

            return result;
        }

        /// <summary>
        /// Change UNICODE to UNIX line endings.
        /// </summary>
        [CanBeNull]
        public static string UnicodeToUnix
            (
                [CanBeNull] this string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            string result = text
                .Replace(NextLine, Unix)
                .Replace(LineSeparator, Unix)
                .Replace(ParagraphSeparator, Unix);

            return result;
        }

        /// <summary>
        /// Change UNIX to MS-DOS line endings.
        /// </summary>
        [CanBeNull]
        public static string UnixToDos
            (
                [CanBeNull] this string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (text.Contains(MsDos)
                || !text.Contains(Unix))
            {
                return text;
            }

            string result = text.Replace
                (
                    Unix,
                    MsDos
                );

            return result;
        }

        #endregion
    }
}
