/* TokenizerSettings.cs -- settings for StringTokenizer
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text.Tokenizer
{
    /// <summary>
    /// Settings for <see cref="StringTokenizer"/>
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("IgnoreNewLine={IgnoreNewLine} "
        + "IgnoreWhitespace={IgnoreWhitespace}")]
#endif
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

        /// <summary>
        /// Unescape strings.
        /// </summary>
        [DefaultValue(false)]
        public bool UnescapeStrings { get; set; }

        /// <summary>
        /// Trim delimiter
        /// </summary>
        [DefaultValue(true)]
        public bool TrimDelimiter { get; set; }

        /// <summary>
        /// Array of the combined symbols.
        /// </summary>
        public string[] CombinedSymbols { get; set; }

        /// <summary>
        /// Trim quotes.
        /// </summary>
        public bool TrimQuotes { get; set; }

        /// <summary>
        /// Accept floating point number.
        /// </summary>
        [DefaultValue(true)]
        public bool AcceptFloatingPoint { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TokenizerSettings()
        {
            IgnoreNewLine = true;
            IgnoreWhitespace = true;
            TrimDelimiter = true;
            AcceptFloatingPoint = true;
            SymbolChars = new []
            {
                '=', '+', '-', '/', ',', '.', '*', '~', '!', '@',
                '#', '$', '%', '^', '&', '(', ')', '{', '}', '[',
                ']', ':', ';', '<', '>', '?', '|', '\\'
            };
            CombinedSymbols = new[]
            {
                "+=", "-=", "*=", "/=", "%=", "<=", ">=", "<<",
                ">>", "==", "!=", "++", "--"
            };
        }

        #endregion

        #region Public methods

        #endregion
    }
}
