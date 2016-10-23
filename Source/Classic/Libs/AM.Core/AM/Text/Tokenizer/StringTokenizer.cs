/* StringTokenizer2.cs -- tokenizes text
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text.Tokenizer
{
    /// <summary>
    /// Tokenizes text.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class StringTokenizer
        : IEnumerable<Token>
    {
        #region Constants

        /// <summary>
        /// Признак конца текста.
        /// </summary>
        private const char EOF = '\0';

        #endregion

        #region Properties

        /// <summary>
        /// Tokenizer settings.
        /// </summary>
        [NotNull]
        public TokenizerSettings Settings
        {
            get { return _settings; }
            set
            {
                Code.NotNull(value, "value");

                _settings = value;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public StringTokenizer
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            _text = text;
            _length = _text.Length;
            _position = 0;
            _line = 1;
            _column = 1;
            Settings = new TokenizerSettings();
        }

        #endregion

        #region Private members

        private int _length, _position;

        private int _line, _column;

        private string _text;

        private TokenizerSettings _settings;

        private Token _CreateToken
            (
                TokenKind kind
            )
        {
            return new Token
                (
                    kind,
                    string.Empty,
                    _line,
                    _column
                );
        }

        private bool _IsWhitespace
            (
                char c
            )
        {
            return char.IsWhiteSpace(c)
                && !((c == '\r') || (c == '\n'));
        }

        private bool _IsWord
            (
                char c
            )
        {
            return char.IsLetterOrDigit(c)
                   || (c == '_');
        }

        private bool _IsSymbol
            (
                char c
            )
        {
            return (Array.IndexOf(Settings.SymbolChars, c) >= 0);
        }

        private Token _SetTokenValue
            (
                Token token,
                int begin
            )
        {
            token.Value = _text.Substring(begin, _position - begin);
            return token;
        }

        private Token _ReadWhitespace()
        {
            Token result = _CreateToken(TokenKind.Whitespace);
            int begin = _position;
            ReadChar();
            while (true)
            {
                char c = PeekChar();
                if (!_IsWhitespace(c))
                {
                    break;
                }
                ReadChar();
            }

            return _SetTokenValue(result, begin);
        }

        private Token _ReadNumber()
        {
            Token result = _CreateToken(TokenKind.Number);
            int begin = _position;
            ReadChar();
            bool dotFound = _text[begin] == '.';
            char c;
            while (true)
            {
                c = PeekChar();
                if (c == '.' && !dotFound)
                {
                    dotFound = true;
                    ReadChar();
                    continue;
                }
                if ((c == 'E') || (c == 'e'))
                {
                    ReadChar();
                    c = PeekChar();
                    if (c == '-')
                    {
                        ReadChar();
                        c = PeekChar();
                    }
                    break;
                }
                if (!char.IsDigit(c))
                {
                    goto DONE;
                }
                ReadChar();
            }
            if (!char.IsDigit(c))
            {
                throw new TokenizerException("Floating point format error");
            }
            while (char.IsDigit(c))
            {
                ReadChar();
                c = PeekChar();
            }

            DONE:
            return _SetTokenValue(result, begin);
        }

        private Token _ReadString()
        {
            Token result = _CreateToken(TokenKind.QuotedString);
            int begin = _position;
            char stop = ReadChar();
            while (true)
            {
                char c = PeekChar();
                if (c == '\\')
                {
                    ReadChar();
                    c = ReadChar(); // handle \t, \n etc
                    if (c == 'x') // handle \x123
                    {
                        ReadChar();
                        while (char.IsDigit(c))
                        {
                            c = ReadChar();
                        }
                    }
                }
                else if (c == stop)
                {
                    ReadChar();
                    c = PeekChar();
                    if (c == stop)
                    {
                        // Удвоение ограничителя означает его экранирование
                        ReadChar();
                    }
                    else
                    {
                        break;
                    }
                }
                ReadChar();
            }

            _SetTokenValue(result, begin);

            if (Settings.TrimQuotes
                && !string.IsNullOrEmpty(result.Value))
            {
                result.Value = result.Value.Unquote(stop);

                result.Value = result.Value.Replace
                    (
                        stop.ToString() + stop,
                        stop.ToString()
                    );
            }

            return result;
        }

        private Token _ReadWord()
        {
            Token result = _CreateToken(TokenKind.Word);
            int begin = _position;
            ReadChar();
            while (true)
            {
                char c = PeekChar();
                if (!_IsWord(c))
                {
                    break;
                }
                ReadChar();
            }

            return _SetTokenValue(result, begin);
        }

        private Token _ReadSymbol()
        {
            Token result = _CreateToken(TokenKind.Symbol);
            int begin = _position;
            ReadChar();

            return _SetTokenValue(result, begin);
        }

        private Token _ReadUnknown()
        {
            Token result = _CreateToken(TokenKind.Unknown);
            int begin = _position;
            ReadChar();

            return _SetTokenValue(result, begin);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get all tokens.
        /// </summary>
        public Token[] GetAllTokens()
        {
            List<Token> result = new List<Token>();

            while (true)
            {
                Token token = NextToken();
                if (token.IsEOF)
                {
                    if (!Settings.IgnoreEOF)
                    {
                        result.Add(token);
                    }
                    break;
                }
                result.Add(token);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Заглядывание на один символ вперед.
        /// </summary>
        /// <returns>EOF, если достигнут конец текста.</returns>
        public char PeekChar()
        {
            if (_position >= _length)
            {
                return EOF;
            }
            return _text[_position];
        }

        /// <summary>
        /// Чтение одного символа с продвижением вперед.
        /// </summary>
        /// <returns>EOF, если достигнут конец текста.</returns>
        public char ReadChar()
        {
            if (_position >= _length)
            {
                return EOF;
            }
            char result = _text[_position];
            _position++;
            _column++;
            return result;
        }

        /// <summary>
        /// Get the next token.
        /// </summary>
        [NotNull]
        public Token NextToken()
        {
        BEGIN: char c = PeekChar();
            Token result;

            if (c == EOF)
            {
                return _CreateToken(TokenKind.EOF);
            }
            if (c == '\r')
            {
                ReadChar();
                goto BEGIN;
            }
            if (c == '\n')
            {
                ReadChar();
                result = _CreateToken(TokenKind.EOL);
                _line++;
                _column = 1;
                if (!Settings.IgnoreNewLine)
                {
                    return result;
                }
                goto BEGIN;
            }
            if (char.IsWhiteSpace(c))
            {
                result = _ReadWhitespace();
                if (!Settings.IgnoreWhitespace)
                {
                    return result;
                }
                goto BEGIN;
            }
            if (char.IsDigit(c)||(c=='.'))
            {
                return _ReadNumber();
            }
            if ((c == '"') || (c == '\''))
            {
                return _ReadString();
            }
            if (_IsWord(c))
            {
                return _ReadWord();
            }
            if (_IsSymbol(c))
            {
                return _ReadSymbol();
            }

            return _ReadUnknown();
        }

        #endregion

        #region IEnumerable<Token> members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through
        /// the collection.
        /// </summary>
        public IEnumerator<Token> GetEnumerator()
        {
            while (true)
            {
                Token result = NextToken();
                yield return result;
                if (result.IsEOF)
                {
                    yield break;
                }
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
