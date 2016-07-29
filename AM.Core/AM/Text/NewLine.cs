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

namespace AM.Text
{
    //
    // Carriage Return = ASCII 13 (0x0D), '\r'
    // Line Feed       = ASCII 10 (0x0A), '\n'
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
        /// Line Feed.
        /// </summary>
        public const string LF = "\n";

        /// <summary>
        /// Line Feed.
        /// </summary>
        public const string LineFeed = "\n";

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
