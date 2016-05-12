/* StreamParser.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text
{
    /// <summary>
    /// Считывание из потока чисел, идентификаторов
    /// и прочего.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class StreamParser
        : IDisposable
    {
        #region Constants

        /// <summary>
        /// End of stream reached.
        /// </summary>
        public const char EOF = unchecked ((char)-1);

        #endregion

        #region Properties

        /// <summary>
        /// Is end of stream reached.
        /// </summary>
        public bool EndOfStream
        {
            get { return Reader.Peek() == EOF; }
        }

        /// <summary>
        /// Underlying <see cref="TextReader"/>
        /// </summary>
        [NotNull]
        public TextReader Reader { get { return _reader; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public StreamParser
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            _reader = reader;
        }

        #endregion

        #region Private members

        private TextReader _reader;

        #endregion

        #region Public methods

        [NotNull]
        public static StreamParser FromString
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            StringReader reader = new StringReader(text);
            StreamParser result = new StreamParser(reader);

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
        /// Peek one character from stream.
        /// </summary>
        public char PeekChar()
        {
            return unchecked ((char)Reader.Peek());
        }

        /// <summary>
        /// Read one character from stream.
        /// </summary>
        /// <returns></returns>
        public char ReadChar()
        {
            return unchecked ((char) Reader.Read());
        }

        /// <summary>
        /// Read 16-bit signed integer from stream.
        /// </summary>
        /// <returns></returns>
        [CanBeNull]
        public short? ReadInt16()
        {
            if (!SkipWhitespace())
            {
                return null;
            }

            StringBuilder result = new StringBuilder();
            if (PeekChar() == '-')
            {
                result.Append(ReadChar());
            }
            while (IsDigit())
            {
                result.Append(ReadChar());
            }

            return short.Parse(result.ToString());
        }


        /// <summary>
        /// Read 32-bit signed integer from stream.
        /// </summary>
        /// <returns></returns>
        [CanBeNull]
        public int? ReadInt32 ()
        {
            if (!SkipWhitespace())
            {
                return null;
            }

            StringBuilder result = new StringBuilder();
            if (PeekChar() == '-')
            {
                result.Append(ReadChar());
            }
            while (IsDigit())
            {
                result.Append(ReadChar());
            }

            return int.Parse(result.ToString());
        }

        /// <summary>
        /// Read 64-bit signed integer from stream.
        /// </summary>
        /// <returns></returns>
        [CanBeNull]
        public long? ReadInt64()
        {
            if (!SkipWhitespace())
            {
                return null;
            }

            StringBuilder result = new StringBuilder();
            if (PeekChar() == '-')
            {
                result.Append(ReadChar());
            }
            while (IsDigit())
            {
                result.Append(ReadChar());
            }

            return long.Parse(result.ToString());
        }

        /// <summary>
        /// Read 16-bit unsigned integer from stream.
        /// </summary>
        /// <returns></returns>
        [CanBeNull]
        public ushort? ReadUInt16()
        {
            if (!SkipWhitespace())
            {
                return null;
            }

            StringBuilder result = new StringBuilder();
            while (IsDigit())
            {
                result.Append(ReadChar());
            }

            return ushort.Parse(result.ToString());
        }

        /// <summary>
        /// Read 16-bit unsigned integer from stream.
        /// </summary>
        /// <returns></returns>
        [CanBeNull]
        public uint? ReadUInt32()
        {
            if (!SkipWhitespace())
            {
                return null;
            }

            StringBuilder result = new StringBuilder();
            while (IsDigit())
            {
                result.Append(ReadChar());
            }

            return uint.Parse(result.ToString());
        }

        /// <summary>
        /// Read 64-bit unsigned integer from stream.
        /// </summary>
        /// <returns></returns>
        [CanBeNull]
        public ulong? ReadUInt64()
        {
            if (!SkipWhitespace())
            {
                return null;
            }

            StringBuilder result = new StringBuilder();
            while (IsDigit())
            {
                result.Append(ReadChar());
            }

            return ulong.Parse(result.ToString());
        }

        /// <summary>
        /// Пропускаем управляющие символы.
        /// </summary>
        public bool SkipControl()
        {
            while (true)
            {
                if (EndOfStream)
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
                if (EndOfStream)
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
                if (EndOfStream)
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

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated
        /// with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // TODO
        }

        #endregion
    }
}
