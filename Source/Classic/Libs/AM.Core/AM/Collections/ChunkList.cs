// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChunkArray.cs --
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
    /// <typeparam name="T"></typeparam>
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
        /// Length.
        /// </summary>
        public int Length
        {
            get { return _length; }
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

        private int _capacity, _length;

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
            _capacity = capacity;
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
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ICollection{T}.Add" />
        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ICollection{T}.Clear" />
        public void Clear()
        {
            _length = 0;
        }

        /// <inheritdoc cref="ICollection{T}.Contains" />
        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ICollection{T}.CopyTo" />
        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ICollection{T}.Remove" />
        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ICollection{T}.Count" />
        public int Count
        {
            get { return _length; }
        }

        /// <inheritdoc cref="ICollection{T}.IsReadOnly" />
        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region IList<T> members

        /// <inheritdoc cref="IList{T}.IndexOf" />
        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IList{T}.Insert" />
        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IList{T}.RemoveAt" />
        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IList{T}.this" />
        public T this[int index]
        {
            get
            {
                Code.ValidIndex(index, "index", _length);

                return _chunks[index / ChunkSize][index % ChunkSize];
            }
            set
            {
                Code.ValidIndex(index, "index", _length);

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
            return EmptyArray<T>.Value;
        }

        #endregion
    }
}
