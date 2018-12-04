// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LocalList.cs --
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

namespace AM.Collections
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public struct LocalList<T>
    {
        #region Nested classes

        /// <summary>
        /// Enumerator for <see cref="LocalList{T}"/>.
        /// </summary>
        public struct Enumerator
        {
            #region Private members

            // ReSharper disable InconsistentNaming
            internal T[] _array;

            internal int _size, _index;
            // ReSharper restore InconsistentNaming

            #endregion

            #region IEnumerator<T> members

            /// <inheritdoc cref="IEnumerator{T}.Current" />
            public T Current
            {
                get { return _array[_index]; }
            }

            /// <inheritdoc cref="IEnumerator.MoveNext" />
            public bool MoveNext()
            {
                if (++_index >= _size)
                {
                    return false;
                }

                return true;
            }

            #endregion
        }

        #endregion

        #region Private members

        private const int InitialCapacity = 4;

        private T[] _array;
        private int _size;

        private void _Extend(int newSize)
        {
            T[] newArray = new T[newSize];
            if (!ReferenceEquals(_array, null))
            {
                _array.CopyTo(newArray, 0);
            }

            _array = newArray;
        }

        private void _GrowAsNeeded()
        {
            if (_size >= _array.Length)
            {
                _Extend(_size * 2);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalList
            (
                int capacity
            )
            : this()
        {
            Code.Positive(capacity, "capacity");

            _Extend(capacity);
        }

        #endregion

        #region Public methods

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public Enumerator GetEnumerator()
        {
            Enumerator result = new Enumerator
            {
                _array = _array,
                _index = -1,
                _size = _size
            };

            return result;
        }

        /// <inheritdoc cref="ICollection{T}.Add" />
        public void Add
            (
                T item
            )
        {
            if (ReferenceEquals(_array, null))
            {
                _Extend(InitialCapacity);
            }

            _GrowAsNeeded();
            _array[_size++] = item;
        }

        /// <summary>
        /// Add some items.
        /// </summary>
        public void AddRange
            (
                [NotNull] IEnumerable<T> items
            )
        {
            if (ReferenceEquals(_array, null))
            {
                _Extend(InitialCapacity);
            }

            foreach (T item in items)
            {
                _GrowAsNeeded();
                _array[_size++] = item;
            }
        }

        /// <inheritdoc cref="ICollection{T}.Clear" />
        public void Clear()
        {
            _size = 0;
        }

        /// <inheritdoc cref="ICollection{T}.Contains" />
        public bool Contains
            (
                T item
            )
        {
            if (ReferenceEquals(_array, null))
            {
                return false;
            }

            int index = Array.IndexOf(_array, item, 0, _size);
            return index >= 0;
        }

        /// <inheritdoc cref="ICollection{T}.CopyTo" />
        public void CopyTo
            (
                T[] array,
                int arrayIndex
            )
        {
            if (!ReferenceEquals(_array, null))
            {
                Array.Copy(_array, 0, array, arrayIndex, _size);
            }
        }

        /// <inheritdoc cref="ICollection{T}.Remove" />
        public bool Remove
            (
                T item
            )
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);

                return true;
            }

            return false;
        }

        /// <inheritdoc cref="ICollection{T}.Count" />
        public int Count
        {
            get { return _size; }
        }

        /// <inheritdoc cref="ICollection{T}.IsReadOnly" />
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <inheritdoc cref="IList{T}.IndexOf" />
        public int IndexOf
            (
                T item
            )
        {
            return ReferenceEquals(_array, null)
                ? -1
                : Array.IndexOf(_array, item, 0, _size);
        }

        /// <inheritdoc cref="IList{T}.Insert" />
        public void Insert
            (
                int index,
                T item
            )
        {
            if (ReferenceEquals(_array, null))
            {
                _Extend(InitialCapacity);
            }

            if (_size != 0 && index != _size - 1)
            {
                Array.Copy(_array, index, _array, index + 1, _size - index - 1);
            }

            _array[index] = item;
            _size++;
        }

        /// <inheritdoc cref="IList{T}.RemoveAt" />
        public void RemoveAt
            (
                int index
            )
        {
            if (!ReferenceEquals(_array, null))
            {
                if (index != _size - 1)
                {
                    Array.Copy(_array, index + 1, _array, index, _size - index - 1);
                }

                _size--;
            }
        }

        /// <inheritdoc cref="IList{T}.this" />
        public T this[int index]
        {
            get
            {
                if (ReferenceEquals(_array, null))
                {
                    throw new IndexOutOfRangeException();
                }

                return _array[index];
            }
            set
            {
                if (ReferenceEquals(_array, null))
                {
                    throw new IndexOutOfRangeException();
                }

                _array[index] = value;
            }
        }

        /// <summary>
        /// Convert the list to array.
        /// </summary>
        [NotNull]
        public T[] ToArray()
        {
            if (ReferenceEquals(_array, null) || _size == 0)
            {
                return EmptyArray<T>.Value;
            }

            if (_size == _array.Length)
            {
                return _array;
            }

            T[] result = new T[_size];
            Array.Copy(_array, result, _size);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public List<T> ToUniqueList()
        {
            List<T> result = new List<T>(_size);
            if (_size != 0)
            {
                for (int i = 0; i < _size; i++)
                {
                    // TODO Implement properly
                    T item = _array[i];
                    if (!result.Contains(item))
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Convert the list to <see cref="List{T}"/>.
        /// </summary>
        public List<T> ToList()
        {
            if (ReferenceEquals(_array, null) || _size == 0)
            {
                return new List<T>();
            }

            List<T> result = new List<T>(_size);
            for (int i = 0; i < _size; i++)
            {
                result.Add(_array[i]);
            }

            return result;
        }

        #endregion
    }
}
