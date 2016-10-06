/* Token.cs -- токен
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using CodeJam;

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
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Тип токена.
        /// </summary>
        [XmlAttribute("kind")]
        [JsonProperty("kind")]
        public TokenKind Kind { get; private set; }

        /// <summary>
        /// Номер колонки.
        /// </summary>
        [XmlAttribute("column")]
        [JsonProperty("column")]
        public int Column { get; private set; }

        /// <summary>
        /// Номер строки.
        /// </summary>
        [XmlAttribute("line")]
        [JsonProperty("line")]
        public int Line { get; private set; }

        /// <summary>
        /// Значение.
        /// </summary>
        [CanBeNull]
        [XmlText]
        [JsonProperty("value")]
        public string Value { get; internal set; }

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
        /// Constructor.
        /// </summary>
        public Token()
        {
        }

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
            Kind = kind;
            Value = value;
            Line = line;
            Column = column;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert array of words to array of tokens.
        /// </summary>
        [NotNull]
        public static Token[] Convert
            (
                [NotNull] string[] words
            )
        {
            Code.NotNull(words, "words");

            Token[] result = new Token[words.Length];
            for (int i = 0; i < words.Length; i++)
            {
                result[i] = new Token
                    (
                        TokenKind.Unknown,
                        words[i],
                        0,
                        0
                    );
            }

            return result;
        }

        /// <summary>
        /// Create token from <see cref="TextNavigator"/>.
        /// </summary>
        [NotNull]
        public static Token FromNavigator
            (
                [NotNull] TextNavigator navigator,
                string value
            )
        {
            Code.NotNull(navigator, "navigator");

            Token result = new Token
                (
                    TokenKind.Unknown,
                    value,
                    navigator.Line,
                    navigator.Column
                );

            return result;
        }

        /// <summary>
        /// Create token from <see cref="TextNavigator"/>.
        /// </summary>
        [NotNull]
        public static Token FromNavigator
            (
                [NotNull] TextNavigator navigator,
                TokenKind kind,
                string value
            )
        {
            Code.NotNull(navigator, "navigator");

            Token result = new Token
                (
                    kind,
                    value,
                    navigator.Line,
                    navigator.Column
                );

            return result;
        }

        /// <summary>
        /// Convert token to string.
        /// </summary>
        [CanBeNull]
        public static implicit operator string
            (
                [CanBeNull] Token token
            )
        {
            return token == null
                ? null
                : token.Value;
        }

        /// <summary>
        /// Convert text to token.
        /// </summary>
        [NotNull]
        public static implicit operator Token
            (
                [CanBeNull] string text
            )
        {
            return new Token(TokenKind.Unknown, text, 0, 0);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Kind = (TokenKind) reader.ReadPackedInt32();
            Column = reader.ReadPackedInt32();
            Line = reader.ReadPackedInt32();
            Value = reader.ReadNullableString();
        }

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WritePackedInt32((int) Kind)
                .WritePackedInt32(Column)
                .WritePackedInt32(Line)
                .WriteNullable(Value);
        }

        #endregion

        #region Object members

        /// <inheritdoc />
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
