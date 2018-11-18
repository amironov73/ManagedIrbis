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
        #region Properties

        /// <summary>
        ///
        /// </summary>
        [CanBeNull]
        public List<T> InnerList { get; private set; }

        #endregion

        #region Public methods

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<T> GetEnumerator()
        {
            if (ReferenceEquals(InnerList, null))
            {
                return ((IEnumerable<T>)EmptyArray<T>.Value).GetEnumerator();
            }

            return ((IEnumerable<T>)InnerList).GetEnumerator();
        }

        /// <inheritdoc cref="ICollection{T}.Add" />
        public void Add
            (
                T item
            )
        {
            if (ReferenceEquals(InnerList, null))
            {
                InnerList = new List<T>();
            }

            InnerList.Add(item);
        }

        /// <summary>
        /// Add some items.
        /// </summary>
        public void AddRange
            (
                IEnumerable<T> items
            )
        {
            if (ReferenceEquals(InnerList, null))
            {
                InnerList = new List<T>();
            }

            foreach (T item in items)
            {
                InnerList.Add(item);
            }
        }

        /// <inheritdoc cref="ICollection{T}.Clear" />
        public void Clear()
        {
            if (!ReferenceEquals(InnerList, null))
            {
                InnerList.Clear();
            }
        }

        /// <inheritdoc cref="ICollection{T}.Contains" />
        public bool Contains
            (
                T item
            )
        {
            if (ReferenceEquals(InnerList, null))
            {
                return false;
            }

            return InnerList.Contains(item);
        }

        /// <inheritdoc cref="ICollection{T}.CopyTo" />
        public void CopyTo
            (
                T[] array,
                int arrayIndex
            )
        {
            if (!ReferenceEquals(InnerList, null))
            {
                InnerList.CopyTo(array, arrayIndex);
            }
        }

        /// <inheritdoc cref="ICollection{T}.Remove" />
        public bool Remove
            (
                T item
            )
        {
            if (ReferenceEquals(InnerList, null))
            {
                return false;
            }

            return InnerList.Remove(item);
        }

        /// <inheritdoc cref="ICollection{T}.Count" />
        public int Count
        {
            get
            {
                return ReferenceEquals(InnerList, null)
                    ? 0
                    : InnerList.Count;
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
            return ReferenceEquals(InnerList, null)
                ? -1
                : InnerList.IndexOf(item);
        }

        /// <inheritdoc cref="IList{T}.Insert" />
        public void Insert
            (
                int index,
                T item
            )
        {
            if (ReferenceEquals(InnerList, null))
            {
                InnerList = new List<T>();
            }

            InnerList.Insert(index, item);
        }

        /// <inheritdoc cref="IList{T}.RemoveAt" />
        public void RemoveAt
            (
                int index
            )
        {
            if (!ReferenceEquals(InnerList, null))
            {
                InnerList.RemoveAt(index);
            }
        }

        /// <inheritdoc cref="IList{T}.this" />
        public T this[int index]
        {
            get
            {
                if (ReferenceEquals(InnerList, null))
                {
                    throw new IndexOutOfRangeException();
                }

                return InnerList[index];
            }
            set
            {
                if (ReferenceEquals(InnerList, null))
                {
                    throw new IndexOutOfRangeException();
                }

                InnerList[index] = value;
            }
        }

        /// <summary>
        /// Convert the list to array.
        /// </summary>
        [NotNull]
        public T[] ToArray()
        {
            return ReferenceEquals(InnerList, null)
                ? EmptyArray<T>.Value
                : InnerList.ToArray();
        }

        #endregion
    }
}
