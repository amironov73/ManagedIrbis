// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DictionaryList.cs -- hybrid of dictionary and list
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.Collections
{
    /// <summary>
    /// Hybrid of Dictionary and List.
    /// </summary>
    [PublicAPI]
    public class DictionaryList<TKey, TValue>
        : IEnumerable<ValuePair<TKey, TValue[]>>
    {
        #region Properties

        /// <summary>
        /// Number of keys.
        /// </summary>
        public int Count
        {
            get
            {
                lock (_syncRoot)
                {
                    return _dictionary.Count;
                }
            }
        }

        /// <summary>
        /// Keys.
        /// </summary>
        [NotNull]
        public TKey[] Keys
        {
            get
            {
                lock (_syncRoot)
                {
                    List<TKey> result = new List<TKey>(_dictionary.Keys);

                    return result.ToArray();
                }
            }
        }

        /// <summary>
        /// Array of values for specified key.
        /// </summary>
        [NotNull]
        public TValue[] this[[NotNull]TKey key]
        {
            get
            {
                lock (_syncRoot)
                {
                    List<TValue> result = GetValues(key);

                    return (ReferenceEquals(result, null))
                        ? new TValue[0]
                        : result.ToArray();
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DictionaryList()
        {
            _dictionary = new Dictionary<TKey, List<TValue>>();
            _syncRoot = new object();
        }

        #endregion

        #region Private members

        private readonly Dictionary<TKey, List<TValue>> _dictionary;

        private readonly object _syncRoot;

        #endregion

        #region Public methods

        /// <summary>
        /// Add an item.
        /// </summary>
        [NotNull]
        public DictionaryList<TKey, TValue> Add
            (
                [NotNull] TKey key,
                TValue value
            )
        {
            lock (_syncRoot)
            {
                if (!_dictionary.TryGetValue(key, out var list))
                {
                    list = new List<TValue>();
                    _dictionary.Add(key, list);
                }

                list.Add(value);
            }

            return this;
        }

        /// <summary>
        /// Add some items with one key.
        /// </summary>
        [NotNull]
        public DictionaryList<TKey, TValue> AddRange
            (
                [NotNull] TKey key,
                [NotNull] IEnumerable<TValue> values
            )
        {
            Code.NotNull(values, nameof(values));

            lock (_syncRoot)
            {
                if (!_dictionary.TryGetValue(key, out var list))
                {
                    list = new List<TValue>();
                    _dictionary.Add(key, list);
                }

                list.AddRange(values);
            }

            return this;
        }

        /// <summary>
        /// Clear.
        /// </summary>
        [NotNull]
        public DictionaryList<TKey, TValue> Clear()
        {
            lock (_syncRoot)
            {
                _dictionary.Clear();
            }

            return this;
        }

            /// <summary>
        /// Get values for specified key.
        /// </summary>
        [CanBeNull]
        public List<TValue> GetValues
            (
                [NotNull] TKey key
            )
        {
            lock (_syncRoot)
            {
                _dictionary.TryGetValue(key, out var result);

                return result;
            }
        }

        #endregion

        #region IEnumerable members

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<ValuePair<TKey, TValue[]>> GetEnumerator()
        {
            foreach (TKey key in Keys)
            {
                ValuePair<TKey, TValue[]> pair = new ValuePair<TKey, TValue[]>
                    (
                        key,
                        this[key]
                    );

                yield return pair;
            }
        }

        #endregion
    }
}
