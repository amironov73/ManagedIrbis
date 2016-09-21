/* VirtualList.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

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
                Action<Parameters> retriever,
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
            if (index >= _cacheIndex
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

        /// <inheritdoc />
        public int Count { get; private set; }

        /// <inheritdoc />
        void ICollection<T>.Add(T item)
        {
            _ThrowReadonly();
        }

        /// <inheritdoc />
        void ICollection<T>.Clear()
        {
            _ThrowReadonly();
        }

        /// <inheritdoc />
        public bool Contains(T item)
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

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            if (ReferenceEquals(_cache, null))
            {
                GetItem(0);
            }

            if (ReferenceEquals(_cache, null))
            {
                throw new InvalidOperationException();
            }

            foreach (T item in _cache)
            {
                yield return item;
            }
        }

        /// <inheritdoc />
        public bool IsReadOnly { get { return true; } }

        /// <inheritdoc />
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

        void IList<T>.Insert(int index, T item)
        {
            _ThrowReadonly();
        }

        /// <inheritdoc />
        bool ICollection<T>.Remove(T item)
        {
            _ThrowReadonly();
            return false;
        }

        void IList<T>.RemoveAt(int index)
        {
            _ThrowReadonly();
        }

        /// <inheritdoc />
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
                _ThrowReadonly();
            }
        }

        #endregion
    }
}
