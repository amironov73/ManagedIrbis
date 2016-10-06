/* DictionaryList.cs -- hybrid of dictionary and list
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
    /// Hybrid of Dictionary and List.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class DictionaryList<TKey, TValue>
        : IEnumerable<Pair<TKey, TValue[]>>
    {
        #region Properties

        /// <summary>
        /// Number of keys.
        /// </summary>
        public int Count
        {
            get { return _dictionary.Count; }
        }

        /// <summary>
        /// Keys.
        /// </summary>
        [NotNull]
        public TKey[] Keys
        {
            get
            {
                List<TKey> result = new List<TKey>(_dictionary.Keys);

                return result.ToArray();
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
                List<TValue> result = GetValues(key);

                return (ReferenceEquals(result, null))
                    ? new TValue[0]
                    : result.ToArray();
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
                List<TValue> list;
                if (!_dictionary.TryGetValue(key, out list))
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
            Code.NotNull(values, "values");

            lock (_syncRoot)
            {
                List<TValue> list;
                if (!_dictionary.TryGetValue(key, out list))
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
                List<TValue> result;
                _dictionary.TryGetValue(key, out result);

                return result;
            }
        }

        #endregion

        #region IEnumerable members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" />
        /// object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" />
        /// that can be used to iterate through the collection.</returns>
        public IEnumerator<Pair<TKey, TValue[]>> GetEnumerator()
        {
            foreach (TKey key in Keys)
            {
                Pair<TKey, TValue[]> pair = new Pair<TKey, TValue[]>
                    (
                        key,
                        this[key]
                    );

                yield return pair;
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
