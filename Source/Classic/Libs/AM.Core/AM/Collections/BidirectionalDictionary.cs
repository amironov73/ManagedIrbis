// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BidirectionalDictionary.cs -- bidirectional key-to-value mapping
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// Bidirectional key-to-value mapping.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("Count={Count}")]
    public class BidirectionalDictionary<TKey, TValue>
        : IDictionary<TKey, TValue>
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="BidirectionalDictionary{TKey, TValue}"/> 
        /// class.
        /// </summary>
        public BidirectionalDictionary()
        {
            _straight = new Dictionary<TKey, TValue>();
            _reverse = new Dictionary<TValue, TKey>();
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="BidirectionalDictionary{TKey, TValue}"/> 
        /// class.
        /// </summary>
        /// <param name="straightComparer">The straight comparer.
        /// </param>
        /// <param name="reverseComparer">The reverse comparer.
        /// </param>
        public BidirectionalDictionary
            (
                [NotNull] IEqualityComparer<TKey> straightComparer,
                [NotNull] IEqualityComparer<TValue> reverseComparer
            )
        {
            Code.NotNull(straightComparer, "straightComparer");
            Code.NotNull(reverseComparer, "reverseComparer");

            _straight = new Dictionary<TKey, TValue>(straightComparer);
            _reverse = new Dictionary<TValue, TKey>(reverseComparer);
        }

        #endregion

        #region Private members

        private bool _isReadOnly;

        private Dictionary<TValue, TKey> _reverse;
        private Dictionary<TKey, TValue> _straight;

        private volatile object _syncRoot = new object();

        /// <summary>
        /// Adds the key-value pair to the dictionary.
        /// </summary>
        /// <param name="item">Key-value pair.</param>
        private void _Add
            (
                KeyValuePair<TKey, TValue> item
            )
        {
            lock (_syncRoot)
            {
                _straight.Add(item.Key, item.Value);
                _reverse.Add(item.Value, item.Key);
            }
        }

        /// <summary>
        /// Adds specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        private void _Add
            (
                TKey key,
                TValue value
            )
        {
            if (_isReadOnly)
            {
                Log.Error
                    (
                        "BidirectionalDictionary::_Add: "
                        + "dictionary is read-only"
                    );

                throw new NotSupportedException();
            }
            lock (_syncRoot)
            {
                _straight.Add(key, value);
                _reverse.Add(value, key);
            }
        }

        /// <summary>
        /// Clears the dictionary.
        /// </summary>
        private void _Clear()
        {
            if (_isReadOnly)
            {
                Log.Error
                    (
                        "BidirectionalDictionary::_Clear: "
                        + "dictionary is read-only"
                    );

                throw new NotSupportedException();
            }
            lock (_syncRoot)
            {
                _straight.Clear();
                _reverse.Clear();
            }
        }

        /// <summary>
        /// Determines whether given key exists in the dictionary.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if the dictionary contains
        /// specified key; <c>false</c> otherwise.</returns>
        private bool _Contains
            (
                ref KeyValuePair<TKey, TValue> item
            )
        {
            lock (_syncRoot)
            {
                return ((IDictionary<TKey, TValue>) _straight)
                    .Contains(item);
            }
        }

        /// <summary>
        /// Determines whether given key exists in the dictionary.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the dictionary contains
        /// specified key; <c>false</c> otherwise.</returns>
        private bool _ContainsKey
            (
                TKey key
            )
        {
            lock (_syncRoot)
            {
                return _straight.ContainsKey(key);
            }
        }

        /// <summary>
        /// Copies key-value pairs to specified array starting with
        /// given index.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        private void _CopyTo
            (
                KeyValuePair<TKey, TValue>[] array,
                int arrayIndex
            )
        {
            lock (_syncRoot)
            {
                ((IDictionary<TKey, TValue>)_straight)
                    .CopyTo
                    (
                        array,
                        arrayIndex
                    );
            }
        }

        /// <summary>
        /// Gets count of key-value pairs in the dictionary. 
        /// </summary>
        /// <returns></returns>
        private int _Count()
        {
            lock (_syncRoot)
            {
                return _straight.Count;
            }
        }

        /// <summary>
        /// Gets the associated value for given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Associated value.</returns>
        private TValue _Get
            (
                TKey key
            )
        {
            lock (_syncRoot)
            {
                return _straight[key];
            }
        }

        /// <summary>
        /// Gets the enumerator for the dictionary.
        /// </summary>
        /// <returns>The enumerator for the dictionary</returns>
        private IEnumerator<KeyValuePair<TKey, TValue>> _GetEnumerator()
        {
            lock (_syncRoot)
            {
                return _straight.GetEnumerator();
            }
        }

        /// <summary>
        /// _s the keys.
        /// </summary>
        /// <returns></returns>
        private ICollection<TKey> _Keys()
        {
            lock (_syncRoot)
            {
                return _straight.Keys;
            }
        }

        /// <summary>
        /// Removes the specified key and associated value 
        /// from the dictionary.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if key and value was removed;
        /// <c>false</c> otherwise.</returns>
        private bool _Remove
            (
                TKey key
            )
        {
            if (_isReadOnly)
            {
                Log.Error
                    (
                        "BidirectionalDictionary::_Remove: "
                        + "dictionary is read-only"
                    );

                throw new NotSupportedException();
            }
            lock (_syncRoot)
            {
                TValue value;
                if (_straight.TryGetValue(key, out value))
                {
                    bool straightSuccess = _straight.Remove(key);
                    bool reverseSuccess = _reverse.Remove(value);
                    Debug.Assert(straightSuccess == reverseSuccess);

                    return straightSuccess;
                }

                return false;
            }
        }

        /// <summary>
        /// Removes the specified key and associated value 
        /// from the dictionary.
        /// </summary>
        /// <param name="item">The key.</param>
        /// <returns><c>true</c> if key and value was removed;
        /// <c>false</c> otherwise.</returns>
        private bool _Remove
            (
                ref KeyValuePair<TKey, TValue> item
            )
        {
            if (_isReadOnly)
            {
                Log.Error
                    (
                        "BidirectionalDictionary::_Remove: "
                        + "dictionary is read-only"
                    );

                throw new NotSupportedException();
            }
            lock (_syncRoot)
            {
                bool straightSuccess = _straight.Remove(item.Key);
                bool reverseSuccess = _reverse.Remove(item.Value);
                
                Debug.Assert(straightSuccess == reverseSuccess);
                
                return straightSuccess;
            }
        }

        /// <summary>
        /// Associates the specified value with given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        private void _Set
            (
                TKey key,
                TValue value
            )
        {
            if (_isReadOnly)
            {
                Log.Error
                    (
                        "BidirectionalDictionary::_Set: "
                        + "dictionary is read-only"
                    );

                throw new NotSupportedException();
            }
            lock (_syncRoot)
            {
                _straight[key] = value;
                _reverse[value] = key;
            }
        }

        /// <summary>
        /// Tries to get associated value for given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if key exists;
        /// <c>false</c> otherwise.</returns>
        private bool _TryGetValue
            (
                TKey key,
                out TValue value
            )
        {
            lock (_syncRoot)
            {
                return _straight.TryGetValue
                    (
                        key, 
                        out value
                    );
            }
        }

        /// <summary>
        /// Gets associated values collection.
        /// </summary>
        /// <returns>Collection of associated values.</returns>
        private ICollection<TValue> _Values()
        {
            lock (_syncRoot)
            {
                return _straight.Values;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets read-only shallow copy of the 
        /// <see cref="BidirectionalDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <returns>Read-only shallow copy of the dictionary.</returns>
        public BidirectionalDictionary<TKey, TValue> AsReadOnly()
        {
            lock (_syncRoot)
            {
                BidirectionalDictionary<TKey, TValue> result
                    = new BidirectionalDictionary<TKey, TValue>
                    {
                        _straight = _straight,
                        _reverse = _reverse,
                        _isReadOnly = true
                    };

                return result;
            }
        }

        /// <summary>
        /// Determines whether the <see cref="BidirectionalDictionary{TKey,TValue}"/> 
        /// contains given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>
        /// <c>true</c> if the dictionary contains value; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsValue
            (
                TValue value
            )
        {
            lock (_syncRoot)
            {
                return _reverse.ContainsKey(value);
            }
        }

        /// <summary>
        /// Gets the associated key for given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">
        /// Specified <paramref name="value"/> not found.
        /// </exception>
        public TKey GetKey
            (
                TValue value
            )
        {
            TKey result;
            if (!TryGetKey(value, out result))
            {
                Log.Error
                    (
                        "BidirectionalDictionary::GetKey: "
                        + "key not found: "
                        + value.ToVisibleString()
                    );

                throw new KeyNotFoundException();
            }

            return result;
        }

        /// <summary>
        /// Gets the key associated with the specified value.
        /// </summary>
        /// <param name="value">The value of the key to get.</param>
        /// <param name="key">When this method returns, contains the 
        /// key associated with the specified value, if the one is found; 
        /// otherwise, the default value for the type of the key parameter. 
        /// This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the 
        /// <see cref="BidirectionalDictionary{TKey,TValue}"/> 
        /// contains an element with the specified key; otherwise, 
        /// <c>false</c>.</returns>
        public bool TryGetKey
            (
                TValue value,
                out TKey key
            )
        {
            lock (_syncRoot)
            {
                return _reverse.TryGetValue(value, out key);
            }
        }

        #endregion

        #region IDictionary<TKey, TValue> members

        /// <inheritdoc cref="IDictionary{TKey,TValue}.this" />
        public TValue this[TKey key]
        {
            get
            {
                return _Get(key);
            }
            set
            {
                _Set(key, value);
            }
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}.Add(TKey,TValue)" />
        public void Add
            (
                TKey key,
                TValue value
            )
        {
            _Add(key, value);
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}.ContainsKey" />
        public bool ContainsKey
            (
                TKey key
            )
        {
            return _ContainsKey(key);
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}.Keys" />
        public ICollection<TKey> Keys
        {
            get
            {
                return _Keys();
            }
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}.Remove(TKey)" />
        public bool Remove
            (
                TKey key
            )
        {
            return _Remove(key);
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}.TryGetValue" />
        public bool TryGetValue
            (
                TKey key,
                out TValue value
            )
        {
            return _TryGetValue(key, out value);
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}.Values" />
        public ICollection<TValue> Values
        {
            get
            {
                return _Values();
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> members

        /// <inheritdoc cref="ICollection{T}.Add" />
        public void Add
            (
                KeyValuePair<TKey, TValue> item
            )
        {
            _Add(item);
        }

        /// <inheritdoc cref="ICollection{T}.Clear" />
        public void Clear()
        {
            _Clear();
        }

        /// <inheritdoc cref="ICollection{T}.Contains" />
        public bool Contains
            (
                KeyValuePair<TKey, TValue> item
            )
        {
            return _Contains(ref item);
        }

        /// <inheritdoc cref="ICollection{T}.CopyTo" />
        public void CopyTo
            (
                KeyValuePair<TKey, TValue>[] array,
                int arrayIndex
            )
        {
            _CopyTo(array, arrayIndex);
        }

        /// <inheritdoc cref="ICollection{T}.Count" />
        public int Count
        {
            get
            {
                return _Count();
            }
        }

        /// <inheritdoc cref="ICollection{T}.IsReadOnly" />
        public bool IsReadOnly
        {
            [DebuggerStepThrough]
            get
            {
                return _isReadOnly;
            }
        }

        /// <inheritdoc cref="ICollection{T}.Remove" />
        public bool Remove
            (
                KeyValuePair<TKey, TValue> item
            )
        {
            return _Remove(ref item);
        }

        #endregion

        #region IEnumerable <KeyValuePair<TKey,TValue>> members

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _GetEnumerator();
        }

        #endregion

        #region IEnumerable membres

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _GetEnumerator();
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public object Clone()
        {
            lock (_syncRoot)
            {
                BidirectionalDictionary<TKey, TValue> result
                    = new BidirectionalDictionary<TKey, TValue>();
                foreach (KeyValuePair<TKey, TValue> pair in _straight)
                {
                    result._straight.Add(pair.Key, pair.Value);
                    result._reverse.Add(pair.Value, pair.Key);
                }

                return result;
            }
        }

        #endregion
    }
}
