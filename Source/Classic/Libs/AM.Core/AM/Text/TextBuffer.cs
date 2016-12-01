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
        public int Row { get { return _row; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextBuffer()
        {
            _array = new char[1024];
            _column = 1;
            _row = 1;
        }

        #endregion

        #region Private members

        private char[] _array;

        private int _length, _column, _row;

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
                _row++;
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
                    _row++;
                    _column = 1;
                }
                else
                {
                    _column++;
                }
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
