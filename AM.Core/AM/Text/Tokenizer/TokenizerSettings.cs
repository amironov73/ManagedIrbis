/* StringTokenizer2.cs -- tokenizes text
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text.Tokenizer
{
    /// <summary>
    /// Settings for <see cref="StringTokenizer"/>
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("IgnoreNewLine={IgnoreNewLine} "
        + "IgnoreWhitespace={IgnoreWhitespace}")]
    public sealed class TokenizerSettings
    {
        #region Properties

        /// <summary>
        /// Ignore newline.
        /// </summary>
        [DefaultValue(true)]
        public bool IgnoreNewLine { get; set; }

        /// <summary>
        /// Ignore whitespace.
        /// </summary>
        [DefaultValue(true)]
        public bool IgnoreWhitespace { get; set; }

        /// <summary>
        /// Ignore EOF in AllTokens().
        /// </summary>
        [DefaultValue(true)]
        public bool IgnoreEOF { get; set; }

        /// <summary>
        /// Symbol characters.
        /// </summary>
        public char[] SymbolChars { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TokenizerSettings()
        {
            IgnoreNewLine = true;
            IgnoreWhitespace = true;
            SymbolChars = new []
            {
                '=', '+', '-', '/', ',', '.', '*', '~', '!', '@',
                '#', '$', '%', '^', '&', '(', ')', '{', '}', '[',
                ']', ':', ';', '<', '>', '?', '|', '\\'
            };
        }

        #endregion

        #region Public methods

        #endregion
    }
}
