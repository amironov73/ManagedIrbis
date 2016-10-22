/* IrbisText.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

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

        #endregion

        #region Public methods

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
