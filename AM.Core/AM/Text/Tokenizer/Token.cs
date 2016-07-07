/* Token.cs -- токен
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Xml.Serialization;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.Text.Tokenizer
{
    /// <summary>
    /// Текстовый токен.
    /// </summary>
    [PublicAPI]
    [XmlRoot("token")]
    [MoonSharpUserData]
    [DebuggerDisplay("Kind={Kind} Value='{Value}'")]
    public class Token
    {
        #region Properties

        /// <summary>
        /// Тип токена.
        /// </summary>
        [XmlAttribute("kind")]
        [JsonProperty("kind")]
        public TokenKind Kind
        {
            get { return _kind; }
        }

        /// <summary>
        /// Номер колонки.
        /// </summary>
        [XmlAttribute("column")]
        [JsonProperty("column")]
        public int Column
        {
            get { return _column; }
        }

        /// <summary>
        /// Номер строки.
        /// </summary>
        [XmlAttribute("line")]
        [JsonProperty("line")]
        public int Line
        {
            get { return _line; }
        }

        /// <summary>
        /// Значение.
        /// </summary>
        [CanBeNull]
        [XmlText]
        [JsonProperty("value")]
        public string Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Признак конца текста?
        /// </summary>
        public bool IsEOF
        {
            get { return Kind == TokenKind.EOF; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Token
            (
                TokenKind kind,
                [CanBeNull] string value,
                int line,
                int column
            )
        {
            _kind = kind;
            _value = value;
            _line = line;
            _column = column;
        }

        #endregion

        #region Private members

        private int _line;
        private int _column;
        internal string _value;
        private TokenKind _kind;

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format
                (
                    "Kind: {0}, Column: {1}, Line: {2}, Value: {3}", 
                    Kind, 
                    Column, 
                    Line, 
                    Value.ToVisibleString()
                );
        }

        #endregion
    }
}
