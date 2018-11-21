// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChunkArray.cs -- character set
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;


#endregion

// ReSharper disable ForCanBeConvertedToForeach

namespace AM.Collections
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ChunkArray<T>
    {
        #region Constants

        /// <summary>
        /// Default chunk size.
        /// </summary>
        public const int ChunkSize = 1024;

        #endregion

        #region Properties

        /// <summary>
        /// Length of the array.
        /// </summary>
        public int Length { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChunkArray
            (
                int length
            )
        {
            Code.Nonnegative(length, "length");

            Length = length;
            int chunkCount = (length + ChunkSize - 1) / ChunkSize;
            int lastChunk = chunkCount - 1;
            _chunks = new T[chunkCount][];
            for (int i = 0; i < lastChunk; i++)
            {
                _chunks[i] = new T[ChunkSize];
            }

            if (lastChunk >= 0)
            {
                _chunks[lastChunk] = new T[length % ChunkSize];
            }
        }

        #endregion

        #region Private members

        private readonly T[][] _chunks;

        #endregion

        #region Public methods

        /// <inheritdoc cref="Array.CopyTo(Array,int)"/>
        public void CopyTo
            (
                [NotNull] Array array,
                int index
            )
        {
            Code.NotNull(array, "array");

            foreach (T[] chunk in _chunks)
            {
                int length = chunk.Length;
                Array.Copy(chunk, 0, array, index, length);
                index += length;
            }
        }

        /// <inheritdoc cref="Array.IndexOf{T}(T[],T)"/>
        public int IndexOf
            (
                T item
            )
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;

            int index = 0;
            foreach (T[] chunk in _chunks)
            {
                for (int i = 0; i < chunk.Length; i++)
                {
                    if (comparer.Equals(item, chunk[i]))
                    {
                        return index;
                    }
                    index++;
                }
            }

            return -1;
        }

        /// <inheritdoc cref="IList{T}.this"/>
        public T this[int index]
        {
            get
            {
                Code.ValidIndex(index, "index", Length);

                return _chunks[index / ChunkSize][index % ChunkSize];
            }
            set
            {
                Code.ValidIndex(index, "index", Length);

                _chunks[index / ChunkSize][index % ChunkSize] = value;
            }
        }

        #endregion
    }
}
