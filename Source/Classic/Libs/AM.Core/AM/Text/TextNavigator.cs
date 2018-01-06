// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextNavigator.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AM.IO;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text
{
    /// <summary>
    /// Навигатор по тексту.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class TextNavigator
    {
        #region Constants

        /// <summary>
        /// Признак конца текста.
        /// </summary>
        public const char EOF = '\0';

        #endregion

        #region Properties

        /// <summary>
        /// Текущая колонка текста. Нумерация с 1.
        /// </summary>
        public int Column { get { return _column; } }

        /// <summary>
        /// Текст закончился?
        /// </summary>
        public bool IsEOF { get { return _position >= _length; } }

        /// <summary>
        /// Длина текста.
        /// </summary>
        public int Length { get { return _length; } }

        /// <summary>
        /// Текущая строка текста. Нумерация с 1.
        /// </summary>
        public int Line { get { return _line; } }

        /// <summary>
        /// Текущая позиция.
        /// </summary>
        public int Position { get { return _position; } }

        /// <summary>
        /// Обрабатываемый текст.
        /// </summary>
        [NotNull]
        public string Text { get { return _text; } }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public TextNavigator
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            // Not supported in .NET Core
            //_text = text.Normalize();
            _text = text;

            _position = 0;
            _length = _text.Length;
            _line = 1;
            _column = 1;
        }

        #endregion

        #region Private members

        private readonly string _text;

        private int _position, _length, _line, _column;

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the navigator.
        /// </summary>
        [NotNull]
        public TextNavigator Clone()
        {
            TextNavigator result = new TextNavigator(_text)
            {
                _column = _column,
                _length = _length,
                _line = _line,
                _position = _position
            };

            return result;
        }

        /// <summary>
        /// Навигатор по текстовому файлу.
        /// </summary>
        [NotNull]
        public static TextNavigator FromFile
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string text = FileUtility.ReadAllText(fileName, encoding);
            TextNavigator result = new TextNavigator(text);

            return result;
        }

        /// <summary>
        /// Навигатор по текстовому файлу.
        /// </summary>
        [NotNull]
        public static TextNavigator FromFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string text = FileUtility.ReadAllText(fileName, Encoding.UTF8);
            TextNavigator result = new TextNavigator(text);

            return result;
        }

        /// <summary>
        /// Выдать остаток текста.
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        [CanBeNull]
        public string GetRemainingText()
        {
            if (IsEOF)
            {
                return null;
            }

            string result = _text.Substring
                (
                    _position,
                    _length - _position
                );

            return result;
        }

        /// <summary>
        /// Управляющий символ?
        /// </summary>
        public bool IsControl()
        {
            char c = PeekChar();
            return char.IsControl(c);
        }

        /// <summary>
        /// Цифра?
        /// </summary>
        public bool IsDigit()
        {
            char c = PeekChar();
            return char.IsDigit(c);
        }

        /// <summary>
        /// Буква?
        /// </summary>
        public bool IsLetter()
        {
            char c = PeekChar();
            return char.IsLetter(c);
        }

        /// <summary>
        /// Буква или цифра?
        /// </summary>
        public bool IsLetterOrDigit()
        {
            char c = PeekChar();
            return char.IsLetterOrDigit(c);
        }

        /// <summary>
        /// Часть числа?
        /// </summary>
        public bool IsNumber()
        {
            char c = PeekChar();
            return char.IsNumber(c);
        }

        /// <summary>
        /// Знак пунктуации?
        /// </summary>
        public bool IsPunctuation()
        {
            char c = PeekChar();
            return char.IsPunctuation(c);
        }

        /// <summary>
        /// Разделитель?
        /// </summary>
        public bool IsSeparator()
        {
            char c = PeekChar();
            return char.IsSeparator(c);
        }

        /// <summary>
        /// Суррогат?
        /// </summary>
        public bool IsSurrogate()
        {
            char c = PeekChar();
            return char.IsSurrogate(c);
        }

        /// <summary>
        /// Символ?
        /// </summary>
        public bool IsSymbol()
        {
            char c = PeekChar();
            return char.IsSymbol(c);
        }

        /// <summary>
        /// Пробельный символ?
        /// </summary>
        public bool IsWhiteSpace()
        {
            char c = PeekChar();
            return char.IsWhiteSpace(c);
        }

        /// <summary>
        /// Заглядывание вперёд на 1 позицию.
        /// </summary>
        /// <remarks>Это на 1 позицию дальше,
        /// чем <see cref="PeekChar()"/>
        /// </remarks>
        public char LookAhead()
        {
            int newPosition = _position + 1;
            if (newPosition >= _length)
            {
                return EOF;
            }

            return _text[newPosition];
        }

        /// <summary>
        /// Заглядывание вперёд.
        /// </summary>
        public char LookAhead
            (
                int distance
            )
        {
            Code.Nonnegative(distance, "distance");

            int newPosition = _position + distance;
            if (newPosition >= _length)
            {
                return EOF;
            }

            return _text[newPosition];
        }

        /// <summary>
        /// Заглядывание назад.
        /// </summary>
        public char LookBehind()
        {
            if (_position == 0)
            {
                return EOF;
            }

            return _text[_position - 1];
        }

        /// <summary>
        /// Заглядывание назад.
        /// </summary>
        public char LookBehind
            (
                int distance
            )
        {
            Code.Positive(distance, "distance");

            if (_position < distance)
            {
                return EOF;
            }

            return _text[_position - distance];
        }

        /// <summary>
        /// Смещение указателя.
        /// </summary>
        public void Move
            (
                int distance
            )
        {
            // TODO Some checks

            _position += distance;
            _column += distance;
        }

        /// <summary>
        /// Подглядывание текущего символа.
        /// </summary>
        public char PeekChar()
        {
            if (_position >= _length)
            {
                return EOF;
            }

            return _text[_position];
        }

        /// <summary>
        /// Подглядывание текущего символа за исключением CR/LF.
        /// </summary>
        public char PeekCharNoCrLf()
        {
            int distance = 0;
            char result = LookAhead(distance);

            while (result == '\r'
                || result == '\n')
            {
                result = LookAhead(++distance);
            }

            return result;
        }

        /// <summary>
        /// Подглядывание строки вплоть до указанной длины.
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        [CanBeNull]
        public string PeekString
            (
                int length
            )
        {
            Code.Positive(length, "length");

            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position, saveColumn = _column,
                saveLine = _line;
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                char c = ReadChar();
                if (c == EOF)
                {
                    break;
                }
                result.Append(c);
            }

            _position = savePosition;
            _column = saveColumn;
            _line = saveLine;

            return result.ToString();
        }

        /// <summary>
        /// Подглядывание строки вплоть до указанной длины.
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        [CanBeNull]
        public string PeekStringNoCrLf
            (
                int length
            )
        {
            Code.Positive(length, "length");

            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position, saveColumn = _column,
                saveLine = _line;
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                char c = ReadCharNoCrLf();
                if (c == EOF)
                {
                    break;
                }
                result.Append(c);
            }

            _position = savePosition;
            _column = saveColumn;
            _line = saveLine;

            return result.ToString();
        }

        /// <summary>
        /// Подглядывание вплоть до указанного символа
        /// (включая его).
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        [CanBeNull]
        public string PeekTo
            (
                char stopChar
            )
        {
            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position, saveColumn = _column,
                saveLine = _line;

            string result = ReadTo(stopChar);

            _position = savePosition;
            _column = saveColumn;
            _line = saveLine;

            return result;
        }

        /// <summary>
        /// Подглядывание вплоть до указанных символов
        /// (включая их).
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        [CanBeNull]
        public string PeekTo
            (
                char[] stopChars
            )
        {
            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position, saveColumn = _column,
                saveLine = _line;

            string result = ReadTo(stopChars);

            _position = savePosition;
            _column = saveColumn;
            _line = saveLine;

            return result;
        }

        /// <summary>
        /// Подглядывание вплоть до указанного символа.
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        [CanBeNull]
        public string PeekUntil
            (
                char stopChar
            )
        {
            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position, saveColumn = _column,
                saveLine = _line;

            string result = ReadUntil(stopChar);

            _position = savePosition;
            _column = saveColumn;
            _line = saveLine;

            return result;
        }

        /// <summary>
        /// Подглядывание вплоть до указанных символов.
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        [CanBeNull]
        public string PeekUntil
            (
                char[] stopChars
            )
        {
            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position, saveColumn = _column,
                saveLine = _line;

            string result = ReadUntil(stopChars);

            _position = savePosition;
            _column = saveColumn;
            _line = saveLine;

            return result;
        }

        /// <summary>
        /// Считывание символа.
        /// </summary>
        public char ReadChar()
        {
            if (_position >= _length)
            {
                return EOF;
            }

            char result = _text[_position];
            _position++;
            if (result == '\n')
            {
                _line++;
                _column = 1;
            }
            else
            {
                _column++;
            }

            return result;
        }

        /// <summary>
        /// Считывание следующего символа, исключая CR/LF.
        /// </summary>
        public char ReadCharNoCrLf()
        {
            char result = ReadChar();

            while (result == '\r'
                || result == '\n')
            {
                result = ReadChar();
            }

            return result;
        }

        /// <summary>
        /// Считывание экранированной строки вплоть до разделителя
        /// (не включая его).
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        [CanBeNull]
        public string ReadEscapedUntil
            (
                char escapeChar,
                char stopChar
            )
        {
            if (IsEOF)
            {
                return null;
            }

            StringBuilder result = new StringBuilder();
            while (true)
            {
                char c = ReadChar();
                if (c == EOF)
                {
                    break;
                }

                if (c == escapeChar)
                {
                    c = ReadChar();
                    if (c == EOF)
                    {
                        Log.Error
                            (
                                "TextNavigator::ReadEscapedUntil: "
                                + "unexpected end of stream"
                            );

                        throw new FormatException();
                    }
                    result.Append(c);
                }
                else if (c == stopChar)
                {
                    break;
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Считывание начиная с открывающего символа
        /// до закрывающего (включая их).
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// Пустая строка, если нет открывающего
        /// или закрывающего символа.
        /// </returns>
        [CanBeNull]
        public string ReadFrom
            (
                char openChar,
                char closeChar
            )
        {
            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position;

            char c = PeekChar();
            if (c != openChar)
            {
                return string.Empty;
            }
            ReadChar();

            while (true)
            {
                c = ReadChar();
                if (c == EOF)
                {
                    return string.Empty;
                }
                if (c == closeChar)
                {
                    break;
                }
            }

            string result = _text.Substring
                (
                    savePosition,
                    _position - savePosition
                );

            return result;
        }

        /// <summary>
        /// Считывание начиная с открывающего символа
        /// до закрывающего (включая их).
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// Пустая строка, если нет открывающего
        /// или закрывающего символа.
        /// </returns>
        [CanBeNull]
        public string ReadFrom
            (
                char[] openChars,
                char[] closeChars
            )
        {
            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position;

            char c = PeekChar();
            if (Array.IndexOf(openChars, c) < 0)
            {
                return string.Empty;
            }
            ReadChar();

            while (true)
            {
                c = ReadChar();
                if (c == EOF)
                {
                    return string.Empty;
                }
                if (Array.IndexOf(closeChars, c) >= 0)
                {
                    break;
                }
            }

            string result = _text.Substring
                (
                    savePosition,
                    _position - savePosition
                );

            return result;
        }

        /// <summary>
        /// Чтение беззнакового целого.
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// Пустую строку, если не число.</returns>
        [CanBeNull]
        public string ReadInteger ()
        {
            if (IsEOF)
            {
                return null;
            }

            if (!IsDigit())
            {
                return string.Empty;
            }

            int savePosition = _position;

            while (IsDigit())
            {
                ReadChar();
            }

            string result = _text.Substring
                (
                    savePosition,
                    _position - savePosition
                );

            return result;
        }

        /// <summary>
        /// Чтение до конца строки.
        /// </summary>
        [CanBeNull]
        public string ReadLine()
        {
            if (IsEOF)
            {
                return null;
            }

            StringBuilder result = new StringBuilder();

            while (!IsEOF)
            {
                char c = PeekChar();
                if (c == '\r' || c == '\n')
                {
                    break;
                }
                c = ReadChar();
                result.Append(c);
            }

            if (!IsEOF)
            {
                char c = PeekChar();

                if (c == '\r')
                {
                    ReadChar();
                    c = PeekChar();
                }
                if (c == '\n')
                {
                    ReadChar();
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Чтение строки вплоть до указанной длины.
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        [CanBeNull]
        public string ReadString
            (
                int length
            )
        {
            Code.Positive(length, "length");

            if (IsEOF)
            {
                return null;
            }

            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                char c = ReadChar();
                if (c == EOF)
                {
                    break;
                }
                result.Append(c);
            }

            return result.ToString();
        }

        /// <summary>
        /// Считывание вплоть до указанного символа
        /// (включая его).
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        [CanBeNull]
        public string ReadTo
            (
                char stopChar
            )
        {
            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position;

            while (true)
            {
                char c = ReadChar();
                if (c == EOF || c == stopChar)
                {
                    break;
                }
            }

            string result = _text.Substring
                (
                    savePosition,
                    _position - savePosition
                );

            return result;
        }

        /// <summary>
        /// Считывание вплоть до указанного разделителя
        /// (разделитель не помещается в возвращаемое значение,
        /// однако, считывается).
        /// </summary>
        [CanBeNull]
        public string ReadTo
            (
                [NotNull] string stopString
            )
        {
            Code.NotNullNorEmpty(stopString, "stopString");

            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position;
            int length = 0;

            while (true)
            {
            AGAIN:
                char c = ReadChar();
                if (c == EOF)
                {
                    _position = savePosition;
                    return null;
                }

                length++;
                if (length >= stopString.Length)
                {
                    int start = _position - stopString.Length;
                    for (int i = 0; i < stopString.Length; i++)
                    {
                        if (_text[start + i] != stopString[i])
                        {
                            goto AGAIN;
                        }
                    }
                    break;
                }
            }

            string result = _text.Substring
                (
                    savePosition,
                    _position - savePosition - stopString.Length
                );

            return result;
        }

        /// <summary>
        /// Считывание вплоть до указанного символа
        /// (включая один из них).
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        [CanBeNull]
        public string ReadTo
            (
                params char[] stopChars
            )
        {
            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position;

            while (true)
            {
                char c = ReadChar();
                if (c == EOF 
                    || Array.IndexOf(stopChars, c) >= 0)
                {
                    break;
                }
            }

            string result = _text.Substring
                (
                    savePosition,
                    _position - savePosition
                );

            return result;
        }

        /// <summary>
        /// Считывание вплоть до указанного символа
        /// (не включая его).
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        [CanBeNull]
        public string ReadUntil
            (
                char stopChar
            )
        {
            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position;

            while (true)
            {
                char c = PeekChar();
                if (c == EOF || c == stopChar)
                {
                    break;
                }
                ReadChar();
            }

            string result = _text.Substring
                (
                    savePosition,
                    _position - savePosition
                );

            return result;
        }

        /// <summary>
        /// Считывание вплоть до указанного разделителя
        /// (разделитель не помещается в возвращаемое значение
        /// и не считывается).
        /// </summary>
        [CanBeNull]
        public string ReadUntil
            (
                [NotNull] string stopString
            )
        {
            Code.NotNullNorEmpty(stopString, "stopString");

            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position;
            int length = 0;

            while (true)
            {
                AGAIN:
                char c = ReadChar();
                if (c == EOF)
                {
                    _position = savePosition;

                    return null;
                }

                length++;
                if (length >= stopString.Length)
                {
                    int start = _position - stopString.Length;
                    for (int i = 0; i < stopString.Length; i++)
                    {
                        if (_text[start + i] != stopString[i])
                        {
                            goto AGAIN;
                        }
                    }
                    break;
                }
            }

            string result = _text.Substring
                (
                    savePosition,
                    _position - savePosition - stopString.Length
                );
            _position -= stopString.Length;

            return result;
        }


        /// <summary>
        /// Считывание вплоть до указанного символа
        /// (не включая его).
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        [CanBeNull]
        public string ReadUntilNoCrLf
            (
                char stopChar
            )
        {
            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position;

            while (true)
            {
                char c = PeekCharNoCrLf();
                if (c == EOF || c == stopChar)
                {
                    break;
                }
                ReadCharNoCrLf();
            }

            string result = _text.Substring
            (
                savePosition,
                _position - savePosition
            );

            return result;
        }

        /// <summary>
        /// Считывание вплоть до указанных символов
        /// (не включая их).
        /// </summary>
        /// <remarks><c>null</c>, если достигнут конец текста.
        /// </remarks>
        [CanBeNull]
        public string ReadUntil
            (
                params char[] stopChars
            )
        {
            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position;

            while (true)
            {
                char c = PeekChar();
                if (c == EOF
                    || Array.IndexOf(stopChars, c) >= 0)
                {
                    break;
                }
                ReadChar();
            }

            string result = _text.Substring
                (
                    savePosition,
                    _position - savePosition
                );

            return result;
        }

        /// <summary>
        /// Считывание вплоть до указанных символов
        /// (не включая их).
        /// </summary>
        /// <remarks><c>null</c>, если достигнут конец текста.
        /// </remarks>
        [CanBeNull]
        public string ReadUntil
            (
                [NotNull] char[] openChars,
                [NotNull] char[] closeChars,
                [NotNull] char[] stopChars
            )
        {
            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position;
            int level = 0;

            while (true)
            {
                char c = PeekChar();

                if (c == EOF)
                {
                    _position = savePosition;
                    return null;
                }

                if (c.OneOf(openChars))
                {
                    level++;
                }
                else if (c.OneOf(closeChars))
                {
                    if (level == 0
                        && c.OneOf(stopChars))
                    {
                        break;
                    }
                    level--;
                }
                else if (c.OneOf(stopChars))
                {
                    if (level == 0)
                    {
                        break;
                    }
                }
                ReadChar();
            }

            string result = _text.Substring
                (
                    savePosition,
                    _position - savePosition
                );

            return result;
        }

        /// <summary>
        /// Считывание строки, пока не будет
        /// встречен пробельный символ или конец текста.
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        [CanBeNull]
        public string ReadUntilWhiteSpace()
        {
            if (IsEOF)
            {
                return null;
            }

            StringBuilder result = new StringBuilder();

            while (!IsEOF)
            {
                char c = ReadChar();
                if (char.IsWhiteSpace(c))
                {
                    break;
                }
                result.Append(c);
            }

            return result.ToString();
        }

        /// <summary>
        /// Считывание, пока встречается указанный символ.
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        [CanBeNull]
        public string ReadWhile
            (
                char goodChar
            )
        {
            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position;

            while (true)
            {
                char c = PeekChar();
                if (c == EOF || c != goodChar)
                {
                    break;
                }
                ReadChar();
            }

            string result = _text.Substring
                (
                    savePosition,
                    _position - savePosition
                );

            return result;
        }

        /// <summary>
        /// Считывание, пока встречается указанные символы.
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        [CanBeNull]
        public string ReadWhile
            (
                params char[] goodChars
            )
        {
            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position;

            while (true)
            {
                char c = PeekChar();
                if (c == EOF
                    || Array.IndexOf(goodChars, c) < 0)
                {
                    break;
                }
                ReadChar();
            }

            string result = _text.Substring
                (
                    savePosition,
                    _position - savePosition
                );

            return result;
        }

        /// <summary>
        /// Read word.
        /// </summary>
        [CanBeNull]
        public string ReadWord()
        {
            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position;

            while (true)
            {
                char c = PeekChar();
                if (c == EOF
                    || !char.IsLetterOrDigit(c))
                {
                    break;
                }
                ReadChar();
            }

            string result = _text.Substring
                (
                    savePosition,
                    _position - savePosition
                );

            return result;
        }

        /// <summary>
        /// Read word.
        /// </summary>
        [CanBeNull]
        public string ReadWord
            (
                params char[] additionalWordCharacters
            )
        {
            if (IsEOF)
            {
                return null;
            }

            int savePosition = _position;

            while (true)
            {
                char c = PeekChar();
                if (c == EOF
                    || !char.IsLetterOrDigit(c)
                        && Array.IndexOf(additionalWordCharacters, c) < 0)
                {
                    break;
                }
                ReadChar();
            }

            string result = _text.Substring
            (
                savePosition,
                _position - savePosition
            );

            return result;
        }

        /// <summary>
        /// Get recent text.
        /// </summary>
        [NotNull]
        public string RecentText
            (
                int length
            )
        {
            int start = _position - length;
            if (start < 0)
            {
                length += start;
                start = 0;
            }
            if (start + length > _length)
            {
                length = _length - start;
            }

            return _text.Substring
                (
                    start,
                    length
                );
        }

        /// <summary>
        /// Restore previously saved position.
        /// </summary>
        public void RestorePosition
            (
                [NotNull] TextPosition saved
            )
        {
            Code.NotNull(saved, "saved");

            _column = saved.Column;
            _line = saved.Line;
            _position = saved.Position;
        }

        /// <summary>
        /// Save current position.
        /// </summary>
        [NotNull]
        public TextPosition SavePosition()
        {
            return new TextPosition(this);
        }

        /// <summary>
        /// Пропускает один символ, если он совпадает с указанным.
        /// </summary>
        /// <returns><c>true</c>, если символ был съеден успешно
        /// </returns>
        public bool SkipChar
            (
                char c
            )
        {
            if (PeekChar() == c)
            {
                ReadChar();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Пропускает указанное число символов.
        /// </summary>
        public bool SkipChar
            (
                int n
            )
        {
            for (int i = 0; i < n; i++)
            {
                ReadChar();
            }

            return !IsEOF;
        }

        /// <summary>
        /// Пропускает один символ, если он совпадает с любым
        /// из указанных.
        /// </summary>
        /// <returns><c>true</c>, если символ был съеден успешно
        /// </returns>
        public bool SkipChar
            (
                params char[] allowed
            )
        {
            if (Array.IndexOf(allowed, PeekChar()) >= 0)
            {
                ReadChar();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Пропускаем управляющие символы.
        /// </summary>
        public bool SkipControl()
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }
                if (IsControl())
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Пропускаем пунктуацию.
        /// </summary>
        public bool SkipPunctuation()
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }
                if (IsPunctuation())
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Skip non-word characters.
        /// </summary>
        public bool SkipNonWord()
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }
                char c = PeekChar();
                if (!char.IsLetterOrDigit(c))
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }
        }


        /// <summary>
        /// Skip non-word characters.
        /// </summary>
        public bool SkipNonWord
            (
                params char[] additionalWordCharacters
            )
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }
                char c = PeekChar();
                if (!char.IsLetterOrDigit(c)
                    && Array.LastIndexOf(additionalWordCharacters, c) < 0)
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Пропускаем произвольное количество символов
        /// из указанного диапазона.
        /// </summary>
        public bool SkipRange
            (
                char fromChar,
                char toChar
            )
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }
                char c = PeekChar();
                if (c >= fromChar && c <= toChar)
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Пропустить указанный символ.
        /// </summary>
        public bool SkipWhile
            (
                char skipChar
            )
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }
                char c = PeekChar();
                if (c == skipChar)
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Пропустить указанные символы.
        /// </summary>
        public bool SkipWhile
            (
                params char[] skipChars
            )
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }
                char c = PeekChar();
                if (Array.IndexOf(skipChars, c) >= 0)
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Пропустить, пока не встретится указанный символ.
        /// Сам символ не считывается.
        /// </summary>
        public bool SkipTo
            (
                char stopChar
            )
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }

                char c = PeekChar();
                if (c == stopChar)
                {
                    return true;
                }
                ReadChar();
            }
        }

        /// <summary>
        /// Пропустить, пока не встретятся указанные символы.
        /// </summary>
        public bool SkipWhileNot
            (
                params char[] goodChars
            )
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }
                char c = PeekChar();
                if (Array.IndexOf(goodChars, c) < 0)
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Пропускаем пробельные символы.
        /// </summary>
        public bool SkipWhitespace()
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }
                if (IsWhiteSpace())
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Пропускаем пробельные символы и пунктуацию.
        /// </summary>
        public bool SkipWhitespaceAndPunctuation()
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }
                if (IsWhiteSpace() || IsPunctuation())
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Split text by given good characters.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public string[] SplitByGoodCharacters
            (
                params char[] goodCharacters
            )
        {
            List<string> result = new List<string>();

            while (!IsEOF)
            {
                SkipWhileNot(goodCharacters);
                string word = ReadWhile(goodCharacters);
                if (!string.IsNullOrEmpty(word))
                {
                    result.Add(word);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Split the remaining text to array of words.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public string[] SplitToWords()
        {
            List<string> result = new List<string>();

            while (true)
            {
                if (!SkipNonWord())
                {
                    break;
                }
                string word = ReadWord();
                if (string.IsNullOrEmpty(word))
                {
                    break;
                }
                result.Add(word);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Split the remaining text to array of words.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public string[] SplitToWords
            (
                params char[] additionalWordCharacters
            )
        {
            List<string> result = new List<string>();

            while (true)
            {
                if (!SkipNonWord(additionalWordCharacters))
                {
                    break;
                }
                string word = ReadWord(additionalWordCharacters);
                if (string.IsNullOrEmpty(word))
                {
                    break;
                }
                result.Add(word);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Get substring.
        /// </summary>
        [NotNull]
        public string Substring
            (
                int offset,
                int length
            )
        {
            string result = _text.Substring(offset, length);

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Line={0}, Column={1}",
                    Line,
                    Column
                );
        }

        #endregion
    }
}
