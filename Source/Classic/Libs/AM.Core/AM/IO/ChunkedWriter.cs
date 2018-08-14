// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChunkedWriter.cs -- analog for MemoryStream that uses small chunks
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
    public sealed class ChunkedWriter
    {
        #region Nested classes

        // TODO use struct?
        internal class Chunk
        {
            public int Position;
            public readonly byte[] Buffer;
            public Chunk Next;

            public Chunk(int size)
            {
                Position = 0;
                Buffer = new byte[size];
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChunkedWriter()
            : this(2048)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChunkedWriter
            (
                int chunkSize
            )
        {
            Code.Positive(chunkSize, "chunkSize");

            _chunkSize = chunkSize;
            _firstChunk = new Chunk(chunkSize);
            _lastChunk = _firstChunk;
        }

        #endregion

        #region Private members

        internal readonly Chunk _firstChunk;
        private Chunk _lastChunk;
        internal readonly int _chunkSize;

        private void AppendChunk()
        {
            Chunk newChunk = new Chunk(_chunkSize);
            _lastChunk.Next = newChunk;
            _lastChunk = newChunk;
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
        /// Get internal buffers.
        /// </summary>
        [NotNull]
        public byte[][] ToArrays
            (
                bool addPrefix
            )
        {
            List<byte[]> result = new List<byte[]>();

            if (addPrefix)
            {
                result.Add(new byte[0]);
            }

            for (Chunk chunk = _firstChunk; chunk != _lastChunk; chunk = chunk.Next)
            {
                result.Add(chunk.Buffer);
            }

            if (_lastChunk.Position != 0)
            {
                byte[] tail = new byte[_lastChunk.Position];
                result.Add(tail);
                Array.Copy(_lastChunk.Buffer, tail, _lastChunk.Position);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Get all data as one big array of bytes.
        /// </summary>
        public byte[] ToBigArray()
        {
            int totalLength = 0;
            for (Chunk chunk = _firstChunk; chunk != null; chunk = chunk.Next)
            {
                totalLength += chunk.Position;
            }

            byte[] result = new byte[totalLength];
            int offset = 0;
            for (Chunk chunk = _firstChunk; chunk != null; chunk = chunk.Next)
            {
                int length = chunk.Position;
                Array.Copy(chunk.Buffer, 0, result, offset, length);
                offset += length;
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public ChunkedReader ToReader()
        {
            ChunkedReader result = new ChunkedReader(this);

            return result;
        }

        /// <summary>
        /// Writes a block of bytes to the current stream
        /// using data read from a buffer.
        /// </summary>
        [NotNull]
        public ChunkedWriter Write
            (
                [NotNull] byte[] buffer
            )
        {
            return Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Writes a block of bytes to the current stream
        /// using data read from a buffer.
        /// </summary>
        [NotNull]
        public ChunkedWriter Write
            (
                [NotNull] byte[] buffer,
                int offset,
                int count
            )
        {
            if (count <= 0)
            {
                return this;
            }

            do
            {
                int free = _chunkSize - _lastChunk.Position;
                if (free == 0)
                {
                    AppendChunk();
                    free = _chunkSize;
                }
                int portion = Math.Min(count, free);

                Array.Copy
                    (
                        buffer,
                        offset,
                        _lastChunk.Buffer,
                        _lastChunk.Position,
                        portion
                    );

                _lastChunk.Position += portion;
                count -= portion;
                offset += portion;
            } while (count > 0);

            return this;
        }

        /// <summary>
        /// Write the text with encoding.
        /// </summary>
        [NotNull]
        public ChunkedWriter Write
            (
                [NotNull] string text,
                [NotNull] Encoding encoding
            )
        {
            byte[] bytes = encoding.GetBytes(text);

            return Write(bytes);
        }

        /// <summary>
        /// Writes a byte to the current stream at the current position.
        /// </summary>
        [NotNull]
        public ChunkedWriter WriteByte
            (
                byte value
            )
        {
            int position = _lastChunk.Position;
            if (position >= _chunkSize)
            {
                AppendChunk();
                position = _lastChunk.Position;
            }
            _lastChunk.Buffer[position] = value;
            _lastChunk.Position = position + 1;

            return this;
        }

        #endregion
    }
}
