// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CommandLineUtility.cs -- common routines for command line
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM.Text;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.CommandLine
{
    /// <summary>
    /// Common routines for command line processing.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class CommandLineUtility
    {
        #region Properties

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Split the text to command line arguments.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static string[] SplitText
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return new string[0];
            }

            List<string> result = new List<string>();
            TextNavigator navigator = new TextNavigator(text);

            while (!navigator.IsEOF)
            {
                navigator.SkipWhitespace();
                if (navigator.IsEOF)
                {
                    break;
                }

                char c = navigator.ReadChar();
                string item;
                if (c == CommandLineSettings.ArgumentDelimiter)
                {
                    item = c + navigator.ReadTo
                        (
                            CommandLineSettings.ArgumentDelimiter
                        );
                }
                else
                {
                    item = c + navigator.ReadUntilWhiteSpace();
                }
                result.Add(item);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Wrap argument with delimiters if needed.
        /// </summary>
        [CanBeNull]
        public static string WrapArgumentIfNeeded
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (text.Contains(' '))
            {
                text = CommandLineSettings.ArgumentDelimiter
                       + text
                       + CommandLineSettings.ArgumentDelimiter;
            }

            return text;
        }

        #endregion
    }
}
