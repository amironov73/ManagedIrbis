// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BiblioUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class BiblioUtility
    {
        #region Private members

        private static char[] _delimiters = { '.', '!', '?', ')', ':', '}' };

        private static Regex _commandRegex = new Regex(@"\\[a-z]\d+$");

        private static void _AddDot
            (
                [NotNull] StringBuilder builder,
                [CanBeNull] string line
            )
        {
            if (!string.IsNullOrEmpty(line))
            {
                line = line.TrimEnd();
                builder.Append(line);
                if (!string.IsNullOrEmpty(line))
                {
                    char lastChar = line.LastChar();
                    if (!lastChar.OneOf(_delimiters)
                        && !_commandRegex.IsMatch(line))
                    {
                        builder.Append('.');
                    }
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add trailing dot to every line in the text.
        /// </summary>
        [NotNull]
        public static string AddTrailingDot
            (
                [NotNull] string text
            )
        {
            StringBuilder result = new StringBuilder(text.Length + 10);
            TextNavigator navigator = new TextNavigator(text);
            string line;
            while (!navigator.IsEOF)
            {
                line = navigator.ReadTo("\\par");
                if (ReferenceEquals(line, null))
                {
                    break;
                }

                string recent = navigator.RecentText(4);
                bool par = false;
                if (recent == "\\par")
                {
                    if (navigator.PeekChar() == 'd')
                    {
                        result.Append(line);
                        result.Append("\\par");
                        result.Append(navigator.ReadChar());
                        continue;
                    }
                    par = true;
                }

                _AddDot(result, line);

                if (par)
                {
                    result.Append("\\par");
                }
            }

            line = navigator.GetRemainingText();
            _AddDot(result, line);

            return result.ToString();
        }

        #endregion
    }
}
