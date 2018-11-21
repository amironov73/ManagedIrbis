// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LocalList.cs --
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

namespace AM.Collections
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [PublicAPI]
    [MoonSharpUserData]
    public struct FrugalList<T>
    {
        #region Private members

        private int _size;

        private T _first, _second;

        private T[] _array;

        #endregion

        #region Public members

        /// <inheritdoc cref="ICollection{T}.Add" />
        public void Add
            (
                T item
            )
        {
            switch (_size)
            {
                case 0:
                    _first = item;
                    break;

                case 1:
                    _second = item;
                    break;

                case 2:
                    _array = new T[4];
                    _array[0] = _first;
                    _array[1] = _second;
                    _array[2] = item;
                    break;

                default:
                    if (_size == _array.Length)
                    {
                        T[] newArray = new T[_size * 2];
                        Array.Copy(_array, newArray, _size);
                        _array = newArray;
                    }

                    _array[_size] = item;
                    break;
            }

            _size++;
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
            int index = IndexOf(item);
            return index >= 0;
        }

        /// <inheritdoc cref="ICollection{T}.CopyTo" />
        public void CopyTo
            (
                T[] array,
                int arrayIndex
            )
        {
            if (_size == 0)
            {
                return;
            }
            if (_size == 1)
            {
                array[arrayIndex] = _first;
            }
            else if (_size == 2)
            {
                array[arrayIndex] = _first;
                array[arrayIndex + 1] = _second;
            }
            else
            {
                Array.Copy(_array, 0, array, arrayIndex, _size);
            }
        }

        /// <inheritdoc cref="ICollection{T}.Count" />
        public int Count
        {
            get { return _size; }
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<T> GetEnumerator()
        {
            if (_size == 0)
            {
                yield break;
            }

            if (_size == 1)
            {
                yield return _first;
            }

            if (_size == 2)
            {
                yield return _first;
                yield return _second;
            }

            if (_size > 2)
            {
                for (int i = 0; i < _size; i++)
                {
                    yield return _array[i];
                }
            }
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
            if (_size == 0)
            {
                return -1;
            }

            if (_size == 1)
            {
                EqualityComparer<T> comparer = EqualityComparer<T>.Default;
                if (comparer.Equals(_first, item))
                {
                    return 0;
                }

                return -1;
            }

            if (_size == 2)
            {
                EqualityComparer<T> comparer = EqualityComparer<T>.Default;
                if (comparer.Equals(_first, item))
                {
                    return 0;
                }

                if (comparer.Equals(_second, item))
                {
                    return 1;
                }

                return -1;
            }

            return Array.IndexOf(_array, item);
        }

        /// <inheritdoc cref="IList{T}.this" />
        public T this[int index]
        {
            get
            {
                Code.ValidIndex(index, "index", _size);

                if (_size <= 2)
                {
                    if (index == 0)
                    {
                        return _first;
                    }

                    return _second;
                }

                return _array[index];
            }
            set
            {
                Code.ValidIndex(index, "index", _size);

                if (_size <= 2)
                {
                    if (index == 0)
                    {
                        _first = value;
                    }

                    _second = value;
                }
                else
                {
                    _array[index] = value;
                }

            }
        }

        /// <summary>
        /// Convert the list to array.
        /// </summary>
        [NotNull]
        public T[] ToArray()
        {
            switch (_size)
            {
                case 0:
                    return EmptyArray<T>.Value;

                case 1:
                    return new[] {_first};

                case 2:
                    return new[] {_first, _second};
            }

            if (_size == _array.Length)
            {
                return _array;
            }

            T[] result = new T[_size];
            Array.Copy(_array, result, _size);

            return result;
        }

        #endregion
    }
}
