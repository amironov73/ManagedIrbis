// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChunkList.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
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
    public class ChunkList<T>
        : IList<T>
    {
        #region Constants

        /// <summary>
        /// Chunk size.
        /// </summary>
        public const int ChunkSize = 512;

        #endregion

        #region Properties

        /// <summary>
        /// Capacity.
        /// </summary>
        public int Capacity
        {
            get { return _capacity; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChunkList()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChunkList
            (
                int capacity
            )
        {
            Code.Positive(capacity, "capacity");

            EnsureCapacity(capacity);
        }

        #endregion

        #region Private members

        private T[][] _chunks;

        private int _capacity, _count;

        private void EnsureCapacity
            (
                int capacity
            )
        {
            if (_capacity >= capacity)
            {
                return;
            }

            int oldCount = 0;
            if (!ReferenceEquals(_chunks, null))
            {
                oldCount = _chunks.Length;
            }

            int newCount = (capacity + ChunkSize - 1) / ChunkSize;
            _capacity = newCount * ChunkSize;
            T[][] newChunks = new T[newCount][];
            if (!ReferenceEquals(_chunks, null))
            {
                for (int i = 0; i < oldCount; i++)
                {
                    newChunks[i] = _chunks[i];
                }
            }

            for (int i = oldCount; i < newCount; i++)
            {
                newChunks[i] = new T[ChunkSize];
            }

            _chunks = newChunks;
        }

        #endregion

        #region ICollection<T> members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>) this).GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<T> GetEnumerator()
        {
            int nchunks = _count / ChunkSize;
            for (int i = 0; i < nchunks; i++)
            {
                T[] chunk = _chunks[i];
                for (int j = 0; j < chunk.Length; j++)
                {
                    yield return chunk[j];
                }
            }

            int xtra = _count % ChunkSize;
            if (xtra != 0)
            {
                T[] chunk = _chunks[nchunks];
                for (int i = 0; i < xtra; i++)
                {
                    yield return chunk[i];
                }
            }
        }

        /// <inheritdoc cref="ICollection{T}.Add" />
        public void Add
            (
                T item
            )
        {
            EnsureCapacity(_count + 1);
            _chunks[_count / ChunkSize][_count % ChunkSize] = item;
            _count = _count + 1;
        }

        /// <inheritdoc cref="ICollection{T}.Clear" />
        public void Clear()
        {
            _count = 0;
        }

        /// <inheritdoc cref="ICollection{T}.Contains" />
        public bool Contains
            (
                T item
            )
        {
            return IndexOf(item) >= 0;
        }

        /// <inheritdoc cref="ICollection{T}.CopyTo" />
        public void CopyTo
            (
                T[] array,
                int arrayIndex
            )
        {
            int nchunks = _count / ChunkSize;
            for (int i = 0; i < nchunks; i++)
            {
                Array.Copy(_chunks[i], 0, array, arrayIndex, ChunkSize);
                arrayIndex += ChunkSize;
            }

            int xtra = _count % ChunkSize;
            if (xtra != 0)
            {
                Array.Copy(_chunks[nchunks], 0, array, arrayIndex, xtra);
            }
        }

        /// <inheritdoc cref="ICollection{T}.Remove" />
        public bool Remove
            (
                T item
            )
        {
            int index = IndexOf(item);
            if (index < 0)
            {
                return false;
            }

            RemoveAt(index);

            return true;
        }

        /// <inheritdoc cref="ICollection{T}.Count" />
        public int Count
        {
            get { return _count; }
        }

        /// <inheritdoc cref="ICollection{T}.IsReadOnly" />
        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region IList<T> members

        /// <inheritdoc cref="IList{T}.IndexOf" />
        public int IndexOf
            (
                T item
            )
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            int nchunks = _count / ChunkSize;
            int index = 0;
            for (int i = 0; i < nchunks; i++)
            {
                T[] chunk = _chunks[i];
                for (int j = 0; j < chunk.Length; j++)
                {
                    if (comparer.Equals(item, chunk[j]))
                    {
                        return index;
                    }

                    index++;
                }
            }

            int xtra = _count % ChunkSize;
            if (xtra != 0)
            {
                T[] chunk = _chunks[nchunks];
                for (int i = 0; i < xtra; i++)
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

        /// <inheritdoc cref="IList{T}.Insert" />
        public void Insert
            (
                int index,
                T item
            )
        {
            int count = _count + 1;
            Code.ValidIndex(index, "index", count);
            EnsureCapacity(count);

            // TODO implement properly

            _count = count;
            for (int i = _count - 1; i > index; i--)
            {
                this[i] = this[i - 1];
            }

            this[index] = item;
        }

        /// <inheritdoc cref="IList{T}.RemoveAt" />
        public void RemoveAt
            (
                int index
            )
        {
            Code.ValidIndex(index, "index", _count);

            // TODO implement properly
            int count = _count - 1;
            for (int i = index; i < count; i++)
            {
                this[i] = this[i + 1];
            }

            _count = count;
        }

        /// <inheritdoc cref="IList{T}.this" />
        public T this[int index]
        {
            get
            {
                Code.ValidIndex(index, "index", _count);

                return _chunks[index / ChunkSize][index % ChunkSize];
            }
            set
            {
                Code.ValidIndex(index, "index", _count);

                _chunks[index / ChunkSize][index % ChunkSize] = value;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Convert to array.
        /// </summary>
        public T[] ToArray()
        {
            if (_count == 0)
            {
                return EmptyArray<T>.Value;
            }

            T[] result = new T[_count];
            CopyTo(result, 0);

            return result;
        }

        #endregion
    }
}
