/* TextNavigator.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            _text = text.Normalize();
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

        #endregion
    }
}
