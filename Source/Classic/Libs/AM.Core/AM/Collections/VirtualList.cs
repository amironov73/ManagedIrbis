// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* VirtualList.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// Virtual list.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class VirtualList<T>
        : IList<T>
    {
        #region Nested classes

        /// <summary>
        /// Parameters.
        /// </summary>
        public sealed class Parameters
        {
            #region Properties

            /// <summary>
            /// List.
            /// </summary>
            [NotNull]
            public VirtualList<T> List { get; private set; }

            /// <summary>
            /// Index.
            /// </summary>
            public int Index { get; private set; }

            /// <summary>
            /// Up direction.
            /// </summary>
            public bool Up { get; private set; }

            #endregion

            #region Construction

            /// <summary>
            /// Constructor.
            /// </summary>
            public Parameters
                (
                    [NotNull] VirtualList<T> list,
                    int index,
                    bool up
                )
            {
                Code.NotNull(list, "list");

                List = list;
                Index = index;
                Up = up;
            }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Cache size.
        /// </summary>
        public int CacheSize { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public VirtualList
            (
                [NotNull] Action<Parameters> retriever,
                int count,
                int cacheSize
            )
        {
            Code.NotNull(retriever, "retriever");
            Code.Positive(count, "count");
            Code.Positive(cacheSize, "cacheSize");

            if (cacheSize > count)
            {
                cacheSize = count;
            }

            _retriever = retriever;
            Count = count;
            CacheSize = cacheSize;

            _cache = null;
            _cacheIndex = 0;
            _cacheLength = 0;
        }

        #endregion

        #region Private members

        private Action<Parameters> _retriever;

        private T[] _cache;

        private int _cacheIndex;

        private int _cacheLength;

        [ContractAnnotation("=> halt")]
        private void _ThrowReadonly()
        {
            Log.Error
                (
                    "VirtualList::_ThrowReadOnly"
                );

            throw new ReadOnlyException();
        }

        private void _Retrieve
            (
                int index
            )
        {
            if (index >= _cacheIndex
                && index < _cacheIndex + _cacheLength)
            {
                return;
            }

            Parameters parameters = new Parameters
                (
                    this,
                    index,
                    false
                );
            _retriever(parameters);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get item by index.
        /// </summary>
        public T GetItem
            (
                int index
            )
        {
            Code.Nonnegative(index, "index");

            _Retrieve(index);
            T result = default(T);
            if (!ReferenceEquals(_cache, null)
                && index >= _cacheIndex
                && index <= _cacheIndex + _cacheLength)
            {
                result = _cache[index - _cacheIndex];
            }

            return result;
        }

        /// <summary>
        /// Set cache (called by retriever);
        /// </summary>
        public void SetCache
            (
                [NotNull] T[] cache,
                int index
            )
        {
            Code.NotNull(cache, "cache");
            Code.Positive(cache.Length, "cache");
            Code.Nonnegative(index, "index");

            _cache = cache;
            _cacheIndex = index;
            _cacheLength = cache.Length;
            int end = _cacheIndex + _cacheLength;
            if (end > Count)
            {
                Count = end;
            }
        }

        #endregion

        #region IList<T> members

        /// <inheritdoc cref="ICollection{T}.Count" />
        public int Count { get; private set; }

        /// <inheritdoc cref="ICollection{T}.Add" />
        [ExcludeFromCodeCoverage]
        void ICollection<T>.Add(T item)
        {
            _ThrowReadonly();
        }

        /// <inheritdoc cref="ICollection{T}.Clear" />
        [ExcludeFromCodeCoverage]
        void ICollection<T>.Clear()
        {
            _ThrowReadonly();
        }

        /// <inheritdoc cref="ICollection{T}.Contains" />
        public bool Contains
            (
                T item
            )
        {
            if (ReferenceEquals(_cache, null))
            {
                GetItem(0);
            }
            if (ReferenceEquals(_cache, null))
            {
                return false;
            }

            foreach (T i in _cache)
            {
                if (EqualityComparer<T>.Default.Equals
                (
                    item,
                    i
                ))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc cref="ICollection{T}.CopyTo" />
        [ExcludeFromCodeCoverage]
        void ICollection<T>.CopyTo
            (
                T[] array,
                int arrayIndex
            )
        {
            foreach (T item in this)
            {
                array.SetValue(item, arrayIndex++);
            }
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<T> GetEnumerator()
        {
            if (ReferenceEquals(_cache, null))
            {
                GetItem(0);
            }

            //if (ReferenceEquals(_cache, null))
            //{
            //    throw new InvalidOperationException();
            //}

            foreach (T item in _cache)
            {
                yield return item;
            }
        }

        /// <inheritdoc cref="ICollection{T}.IsReadOnly" />
        public bool IsReadOnly { get { return true; } }

        /// <inheritdoc cref="IList{T}.IndexOf" />
        public int IndexOf(T item)
        {
            if (ReferenceEquals(_cache, null))
            {
                GetItem(0);
            }
            if (ReferenceEquals(_cache, null))
            {
                return -1;
            }

            for (int i = 0; i < _cache.Length; i++)
            {
                if (EqualityComparer<T>.Default.Equals
                (
                    item,
                    _cache[i]
                ))
                {
                    return _cacheIndex + i;
                }
            }

            return -1;
        }

        /// <inheritdoc cref="IList{T}.Insert" />
        [ExcludeFromCodeCoverage]
        void IList<T>.Insert(int index, T item)
        {
            _ThrowReadonly();
        }

        /// <inheritdoc cref="ICollection{T}.Remove" />
        [ExcludeFromCodeCoverage]
        bool ICollection<T>.Remove(T item)
        {
            _ThrowReadonly();

            return false;
        }

        /// <inheritdoc cref="IList{T}.RemoveAt" />
        [ExcludeFromCodeCoverage]
        void IList<T>.RemoveAt(int index)
        {
            _ThrowReadonly();
        }

        /// <inheritdoc cref="IList{T}.this" />
        public T this[int index]
        {
            get
            {
                T result = GetItem(index);

                return result;
            }
            // ReSharper disable once ValueParameterNotUsed
            set
            {
                throw new ReadOnlyException();
            }
        }

        #endregion
    }
}
