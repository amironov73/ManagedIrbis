// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StreamParser.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
            get { return PeekChar() == EOF; }
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

        /// <summary>
        /// Constructor.
        /// </summary>
        public StreamParser
            (
                [NotNull] TextReader reader,
                bool ownReader
            )
        {
            Code.NotNull(reader, "reader");

            _reader = reader;
            _ownReader = ownReader;
        }

        #endregion

        #region Private members

        private TextReader _reader;

        private bool _ownReader;

        private StringBuilder _ReadNumber()
        {
            StringBuilder result = new StringBuilder();
            char c = PeekChar();
            if ((c == '-') || (c == '+'))
            {
                result.Append(ReadChar());
            }
            while (IsDigit())
            {
                result.Append(ReadChar());
            }
            c = PeekChar();
            if (c == '.')
            {
                result.Append(ReadChar());
                while (IsDigit())
                {
                    result.Append(ReadChar());
                }
                c = PeekChar();
            }
            if ((c == 'e') || (c == 'E'))
            {
                result.Append(ReadChar());
                c = PeekChar();
                if ((c == '-') || (c == '+'))
                {
                    result.Append(ReadChar());
                }
                while (IsDigit())
                {
                    result.Append(ReadChar());
                }
                c = PeekChar();
            }
            //if ((c == 'F') || (c == 'f') || (c == 'D') || (c == 'd')
            //    || (c == 'M') || (c == 'm'))
            //{
            //    result.Append(ReadChar());
            //}
            return result;
        }

        #endregion

        #region Public methods

#if !WIN81 && !PORTABLE

        /// <summary>
        /// Construct the <see cref="StreamParser"/>
        /// from the local text file.
        /// </summary>
        [NotNull]
        public static StreamParser FromFile
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(fileName, "fileName");
            Code.FileExists(fileName, "fileName");
            Code.NotNull(encoding, "encoding");

            StreamReader reader = new StreamReader
                (
                    File.OpenRead
                    (
                        fileName
                    ),
                    encoding
                );
            StreamParser result = new StreamParser
                (
                    reader,
                    true
                );

            return result;
        }

#endif

        /// <summary>
        /// Construct the <see cref="StreamParser"/>
        /// from the given text.
        /// </summary>
        [NotNull]
        public static StreamParser FromString
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            StringReader reader = new StringReader(text);
            StreamParser result = new StreamParser
                (
                    reader,
                    true
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
        /// Read fixed point number from stream.
        /// </summary>
        public decimal? ReadDecimal
            (
                [NotNull] IFormatProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            if (!SkipWhitespace())
            {
                return null;
            }

            StringBuilder result = _ReadNumber();

            return decimal.Parse
                (
                    result.ToString(),
                    provider
                );
        }

        /// <summary>
        /// Read fixed point number from stream.
        /// </summary>
        public decimal? ReadDecimal()
        {
            return ReadDecimal(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Read floating point number from stream.
        /// </summary>
        public double? ReadDouble
            (
                [NotNull] IFormatProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            if (!SkipWhitespace())
            {
                return null;
            }

            StringBuilder result = _ReadNumber();

            return double.Parse
                (
                    result.ToString(),
                    provider
                );
        }

        /// <summary>
        /// Read floating point number from stream.
        /// </summary>
        public double? ReadDouble()
        {
            return ReadDouble(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Read 16-bit signed integer from stream.
        /// </summary>
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
        /// Read floating point number from stream.
        /// </summary>
        public float? ReadSingle
            (
                [NotNull] IFormatProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            if (!SkipWhitespace())
            {
                return null;
            }

            StringBuilder result = _ReadNumber();

            return float.Parse
                (
                    result.ToString(),
                    provider
                );
        }

        /// <summary>
        /// Read floating point number from stream.
        /// </summary>
        public float? ReadSingle()
        {
            return ReadSingle(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Read 16-bit unsigned integer from stream.
        /// </summary>
        [CanBeNull]
        [CLSCompliant(false)]
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
        [CanBeNull]
        [CLSCompliant(false)]
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
        [CanBeNull]
        [CLSCompliant(false)]
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

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (_ownReader)
            {
                _reader.Dispose();
            }
        }

        #endregion
    }
}
