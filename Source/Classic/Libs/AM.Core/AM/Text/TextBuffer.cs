// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextBuffer.cs -- 
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
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class TextBuffer
    {
        #region Properties

        /// <summary>
        /// Column number (starting from 1).
        /// </summary>
        public int Column { get { return _column; } }

        /// <summary>
        /// Length (number of characters stored).
        /// </summary>
        public int Length { get { return _length; } }

        /// <summary>
        /// Row number (starting from 1).
        /// </summary>
        public int Line { get { return _line; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextBuffer()
        {
            _array = new char[1024];
            _column = 1;
            _line = 1;
        }

        #endregion

        #region Private members

        private char[] _array;

        private int _length, _column, _line;

        private void _CalculateColumn()
        {
            for (int i = _length - 1; i >= 0; i--)
            {
                if (_array[i] == '\n')
                {
                    break;
                }

                _column++;
            }
        }

        private void _EnsureCapacity
            (
                int required
            )
        {
            int length = _array.Length;
            bool needResize = false;

            while (length < required)
            {
                length = length * 2;
                needResize = true;
            }

            if (needResize)
            {
                Array.Resize(ref _array, length);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Delete last char (if present).
        /// </summary>
        public bool Backspace()
        {
            if (_length == 0)
            {
                return false;
            }

            _length--;
            _column--;
            if (_column == 0)
            {
                _line--;
                _column = 1;
                _CalculateColumn();
            }

            return true;
        }

        /// <summary>
        /// Clear all text in the buffer.
        /// </summary>
        [NotNull]
        public TextBuffer Clear()
        {
            _length = 0;
            _line = 1;
            _column = 1;

            return this;
        }

        /// <summary>
        /// Get last char.
        /// </summary>
        public char GetLastChar ()
        {
            if (_length == 0)
            {
                return '\0';
            }

            char result = _array[_length - 1];

            return result;
        }

        /// <summary>
        /// Предваряется явным переводом строки?
        /// </summary>
        public bool PrecededByNewLine()
        {
            char[] newLine = Environment.NewLine.ToCharArray();
            int len = newLine.Length;

            if (_length < len)
            {
                return false;
            }

            bool result = ArrayUtility.Coincide
                (
                    _array,
                    _length - len,
                    newLine,
                    0,
                    len
                );

            return result;
        }

        /// <summary>
        /// Remove sequential empty lines.
        /// </summary>
        [NotNull]
        public TextBuffer RemoveEmptyLines()
        {
            char[] newLine = Environment.NewLine.ToCharArray();
            int len = newLine.Length;

            while (_length > len)
            {
                if (!ArrayUtility.Coincide
                    (
                        _array,
                        _length - len,
                        newLine,
                        0,
                        len
                    ))
                {
                    break;
                }

                _length -= len;
                _line--;
                _column = 1;
                _CalculateColumn();
            }

            return this;
        }

        /// <summary>
        /// Write the character.
        /// </summary>
        [NotNull]
        public TextBuffer Write
            (
                char c
            )
        {
            _EnsureCapacity(_length+1);
            _array[_length] = c;
            _length++;

            if (c == '\n')
            {
                _line++;
                _column = 1;
            }
            else
            {
                _column++;
            }

            return this;
        }

        /// <summary>
        /// Write the text.
        /// </summary>
        [NotNull]
        public TextBuffer Write
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return this;
            }

            char[] characters = text.ToCharArray();
            _EnsureCapacity(_length + characters.Length);

            foreach (char c in characters)
            {
                if (c == '\n')
                {
                    _line++;
                    _column = 1;
                }
                else
                {
                    _column++;
                }
                _array[_length] = c;
                _length++;
            }

            return this;
        }

        /// <summary>
        /// Write formatted text.
        /// </summary>
        [NotNull]
        public TextBuffer Write
            (
                [NotNull] string format,
                params object[] arguments
            )
        {
            Code.NotNull(format, "format");

            string text = string.Format(format, arguments);

            return Write(text);
        }

        /// <summary>
        /// Write new line symbol.
        /// </summary>
        [NotNull]
        public TextBuffer WriteLine()
        {
            return Write(Environment.NewLine);
        }

        /// <summary>
        /// Write text followed by new line symbol.
        /// </summary>
        [NotNull]
        public TextBuffer WriteLine
            (
                [CanBeNull] string text
            )
        {
            Write(text);
            return WriteLine();
        }

        /// <summary>
        /// Write formatted text followed by new line symbol.
        /// </summary>
        [NotNull]
        public TextBuffer WriteLine
            (
                [NotNull] string format,
                params object[] arguments
            )
        {
            Code.NotNull(format, "format");

            string text = string.Format(format, arguments);

            return WriteLine(text);
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return new string(_array, 0, _length);
        }

        #endregion
    }
}
