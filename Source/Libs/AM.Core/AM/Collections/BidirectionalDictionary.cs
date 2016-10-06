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
          //ICloneable
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
        private void _Add(TKey key, TValue value)
        {
            if (_isReadOnly)
            {
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
        private bool _Contains(ref KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)_straight).Contains(item);
        }

        /// <summary>
        /// Determines whether given key exists in the dictionary.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the dictionary contains
        /// specified key; <c>false</c> otherwise.</returns>
        private bool _ContainsKey(TKey key)
        {
            return _straight.ContainsKey(key);
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
                ((IDictionary<TKey, TValue>)_straight).CopyTo(array, arrayIndex);
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
        private TValue _Get(TKey key)
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
            return _straight.GetEnumerator();
        }

        /// <summary>
        /// _s the keys.
        /// </summary>
        /// <returns></returns>
        private ICollection<TKey> _Keys()
        {
            return _straight.Keys;
        }

        /// <summary>
        /// Removes the specified key and associated value 
        /// from the dictionary.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if key and value was removed;
        /// <c>false</c> otherwise.</returns>
        private bool _Remove(TKey key)
        {
            if (_isReadOnly)
            {
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
        private bool _Remove(ref KeyValuePair<TKey, TValue> item)
        {
            if (_isReadOnly)
            {
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
        private void _Set(TKey key, TValue value)
        {
            if (_isReadOnly)
            {
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
        private bool _TryGetValue(TKey key, out TValue value)
        {
            return _straight.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets associated values collection.
        /// </summary>
        /// <returns>Collection of associated values.</returns>
        private ICollection<TValue> _Values()
        {
            return _straight.Values;
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
            BidirectionalDictionary<TKey, TValue> result
                = new BidirectionalDictionary<TKey, TValue>();
            result._straight = _straight;
            result._reverse = _reverse;
            result._isReadOnly = true;
            return result;
        }

        /// <summary>
        /// Determines whether the <see cref="BidirectionalDictionary{TKey,TValue}"/> 
        /// contains given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>
        /// 	<c>true</c> if the dictionary contains value; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsValue(TValue value)
        {
            return _reverse.ContainsKey(value);
        }

        /// <summary>
        /// Gets the associated key for given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">
        /// Specified <paramref name="value"/> not found.
        /// </exception>
        public TKey GetKey(TValue value)
        {
            TKey result;
            if (!TryGetKey(value, out result))
            {
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
        public bool TryGetKey(TValue value, out TKey key)
        {
            lock (_syncRoot)
            {
                return _reverse.TryGetValue(value, out key);
            }
        }

        #endregion

        #region IDictionary<TKey, TValue> members

        ///<summary>
        /// Gets or sets the element with the specified key.
        ///</summary>
        ///<returns>
        /// The element with the specified key.
        ///</returns>
        ///<param name="key">The key of the element to get or set.</param>
        ///<exception cref="T:System.NotSupportedException">
        /// The property is set and the 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>
        /// is read-only.</exception>
        ///<exception cref="T:System.ArgumentNullException">key is null.</exception>
        ///<exception cref="T:System.Collections.Generic.KeyNotFoundException">
        /// The property is retrieved and key is not found.</exception>
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

        ///<summary>
        /// Adds an element with the provided key and value to the 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        ///</summary>
        ///<param name="value">The object to use as the value of the 
        /// element to add.</param>
        ///<param name="key">The object to use as the key of the 
        /// element to add.</param>
        ///<exception cref="T:System.NotSupportedException">The 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>
        /// is read-only.</exception>
        ///<exception cref="T:System.ArgumentException">An element 
        /// with the same key already exists in the 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </exception>
        ///<exception cref="T:System.ArgumentNullException">
        /// key is <c>null</c>.</exception>
        public void Add(TKey key, TValue value)
        {
            _Add(key, value);
        }

        ///<summary>
        /// Determines whether the 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/> 
        /// contains an element with the specified key.
        ///</summary>
        ///<returns>
        /// <c>true</c> if the 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>
        /// contains an element with the key; otherwise, <c>false</c>.
        ///</returns>
        ///<param name="key">The key to locate in the 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.</param>
        ///<exception cref="T:System.ArgumentNullException">key is 
        /// <c>null</c>.</exception>
        public bool ContainsKey(TKey key)
        {
            return _ContainsKey(key);
        }

        ///<summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// containing the keys of the 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        ///</summary>
        ///<returns>
        /// An <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// containing the keys of the object that implements 
        /// <see cref="T:System.Collections.Generic.IDictionary`2" />.
        ///</returns>
        public ICollection<TKey> Keys
        {
            get
            {
                return _Keys();
            }
        }

        ///<summary>
        /// Removes the element with the specified key from the 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        ///</summary>
        ///<returns>
        /// <c>true</c> if the element is successfully removed; 
        /// otherwise, <c>false</c>.  This method also returns <c>false</c>
        /// if key was not found in the original 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        ///</returns>
        ///<param name="key">The key of the element to remove.</param>
        ///<exception cref="T:System.NotSupportedException">The 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>
        /// is read-only.</exception>
        ///<exception cref="T:System.ArgumentNullException">key is 
        /// <c>null</c>.</exception>
        public bool Remove(TKey key)
        {
            return _Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the 
        /// value associated with the specified key, if the key is found; 
        /// otherwise, the default value for the type of the value parameter. 
        /// This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the 
        /// <see cref="BidirectionalDictionary{TKey,TValue}"/> 
        /// contains an element with the specified key; otherwise, 
        /// <c>false</c>.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _TryGetValue(key, out value);
        }

        ///<summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// containing the values in the 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        ///</summary>
        ///<returns>
        /// An 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// containing the values in the object that implements 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        ///</returns>
        public ICollection<TValue> Values
        {
            get
            {
                return _Values();
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> members

        ///<summary>
        /// Adds an item to the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///</summary>
        ///<param name="item">The object to add to the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        ///<exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// is read-only.</exception>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _Add(item);
        }

        ///<summary>
        /// Removes all items from the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///</summary>
        ///<exception cref="T:System.NotSupportedException">The 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// is read-only.</exception>
        public void Clear()
        {
            _Clear();
        }

        ///<summary>
        /// Determines whether the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// contains a specific value.
        ///</summary>
        ///<returns>
        /// <c>true</c> if item is found in the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>; 
        /// otherwise, <c>false</c>.
        ///</returns>
        ///<param name="item">The object to locate in the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _Contains(ref item);
        }

        ///<summary>
        /// Copies the elements of the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// to an <see cref="T:System.Array"/>, starting at a particular 
        /// <see cref="T:System.Array"/> index.
        ///</summary>
        ///<param name="array">The one-dimensional 
        /// <see cref="T:System.Array"/> that is the destination of the 
        /// elements copied from 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>. 
        /// The <see cref="T:System.Array"/> must have zero-based indexing.
        /// </param>
        ///<param name="arrayIndex">The zero-based index in array 
        /// at which copying begins.</param>
        ///<exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="arrayIndex"/> is less than 0.</exception>
        ///<exception cref="T:System.ArgumentNullException">
        /// <paramref name="array"/> is <c>null</c>.</exception>
        ///<exception cref="T:System.ArgumentException">
        /// <paramref name="array"/> is multidimensional. -or- 
        /// <paramref name="arrayIndex"/> is equal to or greater 
        /// than the length of array. -or- The number of elements in the source 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// is greater than the available space from 
        /// <paramref name="arrayIndex"/> to the end of the destination array. 
        /// -or- Type T cannot be cast automatically to the type of the destination 
        /// array.</exception>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _CopyTo(array, arrayIndex);
        }

        ///<summary>
        /// Gets the number of elements contained in the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///</summary>
        ///<returns>
        /// The number of elements contained in the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///</returns>
        public int Count
        {
            get
            {
                return _Count();
            }
        }

        ///<summary>
        /// Gets a value indicating whether the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// is read-only.
        ///</summary>
        ///<returns>
        /// <c>true</c> if the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// is read-only; otherwise, <c>false</c>.
        ///</returns>
        public bool IsReadOnly
        {
            [DebuggerStepThrough]
            get
            {
                return _isReadOnly;
            }
        }

        ///<summary>
        /// Removes the first occurrence of a specific object from the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///</summary>
        ///<returns>
        /// <c>true</c> if item was successfully removed from the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>;
        /// otherwise, <c>false</c>. This method also returns <c>false</c>
        /// if item is not found in the original 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///</returns>
        ///<param name="item">The object to remove from the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        ///<exception cref="T:System.NotSupportedException">The 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// is read-only.</exception>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _Remove(ref item);
        }

        #endregion

        #region IEnumerable <KeyValuePair<TKey,TValue>> members

        ///<summary>
        /// Returns an enumerator that iterates through the collection.
        ///</summary>
        ///<returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/>
        /// that can be used to iterate through the collection.
        ///</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _GetEnumerator();
        }

        #endregion

        #region IEnumerable membres

        ///<summary>
        /// Returns an enumerator that iterates through a collection.
        ///</summary>
        ///<returns>
        /// An <see cref="T:System.Collections.IEnumerator"/>
        /// object that can be used to iterate through the collection.
        ///</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _GetEnumerator();
        }

        #endregion

        #region ICloneable members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <remarks>Creates deep copy of the current dictionary.</remarks>
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