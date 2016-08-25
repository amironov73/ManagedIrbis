/* TextNavigator.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Text;

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

            string text = File.ReadAllText(fileName, encoding);
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

            string text = File.ReadAllText(fileName);
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
        /// чем <see cref="PeekChar"/>
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
            Code.Positive(distance, "distance");

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
                if ((c == EOF) || (c == stopChar))
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
                if ((c == EOF) 
                    || (Array.IndexOf(stopChars, c) >= 0))
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
                if ((c == EOF) || (c == stopChar))
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
                if ((c == EOF)
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
                if ((c == EOF) || (c != goodChar))
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
                if ((c == EOF)
                    || (Array.IndexOf(goodChars, c) < 0))
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
                if ((c >= fromChar) && (c <= toChar))
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

        #endregion
    }
}
