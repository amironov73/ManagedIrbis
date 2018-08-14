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
    public sealed class ChunkedReader
    {
        #region Nested classes

        // TODO use struct?
        class Chunk
        {
            public int Position;
            public readonly byte[] Buffer;
            public Chunk Next;

            public Chunk(byte[] buffer)
            {
                Position = 0;
                Buffer = buffer;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChunkedReader
            (
                [NotNull] byte[][] buffers
            )
        {
            _firstChunk = new Chunk(buffers[0])
            {
                Position = buffers[0].Length
            };
            _currentChunk = _firstChunk;
            Chunk current = _firstChunk;
            for(int i = 1; i < buffers.Length; i++)
            {
                Chunk newChunk = new Chunk(buffers[i])
                {
                    Position = buffers[i].Length
                };
                current.Next = newChunk;
                current = newChunk;
            }
        }

        internal ChunkedReader
            (
                [NotNull] ChunkedWriter writer
            )
        {
            _firstChunk = new Chunk(writer._firstChunk.Buffer)
            {
                Position = writer._firstChunk.Position
            };
            _currentChunk = _firstChunk;
            Chunk current = _firstChunk;
            for (ChunkedWriter.Chunk oldChunk = writer._firstChunk.Next;
                oldChunk != null; oldChunk = oldChunk.Next)
            {
                Chunk newChunk = new Chunk(oldChunk.Buffer)
                {
                    Position = oldChunk.Position
                };
                current.Next = newChunk;
                current = newChunk;
            }
        }

        #endregion

        #region Private members

        private readonly Chunk _firstChunk;
        private Chunk _currentChunk;
        private int _position;

        #endregion

        #region Public methods

        /// <summary>
        /// Read one byte.
        /// </summary>
        public int ReadByte()
        {
            AGAIN: if (ReferenceEquals(_currentChunk, null))
            {
                return -1;
            }
            if (_position >= _currentChunk.Position)
            {
                _currentChunk = _currentChunk.Next;
                _position = 0;
                goto AGAIN;
            }

            int result = _currentChunk.Buffer[_position];
            _position++;

            return result;
        }

        /// <summary>
        /// Read bytes.
        /// </summary>
        public int Read(byte[] buffer, int offset, int count)
        {
            if (count == 0)
            {
                return 0;
            }

            AGAIN1: if (ReferenceEquals(_currentChunk, null))
            {
                return 0;
            }
            if (_position >= _currentChunk.Position)
            {
                _currentChunk = _currentChunk.Next;
                _position = 0;
                goto AGAIN1;
            }

            int total = 0;
            do
            {
                AGAIN2: if (ReferenceEquals(_currentChunk, null))
                {
                    break;
                }
                int remaining = _currentChunk.Position - _position;
                if (remaining <= 0)
                {
                    _currentChunk = _currentChunk.Next;
                    _position = 0;
                    goto AGAIN2;
                }

                int portion = Math.Min(count, remaining);
                Array.Copy
                    (
                        _currentChunk.Buffer,
                        _position,
                        buffer,
                        offset,
                        portion
                    );
                _position += portion;
                offset += portion;
                count -= portion;
            } while (count > 0);

            return total;
        }

        #endregion
    }
}
