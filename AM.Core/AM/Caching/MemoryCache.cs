/* MemoryCache.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Caching
{
    /// <summary>
    /// Cache base.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class MemoryCache<TKey, TValue>
        where TValue: class
    {
        #region Properties
        
        #endregion

        #region Construction

        /// <summary>
        /// Dictionary
        /// </summary>
        public MemoryCache()
        {
            _dictionary
                = new ConcurrentDictionary<TKey, CacheItem<TKey, TValue>>();
        }

        #endregion

        #region Private members

        private readonly ConcurrentDictionary<TKey, CacheItem<TKey,TValue>>
            _dictionary;

        #endregion

        #region Public methods

        /// <summary>
        /// Add item with key.
        /// </summary>
        public MemoryCache<TKey, TValue> Add
            (
                TKey key,
                [NotNull] TValue value
            )
        {
            Code.NotNull(value, "value");

            CacheItem<TKey, TValue> item = new CacheItem<TKey, TValue>
                (
                    key,
                    value
                );
            _dictionary[key] = item;

            return this;
        }

        /// <summary>
        /// Clear.
        /// </summary>
        public void Clear()
        {
            _dictionary.Clear();
        }

        /// <summary>
        /// Contains given key?
        /// </summary>
        public bool ContainsKey
            (
                TKey key
            )
        {
            CacheItem<TKey, TValue> item;
            if (!_dictionary.TryGetValue(key, out item))
            {
                return false;
            }

            return item.IsAlive;
        }

        /// <summary>
        /// Get item for given key.
        /// </summary>
        public TValue Get
            (
                TKey key
            )
        {
            CacheItem<TKey, TValue> item;
            if (!_dictionary.TryGetValue(key, out item))
            {
                return default(TValue);
            }

            TValue result = item.Value;
            if (ReferenceEquals(result, null))
            {
                _dictionary.TryRemove(key, out item);
            }

            return result;
        }

        /// <summary>
        /// Remove item for specified key.
        /// </summary>
        public void Remove
            (
                TKey key
            )
        {
            CacheItem<TKey, TValue> item;
            _dictionary.TryRemove(key, out item);
        }

        /// <summary>
        /// Remove dead items.
        /// </summary>
        public void RemoveDeadItems()
        {
            List<CacheItem<TKey,TValue>> list
                = new List<CacheItem<TKey, TValue>>();
            foreach (var item in _dictionary)
            {
                if (!item.Value.IsAlive)
                {
                    list.Add(item.Value);
                }
            }

            foreach (var dead in list)
            {
                CacheItem<TKey, TValue> item;
                _dictionary.TryRemove(dead.Key, out item);
            }
        }

        /// <summary>
        /// Remove too old items.
        /// </summary>
        public void RemoveOldItems
            (
                DateTime before
            )
        {
            List<CacheItem<TKey, TValue>> list
                = new List<CacheItem<TKey, TValue>>();
            foreach (var item in _dictionary)
            {
                if (item.Value.Created.CompareTo(before) < 0)
                {
                    list.Add(item.Value);
                }
            }

            foreach (var dead in list)
            {
                CacheItem<TKey, TValue> item;
                _dictionary.TryRemove(dead.Key, out item);
            }
        }


        /// <summary>
        /// Remove unused items.
        /// </summary>
        public void RemoveUnusedItems
            (
                DateTime before
            )
        {
            List<CacheItem<TKey, TValue>> list
                = new List<CacheItem<TKey, TValue>>();
            foreach (var item in _dictionary)
            {
                if (item.Value.LastUsed.CompareTo(before) < 0)
                {
                    list.Add(item.Value);
                }
            }

            foreach (var dead in list)
            {
                CacheItem<TKey, TValue> item;
                _dictionary.TryRemove(dead.Key, out item);
            }
        }

        #endregion
    }
}
