// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChunkedBuffer.cs -- analog for MemoryStream that uses small chunks
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.IO
{
    /// <summary>
    /// Analog for <see cref="System.IO.MemoryStream"/> that uses
    /// small chunks to hold the data.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ChunkedBuffer
    {
        #region Constants

        /// <summary>
        /// Default chunk size.
        /// </summary>
        public const int DefaultChunkSize = 2048;

        #endregion

        #region Nested classes

        class Chunk
        {
            public readonly byte[] Buffer;

            public Chunk Next;

            public Chunk
                (
                    int size
                )
            {
                Buffer = new byte[size];
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Chunk size.
        /// </summary>
        public int ChunkSize
        {
            get { return _chunkSize; }
        }

        /// <summary>
        /// End of data?
        /// </summary>
        public bool Eof
        {
            get
            {
                if (ReferenceEquals(_current, null))
                {
                    return true;
                }

                if (ReferenceEquals(_current, _last))
                {
                    return _read >= _position;
                }

                return false;
            }
        }

        /// <summary>
        /// Total length.
        /// </summary>
        public int Length
        {
            get
            {
                int result = 0;

                for (
                        Chunk chunk = _first;
                        !ReferenceEquals(chunk, null)
                        && !ReferenceEquals(chunk, _last);
                        chunk = chunk.Next
                    )
                {
                    result += _chunkSize;
                }

                result += _position;

                return result;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChunkedBuffer()
            : this(DefaultChunkSize)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChunkedBuffer
            (
                int chunkSize
            )
        {
            Code.Positive(chunkSize, "chunkSize");

            _chunkSize = chunkSize;
        }

        #endregion

        #region Private members

        private Chunk _first, _current, _last;
        private readonly int _chunkSize;
        private int _position, _read;

        private bool _Advance()
        {
            if (ReferenceEquals(_current, _last))
            {
                return false;
            }

            _current = _current.Next;
            _read = 0;

            return true;
        }

        private void _AppendChunk()
        {
            Chunk newChunk = new Chunk(_chunkSize);
            if (ReferenceEquals(_first, null))
            {
                _first = newChunk;
                _current = newChunk;
            }
            else
            {
                _last.Next = newChunk;
            }
            _last = newChunk;
            _position = 0;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Copy data from the stream.
        /// </summary>
        public void CopyFrom
            (
                [NotNull] Stream stream,
                int bufferSize
            )
        {
            Code.NotNull(stream, "stream");
            Code.Positive(bufferSize, "bufferSize");

            byte[] buffer = new byte[bufferSize];
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                Write(buffer, 0, read);
            }
        }

        /// <summary>
        /// Peek one byte.
        /// </summary>
        public int Peek()
        {
            if (ReferenceEquals(_current, null))
            {
                return -1;
            }

            if (ReferenceEquals(_current, _last))
            {
                if (_read >= _position)
                {
                    return -1;
                }
            }
            else
            {
                if (_read >= _chunkSize)
                {
                    _Advance();
                }
            }

            return _current.Buffer[_read];
        }

        /// <summary>
        /// Read array of bytes.
        /// </summary>
        public int Read
            (
                [NotNull] byte[] buffer
            )
        {
            Code.NotNull(buffer, "buffer");

            return Read(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Read bytes.
        /// </summary>
        public int Read
            (
                byte[] buffer,
                int offset,
                int count
            )
        {
            Code.NotNull(buffer, "buffer");
            Code.Nonnegative(offset, "offset");

            if (count <= 0)
            {
                return 0;
            }

            if (ReferenceEquals(_current, null))
            {
                return 0;
            }

            int total = 0;
            do
            {
                int remaining = ReferenceEquals(_current, _last)
                    ? _position - _read
                    : _chunkSize - _read;

                if (remaining <= 0)
                {
                    if (!_Advance())
                    {
                        break;
                    }
                }

                int portion = Math.Min(count, remaining);
                Array.Copy
                    (
                        _current.Buffer,
                        _read,
                        buffer,
                        offset,
                        portion
                    );
                _read += portion;
                offset += portion;
                count -= portion;
                total += portion;
            } while (count > 0);

            return total;
        }

        /// <summary>
        /// Read one byte.
        /// </summary>
        public int ReadByte()
        {
            if (ReferenceEquals(_current, null))
            {
                return -1;
            }

            if (ReferenceEquals(_current, _last))
            {
                if (_read >= _position)
                {
                    return -1;
                }
            }
            else
            {
                if (_read >= _chunkSize)
                {
                    _Advance();
                }
            }

            return _current.Buffer[_read++];
        }

        /// <summary>
        /// Rewind to the beginning.
        /// </summary>
        public void Rewind()
        {
            _current = _first;
            _read = 0;
        }

        /// <summary>
        /// Get internal buffers.
        /// </summary>
        [NotNull]
        public byte[][] ToArrays
            (
                int prefix
            )
        {
            List<byte[]> result = new List<byte[]>();

            for (int i = 0; i < prefix; i++)
            {
                result.Add(EmptyArray<byte>.Value);
            }

            for (
                    Chunk chunk = _first;
                    !ReferenceEquals(chunk, null)
                    && !ReferenceEquals(chunk, _last);
                    chunk = chunk.Next
                )
            {
                result.Add(chunk.Buffer);
            }

            if (_position != 0)
            {
                byte[] chunk = new byte[_position];
                Array.Copy(_last.Buffer, 0, chunk, 0, _position);
                result.Add(chunk);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Get all data as one big array of bytes.
        /// </summary>
        [NotNull]
        public byte[] ToBigArray()
        {
            int total = Length;
            byte[] result = new byte[total];
            int offset = 0;
            for (
                    Chunk chunk = _first;
                    !ReferenceEquals(chunk, null)
                    && !ReferenceEquals(chunk, _last);
                    chunk = chunk.Next
                )
            {
                Array.Copy(chunk.Buffer, 0, result, offset, _chunkSize);
                offset += _chunkSize;
            }

            if (_position != 0)
            {
                Array.Copy(_last.Buffer, 0, result, offset, _position);
            }

            return result;
        }

        /// <summary>
        /// Write a block of bytes to the current stream
        /// using data read from a buffer.
        /// </summary>
        public void Write
            (
                [NotNull] byte[] buffer
            )
        {
            Write(buffer, 0, buffer.Length);
        }


        /// <summary>
        /// Write a block of bytes to the current stream
        /// using data read from a buffer.
        /// </summary>
        public void Write
            (
                [NotNull] byte[] buffer,
                int offset,
                int count
            )
        {
            Code.NotNull(buffer, "buffer");
            Code.Nonnegative(offset, "offset");

            if (count <= 0)
            {
                return;
            }

            if (ReferenceEquals(_first, null))
            {
                _AppendChunk();
            }

            do
            {
                int free = _chunkSize - _position;
                if (free == 0)
                {
                    _AppendChunk();
                    free = _chunkSize;
                }

                int portion = Math.Min(count, free);
                Array.Copy
                    (
                        buffer,
                        offset,
                        _last.Buffer,
                        _position,
                        portion
                    );

                _position += portion;
                count -= portion;
                offset += portion;
            } while (count > 0);
        }

        /// <summary>
        /// Write the text with encoding.
        /// </summary>
        public void Write
            (
                [NotNull] string text,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(text, "text");
            Code.NotNull(encoding, "encoding");

            byte[] bytes = encoding.GetBytes(text);

            Write(bytes);
        }

        /// <summary>
        /// Write a byte to the current stream at the current position.
        /// </summary>
        public void WriteByte
            (
                byte value
            )
        {
            if (ReferenceEquals(_first, null))
            {
                _AppendChunk();
            }

            if (_position >= _chunkSize)
            {
                _AppendChunk();
            }

            _last.Buffer[_position++] = value;
        }

        #endregion
    }
}
