// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Streamer.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Text;

using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.IO
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Streamer
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// End of the stream?
        /// </summary>
        public bool Eof
        {
            get
            {
                return _position >= _limit && !_Advance();
            }
        }

        /// <summary>
        /// Inner stream.
        /// </summary>
        [NotNull]
        public Stream InnerStream
        {
            get { return _stream; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Streamer
            (
                [NotNull] Stream stream,
                int bufferSize
            )
        {
            Code.NotNull(stream, "stream");
            Code.Positive(bufferSize, "bufferSize");

            if (!stream.CanRead)
            {
                throw new ArgumentException();
            }

            _stream = stream;
            _bufferSize = bufferSize;
            _buffer = new byte[_bufferSize];
            _position = 0;
            _limit = 0;
            _Advance();
        }

        #endregion

        #region Private members

        private readonly Stream _stream;
        private byte[] _buffer;
        private readonly int _bufferSize;
        private int _position, _limit;

        private bool _Advance()
        {
            _limit = _stream.Read(_buffer, 0, _bufferSize);
            _position = 0;

            return _limit > 0;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Peek one byte.
        /// </summary>
        public int PeekByte()
        {
            if (_position < _limit)
            {
                return _buffer[_position];
            }

            if (!_Advance())
            {
                return -1;
            }

            return _buffer[_position];
        }

        /// <summary>
        /// Read some bytes.
        /// </summary>
        public int Read
            (
                [NotNull] byte[] buffer
            )
        {
            return Read(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Read some bytes.
        /// </summary>
        public int Read
            (
                [NotNull] byte[] buffer,
                int offset,
                int count
            )
        {
            Code.NotNull(buffer, "buffer");
            Code.Nonnegative(offset, "offset");

            int total = 0;
            while (count > 0)
            {
                int tail = _limit - _position;
                int portion = Math.Min(tail, count);
                if (portion <= 0)
                {
                    if (!_Advance())
                    {
                        break;
                    }
                    continue;
                }

                Array.Copy
                    (
                        _buffer,
                        _position,
                        buffer,
                        offset,
                        portion
                    );

                count -= portion;
                offset += portion;
                _position += portion;
                total += portion;
            }

            return total;
        }

        /// <summary>
        /// Read one byte.
        /// </summary>
        public int ReadByte()
        {
            if (_position < _limit)
            {
                return _buffer[_position++];
            }

            if (!_Advance())
            {
                return -1;
            }

            return _buffer[_position++];
        }

        /// <summary>
        /// Read one line.
        /// </summary>
        [CanBeNull]
        public string ReadLine
            (
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(encoding, "encoding");

            if (Eof)
            {
                return null;
            }

            using (MemoryStream result = new MemoryStream())
            {
                byte found = 0;
                while (found == 0)
                {
                    while (_position < _limit)
                    {
                        byte one = _buffer[_position++];
                        if (one == '\r' || one == '\n')
                        {
                            found = one;
                            break;
                        }

                        result.WriteByte(one);
                    }

                    if (found == 0 && !_Advance())
                    {
                        break;
                    }
                }

                if (found == '\r')
                {
                    int peek = PeekByte();
                    if (peek == '\n')
                    {
                        ReadByte();
                    }
                }

                return EncodingUtility.GetString(encoding, result.ToArray());
            }
        }

        /// <summary>
        /// Get the rest of the stream.
        /// </summary>
        [NotNull]
        public byte[] ToArray()
        {
            MemoryStream result = new MemoryStream();

            if (_position < _limit)
            {
                result.Write(_buffer, _position, _limit - _position);
                _position = _limit;
            }

            StreamUtility.AppendTo(_stream, result, 0);

            return result.ToArray();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _stream.Dispose();
        }

        #endregion
    }
}
